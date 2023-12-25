using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для StatOperationsWindow.xaml
    /// </summary>
    public partial class StatOperationsWindow : Window
    {
        string pathLogOperations = "LogOperations.csv";
        LogOperations log = null;
        UserData currentUser = null;
        Person currentPerson = null;
        ObservableCollection<Person> persons = null;
        ObservableCollection<Product> bankProducts = null;
        List<OneOperation> listOperations = null;
        public StatOperationsWindow()
        {
            InitializeComponent();
        }

        public void SetRepository(ObservableCollection<Person> pers, ObservableCollection<Product> bp)
        {
            persons = pers;
            bankProducts = bp;
        }

        public void SetUser(UserData user, string pathLog = "")
        {
            currentUser = user;
            if (pathLog != "")
            {
                pathLogOperations = pathLog;
            }
            log = new LogOperations(pathLogOperations);
            if (log.Load())
            {
                CreateTreePersons();
            }
        }

        public void SetPerson(Person pers, string pathLog = "")
        {
            currentPerson = pers;
            if (pathLog != "")
            {
                pathLogOperations = pathLog;
            }
            log = new LogOperations(pathLogOperations);

            if (log.Load())
            {
                listOperations = log.GetSortedList(2, currentPerson.UID.ToString());
                ViewListOperations();

                if (listOperations != null)
                {
                    CreateTreeItemsForPerson();
                    int mode = 1;
                    if (Inc.IsChecked == true) mode = 2;
                    Dictionary<string, float> dict = GetDictCatOper(listOperations, mode);
                    //List<float> data = new List<float>() { 10, 20, 45, 18, 32 };
                    List<float> data = new List<float>();
                    List<string> legend = new List<string>();
                    foreach (var v in dict.Values) data.Add(v);
                    foreach (var l in dict.Keys) legend.Add(l);
                    //DrawPieDiagramm((List<float>));
                    DrawPieDiagramm(data, legend);
                }
            }
        }

        /// <summary>
        /// Формирование словаря сумм по категориям
        /// </summary>
        /// <param name="op">Список операций</param>
        /// <param name="mode">1 - расходы, 2 - поступления</param>
        /// <returns>словарь сумм по категориям</returns>
        private Dictionary<string, float> GetDictCatOper(List<OneOperation> opers, int mode)
        {
            Dictionary<string, float> res = new Dictionary<string, float>();
            foreach(OneOperation op in opers)
            {
                Product bpTo = GetBankProductFromUID(op.ToAccountUID);
                Product bpFr = GetBankProductFromUID(op.FromAccountUID);
                string cat = "";
                float sum;
                string strSum = op.GetSumma().Substring(0, op.GetSumma().Length - 2);
                if (float.TryParse(strSum, out sum) == false) continue;
                if (op.GetMode() == "pay" && mode == 1)
                {
                    BankAccount ba = bpTo as BankAccount;
                    if (ba != null)
                    {
                        LegalPerson lp = GetPersonFromUID(ba.personUID.ToString()) as LegalPerson;
                        if (lp != null)
                        {
                            cat = lp.LegalCategoty;
                            res[cat] = res.ContainsKey(cat) ? (res[cat] + sum) : sum;
                        }
                    }
                }
                if (op.GetMode() == "dec" && mode == 1)
                {
                    cat = "Наличные";
                    if (op.ToAccountUID.Substring(0, 4) == "Банк")
                    {
                        res[cat] = res.ContainsKey(cat) ? (res[cat] + sum) : sum;
                    }
                }
                if (op.GetMode().Substring(0, 3) == "inc" && mode == 2)
                {
                    cat = "Зачисление";
                    if (op.FromAccountUID.Substring(0, 4) == "Банк")
                    {
                        res[cat] = res.ContainsKey(cat) ? (res[cat] + sum) : sum;
                    }
                }
                if (op.GetMode() == "transfer")
                {
                    cat = "Перевод";
                    Person pers = currentPerson;
                    if (pers == null)
                    {
                        pers = GetPersonFromUID(op.UpdatingUID);
                    }
                    if (pers != null)
                    {
                        if (mode == 1)
                        {
                            if (pers.IdProducts.Contains(bpTo.UID) == false)
                            {
                                res[cat] = res.ContainsKey(cat) ? (res[cat] + sum) : sum;
                            }
                        }
                        if (mode == 2)
                        {
                            if (pers.IdProducts.Contains(bpFr.UID) == false)
                            {
                                res[cat] = res.ContainsKey(cat) ? (res[cat] + sum) : sum;
                            }
                        }
                    }
                }
            }
            return res;
        }

        private void CreateTreePersons()
        {
            ObservableCollection<Node> tree = new ObservableCollection<Node>();
            string category = "";
            foreach (Person pers in persons)
            {
                string nm = pers.FIO;
                Node rootNode = new Node() { Name = nm };
                foreach (long id in pers.IdProducts)
                {
                    Product bp = GetBankProductFromUID(id.ToString());
                    if (bp != null)
                    {
                        IProductType productType = bp as IProductType;
                        if (productType != null)
                        {
                            switch (productType.Type)
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
                                curNode.Children.Add(new Node($"{bp.UID} {bp.Name}", curNode));
                            }
                        }
                    }
                }
                tree.Add(rootNode);
            }
            treeView.ItemsSource = tree;
        }

        private void CreateTreeItemsForPerson()
        {
            Node rootNode = new Node() { Name = "Все продукты" };
            string category = "";
            foreach(long id in currentPerson.IdProducts)
            {
                Product bp = GetBankProductFromUID(id.ToString());
                if (bp != null)
                {
                    IProductType productType = bp as IProductType;
                    if (productType != null)
                    {
                        switch (productType.Type)
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
                            curNode.Children.Add(new Node($"{bp.UID} {bp.Name}", curNode));
                        }
                    }
                }
            }

            if (rootNode != null)
            {
                treeView.ItemsSource = new ObservableCollection<Node>() { rootNode };
            }

        }

        private void ViewListOperations()
        {
            List<OperationsInfo> sorce = new List<OperationsInfo>();
            foreach (OneOperation op in listOperations)
            {
                string pathLogo = "";
                string znak = "+";
                string nameTo = "";
                string nameFr = "";
                string cat = "None";
                Product bpTo = GetBankProductFromUID(op.ToAccountUID);
                Product bpFr = GetBankProductFromUID(op.FromAccountUID);
                if (bpFr != null)
                {
                    nameFr = bpFr.Name;
                }
                else
                {
                    nameFr = op.FromAccountUID;
                }
                if (op.GetMode() == "pay")
                {
                    znak = "-";
                    BankAccount ba = bpTo as BankAccount;
                    if (ba != null)
                    {
                        LegalPerson lp = GetPersonFromUID(ba.personUID.ToString()) as LegalPerson;
                        if (lp != null)
                        {
                            nameTo = lp.LegalName;
                            pathLogo = MainWindow.startupPath + "\\" + MainWindow.logoImgPath + "\\" + lp.LogoPath;
                            cat = lp.LegalCategoty;
                        }
                    }
                }
                if (op.ToAccountUID == "Банк" || op.FromAccountUID == "Банк")
                {
                    pathLogo = MainWindow.startupPath + "\\" + MainWindow.logoImgPath + "\\Bank.jpg";
                }
                if (op.ToAccountUID == "Банкомат" || op.FromAccountUID == "Банкомат")
                {
                    pathLogo = MainWindow.startupPath + "\\" + MainWindow.logoImgPath + "\\BankMachine.jpg";
                }
                sorce.Add(new OperationsInfo(pathLogo, nameFr, znak + op.GetSumma(), nameTo, cat));
            }
            listViewInfo.ItemsSource = sorce;

        }

        private void DrawPieDiagramm(List<float> data, List<string> legend)
        {
            var sum = data.Sum();
            var angles = data.Select(d => d * 2.0 * Math.PI / sum);
            var radius = 80.0;
            var startAngle = 0.0;
            var centerPoint = new Point(radius + 50.0f, radius + 20.0f);
            var xyradius = new Size(radius, radius);
            int nBr = 0;
            StackPanel[] aSp = new StackPanel[12] { sp1, sp2, sp3, sp4, sp5, sp6, sp7, sp8, sp9, sp10, sp11, sp12 };
            for (int i = 0; i < aSp.Length; i++) aSp[i].Children.Clear();

            if (data.Count == 0)
            {
                Label l = new Label();
                l.Content = "Нет операций";
                aSp[0].Children.Add(l);
                canvasDiagramm.Children.Clear();
                return;
            }

            if (data.Count == 1)
            {
                SolidColorBrush sbr = new SolidColorBrush(Color.FromRgb((byte)((100 * (1 + nBr)) % 255), (byte)((70 * nBr) % 255), (byte)((170 * (2 + nBr)) % 255)));
                Ellipse el = new Ellipse();
                el.Fill = sbr;
                el.Stroke = Brushes.AliceBlue;
                el.StrokeThickness = 2;
                el.Width = 2 * radius;
                el.Height = 2 * radius;
                el.SetValue(Canvas.LeftProperty, 50.0);
                el.SetValue(Canvas.TopProperty, 20.0);
                el.HorizontalAlignment = HorizontalAlignment.Center;
                el.VerticalAlignment = VerticalAlignment.Center;
                canvasDiagramm.Children.Add(el);

                Rectangle r = new Rectangle();
                r.Fill = sbr;
                r.Width = 10;
                r.Height = 10;
                aSp[0].Children.Add(r);
                Label l = new Label();
                l.Content = $"{legend[0]} {data[0]:0.00} \x20BD";
                aSp[0].Children.Add(l);
            }
            else
            {
                foreach (var angle in angles)
                {
                    var endAngle = startAngle + angle;

                    var startPoint = centerPoint;
                    startPoint.Offset(radius * Math.Cos(startAngle), radius * Math.Sin(startAngle));

                    var endPoint = centerPoint;
                    endPoint.Offset(radius * Math.Cos(endAngle), radius * Math.Sin(endAngle));

                    var angleDeg = angle * 180.0 / Math.PI;
                    SolidColorBrush sbr = new SolidColorBrush(Color.FromRgb((byte)((100 * (1 + nBr)) % 255), (byte)((70 * nBr) % 255), (byte)((170 * (2 + nBr)) % 255)));

                    System.Windows.Shapes.Path p = new System.Windows.Shapes.Path()
                    {
                        Stroke = Brushes.AliceBlue,
                        StrokeThickness = 2,
                        Fill = sbr,
                        Data = new PathGeometry(
                            new PathFigure[]
                            {
                            new PathFigure(
                                centerPoint,
                                new PathSegment[]
                                {
                                    new LineSegment(startPoint, isStroked: true),
                                    new ArcSegment(endPoint, xyradius,
                                                   angleDeg, angleDeg > 180,
                                                   SweepDirection.Clockwise, isStroked: true)
                                },
                                closed: true)
                            })
                    };
                    canvasDiagramm.Children.Add(p);

                    aSp[nBr].Children.Clear();
                    Rectangle r = new Rectangle();
                    r.Fill = sbr;
                    r.Width = 10;
                    r.Height = 10;
                    aSp[nBr].Children.Add(r);
                    Label l = new Label();
                    l.Content = $"{legend[nBr]} {data[nBr]:0.00} \x20BD"; //legend[nBr];
                    aSp[nBr].Children.Add(l);

                    startAngle = endAngle;
                    nBr++;
                }
            }
        }

        private Product GetBankProductFromUID(string id)
        {
            foreach(Product p in bankProducts)
            {
                IPersonProductNumber ipn = p as IPersonProductNumber;
                if (ipn != null && ipn.PersonProductNumber.ToString() == id)
                {
                    return p;
                }
            }
            return null;
        }

        private Person GetPersonFromUID(string id)
        {
            foreach(Person p in persons)
            {
                if (p.UID.ToString() == id)
                {
                    return p;
                }
            }
            return null;
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Node nNode = e.NewValue as Node;
            if (nNode != null)
            {
                string[] arrStr = nNode.Name.Split(' ');
                if (arrStr.Length > 1)
                {
                    Product bp = GetBankProductFromUID(arrStr[0]);
                    string UID = arrStr[0];
                    BankCard bc = bp as BankCard;
                    if (bc != null && !bc.IsRequest)
                    {
                        UID = bc.CardAccount.PersonProductNumber.ToString();
                    }
                    BankDeposit bd = bp as BankDeposit;
                    if (bd != null)
                    {
                        UID = bd.DepositAccount.PersonProductNumber.ToString();
                    }
                    BankCredit bcr = bp as BankCredit;
                    if (bcr != null && !bcr.IsRequest)
                    {
                        UID = bcr.CreditAccount.PersonProductNumber.ToString();
                    }
                    listOperations = log.GetSortedList(3, UID);
                    ViewListOperations();
                    int mode = 1;
                    if (Inc.IsChecked == true) mode = 2;
                    Dictionary<string, float> dict = GetDictCatOper(listOperations, mode);
                    List<float> data = new List<float>();
                    List<string> legend = new List<string>();
                    foreach (var v in dict.Values) data.Add(v);
                    foreach (var l in dict.Keys) legend.Add(l);
                    DrawPieDiagramm(data, legend);
                }
            }
        }

        private void OnIncClick(object sender, RoutedEventArgs e)
        {
            if (listOperations != null)
            {
                int mode = 2;
                Dictionary<string, float> dict = GetDictCatOper(listOperations, mode);
                List<float> data = new List<float>();
                List<string> legend = new List<string>();
                foreach (var v in dict.Values) data.Add(v);
                foreach (var l in dict.Keys) legend.Add(l);
                //DrawPieDiagramm((List<float>));
                DrawPieDiagramm(data, legend);
            }
        }

        private void OnDecClick(object sender, RoutedEventArgs e)
        {
            if (listOperations != null)
            {
                int mode = 1;
                if (Inc.IsChecked == true) mode = 2;
                Dictionary<string, float> dict = GetDictCatOper(listOperations, mode);
                List<float> data = new List<float>();
                List<string> legend = new List<string>();
                foreach (var v in dict.Values) data.Add(v);
                foreach (var l in dict.Keys) legend.Add(l);
                //DrawPieDiagramm((List<float>));
                DrawPieDiagramm(data, legend);
            }
        }
    }

    public class OperationsInfo
    {
        public string Img { get; set; }
        public string NameProduct { get; set; }
        public string StrSumma { get; set; }
        public string LegalName { get; set; }
        public string Category { get; set; }

        public OperationsInfo() { }
        public OperationsInfo(string path, string namePr, string sum, string legalNm, string cat)
        {
            Img = path;
            NameProduct = namePr;
            StrSumma = sum;
            LegalName = legalNm;
            Category = cat;
        }
    }
}
