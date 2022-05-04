using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Cartridge.Model
{
    public partial class Cartridge
    {
        public string NNC
        {
            get
            {
                return id +" "+ number +" "+ name;
            }
        }
    }
}
