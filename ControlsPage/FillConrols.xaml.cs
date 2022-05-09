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
        }

        #region Events
        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UT.Visibility = Visibility.Collapsed;
            ClassesFolder.IDCourierClass.ID = -1;
        }
        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {          
            int id = ClassesFolder.IDCourierClass.ID;
            if (id-1 < 0)
            {
                MessageBox.Show("Ошибка считывания id", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClassesFolder.MainWindowClass.mainWindow.UT.Visibility = Visibility.Collapsed;
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
                        List<Model.Cartridge> cartridges = ClassesFolder.BDClass.bd.Cartridges.ToList();
                        int a = Convert.ToInt32(TBOXCount.Text);
                        int b = cartridges[id - 1].countEmpty - Convert.ToInt32(TBOXCount.Text);
                        if ( b < 0 || a < 0)
                        {
                            MessageBox.Show("В поле не правильное число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            List<Model.Report> reports = ClassesFolder.BDClass.bd.Reports.ToList();
                            int indRoam = -1;
                            for (int i = 0; i < reports.Count; i++)
                            {
                                if (reports[i].title == cartridges[id - 1].NNC)
                                {
                                    indRoam = i;
                                }
                            }
                            cartridges[id - 1].countEmpty = cartridges[id - 1].countEmpty - Convert.ToInt32(TBOXCount.Text);
                            if (indRoam != -1)
                            {
                                reports[indRoam].countSent += Convert.ToInt32(TBOXCount.Text);
                            }
                            else
                            {
                                ClassesFolder.BDClass.bd.Reports.Add(new Model.Report
                                {
                                    title = cartridges[id - 1].NNC,
                                    countDefects = 0,
                                    price = 0,
                                    priceAll = 0,
                                    countSent = Convert.ToInt32(TBOXCount.Text),
                                    countReceived = 0
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
        #endregion

        #region Methods

        #endregion
    }
}
