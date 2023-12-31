﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankWpfApp
{
    [XmlRoot("card")]
    public class Card : Product, IProductType
    {
        /// <summary>
        /// Первые 4 цифры номера карты
        /// </summary>
        public static string CodeBank = "2400";

        /// <summary>
        /// имена типов карт
        /// </summary>
        public static string[] nameTypeCard = { "дебетовая", "кредитная", "предоплаченная" };
        public static int GetNumType(string nm)
        {
            int res = -1;
            for (int i = 0; i < nameTypeCard.Length; i++)
            {
                if (nameTypeCard[i] == nm)
                {
                    res = i;
                    break;
                }
            }
            return res;
        }
        /// <summary>
        /// тип продукта : 0 - карта
        /// </summary>
        public int Type { get; set; } = 0;

        private int _typeCard;
        /// <summary>
        /// тип карты : 0 - дебетовая, 1 - кредитная, 2 - предоплаченная
        /// </summary>
        public int TypeCard
        {
            get
            {
                return _typeCard;
            }
            set
            {
                _typeCard = (value >= 0 && value < 3) ? value : 0;
            }
        }

        private int _code;
        /// <summary>
        /// номер программы для типа карты - число от 1 до 99 для 5 и 6 цифр номера карты
        /// </summary>
        public int CodeProgramm { get => _code; set { _code = (value >= 1 && value < 100) ? value : 0; } }

        /// <summary>
        /// Начисляется ли кешбек
        /// </summary>
        public bool IsCashback { get; set; }

        /// <summary>
        /// процент кешбека
        /// </summary>
        public float CashbackPercent { get; set; }

        /// <summary>
        /// Начисляется ли процент на остаток
        /// </summary>
        public bool IsBalancePercent { get; set; }

        /// <summary>
        /// Величина начисляемого процента на остаток
        /// </summary>
        public float BalancePercent { get; set; }

        /// <summary>
        /// Устанавливается ли лимит по карте
        /// </summary>
        public bool IsLimit { get; set; }

        /// <summary>
        /// Размер лимита карты
        /// </summary>
        public float Limit { get; set; }

        public Card() { }
        public Card(int tp)
        {
            TypeCard = tp;
            Name = Card.nameTypeCard[TypeCard];
        }

        /// <summary>
        /// информация о счёте
        /// </summary>
        /// <returns>соварь из параметров и их значений</returns>
        public override Dictionary<string, string> GetProductInfo()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add("Категория", "Карта");
            res.Add("Тип", Card.nameTypeCard[TypeCard]);
            res.Add("Название", Name);
            res.Add("Код продукта", CodeProgramm.ToString());
            if (IsCashback) res.Add("Кешбек", CashbackPercent.ToString());
            if (IsBalancePercent) res.Add("Процент на остаток", BalancePercent.ToString());
            if (IsLimit) res.Add("Лимит карты", Limit.ToString());
            res.Add("Описание", Description);
            return res;
        }
    }

    public class BankCard : Card, IPersonProductNumber, IIsRequest
    {
        public int personUID { get; set; }
        public float CashbackBalance { get; set; } = 0;
        public float MinBalance { get; set; } = 0;
        public string StrBalance => (CardAccount != null) ? $"{CardAccount.Balans:0.00} Р" : "0,00 Р";
        public string StrNumber => $"{Card.CodeBank} {CodeProgramm:00}** **** {(UID % 10000):0000}";

        public string StrInfo
        {
            get
            {
                if (IsRequest)
                    return "Заявка";
                else
                {
                    return IsCashback ? $"{CashbackBalance:0.00}" : "";
                }
            }
        }

        public BankAccount CardAccount { get; set; } = null;

        public long PersonProductNumber { get; set; }
        public bool IsRequest { get; set; }

        public void CopyParamsProduct(Card cd)
        {
            Description = cd.Description;
            CodeProgramm = cd.CodeProgramm;
            if (cd.IsBalancePercent)
            {
                IsBalancePercent = true;
                BalancePercent = cd.BalancePercent;
            }
            if (cd.IsCashback)
            {
                IsCashback = true;
                CashbackPercent = cd.CashbackPercent;
            }
            if (cd.IsLimit)
            {
                IsLimit = true;
                Limit = cd.Limit;
            }
        }

        public bool CheckCardNumber(string cardNum)
        {
            string CardNumber = $"{Card.CodeBank} {CodeProgramm:00}{((UID / 100000000) % 100):00} {((UID / 10000) % 10000):0000} {(UID % 10000):0000}";
            return CardNumber == cardNum;
        }
    }
}
