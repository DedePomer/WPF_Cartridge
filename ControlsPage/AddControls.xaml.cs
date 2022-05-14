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
    /// Логика взаимодействия для AddControls.xaml
    /// </summary>
    public partial class AddControls : UserControl
    {

        List<Model.Cartridge> cartridges = new List<Model.Cartridge>();
        public AddControls()
        {
            InitializeComponent();
            TBOXcolor.Text = "black";
            TBOXCountFill.Text = 0+"";
            TBOXCountNull.Text = 0 + "";
            TBOXCountUse.Text = 0 + "";
            TBOXName.Text = "Нет";
            TBOXNumber.Text = 0 + "";
            TBOXPrice.Text = 0 + "";
        }



        #region Events

        private void GControls_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UCAdd.Visibility = Visibility.Collapsed;
        }

        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NegativeValue(Convert.ToInt32(TBOXCountFill.Text)) || NegativeValue(Convert.ToInt32(TBOXCountNull.Text)) || NegativeValue(Convert.ToInt32(TBOXCountUse.Text))
                    || NegativeValue(Convert.ToInt32(TBOXNumber.Text)) || NegativeValue(Convert.ToInt32(TBOXPrice.Text)))
                {
                    ClassesFolder.BDClass.bd.Cartridges.Add(new Model.Cartridge
                    {
                        number = Convert.ToInt32(TBOXNumber.Text),
                        name = TBOXName.Text,
                        color = TBOXcolor.Text,
                        countFull = Convert.ToInt32(TBOXCountFill.Text),
                        countEmpty= Convert.ToInt32(TBOXCountNull.Text),
                        countUse = Convert.ToInt32(TBOXCountUse.Text),
                        price = Convert.ToInt32(TBOXPrice.Text)
                    }) ;
                    ClassesFolder.BDClass.bd.SaveChanges();
                    MessageBox.Show("Данные добаленны", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    cartridges = ClassesFolder.BDClass.bd.Cartridges.ToList();
                    ClassesFolder.PagesClass.mainTable.DGCatridges.ItemsSource = cartridges;
                    ClassesFolder.PagesClass.mainTable.DGCatridges.Items.Refresh();
                    ClassesFolder.MainWindowClass.mainWindow.UCAdd.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Естиь отрицательные значения", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception z)
            {
                MessageBox.Show("Не правильные данные: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UCAdd.Visibility = Visibility.Collapsed;
        }

        #endregion

        #region Methods
        private bool NegativeValue(int number)
        {
            if (number < 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
