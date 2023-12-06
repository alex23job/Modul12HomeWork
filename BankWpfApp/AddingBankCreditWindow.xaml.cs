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
    /// Логика взаимодействия для AddingBankCreditWindow.xaml
    /// </summary>
    public partial class AddingBankCreditWindow : Window
    {
        Repository<Product> products = null;
        Repository<Product> bankProducts = null;
        Person currPerson = null;
        public AddingBankCreditWindow()
        {
            InitializeComponent();
        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            if (listViewCredit.SelectedItem != null)
            {
                CreditViewData av = listViewCredit.SelectedItem as CreditViewData;
                if (av != null)
                {
                    if (MessageBox.Show($"Будет оформлена заявка на кредит : {av.Name}\n\nОформить заявку ?", $"Оформление заявки на кпедит для клиента {currPerson.PersonLogin}", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        BankCredit bp = bankProducts.Add(new BankCredit()) as BankCredit;
                        bp.personUID = currPerson.UID;
                        bp.PersonProductNumber = Product.GetNextPersonProductNumber();
                        bp.Name = av.Name;
                        bp.TypeCredit = Credit.GetNumType(av.NameType);
                        currPerson.IdProducts.Add(bp.PersonProductNumber);
                        bp.IsRequest = true;
                        foreach (Product pr in products.AllItems)
                        {
                            Credit cd = pr as Credit;
                            if (cd != null && cd.Name == av.Name)
                            {
                                bp.CopyParamsProduct(cd);
                            }
                        }
                        RequestCreditWindow rcw = new RequestCreditWindow();
                        rcw.SetCredit(bp);
                        if (rcw.ShowDialog() == true)
                        {

                        }
                        else return;
                    }
                }
                DialogResult = true;
            }
        }
        public void SetRepositoty(Repository<Product> prod, Repository<Product> bp)
        {
            bankProducts = bp;
            products = prod;
            ObservableCollection<CreditViewData> arr = new ObservableCollection<CreditViewData>();
            foreach (Product pr in products.AllItems)
            {
                Credit acc = pr as Credit;
                if (acc != null)
                {
                    arr.Add(new CreditViewData(acc.Name, Credit.nameTypeCredit[acc.TypeCredit]));
                }
            }
            listViewCredit.ItemsSource = arr;
        }

        public void SetPerson(Person pers)
        {
            currPerson = pers;
        }
    }

    public class CreditViewData
    {
        public string Name { get; set; }
        public string NameType { get; set; }
        public CreditViewData() { }
        public CreditViewData(string nm, string nmt)
        {
            Name = nm;
            NameType = nmt;
        }
    }
}
