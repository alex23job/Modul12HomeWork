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
    /// Логика взаимодействия для CreditParamsControl.xaml
    /// </summary>
    public partial class CreditParamsControl : UserControl
    {
        public Credit credit = null;
        public CreditParamsControl()
        {
            credit = new Credit();
            InitializeComponent();
            cmbCreditType.ItemsSource = Credit.nameTypeCredit;
        }

        private void OnCheckCollateralClick(object sender, RoutedEventArgs e)
        {
            if (credit.IsCollateral)
            {
                credit.IsCollateral = false;
                txtCreditCollateral.Text = "";
                txtCreditCollateral.IsEnabled = false;
            }
            else
            {
                credit.IsCollateral = true;
                txtCreditCollateral.IsEnabled = true;
            }
        }

        private void OnCheckSuretyClick(object sender, RoutedEventArgs e)
        {
            if (credit.IsSurety)
            {
                credit.IsSurety = false;
                txtCreditSurety.Text = "";
                txtCreditSurety.IsEnabled = false;
            }
            else
            {
                credit.IsSurety = true;
                txtCreditSurety.IsEnabled = true;
            }
        }

        private void OnCheckMaxLimitClick(object sender, RoutedEventArgs e)
        {
            if (credit.IsMaxLimit)
            {
                credit.IsMaxLimit = false;
                txtCreditMaxLimit.Text = "";
                txtCreditMaxLimit.IsEnabled = false;
            }
            else
            {
                credit.IsMaxLimit = true;
                txtCreditMaxLimit.IsEnabled = true;
            }
        }

        private void OnNameCreditTextChanged(object sender, TextChangedEventArgs e)
        {
            credit.Name = txtCreditName.Text;
        }

        private void OnPercentCreditTextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txtCreditPercent.Text, out float percent))
            {
                credit.Percent = percent;
            }
        }

        private void OnCollateralCreditTextChanged(object sender, TextChangedEventArgs e)
        {
            credit.Collateral = txtCreditCollateral.Text;
        }

        private void OnSuretyCreditTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtCreditSurety.Text, out int count))
            {
                credit.Surety = count;
            }
        }

        private void OnMaxLimitCreditTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtCreditMaxLimit.Text, out int limit))
            {
                credit.MaxLimit = limit;
            }
        }

        private void OnDescrCreditTextChanged(object sender, TextChangedEventArgs e)
        {
            credit.Description = txtCreditDescr.Text;
        }

        private void OnSelectionTypeCreditChanged(object sender, SelectionChangedEventArgs e)
        {
            credit.TypeCredit = cmbCreditType.SelectedIndex;
        }
    }
}
