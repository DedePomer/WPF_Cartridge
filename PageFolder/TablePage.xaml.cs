using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Cartridge.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для TablePage.xaml
    /// </summary>
    public partial class TablePage : Page
    {
        List<Model.Cartridge> StartList = ClassesFolder.BDClass.bd.Cartridges.ToList();

        public TablePage()
        {
            InitializeComponent();
            LBTypeList.ItemsSource = StartList;
        }

        private void BFill_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BNull_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
