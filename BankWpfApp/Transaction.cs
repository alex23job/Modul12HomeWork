using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWpfApp
{
    public class Transaction : IId
    {
        static string[] nameModes = { "dec", "inc", "transfer", "pay" };
        public BankAccount accFrom = null;
        public BankAccount accTo = null;
        public float sum = 0f;
        public string From
        {
            set
            {
                from = value;
            }
            get
            {
                if (accFrom != null)
                { 
                    return accFrom.PersonProductNumber.ToString();
                }
                return from;
            }
        }
        public string To
        {
            set
            {
                to = value;
            }
            get
            {
                if (accTo != null)
                {
                    return accTo.PersonProductNumber.ToString();
                }
                return to;
            }
        }
        string from = "";
        string to = "";
        int mode = -1;

        public string NameMode => (mode >= 0 && mode < nameModes.Length) ? nameModes[mode] : "none";

        public int UID { get; set; }

        public Transaction() { }
        public Transaction(float s, int m)
        {
            sum = s;
            mode = m;
        }

        public  void Execute()
        {
            if (accFrom != null && accFrom.Balans >= sum)
            {
                accFrom.Balans -= sum;
            }
            if (accTo != null)
            {
                accTo.Balans += sum;
            }
        }
    }
}
