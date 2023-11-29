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
    /// Логика взаимодействия для ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        UserData user = null;
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private void OnOK_Click(object sender, RoutedEventArgs e)
        {
            if (user != null)
            {
                if (txtOldPass.Text == user.Password)
                {
                    if (txtNewPass1.Text == txtNewPass2.Text)
                    {
                        user.Password = txtNewPass1.Text;
                        this.DialogResult = true;
                    }
                }
            }
        }

        public void SetUser(UserData ud)
        {
            user = ud;
        }

        public UserData GetUser()
        {
            return user;
        }
    }
}
