using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWpfApp
{
    public class Bonus
    {
        private float _percent = 1f;
        public float Percent
        {
            get => _percent;
            set
            {
                if (value >= 0 && value <= 100f) _percent = value;
            }
        }

        public Bonus() { }
        public Bonus(float p)
        {
            Percent = p;
        }

        public override string ToString()
        {
            return $"{Percent:0.00}%";
        }
    }

    public class BonusAction : Bonus
    {
        /// <summary>
        /// количество покупок по акции
        /// </summary>
        public int Count { get; set; } = 1;
        /// <summary>
        /// минимальная сумма покупки, начиная с которой начисляется кешбек
        /// </summary>
        public float MinSumma { get; set; } = 0;
        /// <summary>
        /// максимальная сумма покупки, на которую начисляется кешбек
        /// </summary>
        public float MaxSumma { get; set; } = 1000f;
        /// <summary>
        /// дата начала в формате гггг.мм.дд
        /// </summary>
        public string BeginPeriod { get; set; } = "";
        /// <summary>
        /// дата окончания в формате гггг.мм.дд
        /// </summary>
        public string EndPeriod { get; set; } = "";

        public BonusAction() { }
        public BonusAction(float p, int c, float minSum, float maxSum, string beg, string end) : base(p)
        {
            Count = c;
            MinSumma = minSum;
            MaxSumma = maxSum;
            BeginPeriod = beg;
            EndPeriod = end;
        }

        public string GetStrPeriod()
        {
            string[] aB = BeginPeriod.Split('.');
            string[] aE = EndPeriod.Split('.');
            if (aB.Length == 3 && aE.Length == 3)
            {
                return $"{aB[2]}.{aB[1]}.{aB[0]} - {aE[2]}.{aE[1]}.{aE[0]}   {base.ToString()}";
            }
            return base.ToString();
        }

        public string GetStrSumma()
        {
            string res = "";
            if (MinSumma != 0) res += $"от {MinSumma:0.00}";
            if (MaxSumma != 0)
            {
                if (res != "") res += " ";
                res += $"до {MaxSumma:0.00}";
            }
            return res;
        }
    }

    public class BonusActionPerson : BonusAction
    {
        public int currentCount = 0;
        public int LegalPersonUID = -1;
        public BonusActionPerson() { }
        public BonusActionPerson(BonusAction b, int lpUID)
        {
            CopyParams(b);
            LegalPersonUID = lpUID;
        }

        public void CopyParams(BonusAction ba)
        {
            this.BeginPeriod = ba.BeginPeriod;
            this.EndPeriod = ba.EndPeriod;
            this.MinSumma = ba.MinSumma;
            this.MaxSumma = ba.MaxSumma;
            this.Percent = ba.Percent;
            this.Count = ba.Count;
            currentCount = ba.Count;
        }
        public float GetCashBack(float sum)
        {
            if (currentCount == 0) return 0;
            if (TestPeriod() == false) return 0;
            float res = 0;
            if (sum > MinSumma)
            {
                if (MaxSumma > 0)
                {
                    res = (float)Math.Round((sum < MaxSumma ? sum : MaxSumma) * Percent / 100, 2);
                    currentCount--;
                    return res;
                }
                res = (float)Math.Round(sum * Percent / 100, 2);
                currentCount--;
                return res;
            }
            return res;
        }

        private bool TestPeriod()
        {
            DateTime dt = DateTime.Now;
            string[] beg = BeginPeriod.Split('.');
            string[] end = EndPeriod.Split('.');
            if (beg.Length == 3 && end.Length == 3)
            {
                int year = int.Parse(beg[0]);
                int month = int.Parse(beg[1]);
                int day = int.Parse(beg[2]);
                DateTime begDate = new DateTime(year, month, day);
                year = int.Parse(end[0]);
                month = int.Parse(end[1]);
                day = int.Parse(end[2]);
                DateTime endDate = new DateTime(year, month, day);
                if (dt >= begDate && dt <= endDate) return true;
            }
            return false;
        }
    }
}
