using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BankWpfApp
{
    [XmlRoot("credit")]
    public class Credit : Product, IProductType
    {
        public int Type { get; set; } = 2;
    }
}
