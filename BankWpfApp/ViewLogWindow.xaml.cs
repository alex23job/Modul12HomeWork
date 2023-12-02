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
    /// Логика взаимодействия для ViewLogWindow.xaml
    /// </summary>
    public partial class ViewLogWindow : Window
    {
        List<MyLogView> arrRecords = null;
        public ViewLogWindow()
        {
            InitializeComponent();
        }

        public void SetSource(List<MyLogView> list)
        {
            arrRecords = list;
            dataGridLog.ItemsSource = arrRecords;
        }
    }
}
