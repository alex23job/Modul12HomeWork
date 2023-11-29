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
    /// Логика взаимодействия для CardParamsControl.xaml
    /// </summary>
    public partial class CardParamsControl : UserControl
    {
        public Card card = null;
        public CardParamsControl()
        {
            card = new Card();
            InitializeComponent();
            cmbCardType.ItemsSource = Card.nameTypeCard;
        }

        private void OnDescrCardTextChanged(object sender, TextChangedEventArgs e)
        {
            card.Description = txtCardDescr.Text;
        }

        private void OnNameCardTextChanged(object sender, TextChangedEventArgs e)
        {
            card.Name = txtCardName.Text;
        }

        private void OnCheckCashbackClick(object sender, RoutedEventArgs e)
        {
            if (card.IsCashback)
            {
                card.IsCashback = false;
                txtCardCashback.Text = "";
                txtCardCashback.IsEnabled = false;
            }
            else
            {
                card.IsCashback = true;
                txtCardCashback.IsEnabled = true;
            }    
        }

        private void OnCheckBalPercClick(object sender, RoutedEventArgs e)
        {
            if (card.IsBalancePercent)
            {
                card.IsBalancePercent = false;
                txtCardBalPerc.Text = "";
                txtCardBalPerc.IsEnabled = false;
            }
            else
            {
                card.IsBalancePercent = true;
                txtCardBalPerc.IsEnabled = true;
            }
        }

        private void OnCheckLimitClick(object sender, RoutedEventArgs e)
        {
            if (card.IsLimit)
            {
                card.IsLimit = false;
                txtCardLimit.Text = "";
                txtCardLimit.IsEnabled = false;
            }
            else
            {
                card.IsLimit = true;
                txtCardLimit.IsEnabled = true;
            }
        }

        private void OnCodeCardTextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtCardCode.Text, out int code))
            {
                card.CodeProgramm = code;
            }
        }

        private void OnCashbackCardTextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txtCardCashback.Text, out float cashback))
            {
                card.CashbackPercent = cashback;
            }
        }

        private void OnBalPercCardTextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txtCardBalPerc.Text, out float percent))
            {
                card.BalancePercent = percent;
            }
        }

        private void OnLimitCardTextChanged(object sender, TextChangedEventArgs e)
        {
            if (float.TryParse(txtCardLimit.Text, out float limit))
            {
                card.Limit = limit;
            }
        }

        private void OnSelectionTypeCardChanged(object sender, SelectionChangedEventArgs e)
        {
            card.TypeCard = cmbCardType.SelectedIndex;
        }
    }
}
