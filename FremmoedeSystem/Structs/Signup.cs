using FremmødeSystem.APIs;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;

namespace FremmødeSystem.Structs {
    

    public class Signup {
        private string sheetName = "Forbedret fremmøde";
        private string column = "G";
        private string range = "Holdfast!F3:F300";

        private string rang = "Rekrut";
        private string navn = "";
        private string formular = "";
        private DateTime joined = DateTime.MinValue;
        private string status = "Aktiv";
        private int fremmøde = 0;
        private const int startIndexOfData = 3;

        public int rowNumber = 1; //sheets index starts at 1

        public Signup() {
            formular = $"=B9-F{GetRowNumber()}&\" Dage\"";
        }

        public Signup(string rang, string navn, DateTime joined, string formular, string status, int fremmøde) {
            this.rang = rang ;
            this.navn = navn ;
            this.joined = joined ;
            this.formular = formular ;
            this.status = status ;
            this.rowNumber = GetRowNumber();
        }

        public string Rang { get; set; } = "Rekrut";
        public string Navn { get; set; } = "";
        public DateTime Joined { get; set; } = DateTime.Today;
        public string Status { get; set; } = "Aktiv";
        public int Fremmøde { get; set; } = 0;
        public string Formular {
            get {
                return formular;
            }
            set {
                formular = value;
            }
        }

        /*
        public string Rang { get { return rang; } set { rang = value; } }
        public string Navn { get { return navn; } set { navn = value; } }
        public DateTime Joined { get { return joined; } set { joined = value; } }

        public string Status { get { return status; } set { status = value; } }
        public int Fremmøde { get{ return fremmøde; } set { fremmøde = value; } }
        */

        public int GetRowNumber() {
            var request = SheetController.getRequest(range);
            if(request == null) {
                throw new Exception("Request Failed");
            }
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var execution = SheetController.getRequest(range).Execute();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            IList < IList<object> > values = execution.Values;

            if(values != null) {
                return values.Count + startIndexOfData;
            }
            if(rowNumber == 1 && values.Count > 0) {
                rowNumber  = values.Count + 1;
            }
            return rowNumber;
        }


    }
}
