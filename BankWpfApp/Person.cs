using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankWpfApp
{
    interface IPersonType
    {
        // 0 - simple, 1 - VIP, 2 - legal
        int Type { get; set; }
    }

    [XmlInclude(typeof(LegalPerson))]
    [XmlInclude(typeof(VipPerson))]
    public class Person : IId, IPersonType
    {
        /// <summary>
        /// Идентификатор-
        /// </summary>
        public int UID { get; set; }

        public int Type { get; set; }

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
        public List<long> IdProducts = null;

        public LogPersonUpdate updateInfo = null;

        [XmlIgnore]
        public string Count => (IdProducts != null) ? IdProducts.Count.ToString() : "0";


        public string PersonLogin = "";

        private UserData user = null;
        public int UserUID = 0;

        public Person()
        {
            IdProducts = new List<long>();
            Type = 0;
        }
        public Person(string nm1, string nm2, string nm3, string psp, string tlf, string bd)
        {
            Name = nm1;
            LastName = nm2;
            SecondName = nm3;
            Pasport = psp;
            BirthDay = bd;
            this.Tlf = tlf;
            IdProducts = new List<long>();
            Type = 0;
        }

        public void CopyEditParams(Person pp)
        {
            Name = pp.Name;
            LastName = pp.LastName;
            SecondName = pp.SecondName;
            Pasport = pp.Pasport;
            BirthDay = pp.BirthDay;
            this.Tlf = pp.Tlf;
            Type = pp.Type;
            updateInfo = pp.updateInfo;
        }

        public void SetUserData(UserData ud)
        {
            if (user == null)
            {
                user = ud;
                UserUID = user.UID;
                PersonLogin = user.UserLogin;
            }
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

    public class VipPerson : Person
    {
        public VipPerson() : base()
        {
            Type = 1;
        }

        public VipPerson(string nm1, string nm2, string nm3, string psp, string tlf, string bd) : base(nm1, nm2, nm3, psp, tlf, bd)
        {
            Type = 1;
        }
    }


    public class LegalPerson : Person
    {
        public string LegalName { get; set; }
        public string LegalAddress { get; set; }
        public LegalPerson() : base()
        {
            Type = 2;
        }

        public LegalPerson(string nm1, string nm2, string nm3, string psp, string tlf, string bd, string legnm, string legad) : base(nm1, nm2, nm3, psp, tlf, bd)
        {
            LegalName = legnm;
            LegalAddress = legad;
            Type = 2;
        }
    }
}
