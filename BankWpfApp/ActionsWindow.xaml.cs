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
    /// Логика взаимодействия для ActionsWindow.xaml
    /// </summary>
    public partial class ActionsWindow : Window
    {
        Person pers = null;
        ObservableCollection<Person> persons = null;
        ObservableCollection<Product> products = null;
        int mode = -1;
        public Transaction transaction = null;
        public ActionsWindow()
        {
            InitializeComponent();
        }

        public void SetPersons(Person p, ObservableCollection<Person> arr, ObservableCollection<Product> prod)
        {
            pers = p;
            persons = arr;
            products = prod;
        }

        private void OnDecClick(object sender, RoutedEventArgs e)
        {
            mode = 0;
            radYou.Visibility = Visibility.Hidden;
            radOther.Visibility = Visibility.Hidden;
            List<string> cmbList = GetMyAccNumber();
            if (cmbList.Count > 0)
            {
                cmb1.ItemsSource = cmbList;
                cmb1.Visibility = Visibility.Visible;
                cmb1.SelectedIndex = 0;
                txtBox1.Visibility = Visibility.Hidden;
                txtBox2.Visibility = Visibility.Visible;
                txtBox2.Text = "Банкомат";
                cmb2.Visibility = Visibility.Hidden;
            }
            else
            {
                MessageBox.Show("Нет счетов или соедств на них !");
            }
        }

        private void OnIncClick(object sender, RoutedEventArgs e)
        {
            mode = 1;
            radYou.Visibility = Visibility.Hidden;
            radOther.Visibility = Visibility.Hidden;
            List<string> cmbList = new List<string>();
            foreach(long idp in pers.IdProducts)
            {
                for (int j = 0; j < products.Count; j++)
                {
                    BankAccount ba = products[j] as BankAccount;
                    if (ba != null && ba.PersonProductNumber == idp)
                    {
                        cmbList.Add($"{ba.PersonProductNumber} {ba.Name}");
                        break;
                    }

                    BankCard bc = products[j] as BankCard;
                    if (bc != null && bc.PersonProductNumber == idp)
                    {
                        if (bc.CardAccount != null) cmbList.Add($"{bc.CardAccount.PersonProductNumber} {bc.Name}");
                        break;
                    }

                    BankDeposit bd = products[j] as BankDeposit;
                    if (bd != null && bd.PersonProductNumber == idp)
                    {
                        cmbList.Add($"{bd.DepositAccount.PersonProductNumber} {bd.Name}");
                        break;
                    }

                    BankCredit bcr = products[j] as BankCredit;
                    if (bcr != null && bcr.PersonProductNumber == idp)
                    {
                        if (bcr.CreditAccount != null) cmbList.Add($"{bcr.CreditAccount.PersonProductNumber} {bcr.Name}");
                        break;
                    }
                }
            }
            if (cmbList.Count > 0)
            {
                cmb2.ItemsSource = cmbList;
                cmb2.Visibility = Visibility.Visible;
                cmb2.SelectedIndex = 0;
                txtBox2.Visibility = Visibility.Hidden;
                txtBox1.Visibility = Visibility.Visible;
                txtBox1.Text = "Банкомат";
                cmb1.Visibility = Visibility.Hidden;
            }
        }

        private void OnTransferClick(object sender, RoutedEventArgs e)
        {
            mode = 2;
            List<string> cmbList = GetMyAccNumber();
            if (cmbList.Count > 0)
            {
                cmb1.ItemsSource = cmbList;
                cmb1.Visibility = Visibility.Visible;
                cmb1.SelectedIndex = 0;
                txtBox1.Visibility = Visibility.Hidden;
                radYou.Visibility = Visibility.Visible;
                radOther.Visibility = Visibility.Visible;
                if ((bool)radOther.IsChecked)
                {
                    txtBox2.Visibility = Visibility.Visible;
                    txtBox2.Text = "";
                    cmb2.Visibility = Visibility.Hidden;
                }
                if ((bool)radYou.IsChecked)
                {
                    txtBox2.Visibility = Visibility.Hidden;
                    cmb2.ItemsSource = GetMyAccNumber(false);
                    cmb2.SelectedIndex = 0;
                    cmb2.Visibility = Visibility.Visible;
                }
            }
            else
            {
                MessageBox.Show("Нет счетов или соедств на них !");
            }
        }

        private void OnPayClick(object sender, RoutedEventArgs e)
        {
            mode = 3;
            radYou.Visibility = Visibility.Hidden;
            radOther.Visibility = Visibility.Hidden;
            List<string> cmbList = GetMyAccNumber();
            if (cmbList.Count > 0)
            {
                cmb1.ItemsSource = cmbList;
                cmb1.Visibility = Visibility.Visible;
                cmb1.SelectedIndex = 0;
                txtBox1.Visibility = Visibility.Hidden;
                
                List<string> listLegalPerson = GetLegalPersonList();
                if (listLegalPerson.Count > 0)
                {
                    cmb2.ItemsSource = listLegalPerson;
                    cmb2.SelectedIndex = 0;
                    txtBox2.Visibility = Visibility.Hidden;
                    cmb2.Visibility = Visibility.Visible;
                }
                else
                {
                    cmb2.Visibility = Visibility.Hidden;
                    txtBox2.Text = "";
                    txtBox2.Visibility = Visibility.Visible;
                }                
            }
            else
            {
                MessageBox.Show("Нет счетов или соедств на них !");
            }
        }

        private void OnCMB1SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OnCMB2SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            float sum = 0;
            if (mode != -1 && !float.TryParse(txtBox3.Text, out sum))
                return;
            transaction = new Transaction(sum, mode);
            switch (mode)
            {
                case 0:
                    transaction.accFrom = GetBankAccount((string)cmb1.SelectedItem);
                    transaction.To = "Банкомат";
                    break;
                case 1:
                    transaction.From = "Банкомат";
                    transaction.accTo = GetBankAccount((string)cmb2.SelectedItem);
                    break;
                case 2:
                    transaction.accFrom = GetBankAccount((string)cmb1.SelectedItem);
                    transaction.accTo = ((bool)radOther.IsChecked) ? GetBankAccount(txtBox2.Text) : GetBankAccount((string)cmb2.SelectedItem);
                    if (transaction.accTo == null) transaction.To = txtBox2.Text;
                    break;
                case 3:
                    transaction.accFrom = GetBankAccount((string)cmb1.SelectedItem);
                    transaction.accTo = txtBox2.IsVisible ? GetBankAccount(txtBox2.Text) : GetBankAccount((string)cmb2.SelectedItem);
                    if (transaction.accTo == null) transaction.To = cmb2.Text;
                    break;
            }
            DialogResult = true;
        }

        private List<string> GetLegalPersonList()
        {
            List<string> cmbList = new List<string>();
            foreach (Person ps in persons)
            {
                LegalPerson lps = ps as LegalPerson;
                if (lps != null)
                {
                    foreach (long idpr in lps.IdProducts)
                    {
                        BankAccount tba = GetBankAccount(idpr);
                        if (tba != null)
                        {
                            if (tba.TypeAccount == 0)
                            {
                                cmbList.Add($"{tba.PersonProductNumber} {tba.Name}");
                            }
                        }
                    }
                }
            }
            return cmbList;
        }
        private List<string> GetMyAccNumber(bool IsPositiveBalans = true)
        {
            List<string> cmbList = new List<string>();
            List<long> testProdNum = new List<long>();
            foreach (long idp in pers.IdProducts)
            {
                for (int j = 0; j < products.Count; j++)
                {
                    BankAccount ba = products[j] as BankAccount;
                    if (ba != null && ba.PersonProductNumber == idp)
                    {
                        if (ba.Balans > 0 || !IsPositiveBalans)
                        {
                            if (!testProdNum.Contains(idp))
                            {
                                testProdNum.Add(idp);
                                cmbList.Add($"{ba.PersonProductNumber} {ba.Name}");
                            }
                        }
                        break;
                    }

                    BankCard bc = products[j] as BankCard;
                    if (bc != null && bc.PersonProductNumber == idp)
                    {
                        if (bc.CardAccount != null && (bc.CardAccount.Balans > 0 || !IsPositiveBalans))
                        {
                            cmbList.Add($"{bc.CardAccount.PersonProductNumber} {bc.Name}");
                            testProdNum.Add(bc.CardAccount.PersonProductNumber);
                        }
                        break;
                    }

                    BankDeposit bd = products[j] as BankDeposit;
                    if (bd != null && bd.PersonProductNumber == idp)
                    {
                        if (bd.DepositAccount.Balans > 0 || !IsPositiveBalans)
                        {
                            cmbList.Add($"{bd.DepositAccount.PersonProductNumber} {bd.Name}");
                            testProdNum.Add(bd.DepositAccount.PersonProductNumber);
                        }
                        break;
                    }

                    BankCredit bcr = products[j] as BankCredit;
                    if (bcr != null && bcr.PersonProductNumber == idp)
                    {
                        if (bcr.CreditAccount != null && (bcr.CreditAccount.Balans > 0 || !IsPositiveBalans))
                        {
                            cmbList.Add($"{bcr.CreditAccount.PersonProductNumber} {bcr.Name}");
                            testProdNum.Add(bcr.CreditAccount.PersonProductNumber);
                        }
                        break;
                    }
                }
            }
            return cmbList;
        }

        private BankAccount GetBankAccount(string strIdAcc)
        {
            long idp = 0;
            string[] arrSplit = strIdAcc.Split(' ');
            string idAcc = arrSplit[0];
            if (!long.TryParse(idAcc, out idp))
            {
                if (arrSplit.Length >= 4)
                {   //  а может это номер карты ?
                    BankAccount ca = GetCardAccount(strIdAcc);
                    if (ca == null)
                    {
                        MessageBox.Show($"Карты с номером {strIdAcc} нет в нашем банке !");
                    }
                    return ca;
                }
                else if (idAcc[0] == '8' || idAcc.StartsWith("+7"))
                {   //  а может это номер телефона
                    foreach (Person ps in persons)
                    {
                        if (ps.Tlf == idAcc)
                        {
                            foreach(long idpr in ps.IdProducts)
                            {
                                BankAccount tba = GetBankAccount(idpr);
                                if (tba != null)
                                {
                                    if (mode > 1 && tba.TypeAccount == 0)
                                        return tba;
                                }
                            }
                            MessageBox.Show($"У клиента с номером телефона {idAcc} нет счетов в нашем банке !");
                            return null;
                        }
                    }
                    MessageBox.Show($"Клиента с номером телефона {idAcc} нет в нашем банке !");
                }
                else
                {
                    foreach (Person ps in persons)
                    {
                        LegalPerson lps = ps as LegalPerson;
                        if (lps != null && lps.LegalName == idAcc)
                        {
                            foreach (long idpr in lps.IdProducts)
                            {
                                BankAccount tba = GetBankAccount(idpr);
                                if (tba != null)
                                {
                                    if (mode > 1 && tba.TypeAccount == 0)
                                        return tba;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                return GetBankAccount(idp);
            }
            return null;
        }
        private BankAccount GetBankAccount(long idp)
        {
            for (int j = 0; j < products.Count; j++)
            {
                BankAccount ba = products[j] as BankAccount;
                if (ba != null && ba.PersonProductNumber == idp)
                {
                    return ba;
                }
            }
            return null;
        }

        private BankAccount GetCardAccount(string cardNum)
        {
            for (int j = 0; j < products.Count; j++)
            {
                BankCard bc = products[j] as BankCard;
                if (bc != null && bc.CheckCardNumber(cardNum))
                {
                    return bc.CardAccount;
                }
            }
            return null;
        }

        private void OnRadioYuoClick(object sender, RoutedEventArgs e)
        {
            if ((bool)radYou.IsChecked)
            {
                txtBox2.Visibility = Visibility.Hidden;
                cmb2.ItemsSource = GetMyAccNumber(false);
                cmb2.SelectedIndex = 0;
                cmb2.Visibility = Visibility.Visible;
            }
        }

        private void OnRadioOtherClick(object sender, RoutedEventArgs e)
        {
            if ((bool)radOther.IsChecked)
            {
                txtBox2.Visibility = Visibility.Visible;
                txtBox2.Text = "";
                cmb2.Visibility = Visibility.Hidden;
            }
        }
    }
}
