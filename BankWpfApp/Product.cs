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

    [XmlRoot("root")]
    [XmlInclude(typeof(Account))]
    [XmlInclude(typeof(Deposit))]
    [XmlInclude(typeof(Card))]
    [XmlInclude(typeof(Credit))]
    public class Product : IId, IDictInfo        
    {
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
