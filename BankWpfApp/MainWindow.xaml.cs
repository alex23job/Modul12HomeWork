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

namespace BankWpfApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string pathPersonXML = "Person.xml";
        Repository<Person> persons = new Repository<Person>();

        string pathUserXML = "User.xml";
        Repository<UserData> users = new Repository<UserData>();

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
                                if (per.CheckUserUID(loginUser.UID))
                                {
                                    currentPerson = per;
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
                    }
                    else
                    {
                        MessageBox.Show("Были введены корректные данные клиента а не сотрудника банка !!!");
                    }
                    return;
                }
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
        }
    }
}
