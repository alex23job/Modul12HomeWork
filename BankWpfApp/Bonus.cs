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
}
