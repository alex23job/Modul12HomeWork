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
    /// Логика взаимодействия для PersonEditUserControl.xaml
    /// </summary>
    public partial class PersonEditUserControl : UserControl
    {
        public string strBirthDay = "";
        public PersonEditUserControl()
        {
            InitializeComponent();
            datePickerBirthday.IsDropDownOpen = false;
        }

        private void OnSelectedDataChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? dt = datePickerBirthday.SelectedDate;
            if (dt != null)
            {
                DateTime sdt = (DateTime)dt;
                strBirthDay = string.Format("{0:D04}.{1:D02}.{2:D02}", sdt.Year, sdt.Month, sdt.Day);
            }
        }

        public void SetBirthday(string dt)
        {
            strBirthDay = dt;
            string[] sd = dt.Split('.');
            DateTime dat = DateTime.Now;
            if (sd.Length >= 3)
            {
                if (int.TryParse(sd[0], out int year) && int.TryParse(sd[1], out int month) && int.TryParse(sd[2], out int day))
                {
                    dat = new DateTime(year, month, day);
                }
            }
            datePickerBirthday.SelectedDate = dat;
        }
    }
}
