using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для StatOperationsWindow.xaml
    /// </summary>
    public partial class StatOperationsWindow : Window
    {
        string pathLogOperations = "LogOperations.csv";
        UserData currentUser = null;
        Person currentPerson = null;
        ObservableCollection<Person> persons = null;
        ObservableCollection<Product> bankProducts = null;
        public StatOperationsWindow()
        {
            InitializeComponent();
        }

        public void SetRepository(ObservableCollection<Person> pers, ObservableCollection<Product> bp)
        {
            persons = pers;
            bankProducts = bp;
        }

        public void SetUser(UserData user, string pathLog = "")
        {
            currentUser = user;
            if (pathLog != "")
            {
                pathLogOperations = pathLog;
            }
            LogOperations log = new LogOperations(pathLogOperations);
            if (log.Load())
            {
                //log.GetSortedList(2)
            }
        }

        public void SetPerson(Person pers, string pathLog = "")
        {
            currentPerson = pers;
            if (pathLog != "")
            {
                pathLogOperations = pathLog;
            }
            LogOperations log = new LogOperations(pathLogOperations);
            if (log.Load())
            {
                List<OneOperation> list = log.GetSortedList(2, currentPerson.UID.ToString());
                List<OperationsInfo> sorce = new List<OperationsInfo>();
                foreach(OneOperation op in list)
                {
                    string pathLogo = "";
                    string znak = "+";
                    string nameTo = "";
                    string nameFr = "";
                    string cat = "None";
                    Product bpTo = GetBankProductFromUID(op.ToAccountUID);
                    Product bpFr = GetBankProductFromUID(op.FromAccountUID);
                    if (bpFr != null)
                    {
                        nameFr = bpFr.Name;
                    }
                    else
                    {
                        nameFr = op.FromAccountUID;
                    }
                    if (op.GetMode() == "pay")
                    {
                        znak = "-";
                        BankAccount ba = bpTo as BankAccount;
                        if (ba != null)
                        {
                            LegalPerson lp = GetPersonFromUID(ba.personUID.ToString()) as LegalPerson;
                            if (lp != null)
                            {
                                nameTo = lp.LegalName;
                                pathLogo = MainWindow.startupPath + "\\" + MainWindow.logoImgPath + "\\" + lp.LogoPath;
                                cat = lp.LegalCategoty;
                            }
                        }
                    }
                    if (op.ToAccountUID == "Банк" || op.FromAccountUID == "Банк")
                    {
                        pathLogo = MainWindow.startupPath + "\\" + MainWindow.logoImgPath + "\\Bank.jpg";
                    }
                    if (op.ToAccountUID == "Банкомат" || op.FromAccountUID == "Банкомат")
                    {
                        pathLogo = MainWindow.startupPath + "\\" + MainWindow.logoImgPath + "\\BankMachine.jpg";
                    }
                    sorce.Add(new OperationsInfo(pathLogo, nameFr, znak + op.GetSumma(), nameTo, cat));
                }
                listViewInfo.ItemsSource = sorce;
            }
        }

        private Product GetBankProductFromUID(string id)
        {
            foreach(Product p in bankProducts)
            {
                IPersonProductNumber ipn = p as IPersonProductNumber;
                if (ipn != null && ipn.PersonProductNumber.ToString() == id)
                {
                    return p;
                }
            }
            return null;
        }

        private Person GetPersonFromUID(string id)
        {
            foreach(Person p in persons)
            {
                if (p.UID.ToString() == id)
                {
                    return p;
                }
            }
            return null;
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void OnIncClick(object sender, RoutedEventArgs e)
        {

        }

        private void OnDecClick(object sender, RoutedEventArgs e)
        {

        }
    }

    public class OperationsInfo
    {
        public string Img { get; set; }
        public string NameProduct { get; set; }
        public string StrSumma { get; set; }
        public string LegalName { get; set; }
        public string Category { get; set; }

        public OperationsInfo() { }
        public OperationsInfo(string path, string namePr, string sum, string legalNm, string cat)
        {
            Img = path;
            NameProduct = namePr;
            StrSumma = sum;
            LegalName = legalNm;
            Category = cat;
        }
    }
}
