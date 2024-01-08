using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public void Execute()
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

        public void PayExecute(ObservableCollection<Person> persons, ObservableCollection<Product> products)
        {
            Execute();
            Person payPers = null;
            LegalPerson lp = null;
            BankCard bc = null;
            foreach(Product pr in products)
            {
                bc = pr as BankCard;
                if (bc != null)
                {
                    if (bc.CardAccount != null && bc.CardAccount.PersonProductNumber == accFrom.PersonProductNumber)
                    {
                        break;
                    }
                    else bc = null;
                }
            }
            if (accTo != null)
            {
                foreach(Person pers in persons)
                {
                    if (pers.UID == accTo.personUID)
                    {
                        lp = pers as LegalPerson;
                        if (payPers != null) break;
                    }
                    if (pers.UID == accFrom.personUID)
                    {
                        payPers = pers;
                        if (lp != null) break;
                    }
                }
            }
            if (bc != null && lp != null && payPers != null)
            {
                if (lp.MyBonusAction != null)
                {
                    BonusActionPerson bap = payPers.GetBonusAction(lp.UID);
                    if (bap == null)
                    {
                        if (lp.MyBonusAction.TestPeriod())
                        {
                            bap = new BonusActionPerson(lp.MyBonusAction, lp.UID);
                            payPers.ArrBonusActions.Add(bap);
                        }
                    }
                    if (bap != null)
                    {
                        float cashback = bap.GetCashBack(sum);
                        if (cashback > 0)
                        {
                            bc.CashbackBalance += cashback;
                            return;
                        }
                    }
                }
                if (payPers.BonusCategory > 0)
                {
                    int indexCat = LegalPerson.GetIndexCategory(lp.LegalCategoty);
                    if (indexCat >= 0 && indexCat < LegalPerson.category.Length)
                    {
                        if ((payPers.BonusCategory & (1 << indexCat)) > 0)
                        {
                            bc.CashbackBalance += (float)Math.Round(sum * lp.MyBonus.Percent / 100f, 2);
                            return;
                        }
                    }
                    if (indexCat == -1 && (payPers.BonusCategory & (1 << 10)) > 0)
                    {   //  установлены все покупки 1%
                        bc.CashbackBalance += (float)Math.Round(sum / 100f, 2);
                    }
                }
            }
        }
    }
}
