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
    /// Логика взаимодействия для StatOperationsWindow.xaml
    /// </summary>
    public partial class StatOperationsWindow : Window
    {
        string pathLogOperations = "LogOperations.csv";
        UserData currentUser = null;
        Person currentPerson = null;
        public StatOperationsWindow()
        {
            InitializeComponent();
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
                foreach(OneOperation op in list)
                {

                }
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
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
