using System;
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
using System.Windows.Shapes;

namespace BankWpfApp
{
    /// <summary>
    /// Логика взаимодействия для RequestCreditWindow.xaml
    /// </summary>
    public partial class RequestCreditWindow : Window
    {
        BankCredit credit = null;
        public RequestCreditWindow()
        {
            InitializeComponent();
        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public void SetCredit(BankCredit bc)
        {
            credit = bc;
        }

        private void OnInsuranceClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
