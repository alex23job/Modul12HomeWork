using System;
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
        Brush backLK = null;
        Brush backAdmin = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLK_Enter(object sender, MouseEventArgs e)
        {
            backLK = borderLK.Background;
            borderLK.Background = Brushes.Cyan;
        }

        private void OnLK_Leave(object sender, MouseEventArgs e)
        {
            borderLK.Background = backLK;
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
    }
}
