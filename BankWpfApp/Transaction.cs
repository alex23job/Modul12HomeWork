using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWpfApp
{
    public class Transaction : IId
    {
        public BankAccount accFrom = null;
        public BankAccount accTo = null;
        public float sum = 0f;

        public int UID { get; set; }
    }
}
