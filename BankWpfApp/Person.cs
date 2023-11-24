using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWpfApp
{
    public class Person : IId
    {
        /// <summary>
        /// Идентификатор-
        /// </summary>
        public int UID { get; set; }

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
        /// Дата рождения
        /// </summary>
        public string BirthDay { get; set; }

        /// <summary>
        /// Массив идентификаторов имеющихся у клиента продуктов банка
        /// кредиты, счета, карты хранятся в отдельной базе
        /// </summary>
        public List<int> IdProducts = null;

        public LogPersonUpdate updateInfo = null;

        //[XmlIgnore]
        public string Count => (IdProducts != null) ? IdProducts.Count.ToString() : "0";

        private UserData user = null;

        public Person()
        {
            IdProducts = new List<int>();
        }
        public Person(string nm1, string nm2, string nm3, string psp, string tlf, string bd)
        {
            Name = nm1;
            LastName = nm2;
            SecondName = nm3;
            Pasport = psp;
            BirthDay = bd;
            this.Tlf = tlf;
            IdProducts = new List<int>();
        }

        public void SetUserData(UserData ud)
        {
            if (user == null) user = ud;
        }

        public void UpdateUserLogin(string login)
        {
            if (user != null)
            {
                user.UserLogin = login;
            }
        }
        public void UpdateUserPassword(string pass)
        {
            if (user != null)
            {
                user.Password = pass;
            }
        }

        public bool CheckUser(string login, string pass)
        {
            return user.CheckUser(login, pass);
        }

        public bool CheckUserUID(int id)
        {
            if (user == null) return false;
            return user.UID == id;
        }
    }
}
