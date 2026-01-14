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
    /// Логика взаимодействия для MainTablePage.xaml
    /// </summary>
    public partial class MainTablePage : Page
    {
        private Model.Cartridge cartridgeDel = new Model.Cartridge();
        List<Model.Cartridge> cartridges = ClassesFolder.BDClass.bd.Cartridges.ToList();
        List<Model.Report> reports = ClassesFolder.BDClass.bd.Reports.ToList();

        public MainTablePage()
        {
            InitializeComponent();
            DGCatridges.ItemsSource = cartridges;
            ClassesFolder.PagesClass.mainTable = this;
        }

        #region Events
        private void Badd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool isOk = true;
                for (int i = 0; i < cartridges.Count; i++)
                {
                    if (!VerficationData(cartridges[i]))
                    {
                        isOk = false;
                    }
                }
                if (isOk)
                {
                    ClassesFolder.BDClass.bd.SaveChanges();
                    MessageBox.Show("Данные обнавленны", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    DGCatridges.Items.Refresh();
                    ClassesFolder.PagesClass.tablePage.LBTypeList.Items.Refresh();
                }
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Bstroke_Click(object sender, RoutedEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UCAdd.Visibility = Visibility.Visible;
        }

        private void Bclose_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (cartridgeDel != null)
                {
                    switch (MessageBox.Show("Вы точно хотите удалить строку", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Information))
                    {
                        case MessageBoxResult.Yes:
                            cartridges.Remove(cartridgeDel);
                            ClassesFolder.BDClass.bd.Cartridges.Remove(cartridgeDel);
                            reports = reports.Where(x => x.idCantridges == cartridgeDel.id).ToList();
                            ClassesFolder.BDClass.bd.Reports.Remove(reports[0]);
                            ClassesFolder.BDClass.bd.SaveChanges();
                            DGCatridges.Items.Refresh();
                            reports = ClassesFolder.BDClass.bd.Reports.ToList();
                            break;
                        case MessageBoxResult.No:
                            ClassesFolder.BDClass.bd.SaveChanges();
                            DGCatridges.Items.Refresh();
                            break;
                        default:
                            break;
                    }                   
                }
                else
                {
                    MessageBox.Show("Вы как вообще на это нажали?", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DGCatridges_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.Cartridge cells = (Model.Cartridge)DGCatridges.SelectedItem;
            cartridgeDel = cells;
            Bclose.Visibility = Visibility.Visible;
        }
        #endregion

        #region Methods
        private bool VerficationData(Model.Cartridge cartridge)
        {
            try
            {
                if (cartridge.number < 0 || cartridge.price < 0 || cartridge.countEmpty < 0
                    || cartridge.countFull < 0 || cartridge.countUse < 0)
                {
                    MessageBox.Show("Есть отрицательные значения.\nСтрока " + cartridge.id, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                else
                {
                    return true;
                }                
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        #endregion 
    }
}
