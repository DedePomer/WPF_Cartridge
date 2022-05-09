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
            ClassesFolder.PagesClass.tablePage = this;
        }

        #region Events
        private void BFill_Click(object sender, RoutedEventArgs e)
        {
            
            Button button = (Button)sender;
            ClassesFolder.IDCourierClass.ID = Convert.ToInt32(button.Uid);
            ClassesFolder.ControlsClass.fillControl.TBOXCount.Text = StartList[Convert.ToInt32(button.Uid) - 1].countEmpty + "";
            ClassesFolder.MainWindowClass.mainWindow.UT.Visibility = Visibility.Visible;           
        }
        private void BNull_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BUse_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BDeffect_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region Methods

        #endregion
    }
}
