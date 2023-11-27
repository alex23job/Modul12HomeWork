using System;
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
        public static string[] nameTypeCard = { "дебетовая", "кредитная" };
        /// <summary>
        /// тип продукта : 0 - карта
        /// </summary>
        public int Type { get; set; } = 0;

        private int _typeCard;
        /// <summary>
        /// тип счёта : 0 - дебетовая, 1 - кредитная
        /// </summary>
        public int TypeCard
        {
            get
            {
                return _typeCard;
            }
            set
            {
                _typeCard = (value >= 0 && value < 2) ? value : 0;
            }
        }

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
            res.Add("Номер", UID.ToString());
            res.Add("Описание", Description);
            return res;
        }
    }
}
