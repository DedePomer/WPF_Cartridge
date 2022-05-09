
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Cartridge.Model
{
    public partial class  Report
    {
        #region Properties
        public int id { get; set; }
        public string title { get; set;}
        public int countSent { get; set; }
        public float price { get; set; }
        public float priceAll { get; set; }
        public int countReceived { get; set; }
        public int countDefects { get; set; }
        
        #endregion

        #region Constructors
        public Report() { }
        public Report(string title, int countSent, 
            float price, int countReceived, int countDefects, float priceAll)
        {
            this.title = title;
            this.countSent = countSent;
            this.price = price;
            this.countReceived = countReceived;
            this.countDefects = countDefects;
            this.priceAll = priceAll;
        }
        #endregion
    }
}
