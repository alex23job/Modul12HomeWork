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
        ObservableCollection<Person> persons = null;
        ObservableCollection<Product> products = null;
        public ReviewRequestsWindow()
        {
            InitializeComponent();
        }

        public void SetParams(ObservableCollection<Person> pers, ObservableCollection<Product> prod, UserData us)
        {
            persons = pers;
            products = prod;
            List<RequestsProduct> list = new List<RequestsProduct>();
            foreach(Product pr in products)
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
            listViewBankProducts.ItemsSource = list;
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

        }

        private void OnEndorseClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnDeflectClick(object sender, RoutedEventArgs e)
        {

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
