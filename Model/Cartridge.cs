using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Cartridge.Model
{
    class Cartridge
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }
       
        public Cartridge() { }
        public Cartridge(string type, int count) 
        {
            this.Type = type;
            this.Count = count;
        }

    }
}
