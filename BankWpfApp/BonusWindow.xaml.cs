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
    /// Логика взаимодействия для BonusWindow.xaml
    /// </summary>
    public partial class BonusWindow : Window
    {
        Person currentPerson = null;
        LegalPerson legalPers = null;
        ObservableCollection<Person> persons = null;
        public BonusWindow()
        {
            InitializeComponent();
        }

        public void SetPerson(Person p, ObservableCollection<Person> pers)
        {
            persons = pers;
            ShowActions();
            currentPerson = p;
            legalPers = p as LegalPerson;
            if (legalPers != null)
            {
                LegalBonus.Visibility = Visibility.Visible;
            }
            else
            {
                LegalBonus.Visibility = Visibility.Hidden;
            }
        }

        private void ShowActions()
        {
            List<BonusInfo> list = GetListActions();
            if (list.Count == 0)
            {
                listViewInfo.Visibility = Visibility.Hidden;
                labelNo.Visibility = Visibility.Visible;
            }
            else
            {
                listViewInfo.ItemsSource = list;
                listViewInfo.Visibility = Visibility.Visible;
                labelNo.Visibility = Visibility.Hidden;
            }
        }

        private List<BonusInfo> GetListActions()
        {
            List<BonusInfo> res = new List<BonusInfo>();
            foreach(Person p in persons)
            {
                LegalPerson lp = p as LegalPerson;
                if (lp != null)
                {

                }
            }
            return res;
        }

        private void ClickBtnOk(object sender, RoutedEventArgs e)
        {

        }

        private void Cat1Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cat2Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cat3Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cat4Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cat5Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cat6Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cat7Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cat8Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cat9Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cat10Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClickBtnAdd(object sender, RoutedEventArgs e)
        {

        }

        private void AllClick(object sender, RoutedEventArgs e)
        {

        }
    }

    public class BonusInfo
    {
        public string Img { get; set; }
        public string LegalName { get; set; }
        public string StrSumma { get; set; }
        public string StrPeriod { get; set; }
        public string StrCount { get; set; }

        public BonusInfo() { }
        public BonusInfo(string path, string namePers, string sum, string per, string count)
        {
            Img = path;
            StrPeriod = per;
            StrSumma = sum;
            LegalName = namePers;
            StrCount = count;
        }
    }
}
