using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для PersonsViewWindow.xaml
    /// </summary>
    public partial class PersonsViewWindow : Window
    {
        Repository<Person> persons = null;
        Repository<UserData> users = null;
        UserData user = null;
        string pathUpdateInfo = "";
        public PersonsViewWindow()
        {
            InitializeComponent();
        }

        public void SetPathUpdateInfoFile(string path)
        {
            pathUpdateInfo = path;
        }
        public void SetRepository(Repository<Person> pers, Repository<UserData> urs)
        {
            persons = pers;
            users = urs;
            dataGrid.ItemsSource = persons.AllItems;
        }
        
        public void SetUser(UserData ud)
        {
            switch (ud.Rule)
            {
                case 1:
                    user = new Consultant(ud.UserLogin, ud.Password);
                    break;
                case 2:
                    user = new Manager(ud.UserLogin, ud.Password);
                    break;
                case 3:
                    user = new Administrator(ud.UserLogin, ud.Password);
                    break;
                default:
                    user = ud;
                    break;
            }
            IUserRights userRights = user as IUserRights;
            if (userRights != null)
            {
                AddPerson.IsEnabled = userRights.IsAddingPerson();
                DelPerson.IsEnabled = userRights.IsAddingPerson();
                EditPerson.IsEnabled = true;
            }
            else
            {
                AddPerson.IsEnabled = false;
                DelPerson.IsEnabled = false;
                EditPerson.IsEnabled = false;
            }
        }

        private void OnDataGridMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = e.OriginalSource as TextBlock;
            if (tb != null)
            {
                Person pers = tb.DataContext as Person;
                if (pers != null)
                {
                    dataGrid.SelectedItem = pers;

                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem menuItemEdit = new MenuItem();
                    menuItemEdit.Header = "Изменить пароль клиента";
                    menuItemEdit.Click += (send, args) => {
                        ChangePassword(pers);
                    };
                    contextMenu.Items.Add(menuItemEdit);
                    MenuItem menuItemDel = new MenuItem();
                    menuItemDel.Header = "Удалить запись клиента";
                    menuItemDel.Click += (send, args) => {
                        RemovePerson(pers);
                    };
                    contextMenu.Items.Add(menuItemDel);
                    contextMenu.IsOpen = true;
                }
            }
        }

        private void ChangePassword(Person pers)
        {
            ChangePasswordWindow cpw = new ChangePasswordWindow();
            Person editPers = persons[pers.UID];
            UserData editUser = users[pers.UserUID];
            if (editUser != null)
            {
                cpw.SetUser(editUser);
                if (cpw.ShowDialog() == true)
                {
                    editPers.UpdateUserPassword(cpw.GetUser().Password);
                    editUser.Password = cpw.GetUser().Password;
                    editPers.updateInfo = new LogPersonUpdate("Пароль", "change", user.UserLogin, UserPosition.GetPosition(user.Rule), editPers.UID.ToString());
                    using (StreamWriter outputFile = new StreamWriter(pathUpdateInfo, true))
                    {
                        outputFile.WriteLine(editPers.updateInfo.ToCsvString());
                    }
                }
            }
        }

        private void OnAddPerson(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWin = new RegistrationWindow();
            if (regWin.ShowDialog() == true)
            {
                UserData ud = users.Add(regWin.GetUser());
                Person per = regWin.GetPerson();
                per.SetUserData(ud);
                SaveUpdateInfo("add", persons.Add(per));
                dataGrid.ItemsSource = persons.AllItems;
                dataGrid.Items.Refresh();
            }
        }

        private void OnEditPerson(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedValue == null) return;
            Person pers = dataGrid.SelectedValue as Person;
            if (pers != null)
            {
                //MessageBox.Show(pers.ToString());
                PersonEditWindow pew = new PersonEditWindow();
                pew.SetParam(pers, user);
                if (pew.ShowDialog() == true)
                {
                    Person curPers = persons[pers.UID];
                    SaveUpdateInfo("edit", curPers, pew.GetPerson());
                    curPers.CopyEditParams(pew.GetPerson());
                    dataGrid.ItemsSource = persons.AllItems;
                    dataGrid.Items.Refresh();
                }
            }            
        }

        private void OnDelPerson(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedValue == null) return;
            Person pers = dataGrid.SelectedValue as Person;
            RemovePerson(pers);
        }

        private void RemovePerson(Person pers)
        {
            string info = string.Format("\"Фамилия: {0}   тлф: {1}\"", pers.Name, pers.Tlf);
            if (MessageBox.Show($"Выбрана запись клиента : {info}\n\nУдалить запись ?", "Удаление записи клиента", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Person delPers = persons[pers.UID];
                SaveUpdateInfo("del", delPers);
                UserData delUser = users[delPers.UserUID];
                users.DelItem(delUser);
                persons.DelItem(delPers);
                dataGrid.ItemsSource = persons.AllItems;
                dataGrid.Items.Refresh();
            }
        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void SaveUpdateInfo(string mode, Person oldPer, Person newPer = null)
        {
            if (newPer == null)
            {
                oldPer.updateInfo = new LogPersonUpdate("Все поля", mode, user.UserLogin, UserPosition.GetPosition(user.Rule), oldPer.UID.ToString());
            }
            else
            {
                if (oldPer.Name != newPer.Name)
                {
                    newPer.updateInfo = new LogPersonUpdate("Фамилия", mode, user.UserLogin, UserPosition.GetPosition(user.Rule), newPer.UID.ToString());
                }
                if (oldPer.LastName != newPer.LastName)
                {
                    if (newPer.updateInfo == null)
                    {
                        newPer.updateInfo = new LogPersonUpdate("Имя", mode, user.UserLogin, UserPosition.GetPosition(user.Rule), newPer.UID.ToString());
                    }
                    else
                    {
                        newPer.updateInfo.AddField("Имя");
                    }
                }
                if (oldPer.SecondName != newPer.SecondName)
                {
                    if (newPer.updateInfo == null)
                    {
                        newPer.updateInfo = new LogPersonUpdate("Отчество", mode, user.UserLogin, UserPosition.GetPosition(user.Rule), newPer.UID.ToString());
                    }
                    else
                    {
                        newPer.updateInfo.AddField("Отчество");
                    }
                }
                if (oldPer.Pasport != newPer.Pasport)
                {
                    if (newPer.updateInfo == null)
                    {
                        newPer.updateInfo = new LogPersonUpdate("Паспорт", mode, user.UserLogin, UserPosition.GetPosition(user.Rule), newPer.UID.ToString());
                    }
                    else
                    {
                        newPer.updateInfo.AddField("Паспорт");
                    }
                }
                if (oldPer.Tlf != newPer.Tlf)
                {
                    if (newPer.updateInfo == null)
                    {
                        newPer.updateInfo = new LogPersonUpdate("Телефон", mode, user.UserLogin, UserPosition.GetPosition(user.Rule), newPer.UID.ToString());
                    }
                    else
                    {
                        newPer.updateInfo.AddField("Телефон");
                    }
                }
                if (oldPer.BirthDay != newPer.BirthDay)
                {
                    if (newPer.updateInfo == null)
                    {
                        newPer.updateInfo = new LogPersonUpdate("День рождения", mode, user.UserLogin, UserPosition.GetPosition(user.Rule), newPer.UID.ToString());
                    }
                    else
                    {
                        newPer.updateInfo.AddField("День рождения");
                    }
                }
            }
            using (StreamWriter outputFile = new StreamWriter(pathUpdateInfo, true))
            {
                if (newPer == null)
                {
                    outputFile.WriteLine(oldPer.updateInfo.ToCsvString());
                }
                else if (newPer.updateInfo != null)
                {
                    outputFile.WriteLine(newPer.updateInfo.ToCsvString());
                }
            }
            if (newPer != null && newPer.updateInfo == null)
            {
                newPer.updateInfo = oldPer.updateInfo;
            }
        }
    }
}
