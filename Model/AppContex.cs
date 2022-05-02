using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace WPF_Cartridge.Model
{
    class AppContex: DbContext
    {
        public DbSet<Cartridge> СartridgesInfo { get; set; }

        public AppContex() : base("DefaultConnection") 
        { }
    }
}
