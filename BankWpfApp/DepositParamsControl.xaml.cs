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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankWpfApp
{
    /// <summary>
    /// Логика взаимодействия для DepositParamsControl.xaml
    /// </summary>
    public partial class DepositParamsControl : UserControl
    {
        public Deposit dep = null;
        public DepositParamsControl()
        {
            InitializeComponent();
            dep = new Deposit();
        }

        private void OnNameDepTextChanged(object sender, TextChangedEventArgs e)
        {
            dep.Name = txtDepositName.Text;
        }

        private void OnTermDepTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtDepositTerm.Text, out int res))
            {
                dep.depositTerm = res;
            }
        }

        private void OnPrcDepTextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txtDepositPrc.Text, out float res))
            {
                dep.percent = res;
            }
        }

        private void OnBPerDepTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtDepositBPer.Text, out int res))
            {
                dep.billingPeriod = res;
            }
        }

        private void OnDescrDepTextChanged(object sender, TextChangedEventArgs e)
        {
            dep.Description = txtDepositDescr.Text;
        }

        private void OnAddYesChecked(object sender, RoutedEventArgs e)
        {

        }

        private void OnAddNoChecked(object sender, RoutedEventArgs e)
        {

        }

        private void OnDecYesChecked(object sender, RoutedEventArgs e)
        {

        }

        private void OnDecNoChecked(object sender, RoutedEventArgs e)
        {

        }

        private void OnCapYesChecked(object sender, RoutedEventArgs e)
        {

        }

        private void OnCapNoChecked(object sender, RoutedEventArgs e)
        {

        }

        private void OnBMinDepTextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txtDepositBMin.Text, out float res))
            {
                dep.minBalance = res;
            }
        }
    }
}
