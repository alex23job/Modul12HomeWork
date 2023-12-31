﻿using System;
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

    [XmlInclude(typeof(Bonus))]
    [XmlInclude(typeof(BonusAction))]
    [XmlInclude(typeof(BonusActionPerson))]
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
        /// <summary>
        /// Массив действующих бонус акций предприятий-партнёров, которыми воспользовался клиент при оплате картой
        /// </summary>
        public List<BonusActionPerson> ArrBonusActions = null;
        /// <summary>
        /// Последнее обновление данных клиента
        /// </summary>
        public LogPersonUpdate updateInfo = null;
        /// <summary>
        /// выбранные категории повышенного кэшбека: категория выбрана если соответвующий бит установлен
        /// </summary>
        public int BonusCategory = 0;

        [XmlIgnore]
        public string Count => (IdProducts != null) ? IdProducts.Count.ToString() : "0";

        /// <summary>
        /// Логин из UserData
        /// </summary>
        public string PersonLogin = "";

        /// <summary>
        /// Экземпляр UserData, связанный с этим клиентом для его идентификации при входе в приложение
        /// </summary>
        private UserData user = null;
        public int UserUID = 0;

        /// <summary>
        /// Данные клиента в формате <<  Иванов И.И. >>
        /// </summary>
        public string FIO => $"{Name} {LastName[0]}.{SecondName[0]}.";

        public Person()
        {
            IdProducts = new List<long>();
            ArrBonusActions = new List<BonusActionPerson>();
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
            ArrBonusActions = new List<BonusActionPerson>();
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

        public BonusActionPerson GetBonusAction(int uid)
        {
            foreach(BonusActionPerson bap in ArrBonusActions)
            {
                if (bap.LegalPersonUID == uid) return bap;
            }
            return null;
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
        public static readonly string[] category = { "Еда", "Авто", "Медицина", "Развлечения", "Обучение", "Дом", "Одежда", "Электроника", "Услуги", "Транспорт", "Прочее" };
        /// <summary>
        /// Функция возвращает индекс указанной категории в массиве
        /// </summary>
        /// <param name="cat">название категории</param>
        /// <returns>индекс в массиве категорий</returns>
        public static int GetIndexCategory(string cat)
        {
            for (int i = 0; i < category.Length; i++)
            {
                if (cat == category[i]) return i;
            }
            return -1;
        }
        /// <summary>
        /// Юр.наименование предприятия
        /// </summary>
        public string LegalName { get; set; }
        /// <summary>
        /// Юр.адрес предприятия
        /// </summary>
        public string LegalAddress { get; set; }
        /// <summary>
        /// Имя картинки с логотипом в специальном каталоге
        /// </summary>
        public string LogoPath { get; set; } = "";
        /// <summary>
        /// Котегория предприятия
        /// </summary>
        public string LegalCategoty { get; set; } = "Прочее";

        /// <summary>
        /// Бонус к кэшбеку в категории
        /// </summary>
        public Bonus MyBonus { get; set; } = null;

        /// <summary>
        /// Действующая бонусная акция, или null еслм не установлена
        /// </summary>
        public BonusAction MyBonusAction { get; set; } = null;

        /// <summary>
        /// Список раннее проведённых акций
        /// </summary>
        public List<BonusAction> ArhiveBonusActions = null;

        public LegalPerson() : base()
        {
            Type = 2;
            ArhiveBonusActions = new List<BonusAction>();
        }

        public LegalPerson(string nm1, string nm2, string nm3, string psp, string tlf, string bd, string legnm, string legad, string logo="", string cat= "Прочее") : base(nm1, nm2, nm3, psp, tlf, bd)
        {
            LegalName = legnm;
            LegalAddress = legad;
            LogoPath = logo;
            LegalCategoty = cat;
            Type = 2;
            ArhiveBonusActions = new List<BonusAction>();
        }

        /// <summary>
        /// Проверка действия установленной акции. Если период прошёл, то её в архив, а переменную обнулить
        /// </summary>
        public void CheckBonusAction()
        {
            if (MyBonusAction != null && !MyBonusAction.TestPeriod())
            {
                ArhiveBonusActions.Add(MyBonusAction);
                MyBonusAction = null;
            }
        }
    }
}
