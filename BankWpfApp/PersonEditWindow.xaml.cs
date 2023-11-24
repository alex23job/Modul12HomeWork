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
    /// Логика взаимодействия для PersonEditWindow.xaml
    /// </summary>
    public partial class PersonEditWindow : Window
    {
        Person per = null;
        UserData user = null;
        public PersonEditWindow()
        {
            InitializeComponent();
            per = new Person("", "", "", "", "", "");
        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            if (txtTlf.Text == "" || txtFirstName.Text == "" || txtName.Text == "" || txtPasport.Text == "" || txtSecondName.Text == "")
            {
                MessageBox.Show("Не указано одно или несколько значений параметров !!!");
                return;
            }
            per.Name = txtName.Text;
            per.LastName = txtFirstName.Text;
            per.SecondName = txtSecondName.Text;
            if (txtPasport.Text != "**** ******")
            {
                per.Pasport = txtPasport.Text;
            }
            per.Tlf = txtTlf.Text;
            this.DialogResult = true;
        }

        public bool SetParam(Person pr, UserData us)
        {
            user = us;
            per = new Person(pr.Name, pr.LastName, pr.SecondName, pr.Pasport, pr.Tlf, pr.BirthDay);
            IUserRights userRights = us as IUserRights;
            if (userRights == null)
            {
                MessageBox.Show("Нет прав для редактирования");
                return false;
            }
            txtName.Text = pr.Name;
            txtFirstName.Text = pr.LastName;
            txtSecondName.Text = pr.SecondName;
            txtPasport.Text = userRights.IsViewingField("Pasport") ? pr.Pasport : "**** ******";
            txtTlf.Text = pr.Tlf;
            txtName.IsEnabled = userRights.IsEditingField("Name");
            txtFirstName.IsEnabled = userRights.IsEditingField("FirstName");
            txtSecondName.IsEnabled = userRights.IsEditingField("SecondName");
            txtPasport.IsEnabled = userRights.IsEditingField("Pasport");
            txtTlf.IsEnabled = userRights.IsEditingField("Tlf");
            if (pr.updateInfo != null)
            {
                txtUpdate.Text = pr.updateInfo.ToString();
            }
            return true;
        }

        public Person GetPerson()
        {
            return per;
        }

    }
}
