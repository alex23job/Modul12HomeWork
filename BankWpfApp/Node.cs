using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWpfApp
{
    class Node
    {
        public string Name { get; set; }
        public ObservableCollection<Node> Children { get; } = new ObservableCollection<Node>();

        public Node this[string name]
        {
            get
            {
                if (name == Name) return this;
                else if (Children.Count > 0)
                {
                    Node res = null;
                    for (int i = 0; i < Children.Count; i++)
                    {
                        res = Children[i][name];
                        if (res != null) return res;
                    }
                }
                return null;
            }
        }
    }
}
