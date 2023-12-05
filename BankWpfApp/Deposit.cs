using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankWpfApp
{
    [XmlRoot("deposit")]
    public class Deposit : Product, IProductType
    {
        /// <summary>
        /// имп продукта : 1 - вклад
        /// </summary>
        public int Type { get; set; } = 1;

        /// <summary>
        /// срок действия договора
        /// </summary>
        public int depositTerm { get; set; }

        /// <summary>
        /// начисляемый процент
        /// </summary>
        public float percent { get; set; }

        /// <summary>
        /// расчётный период
        /// </summary>
        public int billingPeriod { get; set; }

        /// <summary>
        /// возможность пополнять
        /// </summary>
        public bool IsCanTopUp { get; set; }
        
        /// <summary>
        /// возможность снимать
        /// </summary>
        public bool IsCanWithdraw { get; set; }

        /// <summary>
        /// начисление процентов на остаток в начале расчётного периода
        /// </summary>
        public bool IsCapitalization { get; set; }

        /// <summary>
        /// минимальная сумма вклада
        /// </summary>
        public float minBalance { get; set; }

        /// <summary>
        /// информация о вкладе
        /// </summary>
        /// <returns>соварь из параметров и их значений</returns>
        public override Dictionary<string, string> GetProductInfo()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add("Категория", "Вклад");
            res.Add("Название", Name);
            res.Add("Срок вклада", depositTerm.ToString());
            res.Add("Процент", percent.ToString());
            res.Add("Расчётный период", billingPeriod.ToString());
            res.Add("Пополнение", IsCanTopUp ? "Да" : "Нет");
            res.Add("Снятие", IsCanWithdraw ? "Да" : "Нет");
            res.Add("C капитализацией", IsCapitalization ? "Да" : "Нет");
            res.Add("Мин. сумма", minBalance.ToString());
            res.Add("Описание", Description);
            return res;
        }
    }


    public class BankDeposit : Deposit, IPersonProductNumber
    {
        public int personUID { get; set; }
        public string StrNumber => $"№ {PersonProductNumber}  {percent:0.0}%";
        public string StrBalance => (DepositAccount != null) ? $"{DepositAccount.Balans:0.00} Р" : "0 Р";
        public float MinPeriodBalance { get; set; } = 0;

        public long PersonProductNumber { get; set; }
        public BankAccount DepositAccount { get; set; } = null;
        public void CopyParamsProduct(Deposit cd)
        {
            Description = cd.Description;
            depositTerm = cd.depositTerm;
            percent = cd.percent;
            billingPeriod = cd.billingPeriod;
            IsCanTopUp = cd.IsCanTopUp;
            IsCanWithdraw = cd.IsCanWithdraw;
            IsCapitalization = cd.IsCapitalization;
            minBalance = cd.minBalance;
        }
    }
}
