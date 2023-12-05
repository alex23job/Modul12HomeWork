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
    /// Логика взаимодействия для AddingBankDepositWindow.xaml
    /// </summary>
    public partial class AddingBankDepositWindow : Window
    {
        Repository<Product> products = null;
        Repository<Product> bankProducts = null;
        Person currPerson = null;
        public AddingBankDepositWindow()
        {
            InitializeComponent();
        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            if (listViewDeps.SelectedItem != null)
            {
                DepositViewData av = listViewDeps.SelectedItem as DepositViewData;
                if (av != null)
                {
                    if (MessageBox.Show($"Будет открыт вклад : {av.Name}\n\nОткрыть вклад ?", $"Открытие вклада для клиента {currPerson.PersonLogin}", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        BankDeposit bd = bankProducts.Add(new BankDeposit()) as BankDeposit;
                        bd.personUID = currPerson.UID;
                        bd.PersonProductNumber = Product.GetNextPersonProductNumber();
                        bd.Name = av.Name;
                        currPerson.IdProducts.Add(bd.PersonProductNumber);
                        foreach (Product pr in products.AllItems)
                        {
                            Deposit dep = pr as Deposit;
                            if (dep != null && dep.Name == av.Name)
                            {
                                bd.CopyParamsProduct(dep);
                            }
                        }
                        bd.DepositAccount = bankProducts.Add(new BankAccount(1, currPerson.UID)) as BankAccount;
                        bd.DepositAccount.PersonProductNumber = Product.GetNextPersonProductNumber();
                        currPerson.IdProducts.Add(bd.DepositAccount.PersonProductNumber);
                    }
                }
                DialogResult = true;
            }
        }

        public void SetRepositoty(Repository<Product> prod, Repository<Product> bp)
        {
            bankProducts = bp;
            products = prod;
            ObservableCollection<DepositViewData> arr = new ObservableCollection<DepositViewData>();
            foreach (Product pr in products.AllItems)
            {
                Deposit dep = pr as Deposit;
                if (dep != null)
                {
                    string depTopUp = dep.IsCanTopUp ? "Да" : "Нет";
                    string depWithdraw = dep.IsCanWithdraw ? "Да" : "Нет";
                    string depInfo = $"{dep.percent} %   Пополнение - {depTopUp}/Снятие - {depWithdraw}";
                    arr.Add(new DepositViewData(dep.Name, depInfo));
                }
            }
            listViewDeps.ItemsSource = arr;
        }

        public void SetPerson(Person pers)
        {
            currPerson = pers;
        }
    }

    public class DepositViewData
    {
        public string Name { get; set; }
        public string DepInfo { get; set; }
        public DepositViewData() { }
        public DepositViewData(string nm, string info)
        {
            Name = nm;
            DepInfo = info;
        }
    }
}
