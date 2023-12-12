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
    /// Логика взаимодействия для ReviewRequestsWindow.xaml
    /// </summary>
    public partial class ReviewRequestsWindow : Window
    {
        UserData user = null;
        LogOperations logOps = null;
        Repository<Product> bankProducts = null;
        ObservableCollection<Person> persons = null;
        ObservableCollection<Product> products = null;
        public ReviewRequestsWindow()
        {
            InitializeComponent();
        }

        public void SetParams(ObservableCollection<Person> pers, Repository<Product> prod, UserData us, LogOperations log)
        {
            persons = pers;
            logOps = log;
            bankProducts = prod;
            products = bankProducts.AllItems;

            listViewBankProducts.ItemsSource = GetListProduct();
        }

        private List<RequestsProduct> GetListProduct()
        {
            List<RequestsProduct> list = new List<RequestsProduct>();
            foreach (Product pr in products)
            {
                BankCard bc = pr as BankCard;
                if (bc != null)
                {
                    if (bc.IsRequest)
                    {
                        Person person = GetPerson(bc.personUID);
                        if (person != null)
                        {
                            string nm = $"{person.Name} {person.LastName[0]}.{person.SecondName[0]}.";
                            list.Add(new RequestsProduct(bc.UID, nm, $"Карта {bc.Name}"));
                        }
                    }
                }
                BankCredit bcr = pr as BankCredit;
                if (bcr != null)
                {
                    if (bcr.IsRequest)
                    {
                        Person person = GetPerson(bcr.personUID);
                        if (person != null)
                        {
                            string nm = $"{person.Name} {person.LastName[0]}.{person.SecondName[0]}.";
                            list.Add(new RequestsProduct(bcr.UID, nm, $"Кредит {bcr.Name}"));
                        }
                    }
                }
            }
            return list;
        }

        private Person GetPerson(int id)
        {
            foreach(Person pers in persons)
            {
                if (pers.UID == id)
                {
                    return pers;
                }
            }
            return null;
        }

        private void OnListViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewBankProducts.SelectedItem != null)
            {
                RequestsProduct rp = listViewBankProducts.SelectedItem as RequestsProduct;
                if (rp != null)
                {
                    //MessageBox.Show($"{rp.PersonName} {rp.ProductType} {rp.ProductUID}");
                    foreach(Product p in products)
                    {
                        if (p.UID == rp.ProductUID)
                        {
                            listViewInfo.ItemsSource = p.GetProductInfo();
                        }
                    }
                }
            }
        }

        private void OnEndorseClick(object sender, RoutedEventArgs e)
        {
            if (listViewBankProducts.SelectedItem != null)
            {
                RequestsProduct rp = listViewBankProducts.SelectedItem as RequestsProduct;
                if (rp != null)
                {
                    foreach (Product p in products)
                    {
                        if (p.UID == rp.ProductUID)
                        {
                            if (rp.ProductType.StartsWith("Кар"))
                            {
                                BankCard bc = p as BankCard;
                                if (bc != null)
                                {
                                    Person pers = GetPerson(bc.personUID);
                                    if (pers != null)
                                    {
                                        pers.IdProducts.Remove(bc.PersonProductNumber);
                                        bc.CardAccount = bankProducts.Add(new BankAccount(bc.TypeCard % 2, pers.UID)) as BankAccount;
                                        bc.CardAccount.PersonProductNumber = Product.GetNextPersonProductNumber();
                                        pers.IdProducts.Add(bc.CardAccount.PersonProductNumber);
                                        bc.IsRequest = false;
                                        if (bc.TypeCard > 0)
                                        {
                                            bc.CardAccount.Balans = bc.Limit;
                                            logOps.SaveOneOption(new OneOperation(bc.Limit.ToString(), "inc", user.UID.ToString(),
                                                UserPosition.GetPosition(user.Rule), pers.UID.ToString(), "Банк", bc.CardAccount.PersonProductNumber.ToString()));
                                        }
                                    }
                                    break;
                                }
                            }
                            //if (rp.ProductType.StartsWith("Кре"))
                            //{
                            //    BankCredit bcr = p as BankCredit;
                            //    if (bcr != null)
                            //    {
                            //        Person pers = GetPerson(bcr.personUID);
                            //        if (pers != null)
                            //        {
                            //            pers.IdProducts.Remove(bcr.PersonProductNumber);
                            //        }
                            //        products.Remove(bcr);
                            //        break;
                            //    }
                            //}
                        }
                    }
                    listViewBankProducts.ItemsSource = GetListProduct();
                }
            }

        }

        private void OnDeflectClick(object sender, RoutedEventArgs e)
        {
            if (listViewBankProducts.SelectedItem != null)
            {
                RequestsProduct rp = listViewBankProducts.SelectedItem as RequestsProduct;
                if (rp != null)
                {
                    //MessageBox.Show($"{rp.PersonName} {rp.ProductType} {rp.ProductUID}");
                    foreach (Product p in products)
                    {
                        if (p.UID == rp.ProductUID)
                        {
                            if (rp.ProductType.StartsWith("Кар"))
                            {
                                BankCard bc = p as BankCard;
                                if (bc != null)
                                {
                                    Person pers = GetPerson(bc.personUID);
                                    if (pers != null)
                                    {
                                        pers.IdProducts.Remove(bc.PersonProductNumber);
                                    }
                                    products.Remove(bc);
                                    break;
                                }
                            }
                            if (rp.ProductType.StartsWith("Кре"))
                            {
                                BankCredit bcr = p as BankCredit;
                                if (bcr != null)
                                {
                                    Person pers = GetPerson(bcr.personUID);
                                    if (pers != null)
                                    {
                                        pers.IdProducts.Remove(bcr.PersonProductNumber);
                                    }
                                    products.Remove(bcr);
                                    break;
                                }
                            }
                        }
                    }
                    listViewBankProducts.ItemsSource = GetListProduct();
                }
            }
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {

        }
    }

    public class RequestsProduct
    {
        public int ProductUID { get; set; }
        public string PersonName { get; set; }
        public string ProductType { get; set; }

        public RequestsProduct() { }
        public RequestsProduct(int id, string persName, string prodType)
        {
            ProductUID = id;
            PersonName = persName;
            ProductType = prodType;
        }
    }
}
