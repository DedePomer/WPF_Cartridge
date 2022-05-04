using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Cartridge.Model
{
    class Cartridge
    {

        #region Properties
        public int id { get; set; }
        public int number { get; set; }
        public string name { get; set; }
        public string color { get; set; }
        public int countFull { get; set; }
        public int countEmpty { get; set; }
        #endregion

        #region Constructors
        public Cartridge() { }
        public Cartridge(int number, string name, 
            string color, int countFull, int countEmpty)
        {
            this.number = number;
            this.name = name;
            this.color = color;
            this.countFull = countFull;
            this.countEmpty = countEmpty;
        }
        #endregion
    }
}
