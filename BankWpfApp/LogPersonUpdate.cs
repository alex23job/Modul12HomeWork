using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankWpfApp
{
    public class MyLogView
    {
        public DateTime DateUpdate { get; set; }
        public string Fields { get; set; }
        public string ModeUpdate { get; set; }
        public string UserLogin { get; set; }
        public string UserPosition { get; set; }
        public MyLogView(string csvStr, string sep = "#")
        {
            string[] s = csvStr.Split(sep[0]);
            if (s.Length == 5)
            {
                DateUpdate = DateTime.Parse(s[0]);
                Fields = s[1];
                ModeUpdate = s[2];
                UserLogin = s[4];
                UserPosition = s[3];
            }
        }
    }

    public class LogPersonUpdate
    {
        DateTime DateUpdate { get; set; }
        string Fields { get; set; }
        string ModeUpdate { get; set; }
        string UserLogin { get; set; }
        string UserPosition { get; set; }

        public string CsvString
        {
            get => ToCsvString();
            set
            {
                string[] s = value.Split('#');
                if (s.Length == 5)
                {
                    DateUpdate = DateTime.Parse(s[0]);
                    Fields = s[1];
                    ModeUpdate = s[2];
                    UserLogin = s[4];
                    UserPosition = s[3];
                }
            }
        }

        public LogPersonUpdate()
        {
            DateUpdate = DateTime.Now;
        }

        public LogPersonUpdate(string fieldName, string mode, string login, string pos)
        {
            DateUpdate = DateTime.Now;
            Fields = fieldName;
            ModeUpdate = mode;
            UserLogin = login;
            UserPosition = pos;
        }

        public LogPersonUpdate(string csvStr, string sep = "#")
        {
            string[] s = csvStr.Split(sep[0]);
            if (s.Length == 5)
            {
                DateUpdate = DateTime.Parse(s[0]);
                Fields = s[1];
                ModeUpdate = s[2];
                UserLogin = s[4];
                UserPosition = s[3];
            }
        }

        public void AddField(string fieldName)
        {
            Fields += (Fields == "") ? fieldName : (", " + fieldName);
        }

        public string ToCsvString(string sep = "#")
        {
            StringBuilder sb = new StringBuilder(DateUpdate.ToString());
            sb.Append(sep + Fields);
            sb.Append(sep + ModeUpdate);
            sb.Append(sep + UserPosition);
            sb.Append(sep + UserLogin);
            return sb.ToString();
        }

        public override string ToString()
        {
            return $"{DateUpdate.ToString()} {Fields} {ModeUpdate} {UserPosition} {UserLogin}";
        }
    }

}
