using System;
using System.IO;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace BankWpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string pathUpdateInfo = "UpdateInfoLog.csv";

        string pathPersonXML = "Person.xml";
        Repository<Person> persons = new Repository<Person>();

        string pathUserXML = "User.xml";
        Repository<UserData> users = new Repository<UserData>();

        string pathProductXML = "Product.xml";
        Repository<Product> products = new Repository<Product>();

        Person currentPerson = null;
        UserData currentUser = null;
        Brush backLK = null;
        Brush backAdmin = null;
        public MainWindow()
        {
            InitializeComponent();
            CreateContextMenuLK();
            CreateRepositorys();
        }

        private void CreateRepositorys()
        {
            if (File.Exists(pathPersonXML))
            {
                persons = Repository<Person>.LoadRepositoryFromFile(pathPersonXML);
            }
            persons.SetSavePath(pathPersonXML);
            persons.SetCurrentNewUID(10000);
            
            if (File.Exists(pathUserXML))
            {
                users = Repository<UserData>.LoadRepositoryFromFile(pathUserXML);
            }
            users.SetSavePath(pathUserXML);
            users.SetCurrentNewUID(10000);
            if (users.Count == 0)
            {
                users.Add(new UserData("admin", "admin", 3));
            }

            if (File.Exists(pathProductXML))
            {
                products = Repository<Product>.LoadRepositoryFromFile(pathProductXML);
            }
            products.SetSavePath(pathProductXML);
            products.SetCurrentNewUID(10);
        }

        private void CreateContextMenuLK()
        {
            borderLK.ContextMenu = new ContextMenu();
            borderLK.ContextMenu.Background = Brushes.Cyan;
            MenuItem menuItemLogin = new MenuItem();
            menuItemLogin.Header = "Войти";
            menuItemLogin.Click += (send, args) =>
            {
                LoginWindow lw = new LoginWindow();
                if (lw.ShowDialog() == true)
                {
                    UserData loginUser = null;
                    foreach(UserData ud in users.AllItems)
                    {
                        if (ud.CheckUser(lw.userLogin, lw.userPassword))
                        {
                            loginUser = ud;
                            break;
                        }
                    }
                    if (loginUser != null)
                    {
                        if (loginUser.Rule == 0)
                        {
                            foreach(Person per in persons.AllItems)
                            {
                                if (per.UserUID == loginUser.UID)
                                {
                                    currentPerson = per;
                                    ShowPersonSP();
                                    break;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Были введены корректные данные сотрудника а не клиента банка !!!");
                        }
                    }
                }
            };
            borderLK.ContextMenu.Items.Add(menuItemLogin);
            MenuItem menuItemRegistration = new MenuItem();
            menuItemRegistration.Header = "Регистрация";
            menuItemRegistration.Click += (send, args) =>
            {
                RegistrationWindow regWin = new RegistrationWindow();
                if (regWin.ShowDialog() == true)
                {
                    UserData ud = users.Add(regWin.GetUser());
                    Person per = regWin.GetPerson();
                    per.SetUserData(ud);
                    currentPerson = persons.Add(per);
                    ShowPersonSP();
                }
            };
            borderLK.ContextMenu.Items.Add(menuItemRegistration);

            //borderLK.ContextMenu.M
            borderLK.ContextMenu.MouseLeave += (send, args) =>
            {
                //borderLK.Background = Brushes.Red;
                borderLK.ContextMenu.IsOpen = false;
            };
        }

        private void ShowPersonSP()
        {
            namePerson.Content = currentPerson.PersonLogin;
            borderLK.Visibility = Visibility.Hidden;
            personSP.Visibility = Visibility.Visible;
            userSP.Visibility = Visibility.Hidden;
            borderAdmin.Visibility = Visibility.Visible;
            currentUser = null;
            ShowPanel(false);
        }

        private void ShowPanel(bool isUser)
        {
            if (isUser)
            {
                pers1.Visibility = Visibility.Hidden;
                pers2.Visibility = Visibility.Hidden;
                pers3.Visibility = Visibility.Hidden;
                pers4.Visibility = Visibility.Hidden;
                user1.Visibility = Visibility.Visible;
                user2.Visibility = Visibility.Visible;
                user3.Visibility = Visibility.Visible;
                user4.Visibility = Visibility.Visible;
                UpdateUsersPanels();
            }
            else
            {
                pers1.Visibility = Visibility.Visible;
                pers2.Visibility = Visibility.Visible;
                pers3.Visibility = Visibility.Visible;
                pers4.Visibility = Visibility.Visible;
                user1.Visibility = Visibility.Hidden;
                user2.Visibility = Visibility.Hidden;
                user3.Visibility = Visibility.Hidden;
                user4.Visibility = Visibility.Hidden;
                UpdatePersonsPanels();
            }
        }

        private void UpdateUsersPanels()
        {
            int[] arrCounters = new int[5];
            int total = 0, i;
            foreach(Person per in persons.AllItems)
            {
                arrCounters[per.Type]++;
            }
            total = arrCounters[0] + arrCounters[1] + arrCounters[2];
            txtPerson.Text = $"Всего клиентов : {total}\nОбычные : {arrCounters[0]}\nVIP           : {arrCounters[1]}\nЮр. лицо : {arrCounters[2]}";

            arrCounters[0] = 0;arrCounters[1] = 0; arrCounters[2] = 0;
            foreach (UserData ud in users.AllItems)
            {
                arrCounters[ud.Rule]++;
            }
            total = arrCounters[3] + arrCounters[1] + arrCounters[2];
            txtWorker.Text = $"Всего сотрудников : {total}\nКонсультанты : {arrCounters[1]}\nМенеджеры    : {arrCounters[2]}\nАдминистраторы : {arrCounters[3]}";

            total = 0;
            if (File.Exists(pathUpdateInfo))
            {
                string[] arrStr = File.ReadAllLines(pathUpdateInfo);
                total = arrStr.Length;
            }
            txtLog.Text = $"Всего записей : {total}";

            arrCounters[0] = 0; arrCounters[1] = 0; arrCounters[2] = 0;arrCounters[3] = 0;
            for (i = 0; i < products.AllItems.Count; i++)
            {
                IProductType pt = products.AllItems[i] as IProductType;
                if (pt != null)
                {
                    arrCounters[pt.Type]++;
                }
            }
            txtProduct.Text = $"Всего продуктов : {products.Count}\nКарты     : {arrCounters[0]}\nВклады   : {arrCounters[1]}\nКредиты : {arrCounters[2]}\nСчета      : {arrCounters[3]}";
        }

        private void UpdatePersonsPanels()
        {

        }

        private void CreateCurrentUser(UserData ud)
        {
            switch (ud.Rule)
            {
                case 1:
                    currentUser = new Consultant(ud.UserLogin, ud.Password);
                    break;
                case 2:
                    currentUser = new Manager(ud.UserLogin, ud.Password);
                    break;
                case 3:
                    currentUser = new Administrator(ud.UserLogin, ud.Password);
                    break;
                default:
                    currentUser = ud;
                    break;
            }
            currentUser.UID = ud.UID;
        }

        private void OnLK_Enter(object sender, MouseEventArgs e)
        {
            backLK = borderLK.Background;
            borderLK.Background = Brushes.Cyan;
            e.Handled = true;
            borderLK.ContextMenu.IsOpen = true;
        }

        private void OnLK_Leave(object sender, MouseEventArgs e)
        {
            borderLK.Background = backLK;
            //if (borderLK.ContextMenu.IsMouseOver == false)
            //{
            //    borderLK.ContextMenu.IsOpen = false;
            //}
        }

        private void OnLK_Down(object sender, MouseButtonEventArgs e)
        {
            LoginWindow lw = new LoginWindow();
            if (lw.ShowDialog() == true)
            {
                //for (int i = 0; i < dbUsers.Count; i++)
                //{
                //    if ((lw.userLogin == dbUsers[i].UserLogin) && (lw.userPassword == dbUsers[i].Password))
                //    {
                //        switch (dbUsers[i].Rule)
                //        {
                //            case 1:
                //                currentUser = new Consultant(dbUsers[i].UserLogin, dbUsers[i].Password);
                //                break;
                //            case 2:
                //                currentUser = new Manager(dbUsers[i].UserLogin, dbUsers[i].Password);
                //                break;
                //            case 3:
                //                currentUser = new Administrator(dbUsers[i].UserLogin, dbUsers[i].Password);
                //                break;
                //            default:
                //                currentUser = dbUsers[i];
                //                break;
                //        }
                //        userName.Content = currentUser.UserLogin;
                //        IUserRights userRights = currentUser as IUserRights;
                //        if (userRights != null)
                //        {
                //            btnAdd.IsEnabled = userRights.IsAddingPerson();
                //            btnDel.IsEnabled = userRights.IsAddingPerson();
                //            btnEdit.IsEnabled = true;
                //            btnEditUsers.Visibility = userRights.IsEditingUser() ? Visibility.Visible : Visibility.Hidden;
                //            btnViewLog.Visibility = userRights.IsEditingUser() ? Visibility.Visible : Visibility.Hidden;
                //        }
                //        else
                //        {
                //            btnAdd.IsEnabled = false;
                //            btnDel.IsEnabled = false;
                //            btnEdit.IsEnabled = false;
                //            btnEditUsers.Visibility = Visibility.Hidden;
                //            btnViewLog.Visibility = Visibility.Hidden;
                //        }
                //        return;
                //    }
                //}
                MessageBox.Show("Ошибка ввода логина и/или пароля !!!");
            }
        }

        private void OnAdminDown(object sender, MouseButtonEventArgs e)
        {
            LoginWindow lw = new LoginWindow();
            if (lw.ShowDialog() == true)
            {
                UserData loginUser = null;
                foreach (UserData ud in users.AllItems)
                {
                    if (ud.CheckUser(lw.userLogin, lw.userPassword))
                    {
                        loginUser = ud;
                        break;
                    }
                }
                if (loginUser != null)
                {
                    if (loginUser.Rule != 0)
                    {
                        CreateCurrentUser(loginUser);
                        nameUser.Content = currentUser.UserLogin;
                        userSP.Visibility = Visibility.Visible;
                        borderAdmin.Visibility = Visibility.Hidden;
                        personSP.Visibility = Visibility.Hidden;
                        borderLK.Visibility = Visibility.Visible;
                        currentPerson = null;
                        ShowPanel(true);
                    }
                    else
                    {
                        MessageBox.Show("Были введены корректные данные клиента а не сотрудника банка !!!");
                    }
                    return;
                }
                MessageBox.Show("Ошибка ввода логина и/или пароля !!!");
            }
        }

        private void OnAdminEnter(object sender, MouseEventArgs e)
        {
            backAdmin = borderAdmin.Background;
            borderAdmin.Background = Brushes.Cyan;
        }

        private void OnAdminLeave(object sender, MouseEventArgs e)
        {
            borderAdmin.Background = backAdmin;
        }

        private void AppClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            users.SaveRepositoryToFile(pathUserXML);
            persons.SaveRepositoryToFile(pathPersonXML);
            products.SaveRepositoryToFile(pathProductXML);
            //products.SaveRepositoryToFileForCusomSerializer(pathProductXML, new XmlSerializer(typeof(ObservableCollection<Product>), "account", "deposit", "card", "credit"));
        }

        private void OnUserExit(object sender, RoutedEventArgs e)
        {
            userSP.Visibility = Visibility.Hidden;
            borderAdmin.Visibility = Visibility.Visible;
            currentUser = null;
        }

        private void OnPersonExit(object sender, RoutedEventArgs e)
        {
            personSP.Visibility = Visibility.Hidden;
            borderLK.Visibility = Visibility.Visible;
            currentPerson = null;
        }

        private void OnViewLog(object sender, RoutedEventArgs e)
        {
            if (File.Exists(pathUpdateInfo))
            {
                string[] arrStr = File.ReadAllLines(pathUpdateInfo);
                List<MyLogView> list = new List<MyLogView>();
                for (int i = 0; i < arrStr.Length; i++)
                {
                    list.Add(new MyLogView(arrStr[i]));
                }
                ViewLogWindow vlw = new ViewLogWindow();
                vlw.SetSource(list);
                vlw.ShowDialog();
            }
        }

        private void OnViewProduct(object sender, RoutedEventArgs e)
        {
            ViewProductsWindow vpw = new ViewProductsWindow();
            vpw.SetProducts(products);
            if (vpw.ShowDialog() == true)
            {

            }
        }

        private void OnViewWorker(object sender, RoutedEventArgs e)
        {
            UserEditWindow uew = new UserEditWindow();
            uew.SetUsers(users);
            if (uew.ShowDialog() == true)
            {

            }
        }

        private void OnViewPerson(object sender, RoutedEventArgs e)
        {
            PersonsViewWindow psw = new PersonsViewWindow();
            psw.SetUser(currentUser);
            psw.SetRepository(persons, users);
            psw.SetPathUpdateInfoFile(pathUpdateInfo);
            psw.ShowDialog();
        }
    }
}
