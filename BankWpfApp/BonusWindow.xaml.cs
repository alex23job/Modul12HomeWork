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
        string strBeginPeriod = "";
        string strEndPeriod = "";
        CheckBox[] arrCheckBoxs = null;
        string[] months = { "январе", "феврале", "марте", "апреле", "мае", "июне", "июле", "августе", "сентябре", "октябре", "ноябре", "декабре" };
        float bonPrc = 0, bonActionPrc = 0, minSum = 0, maxSum = 0;
        int cnt = 0;
        public BonusWindow()
        {
            InitializeComponent();
            arrCheckBoxs = new CheckBox[] { cat1, cat2, cat3, cat4, cat5, cat6, cat7, cat8, cat9, cat10, all };
            for (int i = 0; i < 11; i++)
            {
                arrCheckBoxs[i].IsEnabled = false;
                if (i == 10) break;
                arrCheckBoxs[i].Content = LegalPerson.category[i];
            }
            labelTitle.Content = $"Выберите 3 категории в {months[DateTime.Now.Month - 1]}";
        }

        public void SetPerson(Person p, ObservableCollection<Person> pers)
        {
            persons = pers;
            ShowActions();
            currentPerson = p;
            labelCount.Content = $"Выбрано : {CountCheck()}";
            legalPers = p as LegalPerson;
            if (legalPers != null)
            {
                LegalBonus.Visibility = Visibility.Visible;
                SetActionParams(legalPers);
            }
            else
            {
                LegalBonus.Visibility = Visibility.Hidden;
            }
            for (int i = 0; i < 11; i++)
            {
                arrCheckBoxs[i].IsChecked = (p.BonusCategory & (1 << i)) != 0;
                arrCheckBoxs[i].IsEnabled = true;
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
                labelNo.Visibility = Visibility.Collapsed;
            }
        }

        private void SetActionParams(LegalPerson lp)
        {
            if (lp.MyBonusAction == null)
            {
                btn_Add.Content = "Добавить";
            }
            else
            {
                btn_Add.Content = "Установить";
                strBeginPeriod = lp.MyBonusAction.BeginPeriod;
                strEndPeriod = lp.MyBonusAction.EndPeriod;
                datePickerBegin.SelectedDate = GetDateTime(lp.MyBonusAction.BeginPeriod);
                datePickerBegin.IsDropDownOpen = false;
                datePickerEnd.SelectedDate = GetDateTime(lp.MyBonusAction.EndPeriod);
                datePickerEnd.IsDropDownOpen = false;
                txtCount.Text = lp.MyBonusAction.Count.ToString();
                txtPercent.Text = lp.MyBonusAction.Percent.ToString();
                txtMinSum.Text = lp.MyBonusAction.MinSumma.ToString();
                txtMaxSum.Text = lp.MyBonusAction.MaxSumma.ToString();
            }
            if (lp.MyBonus == null)
            {
                txtCustomPercent.IsEnabled = false;
                radioStd.IsChecked = true;
                radioCustom.IsChecked = false;
            }
            else
            {
                if (lp.MyBonus.Percent == 1f)
                {
                    txtCustomPercent.IsEnabled = false;
                    radioStd.IsChecked = true;
                    radioCustom.IsChecked = false;
                }
                else
                {
                    txtCustomPercent.IsEnabled = true;
                    txtCustomPercent.Text = $"{lp.MyBonus.Percent:0.00}";
                    radioStd.IsChecked = false;
                    radioCustom.IsChecked = true;
                }
            }
        }
        private List<BonusInfo> GetListActions()
        {
            List<BonusInfo> res = new List<BonusInfo>();
            foreach(Person p in persons)
            {
                LegalPerson lp = p as LegalPerson;
                if (lp != null && lp.MyBonusAction != null)
                {
                    string filePathTo = MainWindow.startupPath + "\\" + MainWindow.logoImgPath + "\\" + lp.LogoPath;
                    res.Add(new BonusInfo(filePathTo, lp.LegalName, lp.MyBonusAction.GetStrSumma(), lp.MyBonusAction.GetStrPeriod(), lp.MyBonusAction.Count.ToString()));
                }
            }
            return res;
        }

        private int CountCheck()
        {
            if (currentPerson == null) return 0;
            int res = 0;
            for (int i = 0; i < 11; i++)
            {
                if ((currentPerson.BonusCategory & (1 << i)) != 0) res++;
            }
            return res;
        }

        private void UpdateCheck(int bit)
        {
            if ((currentPerson.BonusCategory & (1 << bit)) != 0)
            {
                currentPerson.BonusCategory ^= 1 << bit;
            }
            else
            {
                if (CountCheck() < 3)
                {
                    currentPerson.BonusCategory ^= 1 << bit;
                }
            }
            arrCheckBoxs[bit].IsChecked = (currentPerson.BonusCategory & (1 << bit)) != 0;
            labelCount.Content = $"Выбрано : {CountCheck()}";
        }

        private void ClickBtnOk(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        #region Cat Click
        private void Cat1Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(0);
        }

        private void Cat2Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(1);
        }

        private void Cat3Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(2);
        }

        private void Cat4Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(3);
        }

        private void Cat5Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(4);
        }

        private void Cat6Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(5);
        }

        private void Cat7Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(6);
        }

        private void Cat8Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(7);
        }

        private void Cat9Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(8);
        }

        private void Cat10Click(object sender, RoutedEventArgs e)
        {
            UpdateCheck(9);
        }
        private void AllClick(object sender, RoutedEventArgs e)
        {
            UpdateCheck(10);
        }
        #endregion

        private void ClickBtnAdd(object sender, RoutedEventArgs e)
        {
            if (TestParams())
            {
                legalPers.MyBonus = new Bonus(bonPrc);
                legalPers.MyBonusAction = new BonusAction(bonActionPrc, cnt, minSum, maxSum, strBeginPeriod, strEndPeriod);
                ShowActions();
            }
            else
            {
                MessageBox.Show("Проверьте заполнены ли все поля и нажмите ещё раз !");
            }
        }

        private bool TestParams()
        {
            bool res = true;
            if (radioStd.IsChecked == true)
            {
                bonPrc = 1f;
            }
            if (radioCustom.IsChecked == true)
            {
                res = float.TryParse(txtCustomPercent.Text, out bonPrc);
            }
            if (txtPercent.Text != "")
            {
                res = float.TryParse(txtPercent.Text, out bonActionPrc);
            }
            if (txtCount.Text != "")
            {
                res = int.TryParse(txtCount.Text, out cnt);
            }
            if (txtMinSum.Text != "")
            {
                res = float.TryParse(txtMinSum.Text, out minSum);
            }
            if (txtMaxSum.Text != "")
            {
                res = float.TryParse(txtMaxSum.Text, out maxSum);
            }
            if (strBeginPeriod == "" || strEndPeriod == "")
            {
                res = false;
            }
            return res;
        }

        private void OnSelectedBeginDataChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? dt = datePickerBegin.SelectedDate;
            if (dt != null)
            {
                DateTime sdt = (DateTime)dt;
                strBeginPeriod = string.Format("{0:D04}.{1:D02}.{2:D02}", sdt.Year, sdt.Month, sdt.Day);
            }
        }

        private void OnSelectedEndDataChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? dt = datePickerEnd.SelectedDate;
            if (dt != null)
            {
                DateTime sdt = (DateTime)dt;
                strEndPeriod = string.Format("{0:D04}.{1:D02}.{2:D02}", sdt.Year, sdt.Month, sdt.Day);
            }
        }

        public DateTime? GetDateTime(string dt)
        {
            string[] sd = dt.Split('.');
            DateTime dat = DateTime.Now;
            if (sd.Length >= 3)
            {
                if (int.TryParse(sd[0], out int year) && int.TryParse(sd[1], out int month) && int.TryParse(sd[2], out int day))
                {
                    dat = new DateTime(year, month, day);
                }
            }
            return dat;
        }

        private void OnRadioStdClick(object sender, RoutedEventArgs e)
        {
            txtCustomPercent.IsEnabled = false;
            radioStd.IsChecked = true;
            radioCustom.IsChecked = false;
        }

        private void OnRadioCustomClick(object sender, RoutedEventArgs e)
        {
            txtCustomPercent.IsEnabled = true;
            radioStd.IsChecked = false;
            radioCustom.IsChecked = true;
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
