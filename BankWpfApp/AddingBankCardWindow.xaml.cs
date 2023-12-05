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
    /// Логика взаимодействия для AddingBankCardWindow.xaml
    /// </summary>
    public partial class AddingBankCardWindow : Window
    {
        Repository<Product> products = null;
        Repository<Product> bankProducts = null;
        Person currPerson = null;
        public AddingBankCardWindow()
        {
            InitializeComponent();
        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            if (listViewCard.SelectedItem != null)
            {
                CardViewData av = listViewCard.SelectedItem as CardViewData;
                if (av != null)
                {
                    if (MessageBox.Show($"Будет оформлена заявка на карту : {av.Name}\n\nОформить заявку ?", $"Оформление заявки на карту для клиента {currPerson.PersonLogin}", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        BankCard bp = bankProducts.Add(new BankCard()) as BankCard;
                        bp.personUID = currPerson.UID;
                        bp.PersonProductNumber = Product.GetNextPersonProductNumber();
                        bp.Name = av.Name;
                        bp.TypeCard = Card.GetNumType(av.NameType);
                        currPerson.IdProducts.Add(bp.PersonProductNumber);
                        bp.IsRequest = true;
                        foreach (Product pr in products.AllItems)
                        {
                            Card cd = pr as Card;
                            if (cd != null && cd.Name == av.Name)
                            {
                                bp.CopyParamsProduct(cd);
                            }
                        }
                    }
                }
                DialogResult = true;
            }
        }

        public void SetRepositoty(Repository<Product> prod, Repository<Product> bp)
        {
            bankProducts = bp;
            products = prod;
            ObservableCollection<CardViewData> arr = new ObservableCollection<CardViewData>();
            foreach (Product pr in products.AllItems)
            {
                Card acc = pr as Card;
                if (acc != null)
                {
                    arr.Add(new CardViewData(acc.Name, Card.nameTypeCard[acc.TypeCard]));
                }
            }
            listViewCard.ItemsSource = arr;
        }

        public void SetPerson(Person pers)
        {
            currPerson = pers;
        }
    }

    public class CardViewData
    {
        public string Name { get; set; }
        public string NameType { get; set; }
        public CardViewData() { }
        public CardViewData(string nm, string nmt)
        {
            Name = nm;
            NameType = nmt;
        }
    }
}
