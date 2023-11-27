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
    /// Логика взаимодействия для AccountParamsControl.xaml
    /// </summary>
    public partial class AccountParamsControl : UserControl
    {
        public Account acc = null;
        public AccountParamsControl()
        {
            InitializeComponent();
            acc = new Account();
            cmbAccountType.ItemsSource = Account.nameTypeAccount;
        }

        private void OnSelectionTypeAccountChanged(object sender, SelectionChangedEventArgs e)
        {
            acc.TypeAccount = cmbAccountType.SelectedIndex;
        }

        private void OnDescrAccTextChanged(object sender, TextChangedEventArgs e)
        {
            acc.Description = txtAccountDescr.Text;
        }

        private void OnNameAccTextChanged(object sender, TextChangedEventArgs e)
        {
            acc.Name = txtAccountName.Text;
        }
    }
}
