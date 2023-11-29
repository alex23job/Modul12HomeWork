using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BankWpfApp
{
    /// <summary>
    /// Логика взаимодействия для UserEditWindow.xaml
    /// </summary>
    public partial class UserEditWindow : Window
    {
        ObservableCollection<UserPosition> usPos = null;
        //ObservableCollection<UserData> dbUsers = null;
        Repository<UserData> dbUsers = null;
        //public ObservableCollection<UserData> GetDB()
        //{
        //    return dbUsers;
        //}
        public UserEditWindow()
        {
            InitializeComponent();
        }

        //public void SetUsers(ObservableCollection<UserData> db)
        public void SetUsers(Repository<UserData> db)
        {
            dbUsers = db;
            usPos = new ObservableCollection<UserPosition>();
            foreach (UserData us in db.AllItems)
            {
                if (us.Rule > 0)
                {
                    usPos.Add(new UserPosition(us.UserLogin, us.Rule));
                }
            }

            listViewUsers.ItemsSource = usPos;
            listViewUsers.SelectedIndex = 0;
        }

        private void ComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OnChangePassword(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            ListViewItem lvi = listViewUsers.ItemContainerGenerator.ContainerFromItem(menu.DataContext) as ListViewItem;
            ChangePasswordWindow cpw = new ChangePasswordWindow();
            int i;
            for (i = 0; i < dbUsers.Count; i++)
            {
                if (dbUsers.AllItems[i].UserLogin == ((UserPosition)lvi.DataContext).UserLogin)
                {
                    cpw.SetUser(dbUsers.AllItems[i]);
                    break;
                }
            }
            if (cpw.ShowDialog() == true)
            {
                for (i = 0; i < dbUsers.Count; i++)
                {
                    if (dbUsers.AllItems[i].UserLogin == cpw.GetUser().UserLogin)
                    {
                        dbUsers.AllItems[i].Password = cpw.GetUser().Password;
                        break;
                    }
                }
            }
        }

        private void OnDelUser(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            ListViewItem lvi = listViewUsers.ItemContainerGenerator.ContainerFromItem(menu.DataContext) as ListViewItem;
            //MessageBox.Show(((UserPosition)lvi.DataContext).ToString());
            string info = string.Format("\"Логин: {0}   должность: {1}\"", ((UserPosition)lvi.DataContext).UserLogin, UserPosition.GetPosition(((UserPosition)lvi.DataContext).Rule));
            if (MessageBox.Show($"Выбрана запись сотрудника : {info}\n\nУдалить запись ?", "Удаление записи сотрудника", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                UserData delUser = null;
                foreach(UserData ud in dbUsers.AllItems)
                {
                    if (ud.UserLogin == ((UserPosition)lvi.DataContext).UserLogin)
                    {
                        delUser = ud;
                        break;
                    }
                }
                usPos.Remove((UserPosition)lvi.DataContext);
                if (delUser != null)
                {
                    dbUsers.DelItem(delUser);
                }
            }

        }

        private void OnAddUserClick(object sender, RoutedEventArgs e)
        {
            if (txtNewUserLogin.Text != "")
            {
                usPos.Add(new UserPosition(txtNewUserLogin.Text, 1));
                dbUsers.Add(new UserData(txtNewUserLogin.Text, "11111", 1));
            }
        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            int i;
            bool isNew = true;
            foreach (UserPosition up in usPos)
            {
                isNew = true;
                //MessageBox.Show(up.ToString());
                for (i = 0; i < dbUsers.AllItems.Count; i++)
                {
                    UserData ud = dbUsers.AllItems[i];
                    if (ud.UserLogin == up.UserLogin)
                    {
                        if (ud.Rule != up.Rule)
                        {
                            ud.Rule = up.Rule;
                        }
                        isNew = false;
                        break;
                    }
                }
                if (isNew)
                {
                    dbUsers.Add(new UserData(up.UserLogin, "11111", up.Rule));
                }
            }
            this.DialogResult = true;
        }
    }

    public class UserPosition
    {
        private static readonly List<string> usersPos;

        static UserPosition()
        {
            usersPos = new List<string>();
            usersPos.Add("Клиент");
            usersPos.Add("Консультант");
            usersPos.Add("Менеджер");
            usersPos.Add("Администратор");
        }

        public UserPosition(string log, int r)
        {
            UserLogin = log;
            Rule = r;
        }

        public string UserLogin { get; set; }
        public int Rule { get; set; }
        public IEnumerable<string> Positions => usersPos;

        public static string GetPosition(int index)
        {
            return usersPos[index];
        }

        public override string ToString()
        {
            return $"Login={UserLogin} Rule={Rule}";
        }
    }
}
