using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWpfApp
{
    [Serializable]
    public class UserData : IId
    {
        public int UID { get; set; }
        
        /// <summary>
        /// логин работника
        /// </summary>
        public string UserLogin { get; set; }
        /// <summary>
        /// пароль работника
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// В зависимости от значения устанавливаются права пользователя по доступу и изменению данных
        /// 1 - user (консультант, читает Ф.И.О., телефон, меняет телефон на другой)
        /// 2 - manager (менеджер, читает Ф.И.О., паспорт, телефон, меняет телефон, добавляет клиентов)
        /// 3 - admin (администратор, полные права по клиентам и работникам)
        /// </summary>
        public int Rule { get; set; }


        public UserData() { }

        public UserData(string login, string pass, int r)
        {
            UserLogin = login;
            Password = pass;
            Rule = r;
        }

        public bool CheckUser(string login, string pass)
        {
            return (UserLogin == login) && (Password == pass);
        }
    }

    public interface IUserRights
    {
        bool IsViewingField(string nameField = "");
        bool IsEditingField(string nameField = "");
        bool IsEditingUser();
        bool IsAddingPerson();
    }

    public class Consultant : UserData, IUserRights
    {
        public bool IsAddingPerson()
        {
            return false;
        }

        public bool IsEditingField(string nameField = "")
        {
            return nameField == "Tlf";
        }

        public bool IsEditingUser()
        {
            return false;
        }

        public bool IsViewingField(string nameField = "")
        {
            return nameField != "Pasport";
        }

        public Consultant() { }

        public Consultant(string log, string pass) : base(log, pass, 1)
        { }
    }

    public class Manager : UserData, IUserRights
    {
        public bool IsAddingPerson()
        {
            return true;
        }

        public bool IsEditingField(string nameField = "")
        {
            return true;
        }

        public bool IsEditingUser()
        {
            return false;
        }

        public bool IsViewingField(string nameField = "")
        {
            return true;
        }

        public Manager() { }
        public Manager(string log, string pass) : base(log, pass, 2)
        { }
    }


    public class Administrator : UserData, IUserRights
    {
        public bool IsAddingPerson()
        {
            return true;
        }

        public bool IsEditingField(string nameField = "")
        {
            return true;
        }

        public bool IsEditingUser()
        {
            return true;
        }

        public bool IsViewingField(string nameField = "")
        {
            return true;
        }

        public Administrator() { }

        public Administrator(string log, string pass) : base(log, pass, 3)
        { }
    }

}
