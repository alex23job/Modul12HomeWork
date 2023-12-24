using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankWpfApp
{
    interface IBalans
    {
        float Balans { get; set; }
    }

    interface IDictInfo
    {
        Dictionary<string, string> GetProductInfo();
    }

    interface IPersonProductNumber
    {
        long PersonProductNumber { get; set; }
    }

    /// <summary>
    /// 0 - Card, 1 - Deposit, 2 - Credit, 3 - Account
    /// </summary>
    interface IProductType
    {
        /// <summary>
        /// 0 - Card, 1 - Deposit, 2 - Credit, 3 - Account
        /// </summary>
        int Type { get; set; }
    }

    interface IIsRequest
    {
        /// <summary>
        /// Это заявка ? true - да; false - нет, это уже продукт
        /// </summary>
        bool IsRequest { get; set; }
    }

    [XmlRoot("root")]
    [XmlInclude(typeof(BankDeposit))]
    [XmlInclude(typeof(BankCredit))]
    [XmlInclude(typeof(BankCard))]
    [XmlInclude(typeof(BankAccount))]
    [XmlInclude(typeof(Account))]
    [XmlInclude(typeof(Deposit))]
    [XmlInclude(typeof(Card))]
    [XmlInclude(typeof(Credit))]
    public class Product : IId, IDictInfo
    {
        static long nextPersonProductNumber = 1000000;
        public static long GetNextPersonProductNumber() => nextPersonProductNumber++;
        public static void SetNextPersonProductNumber(long value)
        {
            if (value > 0) nextPersonProductNumber = value;
        }

        /// <summary>
        /// наименование продукта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// порядковый номер
        /// </summary>
        public int UID { get; set; }

        /// <summary>
        /// описание продукта
        /// </summary>
        public string Description { get; set; }

        public Product() { }
        public Product(string nm)
        {
            Name = nm;
        }

        public virtual Dictionary<string, string> GetProductInfo()
        {
            Dictionary<string, string> res = new Dictionary<string, string>();
            res.Add("Название", "неизвестный продукт");
            return res;
        }
    }
}
