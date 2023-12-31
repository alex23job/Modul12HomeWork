﻿using System;
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
        public static string startupPath = AppDomain.CurrentDomain.BaseDirectory;
        public static string logoImgPath = "LogoImg";

        public static void SaveStrUpdateInfo(string csvStr, string path = "UpdateInfoLog.csv")
        {
            using (StreamWriter outputFile = new StreamWriter(path, true))
            {
                outputFile.WriteLine(csvStr);
            }
        }

        string pathUpdateInfo = "UpdateInfoLog.csv";
        string pathLogOperations = "LogOperations.csv";
        private LogOperations logOperations = null;

        string pathPersonXML = "Person.xml";
        Repository<Person> persons = new Repository<Person>();

        string pathUserXML = "User.xml";
        Repository<UserData> users = new Repository<UserData>();

        string pathProductXML = "Product.xml";
        Repository<Product> products = new Repository<Product>();

        string pathBankProductXML = "BankProduct.xml";
        Repository<Product> bankProducts = new Repository<Product>();

        string pathTransactionXML = "Transaction.xml";
        Repository<Transaction> transactions = new Repository<Transaction>();

        Person currentPerson = null;
        UserData currentUser = null;
        Brush backLK = null;
        Brush backAdmin = null;
        public MainWindow()
        {
            InitializeComponent();
            CreateContextMenuLK();
            CreateRepositorys();
            //MessageBox.Show(startupPath);
        }

        private void CreateRepositorys()
        {
            logOperations = new LogOperations(pathLogOperations);

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
            SynchronizePersonUsers();

            if (File.Exists(pathProductXML))
            {
                products = Repository<Product>.LoadRepositoryFromFile(pathProductXML);
            }
            products.SetSavePath(pathProductXML);
            products.SetCurrentNewUID(10);

            if (File.Exists(pathBankProductXML))
            {
                bankProducts = Repository<Product>.LoadRepositoryFromFile(pathBankProductXML);
            }
            bankProducts.SetSavePath(pathBankProductXML);
            // TODO
            bankProducts.SetCurrentNewUID(1000000);
            //Product.SetNextPersonProductNumber(1000000 + bankProducts.Count);
            Product.SetNextPersonProductNumber(1 + CalcMaxPersonProductNumber(1000000 + bankProducts.Count));
            SynchronizeAccounts();

            if (File.Exists(pathTransactionXML))
            {
                transactions = Repository<Transaction>.LoadRepositoryFromFile(pathTransactionXML);
            }
            transactions.SetSavePath(pathTransactionXML);
            transactions.SetCurrentNewUID(0);
        }

        private long CalcMaxPersonProductNumber(long zn)
        {
            long maxRes = zn;
            foreach(Product pr in bankProducts.AllItems)
            {
                IPersonProductNumber ipr = pr as IPersonProductNumber;
                if (ipr != null)
                {
                    if (ipr.PersonProductNumber > maxRes)
                    {
                        maxRes = ipr.PersonProductNumber;
                    }
                }
            }
            return maxRes;
        }

        private void SynchronizePersonUsers()
        {
            int i, j;
            for (i = 0; i < persons.Count; i++)
            {
                Person p = persons.AllItems[i];
                for (j = 0; j < users.Count; j++)
                {
                    UserData user = users.AllItems[j];
                    if (p.UserUID == user.UID)
                    {
                        p.SetUserData(user);
                        break;
                    }
                }
            }
        }

        private void SynchronizeAccounts()
        {
            int i, j;
            for (i = 0; i < bankProducts.Count; i++)
            {
                BankCard bc = bankProducts.AllItems[i] as BankCard;
                if (bc != null && bc.CardAccount != null)
                {
                    for (j = 0; j < bankProducts.Count; j++)
                    {
                        BankAccount ba = bankProducts.AllItems[j] as BankAccount;
                        if (ba != null && ba.PersonProductNumber == bc.CardAccount.PersonProductNumber)
                        {
                            bc.CardAccount = ba;
                            break;
                        }
                    }
                }

                BankDeposit bd = bankProducts.AllItems[i] as BankDeposit;
                if (bd != null && bd.DepositAccount != null)
                {
                    for (j = 0; j < bankProducts.Count; j++)
                    {
                        BankAccount ba = bankProducts.AllItems[j] as BankAccount;
                        if (ba != null && ba.PersonProductNumber == bd.DepositAccount.PersonProductNumber)
                        {
                            bd.DepositAccount = ba;
                            break;
                        }
                    }
                }

                BankCredit bcr = bankProducts.AllItems[i] as BankCredit;
                if (bcr != null && bcr.CreditAccount != null)
                {
                    for (j = 0; j < bankProducts.Count; j++)
                    {
                        BankAccount ba = bankProducts.AllItems[j] as BankAccount;
                        if (ba != null && ba.PersonProductNumber == bcr.CreditAccount.PersonProductNumber)
                        {
                            bcr.CreditAccount = ba;
                            break;
                        }
                    }
                }
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
                    LogPersonUpdate lpu = new LogPersonUpdate("Все поля", "add", per.PersonLogin, "Клиент", per.UID.ToString());
                    per.updateInfo = lpu;
                    MainWindow.SaveStrUpdateInfo(lpu.ToCsvString(), pathUpdateInfo);
                    currentPerson = persons.Add(per);
                    ShowPersonSP();
                }
            };
            borderLK.ContextMenu.Items.Add(menuItemRegistration);

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
            ObservableCollection<BankAccount> accList = new ObservableCollection<BankAccount>();
            ObservableCollection<BankCard> cardList = new ObservableCollection<BankCard>();
            ObservableCollection<BankDeposit> deposList = new ObservableCollection<BankDeposit>();
            ObservableCollection<BankCredit> creditList = new ObservableCollection<BankCredit>();

            for (int i = 0; i < currentPerson.IdProducts.Count; i++) 
            {
                long id = currentPerson.IdProducts[i];
                for (int j = 0; j < bankProducts.Count; j++)
                {
                    BankAccount ba = bankProducts.AllItems[j] as BankAccount;
                    if (ba != null && ba.PersonProductNumber == id)
                    {
                        accList.Add(ba);
                        break;
                    }

                    BankCard bc = bankProducts.AllItems[j] as BankCard;
                    if (bc != null && bc.PersonProductNumber == id)
                    {
                        cardList.Add(bc);
                        break;
                    }

                    BankDeposit bd = bankProducts.AllItems[j] as BankDeposit;
                    if (bd != null && bd.PersonProductNumber == id)
                    {
                        deposList.Add(bd);
                        break;
                    }

                    BankCredit bcr = bankProducts.AllItems[j] as BankCredit;
                    if (bcr != null && bcr.PersonProductNumber == id)
                    {
                        creditList.Add(bcr);
                        break;
                    }
                }
            }

            listViewAcc.ItemsSource = accList;
            listViewCard.ItemsSource = cardList;
            listViewDeposit.ItemsSource = deposList;
            listViewCredit.ItemsSource = creditList;
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
            bankProducts.SaveRepositoryToFile(pathBankProductXML);
            transactions.SaveRepositoryToFile(pathTransactionXML);
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

        private void OnAddBankAccClick(object sender, RoutedEventArgs e)
        {
            if (currentPerson == null) return;
            AddingBankAccWindow abaw = new AddingBankAccWindow();
            abaw.SetRepositoty(products, bankProducts);
            abaw.SetPerson(currentPerson);
            if (abaw.ShowDialog() == true)
            {
                UpdatePersonsPanels();
            }
        }

        private void OnAddBankCardClick(object sender, RoutedEventArgs e)
        {
            if (currentPerson == null) return;
            AddingBankCardWindow abcw = new AddingBankCardWindow();
            abcw.SetRepositoty(products, bankProducts);
            abcw.SetPerson(currentPerson);
            if (abcw.ShowDialog() == true)
            {
                UpdatePersonsPanels();
            }
        }

        private void OnAddBankDepositClick(object sender, RoutedEventArgs e)
        {
            if (currentPerson == null) return;
            AddingBankDepositWindow abdw = new AddingBankDepositWindow();
            abdw.SetRepositoty(products, bankProducts);
            abdw.SetPerson(currentPerson);
            if (abdw.ShowDialog() == true)
            {
                UpdatePersonsPanels();
            }
        }

        private void OnAddBankCreditClick(object sender, RoutedEventArgs e)
        {
            if (currentPerson == null) return;
            AddingBankCreditWindow abcw = new AddingBankCreditWindow();
            abcw.SetRepositoty(products, bankProducts);
            abcw.SetPerson(currentPerson);
            if (abcw.ShowDialog() == true)
            {
                UpdatePersonsPanels();
            }
        }

        private void OnOperationsClick(object sender, RoutedEventArgs e)
        {
            if (currentPerson == null && currentUser == null) return;
            StatOperationsWindow sow = new StatOperationsWindow();
            sow.SetRepository(persons.AllItems, bankProducts.AllItems);
            if (currentPerson != null) sow.SetPerson(currentPerson);
            if (currentUser != null) sow.SetUser(currentUser);
            sow.ShowDialog();
        }

        private void OnTotalClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnPaymentsClick(object sender, RoutedEventArgs e)
        {
            if (currentPerson != null)
            {
                ActionsWindow aw = new ActionsWindow();
                aw.SetPersons(currentPerson, persons.AllItems, bankProducts.AllItems);
                if (aw.ShowDialog() == true)
                {
                    Transaction tr = aw.transaction;
                    if (tr.NameMode == "pay")
                    {
                        tr.PayExecute(persons.AllItems, bankProducts.AllItems);
                    }
                    else tr.Execute();
                    string fr = tr.From, to = tr.To;
                    OneOperation op = new OneOperation(tr.sum.ToString(), tr.NameMode, currentPerson.UserUID.ToString(), "Клиент", currentPerson.UID.ToString(), fr, to);
                    logOperations.SaveOneOption(op, pathLogOperations);
                    transactions.Add(tr);
                    UpdatePersonsPanels();
                }
            }
            if (currentUser != null)
            {
                ReviewRequestsWindow rrw = new ReviewRequestsWindow();
                rrw.SetParams(persons.AllItems, bankProducts, currentUser, logOperations);
                rrw.ShowDialog();
            }
        }

        private void OnBonusClick(object sender, RoutedEventArgs e)
        {
            if (currentPerson == null) return;
            BonusWindow bw = new BonusWindow();
            bw.SetPerson(currentPerson, persons.AllItems);
            bw.ShowDialog();
        }

        private void OnNamePersonMouseUp(object sender, MouseButtonEventArgs e)
        {
            RegistrationWindow regWin = new RegistrationWindow();
            regWin.SetPerson(currentPerson);
            if (regWin.ShowDialog() == true)
            {
/*                UserData ud = users.Add(regWin.GetUser());
                Person per = regWin.GetPerson();
                per.SetUserData(ud);
                currentPerson = persons.Add(per);
                ShowPersonSP();*/
            }
        }

        private void OnDelAcc(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(e.OriginalSource.ToString());
            Button btn = e.OriginalSource as Button;
            if (btn != null)
            { 
                BankAccount ba = btn.DataContext as BankAccount;
                if (ba != null)
                {
                    if (ba.Balans > 0)
                    {
                        MessageBox.Show($"На счёте {ba.StrBalance}. Переведите деньги чтобы закрыть счёт.");
                        return;
                    }
                    if (currentPerson.IdProducts.Count == 1 && currentPerson is LegalPerson)
                    {
                        MessageBox.Show("Нельзя закрыть последний расчётный счёт юридического лица. Он будет удалён вместе с удалением юр.лица.");
                        return;
                    }
                    if (ba.TypeAccount == 0)
                    {
                        foreach(long id in currentPerson.IdProducts)
                        {
                            foreach(Product bp in bankProducts.AllItems)
                            {
                                BankCard bc = bp as BankCard;
                                if (bc != null && bc.CardAccount != null)
                                {
                                    if (bc.CardAccount.PersonProductNumber == ba.PersonProductNumber && bc.PersonProductNumber == id)
                                    {
                                        MessageBox.Show($"Этот счёт привязан к Вашей банковской карте {bc.StrNumber} и может быть удалён только при её удалении.");
                                        return;
                                    }
                                }
                            }
                        }
                        if (MessageBox.Show($"Будет удалён расчётный счёт {ba.StrNumber}. Удалить ?", "Удаление расчётного счёта", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            currentPerson.IdProducts.Remove(ba.PersonProductNumber);
                            bankProducts.DelItem(ba);
                            UpdatePersonsPanels();
                        }  
                    }

                }                
            }
        }
    }
}
