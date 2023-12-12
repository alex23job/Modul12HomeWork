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

        private CardParamsControl myCardCntr = new CardParamsControl();
        private AccountParamsControl myAccCntr = new AccountParamsControl();
        private DepositParamsControl myDepCntr = new DepositParamsControl();
        private CreditParamsControl myCreditCntr = new CreditParamsControl();
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
                            curNode = new Node(category, rootNode);
                            rootNode.Children.Add(curNode);
                        }
                        if (curNode != null)
                        {
                            curNode.Children.Add(new Node(p.Name, curNode));
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
            TextBlock tb = e.OriginalSource as TextBlock;
            if (tb != null)
            {
                Node curNode = tb.DataContext as Node;
                if ((curNode != null) && (curNode.Parent != null))
                {
                    TreeView tv = sender as TreeView;
                    TreeViewItem tvi = e.OriginalSource as TreeViewItem;
                    //tvi.IsSelected = true;
                    //tv.SelectedItem(tvi);
                    //MessageBox.Show($"node={curNode.Name}  parent={curNode.Parent.Name} => IsCategory={ProductCategory.IsCategory(curNode.Parent.Name)}");
                }
            }
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //TreeViewItem item = (e.OriginalSource as FrameworkElement).Parent as TreeViewItem;
            //if (item == null)
            //{
            //    item = e.Source as TreeViewItem;
            //    if (item == null)
            //    {
            //        return;
            //    }
            //}
            //item.IsSelected = true;
            //e.Handled = true;
            TreeView tv = sender as TreeView;
            //if (tv != null)
            //{
            //    foreach (TreeViewItem tvi in tv.Items)
            //    {
            //        if (tvi != null)
            //        MessageBox.Show(tvi.DataContext.ToString());
            //    }
            //}

            TextBlock tb = e.OriginalSource as TextBlock;
            if (tb != null)
            {
                tb.Focus();
                Node curNode = tb.DataContext as Node;
                if ((curNode != null) && (curNode.Parent != null))
                {
                    tv.Items.MoveCurrentTo(curNode.Parent);
                    //TreeViewItem selectedItem = treeView.ItemContainerGenerator.ContainerFromItem(curNode) as TreeViewItem;
                    //if (selectedItem != null) selectedItem.IsSelected = true;

                    //MessageBox.Show($"node={curNode.Name}  parent={curNode.Parent.Name} => IsCategory={ProductCategory.IsCategory(curNode.Parent.Name)}");
                }
            }
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
                case 0:
                    myCardCntr = new CardParamsControl();
                    ProductParams.Children.Add(myCardCntr);
                    break;
                case 1:
                    myDepCntr = new DepositParamsControl();
                    ProductParams.Children.Add(myDepCntr);
                    break;
                case 2:
                    myCreditCntr = new CreditParamsControl();
                    ProductParams.Children.Add(myCreditCntr);
                    break;
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
                case 0: 
                    products.Add(myCardCntr.card);
                    myCardCntr.card = new Card();
                    break;
                case 1:
                    products.Add(myDepCntr.dep);
                    myDepCntr.dep = new Deposit();
                    break;
                case 2:
                    products.Add(myCreditCntr.credit);
                    myCreditCntr.credit = new Credit();
                    break;
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
                case 0:
                    myCardCntr.card = null;
                    break;
                case 1:
                    myDepCntr.dep = null;
                    break;
                case 2:
                    myCreditCntr.credit = null;
                    break;
                case 3:
                    myAccCntr.acc = null;
                    break;
            }
            cmbCategory.SelectedIndex = 0;
        }

        private void OnTreeViewContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            //MessageBox.Show("OnTreeViewContextMenuOpening");
            //TreeView treeView = e.OriginalSource as TreeView;
            /*if (treeView.SelectedItem != null)
            {

            }*/
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

        public static bool IsCategory(string ct)
        {
            return cats.Contains(ct);
        }
    }
}
