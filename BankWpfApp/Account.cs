using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankWpfApp
{
    [XmlRoot("account")]
    public class Account : Product, IBalans, IProductType
    {
        public static string[] nameTypeAccount = { "расчётный", "депозитный", "кредитный" };
        public static int GetNumType(string nm)
        {
            int res = -1;
            for (int i = 0; i < nameTypeAccount.Length; i++)
            {
                if (nameTypeAccount[i] == nm)
                {
                    res = i;
                    break;
                }
            }
            return res;
        }
        /// <summary>
        /// тип продукта : 3 - счёт
        /// </summary>
        public int Type { get; set; } = 3;

        /// <summary>
        /// Количество денег на счёте
        /// </summary>
        public float Balans { get; set; } = 0;

        private int _typeAccount;
        /// <summary>
        /// тип счёта : 0 - расчётный, 1 - депозитный, 2 - кредитный
        /// </summary>
        public int TypeAccount
        { 
            get
            {
                return _typeAccount;
            }
            set
            {
                _typeAccount = (value >= 0 && value < 3) ? value : 0;
            }
        }

        public Account() { }
        public Account(int tp)
        {
            TypeAccount = tp;
            Name = Account.nameTypeAccount[TypeAccount];
        }

        /// <summary>
        /// информация о счёте
        /// </summary>
        /// <returns>соварь из параметров и их значений</returns>
        public override Dictionary<string, string> GetProductInfo()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add("Категория", "Счёт");
            res.Add("Тип", Account.nameTypeAccount[TypeAccount]);
            res.Add("Номер", UID.ToString());
            res.Add("Описание", Description);
            return res;
        }
    }

    public class BankAccount : Account, IPersonProductNumber
    {
        public int personUID { get; set; }
        public string StrNumber => $"№ {PersonProductNumber}";
        public string StrBalance => $"{Balans:0.00} Р";
        public long PersonProductNumber { get; set; }

        public BankAccount() { }
        public BankAccount(int tp, int persID) : base(tp)
        {
            personUID = persID;
        }
    }
}
