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
    /// Логика взаимодействия для ViewProductsWindow.xaml
    /// </summary>
    public partial class ViewProductsWindow : Window
    {
        private Dictionary<string, string> dictInfo = new Dictionary<string, string>();
        private Repository<Product> products = null;

        private AccountParamsControl myAccCntr = new AccountParamsControl();
        private DepositParamsControl myDepCntr = new DepositParamsControl();
        public ViewProductsWindow()
        {
            InitializeComponent();
            cmbCategory.ItemsSource = ProductCategory.Category;
        }

        public void SetProducts(Repository<Product> prod)
        {
            products = prod;
            CreateTreeProducts();
        }

        private void CreateTreeProducts()
        {
            Node rootNode = new Node() { Name = "Все продукты" };
            string category = "";
            if (products != null)
            {
                foreach (Product p in products.AllItems)
                {
                    IProductType productType = p as IProductType;
                    if (productType != null)
                    {
                        switch(productType.Type)
                        {
                            case 0:
                                category = "Карты";
                                break;
                            case 1:
                                category = "Вклады";
                                break;
                            case 2:
                                category = "Кредиты";
                                break;
                            case 3:
                                category = "Счета";
                                break;
                        }
                        Node curNode = rootNode[category];
                        if (curNode == null)
                        {
                            curNode = new Node() { Name = category };
                            rootNode.Children.Add(curNode);
                        }
                        if (curNode != null)
                        {
                            curNode.Children.Add(new Node() { Name = p.Name });
                        }
                    }
                }
            }
            if (rootNode != null)
            {
                treeView.ItemsSource = new ObservableCollection<Node>() { rootNode };
            }
        }

        private void OnMouseRightButtonClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Node nNode = e.NewValue as Node;
            if (nNode != null)
            {
                foreach(Product p in products.AllItems)
                {
                    if (p.Name == nNode.Name)
                    {
                        listViewInfo.ItemsSource = p.GetProductInfo();
                    }
                }
            }
        }

        private void OnBtnExit(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void OnCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductParams.Children.Clear();
            switch(cmbCategory.SelectedIndex)
            {
                case 0: break;
                case 1:
                    myDepCntr = new DepositParamsControl();
                    ProductParams.Children.Add(myDepCntr);
                    break;
                case 2: break;
                case 3:
                    myAccCntr = new AccountParamsControl();
                    ProductParams.Children.Add(myAccCntr);
                    break;
            }
        }

        private void OnAddProduct(object sender, RoutedEventArgs e)
        {
            switch(cmbCategory.SelectedIndex)
            {
                case 0: break;
                case 1:
                    products.Add(myDepCntr.dep);
                    myDepCntr.dep = new Deposit();
                    break;
                case 2: break;
                case 3:
                    products.Add(myAccCntr.acc);
                    myAccCntr.acc = new Account();
                    break;
            }
            CreateTreeProducts();
            //MessageBox.Show(ProductParams.Children[0].ToString());
        }

        private void OnClearProduct(object sender, RoutedEventArgs e)
        {
            switch (cmbCategory.SelectedIndex)
            {
                case 0: break;
                case 1:
                    myDepCntr.dep = null;
                    break;
                case 2: break;
                case 3:
                    myAccCntr.acc = null;
                    break;
            }
            cmbCategory.SelectedIndex = 0;
        }
    }

    public class ProductCategory
    {
        private static readonly List<string> cats;

        static ProductCategory()
        {
            cats = new List<string>();
            cats.Add("Карты");
            cats.Add("Вклады");
            cats.Add("Кредиты");
            cats.Add("Счета");
        }
        /// <summary>
        /// Название категории
        /// </summary>
        public string Name { get; set; }

        public static IEnumerable<string> Category => cats;

        public static string GetCategory(int index)
        {
            return cats[index];
        }
    }
}
