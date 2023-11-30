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
    /// Логика взаимодействия для PersonsViewWindow.xaml
    /// </summary>
    public partial class PersonsViewWindow : Window
    {
        Repository<Person> persons = null;
        UserData user = null;
        public PersonsViewWindow()
        {
            InitializeComponent();
        }

        public void SetPersonRepository(Repository<Person> pers)
        {
            persons = pers;
            dataGrid.ItemsSource = persons.AllItems;
        }
        
        public void SetUser(UserData ud)
        {
            user = ud;
        }

        private void OnDataGridMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnAddPerson(object sender, RoutedEventArgs e)
        {

        }

        private void OnEditPerson(object sender, RoutedEventArgs e)
        {

        }

        private void OnDelPerson(object sender, RoutedEventArgs e)
        {

        }

        private void OnExit(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
