using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        Person pers = null;
        UserData user = null;
        bool IsLegalPerson = false;
        string onlyFileName = "";

        public RegistrationWindow()
        {
            InitializeComponent();
        }

        public Person GetPerson()
        {
            return pers;
        }

        public UserData GetUser()
        {
            return user;
        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            if (TestFields())
            {
                if (IsLegalPerson)
                {
                    pers = new LegalPerson(personData.txtName.Text, personData.txtFirstName.Text, personData.txtSecondName.Text,
                    personData.txtPasport.Text, personData.txtTlf.Text, personData.strBirthDay, nameLegalPerson.Text, adrLegalPerson.Text, onlyFileName);
                }
                else
                {
                    pers = new Person(personData.txtName.Text, personData.txtFirstName.Text, personData.txtSecondName.Text,
                    personData.txtPasport.Text, personData.txtTlf.Text, personData.strBirthDay);
                }
                user = new UserData(txtLogin.Text, txtNewPass1.Text, 0);
                DialogResult = true;
            }
        }

        private bool TestFields()
        {
            bool res = true;
            StringBuilder sb = new StringBuilder();
            if (personData.txtName.Text == "")
            {
                sb.Append("Не заполнена фамилия клиента\n");
                res = false;
            }
            if (personData.txtFirstName.Text == "")
            {
                sb.Append("Не заполнено имя клиента\n");
                res = false;
            }
            if (personData.txtSecondName.Text == "")
            {
                sb.Append("Не заполнено отчество клиента\n");
                res = false;
            }
            if (personData.txtTlf.Text == "")
            {
                sb.Append("Не указан номер телефона клиента\n");
                res = false;
            }
            if (personData.txtPasport.Text == "")
            {
                sb.Append("Не заполнены данные о паспорте клиента\n");
                res = false;
            }
            if (personData.strBirthDay == "")
            {
                sb.Append("Не указан день рождения клиента\n");
                res = false;
            }
            if (txtLogin.Text == "")
            {
                sb.Append("Не назначен логин клиента\n");
                res = false;
            }
            if (txtNewPass1.Text == "" || txtNewPass2.Text == "")
            {
                sb.Append("Не указан пароль или подтверждение пароля клиента\n");
                res = false;
            }
            else if (txtNewPass1.Text != txtNewPass2.Text)
            {
                sb.Append("Пароль и подтверждение пароля не совпадают!\n");
                res = false;
            }
            if (IsLegalPerson)
            {
                if (nameLegalPerson.Text == "")
                {
                    sb.Append("Не указано название юридического лица");
                    res = false;
                }
                if (adrLegalPerson.Text == "")
                {
                    sb.Append("Не указан адрес юридического лица");
                    res = false;
                }
            }
            if (!res)
            {
                sb.Append("\nДля успешной регистрации нужно заполнить все поля. Если значение отсутствует, то напишите слово <нет>");
                MessageBox.Show(sb.ToString());
            }

            return res;
        }

        private void OnLegalPersonClick(object sender, RoutedEventArgs e)
        {
            if (IsLegalPerson)
            {
                IsLegalPerson = false;
            }
            else
            {
                IsLegalPerson = true;
            }
            nameLegalPerson.IsEnabled = IsLegalPerson;
            adrLegalPerson.IsEnabled = IsLegalPerson;
        }

        private void OnLoadClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                onlyFileName = System.IO.Path.GetFileName(filePath);
                // Use the selected file path here...
                //Bitmap bitmap = new Bitmap("image.jpg");
                //logo.Source = bitmap as ImageSource;
                logo.Source = new BitmapImage(new Uri(filePath));
                File.Copy(filePath, MainWindow.startupPath + "\\" + MainWindow.logoImgPath + "\\" + onlyFileName);
            }
        }
    }
}
