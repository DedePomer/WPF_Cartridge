using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Cartridge.Model
{
    class Cartridge
    {
        public int id { get; set; }
        private string type { get; set; }
        private int count { get; set; }


        public string Type
        {
            get 
            {
                return type;
            }
            set 
            {
                type = value;
            }
        }
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }

        public Cartridge() { }
        public Cartridge(string Type, int Count) 
        {
            this.Type = Type;
            this.Count = Count;
        }

    }
}
