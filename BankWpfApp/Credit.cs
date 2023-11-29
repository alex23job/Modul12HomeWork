using System;
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
        /// информация о счёте
        /// </summary>
        /// <returns>соварь из параметров и их значений</returns>
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
}
