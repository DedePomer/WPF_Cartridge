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

namespace WPF_Cartridge.ControlsPage
{
    /// <summary>
    /// Логика взаимодействия для UseControl.xaml
    /// </summary>
    public partial class UseControl : UserControl
    {

        
        public UseControl()
        {
            InitializeComponent();
        }

        #region Events
        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UCuse.Visibility = Visibility.Collapsed;
            ClassesFolder.IDCourierClass.ID = -1;
        }
        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            List<Model.Cartridge> cartridges = ClassesFolder.BDClass.bd.Cartridges.ToList();
            int id = ClassesFolder.IDCourierClass.ID;
            List<Model.Cartridge> currentCatridges = cartridges.Where(x => x.id == id).ToList();
            Model.Cartridge currentCatridge = currentCatridges[0];
            if (id - 1 < 0)
            {
                MessageBox.Show("Ошибка считывания id", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClassesFolder.MainWindowClass.mainWindow.UCuse.Visibility = Visibility.Collapsed;
            }
            else
            {
                try
                {
                    if (Convert.ToInt32(TBOXCount.Text) == 0)
                    {
                        MessageBox.Show("Поле не может быть равно 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                       
                        if (currentCatridge.countFull - Convert.ToInt32(TBOXCount.Text) < 0 || Convert.ToInt32(TBOXCount.Text) < 0)
                        {
                            MessageBox.Show("В поле не правильное число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            currentCatridge.countFull = cartridges[id - 1].countFull - Convert.ToInt32(TBOXCount.Text);
                            currentCatridge.countUse += Convert.ToInt32(TBOXCount.Text);
                            ClassesFolder.BDClass.bd.SaveChanges();
                            ClassesFolder.PagesClass.tablePage.LBTypeList.Items.Refresh();
                            MessageBox.Show("Данные сохранены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClassesFolder.MainWindowClass.mainWindow.UCuse.Visibility = Visibility.Collapsed;
                            ClassesFolder.IDCourierClass.ID = -1;
                        }
                    }
                }
                catch (Exception z)
                {
                    MessageBox.Show("Не правильные данные: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void GControls_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UCuse.Visibility = Visibility.Collapsed;
            ClassesFolder.IDCourierClass.ID = -1;
        }
        #endregion

        #region Methods

        #endregion
    }
}
