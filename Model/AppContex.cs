using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;



namespace WPF_Cartridge.Model
{
    class AppContex: DbContext
    {
        public AppContex() : base("DefaultConnection")
        { }

        public DbSet<Cartridge> Cartridges { get; set; }
        public DbSet<Report> Reports { get; set; }
    }
}
