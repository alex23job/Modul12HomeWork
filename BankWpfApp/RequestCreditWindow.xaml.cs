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
        float currentPercent;
        float currentSum = 5000000;
        int currentPeriod = 60;
        int currentDay;
        bool IsInsurance = true;
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
            StringBuilder sb = new StringBuilder($"{bc.Name} <{Credit.nameTypeCredit[bc.TypeCredit]}>\n");
            sb.Append($" от {bc.Percent}% ");
            if (bc.IsMaxLimit) sb.Append($" до {bc.MaxLimit} Р ");
            if (bc.IsSurety) sb.Append($"\n Поручители : {bc.Surety} "); else sb.Append("\n");
            if (bc.IsCollateral) sb.Append($" Залог : {bc.Collateral}");
            txtCreditInfo.Text = sb.ToString();
            currentPercent = bc.Percent;
            currentDay = DateTime.Now.Day;
            creditDay.Text = currentDay.ToString();
            creditPeriod.Text = currentPeriod.ToString();
            creditSum.Text = currentSum.ToString();
        }

        private void OnInsuranceClick(object sender, RoutedEventArgs e)
        {
            if (IsInsurance == true)
            {
                IsInsurance = false;
                currentPercent = credit.Percent + 5f;
            }
            else
            {
                IsInsurance = true;
                currentPercent = credit.Percent;
            }
            TxtParamsUpdate();
        }

        private void OnSumChanged(object sender, TextChangedEventArgs e)
        {
            float maxSum = credit.IsMaxLimit ? credit.MaxLimit : 1000000f;
            if (float.TryParse(creditSum.Text, out float sum))
            {                
                if (sum > 0 && sum <= maxSum)
                {
                    currentSum = sum;
                    TxtParamsUpdate();
                }
            }
            else
            {
                MessageBox.Show($"Ошибка ввода суммы кредита. Введите число от 1 до {maxSum} включительно."); 
            }
        }

        private void OnPeriodChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(creditPeriod.Text, out int per))
            {
                if (per > 0 && per < 61)
                {
                    currentPeriod = per;
                    TxtParamsUpdate();
                }
            }
            else
            {
                MessageBox.Show("Ошибка ввода срока кредита. Введите число от 1 до 60 включительно.");
            }
        }

        private void OnDayChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(creditDay.Text, out int day))
            {
                if (day > 0 && day < 32)
                {
                    currentDay = day;
                    TxtParamsUpdate();
                }
            }
            else
            {
                MessageBox.Show("Ошибка ввода числа дня платежа. Введите число от 1 до 31 включительно.");
            }
        }

        private void TxtParamsUpdate()
        {
            StringBuilder sb = new StringBuilder($"Процент по кредиту : {currentPercent}%");
            sb.Append($"\n{currentSum} Р на {currentPeriod} месяцев");
            string payment = "ERROR";
            if (currentPeriod > 0)
            {
                float cm = currentPercent / 1200;
                float zn = currentSum * cm / (1 - 1 / (float)Math.Pow(1 + cm, currentPeriod));
                payment = $"{zn:0.00}";
            }
            sb.Append($"\nЕжемесячно {payment} Р  {currentDay} числа");
            txtParamInfo.Text = sb.ToString();
        }
    }
}
