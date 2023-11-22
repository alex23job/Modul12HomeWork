using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWpfApp
{
    public class Person
    {
        /// <summary>
        /// Фамилия клиента
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Имя клиента
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество клиента
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Серия и номер паспорта клиента
        /// </summary>
        public string Pasport { get; set; }

        /// <summary>
        /// Номер телефона клиента
        /// </summary>
        public string Tlf { get; set; }

        /// <summary>
        /// Массив идентификаторов имеющихся у клиента продуктов банка
        /// кредиты, счета, карты хранятся в отдельной базе
        /// </summary>
        public List<int> IdProducts = null;

        public LogPersonUpdate updateInfo = null;

        //[XmlIgnore]
        public string Count => (IdProducts != null) ? IdProducts.Count.ToString() : "0";

        public Person()
        {
            IdProducts = new List<int>();
        }
        public Person(string nm1, string nm2, string nm3, string psp, string tlf)
        {
            Name = nm1;
            LastName = nm2;
            SecondName = nm3;
            Pasport = psp;
            this.Tlf = tlf;
            IdProducts = new List<int>();
        }
    }
}
