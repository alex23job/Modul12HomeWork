﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankWpfApp
{
    [XmlRoot("credit")]
    public class Credit : Product, IProductType
    {
        /// <summary>
        /// имена типов кредитов
        /// </summary>
        public static string[] nameTypeCredit = { "потребительский", "ипотечный", "автомобильный", "на развитие бизнеса" };
        public static int GetNumType(string nm)
        {
            int res = -1;
            for (int i = 0; i < nameTypeCredit.Length; i++)
            {
                if (nameTypeCredit[i] == nm)
                {
                    res = i;
                    break;
                }
            }
            return res;
        }
        /// <summary>
        /// тип продукта : 2 - кредит
        /// </summary>
        public int Type { get; set; } = 2;

        private int _typeCredit;
        /// <summary>
        /// тип кредита : 0 - потребительский, 1 - ипотечный, 2 - автомобильный, 3 - на развитие бизнеса
        /// </summary>
        public int TypeCredit
        {
            get
            {
                return _typeCredit;
            }
            set
            {
                _typeCredit = (value >= 0 && value < 4) ? value : 0;
            }
        }

        /// <summary>
        /// процент по кредиту
        /// </summary>
        public float Percent { get; set; }

        /// <summary>
        /// Вносится ли залог
        /// </summary>
        public bool IsCollateral { get; set; }

        /// <summary>
        /// имущество под залог
        /// </summary>
        public string Collateral { get; set; }

        /// <summary>
        /// Нужны ли поручители
        /// </summary>
        public bool IsSurety { get; set; }

        /// <summary>
        /// Сколько должно быть поручителей
        /// </summary>
        public int Surety { get; set; }

        /// <summary>
        /// Устанавливается ли максимальный лимит кредита
        /// </summary>
        public bool IsMaxLimit { get; set; }

        /// <summary>
        /// Размер максимального лимита кредита
        /// </summary>
        public int MaxLimit { get; set; }

        public Credit() { }
        public Credit(int tp)
        {
            TypeCredit = tp;
            Name = Credit.nameTypeCredit[TypeCredit];
        }

        /// <summary>
        /// информация о кредите
        /// </summary>
        /// <returns>словарь из параметров и их значений</returns>
        public override Dictionary<string, string> GetProductInfo()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add("Категория", "Кредит");
            res.Add("Тип", Credit.nameTypeCredit[TypeCredit]);
            res.Add("Название", Name);
            res.Add("процент по кредиту", Percent.ToString());
            if (IsCollateral) res.Add("Имущество в залог", Collateral);
            if (IsSurety) res.Add("Поручители", Surety.ToString());
            if (IsMaxLimit) res.Add("Максимальный лимит", MaxLimit.ToString());
            res.Add("Описание", Description);
            return res;
        }
    }

    //[XmlInclude(typeof(NextPayment))]
    public class BankCredit : Credit, IPersonProductNumber, IIsRequest
    {
        /// <summary>
        /// расчёт ежемесячного платежа по аннуитетной системе выплат
        /// </summary>
        /// <param name="s">сумма кредита</param>
        /// <param name="p">процентная ставка</param>
        /// <param name="m">число месяцев</param>
        /// <returns>платеж в месяц</returns>
        public static float CalcMonthlyPayment(float s, float p, int m)
        {
            float cm = p / 1200;
            float zn = s * cm / (1 - 1 / (float)Math.Pow(1 + cm, m));
            return zn;
        }
        public int personUID { get; set; }
        public string StrNumber => $"№ {PersonProductNumber}";

        public string StrBalance => (CreditAccount != null) ? $"{CreditAccount.Balans:0.00} Р" : "0,00 Р";
        public string StrInfo
        {
            get
            {
                if (IsRequest)
                    return "Заявка";
                else
                {
                    return $"-{GetNextPayment():0.00} Р   {NextPayment.DatePayment}";
                }
            }
        }

        public BankAccount CreditAccount { get; set; } = null;

        /// <summary>
        /// Начальная сумма кредита
        /// </summary>
        public float TotalSum { get; set; }

        /// <summary>
        /// Продолжительность кредита в днях
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// День платежа
        /// </summary>
        public int DayOfPayment { get; set; } 

        /// <summary>
        /// Дата оформления кредита
        /// </summary>
        public string RegistrationDate { get; set; }

        public long PersonProductNumber { get; set; }

        [XmlIgnore]
        public NextPayment NextPayment { get; set; }
        /// <summary>
        /// Статус заявки: true - заявка, false - оформлен
        /// </summary>
        public bool IsRequest { get; set; }
        public void CopyParamsProduct(Credit cd)
        {
            Description = cd.Description;
            Percent = cd.Percent;
            if (cd.IsCollateral)
            {
                IsCollateral = true;
                Collateral = cd.Collateral;
            }
            if (cd.IsSurety)
            {
                IsSurety = true;
                Surety = cd.Surety;
            }
            if (cd.IsMaxLimit)
            {
                IsMaxLimit = true;
                MaxLimit = cd.MaxLimit;
            }
            NextPayment = new NextPayment(TotalSum, Percent, Period, DayOfPayment);
        }

        /// <summary>
        /// информация о кредите
        /// </summary>
        /// <returns>словарь из параметров и их значений</returns>
        public override Dictionary<string, string> GetProductInfo()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add("Категория", "Кредит");
            res.Add("Тип", Credit.nameTypeCredit[TypeCredit]);
            res.Add("Название", Name);
            res.Add("Процент по кредиту", Percent.ToString());
            res.Add("Сумма", TotalSum.ToString());
            res.Add("Срок", Period.ToString());
            res.Add("День платежа", DayOfPayment.ToString());
            if (IsCollateral) res.Add("Имущество в залог", Collateral);
            if (IsSurety) res.Add("Поручители", Surety.ToString());
            if (IsMaxLimit) res.Add("Максимальный лимит", MaxLimit.ToString());
            res.Add("Описание", Description);
            return res;
        }

        public float GetNextPayment()
        {
            if (NextPayment == null)
            {
                NextPayment = new NextPayment(TotalSum, Percent, Period, DayOfPayment);
            }
            return NextPayment.Payment;
        }
    }

    public class NextPayment
    {
        public string DatePayment
        { 
            get
            {
                string date = "";
                DateTime dt = DateTime.Now;
                if (day <= dt.Day)
                {
                    date = $"{dt.Year:0000}.{dt.Month:00}.{day:00}";
                }
                else
                {
                    DateTime nextPayDate = new DateTime(dt.Year, dt.Month, day);
                    nextPayDate = nextPayDate.AddMonths(1);
                    date = $"{nextPayDate.Year:0000}.{nextPayDate.Month:00}.{day:00}";
                }
                return date;
            }
        }

        public float Payment
        {
            get
            {
                return BankCredit.CalcMonthlyPayment(Sum, PercentPayment, period);
            }
        }

        public float Sum { get; set; }
        public float PercentPayment { get; set; }
        public float Repayment { get; set; }

        private int period;
        private int day;
        public NextPayment() { }
        public NextPayment(float s, float pr, int per, int d)
        {
            Sum = s;
            PercentPayment = pr;
            period = per;
            day = d;
        }

        public void ReCalc(float prc, float ost, float cash)
        {

        }
    }
}
