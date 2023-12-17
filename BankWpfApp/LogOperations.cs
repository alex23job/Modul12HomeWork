using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWpfApp
{
    public class LogOperations
    {
        string pathToLog = "LogOperations.csv";
        List<OneOperation> arr = new List<OneOperation>();

        public List<OneOperation> AllItems => arr;

        public LogOperations() { }
        public LogOperations(string path)
        {
            pathToLog = path;
        }

        public bool Load(string path = "")
        {
            string filePath = path == "" ? pathToLog : path;
            if (File.Exists(filePath))
            {
                string[] arrStr = File.ReadAllLines(filePath);
                arr.Clear();
                for (int i = 0; i < arrStr.Length; i++)
                {
                    arr.Add(new OneOperation(arrStr[i]));
                }
                return true;
            }
            return false;
        }

        public void SaveOneOption(OneOperation op, string path = "")
        {
            string filePath = path == "" ? pathToLog : path;
            using (StreamWriter outputFile = new StreamWriter(filePath, true))
            {
                outputFile.WriteLine(op.ToCsvString());
            }
        }

        /// <summary>
        /// Получить список операций по UID сотрудника, клиента, счёта
        /// </summary>
        /// <param name="mode">режим отбора: 1 - сотрудник, 2 - клиент, 3 - счёт</param>
        /// <param name="UID">Login сотрудника, UID клиента или счёта</param>
        /// <returns>список операций</returns>
        public List<OneOperation> GetSortedList(int mode, string UID)
        {
            List<OneOperation> res = new List<OneOperation>();
            int i;
            switch(mode)
            {
                case 1:
                    for (i = 0; i < arr.Count; i++)
                    {
                        if (arr[i].UserLogin == UID)
                        {
                            res.Add(arr[i]);
                        }
                    }
                    break;
                case 2:
                    for (i = 0; i < arr.Count; i++)
                    {
                        if (arr[i].UpdatingUID == UID)
                        {
                            res.Add(arr[i]);
                        }
                    }
                    break;
                case 3:
                    for (i = 0; i < arr.Count; i++)
                    {
                        if (arr[i].FromAccountUID == UID || arr[i].ToAccountUID == UID)
                        {
                            res.Add(arr[i]);
                        }
                    }
                    break;
            }
            return res;
        }
    }

    public class OneOperation
    { 
        DateTime DateUpdate { get; set; }

        /// <summary>
        /// сумма денег
        /// </summary>
        string AmountOfMoney { get; set; }

        public string GetSumma()
        {
            return $"{AmountOfMoney} Р";
        }

        /// <summary>
        /// billingUp, withdraw, transfer (пополнить, снять, перевести)
        /// </summary>
        string ModeUpdate { get; set; }

        public string GetMode()
        {
            return ModeUpdate;
        }

        /// <summary>
        /// логин сотрудника
        /// </summary>
        public string UserLogin { get; set; }

        /// <summary>
        /// должность сотрудника
        /// </summary>
        string UserPosition { get; set; }

        /// <summary>
        /// UID клиента, для которого выполнена операция
        /// </summary>
        public string UpdatingUID { get; set; }

        /// <summary>
        /// UID счёта списания (может быть банкомат)
        /// </summary>
        public string FromAccountUID { get; set; }

        /// <summary>
        /// UID счёта зачисления (может быть банкомат)
        /// </summary>
        public string ToAccountUID { get; set; }

        public string CsvString
        {
            get => ToCsvString();
            set
            {
                string[] s = value.Split('#');
                if (s.Length >= 8)
                {
                    DateUpdate = DateTime.Parse(s[0]);
                    AmountOfMoney = s[1];
                    ModeUpdate = s[2];
                    UserLogin = s[4];
                    UserPosition = s[3];
                    UpdatingUID = s[5];
                    FromAccountUID = s[6];
                    ToAccountUID = s[7];
                }
            }
        }

        public OneOperation()
        {
            DateUpdate = DateTime.Now;
        }

        public OneOperation(string sum, string mode, string login, string pos, string id, string from, string to)
        {
            DateUpdate = DateTime.Now;
            AmountOfMoney = sum;
            ModeUpdate = mode;
            UserLogin = login;
            UserPosition = pos;
            UpdatingUID = id;
            FromAccountUID = from;
            ToAccountUID = to;
        }

        public OneOperation(string csvStr, string sep = "#")
        {
            string[] s = csvStr.Split(sep[0]);
            if (s.Length >= 8)
            {
                DateUpdate = DateTime.Parse(s[0]);
                AmountOfMoney = s[1];
                ModeUpdate = s[2];
                UserLogin = s[4];
                UserPosition = s[3];
                UpdatingUID = s[5];
                FromAccountUID = s[6];
                ToAccountUID = s[7];
            }
        }

        public string ToCsvString(string sep = "#")
        {
            StringBuilder sb = new StringBuilder(DateUpdate.ToString());
            sb.Append(sep + AmountOfMoney);
            sb.Append(sep + ModeUpdate);
            sb.Append(sep + UserPosition);
            sb.Append(sep + UserLogin);
            sb.Append(sep + UpdatingUID);
            sb.Append(sep + FromAccountUID);
            sb.Append(sep + ToAccountUID);
            return sb.ToString();
        }

        public override string ToString()
        {
            return $"{DateUpdate.ToString()} {AmountOfMoney} {ModeUpdate} {UserPosition} {UserLogin} {UpdatingUID} {FromAccountUID} {ToAccountUID}";
        }
    }
}
