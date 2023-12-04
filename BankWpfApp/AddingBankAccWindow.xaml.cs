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
    /// Логика взаимодействия для AddingBankAccWindow.xaml
    /// </summary>
    public partial class AddingBankAccWindow : Window
    {
        Repository<Product> products = null;
        Repository<Product> bankAccounts = null;
        Person currPerson = null;

        public AddingBankAccWindow()
        {
            InitializeComponent();
        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            if (listViewAcc.SelectedItem != null)
            {
                AccountViewData av = listViewAcc.SelectedItem as AccountViewData;
                if (av != null)
                {
                    if (MessageBox.Show($"Будет открыт счёт : {av.Name}\n\nОткрыть счёт ?", $"Открытие счёта для клиента {currPerson.PersonLogin}", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        BankAccount ba = bankAccounts.Add(new BankAccount()) as BankAccount;
                        ba.personUID = currPerson.UID;
                        ba.PersonProductNumber = Product.GetNextPersonProductNumber();
                        ba.Name = av.Name;
                        ba.TypeAccount = Account.GetNumType(av.NameType);
                        currPerson.IdProducts.Add(ba.PersonProductNumber);
                    }
                }
                DialogResult = true;
            }
        }

        public void SetRepositoty(Repository<Product> prod, Repository<Product> ba)
        {
            bankAccounts = ba;
            products = prod;
            ObservableCollection<AccountViewData> arr = new ObservableCollection<AccountViewData>();
            foreach (Product pr in products.AllItems)
            {
                Account acc = pr as Account;
                if (acc != null)
                {
                    arr.Add(new AccountViewData(acc.Name, Account.nameTypeAccount[acc.TypeAccount]));
                }
            }
            listViewAcc.ItemsSource = arr;
        }

        public void SetPerson(Person pers)
        {
            currPerson = pers;
        }
    }

    public class AccountViewData
    {
        public string Name { get; set; }
        public string NameType { get; set; }
        public AccountViewData() { }
        public AccountViewData(string nm, string nmt)
        {
            Name = nm;
            NameType = nmt;
        }
    }
}
