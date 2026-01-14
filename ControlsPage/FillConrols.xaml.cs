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
    /// Логика взаимодействия для FillConrols.xaml
    /// </summary>
    public partial class FillConrols : UserControl
    {
        
        

        public FillConrols()
        {
            InitializeComponent();
            CBEnable.IsChecked = true;
        }

        #region Events
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            TBOXPrice.IsEnabled = false;
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TBOXPrice.IsEnabled = true;
        }
        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UT.Visibility = Visibility.Collapsed;
            ClassesFolder.IDCourierClass.ID = -1;
        }
        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            List<Model.Report> reports = ClassesFolder.BDClass.bd.Reports.ToList();
            List<Model.Cartridge> cartridges = ClassesFolder.BDClass.bd.Cartridges.ToList();
            int id = ClassesFolder.IDCourierClass.ID;
            List<Model.Cartridge> currentCatridges = cartridges.Where(x => x.id == id).ToList();
            Model.Cartridge currentCatridge = currentCatridges[0];

            if (id-1 < 0)
            {
                MessageBox.Show("Ошибка считывания id", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClassesFolder.MainWindowClass.mainWindow.UT.Visibility = Visibility.Collapsed;
            }
            else
            {
                try
                {
                    if (Convert.ToInt32(TBOXCount.Text) == 0 || Convert.ToInt32(TBOXPrice.Text) == 0)
                    {
                        MessageBox.Show("Поля не может быть равно 0", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        
                        int a = Convert.ToInt32(TBOXCount.Text);
                        int b = currentCatridge.countEmpty - Convert.ToInt32(TBOXCount.Text);
                        if ( b < 0 || a < 0 || Convert.ToInt32(TBOXPrice.Text)<0)
                        {
                            MessageBox.Show("В полях не правильное число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            
                            int indRoam = -1;
                            for (int i = 0; i < reports.Count; i++)
                            {
                                if (reports[i].idCantridges == currentCatridge.id)
                                {
                                    indRoam = i;
                                }
                            }
                            currentCatridge.countEmpty = currentCatridge.countEmpty - Convert.ToInt32(TBOXCount.Text);
                            if (indRoam != -1)
                            {
                                reports[indRoam].countSent += Convert.ToInt32(TBOXCount.Text);
                                reports[indRoam].countNotFill += Convert.ToInt32(TBOXCount.Text);
                                reports[indRoam].idCantridges = currentCatridge.id;
                                cartridges[id - 1].price = Convert.ToInt32(TBOXPrice.Text);
                                reports[indRoam].price = currentCatridge.price;
                                reports[indRoam].title = currentCatridge.NNC;
                            }
                            else
                            {
                                currentCatridge.price = Convert.ToInt32(TBOXPrice.Text);
                                ClassesFolder.BDClass.bd.Reports.Add(new Model.Report
                                {
                                    title = currentCatridge.NNC,
                                    countDefects = 0,
                                    price = Convert.ToInt32(TBOXPrice.Text),
                                    priceAll = 0,
                                    countSent = Convert.ToInt32(TBOXCount.Text),
                                    countReceived = 0,
                                    countNotFill = Convert.ToInt32(TBOXCount.Text),
                                    idCantridges = currentCatridge.id
                                });
                            }
                            ClassesFolder.BDClass.bd.SaveChanges();
                            ClassesFolder.PagesClass.tablePage.LBTypeList.Items.Refresh();
                            MessageBox.Show("Данные сохранены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClassesFolder.MainWindowClass.mainWindow.UT.Visibility = Visibility.Collapsed;
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
            ClassesFolder.MainWindowClass.mainWindow.UT.Visibility = Visibility.Collapsed;
            ClassesFolder.IDCourierClass.ID = -1;
        }

        #endregion

        #region Methods

        #endregion

        
    }
}
