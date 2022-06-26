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
    /// Логика взаимодействия для DeffectedControls.xaml
    /// </summary>
    public partial class DeffectedControls : UserControl
    {

        
        public DeffectedControls()
        {
            InitializeComponent();
        }

        #region Events
        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UCDefect.Visibility = Visibility.Collapsed;
            ClassesFolder.IDCourierClass.ID = -1;
        }
        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            List<Model.Report> reports = ClassesFolder.BDClass.bd.Reports.ToList();
            List<Model.Cartridge> cartridges = ClassesFolder.BDClass.bd.Cartridges.ToList();
            int id = ClassesFolder.IDCourierClass.ID;
            List<Model.Cartridge> currentCatridges = cartridges.Where(x => x.id == id).ToList();
            Model.Cartridge currentCatridge = currentCatridges[0];

            if (id - 1 < 0)
            {
                MessageBox.Show("Ошибка считывания id", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                ClassesFolder.MainWindowClass.mainWindow.UCDefect.Visibility = Visibility.Collapsed;
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
                        
                        int a = Convert.ToInt32(TBOXCount.Text);
                        int b = currentCatridge.countFull - Convert.ToInt32(TBOXCount.Text);
                        if (b < 0 || a < 0) //проверка на то что поле пустое или количкство пустых катриджей больше чем значение поля
                        {
                            MessageBox.Show("В поле не правильное число", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                            if (indRoam != -1)
                            {                                    
                                    reports[indRoam].countDefects += Convert.ToInt32(TBOXCount.Text);
                                    reports[indRoam].countNotDef += Convert.ToInt32(TBOXCount.Text);
                                    currentCatridge.countFull -= Convert.ToInt32(TBOXCount.Text);
                                    //cartridges[indRoam].countFull -= Convert.ToInt32(TBOXCount.Text);
                                    ClassesFolder.BDClass.bd.SaveChanges();
                                    ClassesFolder.PagesClass.tablePage.LBTypeList.Items.Refresh();
                                    MessageBox.Show("Данные сохранены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ClassesFolder.MainWindowClass.mainWindow.UCDefect.Visibility = Visibility.Collapsed;
                                    ClassesFolder.IDCourierClass.ID = -1;
                            }
                            else
                            {

                                cartridges[id - 1].countFull -= Convert.ToInt32(TBOXCount.Text);
                                ClassesFolder.BDClass.bd.SaveChanges();
                                ClassesFolder.BDClass.bd.Reports.Add(new Model.Report
                                {
                                    title = currentCatridge.NNC,
                                    countDefects = Convert.ToInt32(TBOXCount.Text),
                                    price = 0,
                                    priceAll = 0,
                                    countSent = 0,
                                    countReceived = 0,
                                    idCantridges = currentCatridge.id,
                                    countNotDef = Convert.ToInt32(TBOXCount.Text)
                                }); 
                                    ClassesFolder.BDClass.bd.SaveChanges();
                                    ClassesFolder.PagesClass.tablePage.LBTypeList.Items.Refresh();
                                    MessageBox.Show("Данные сохранены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ClassesFolder.MainWindowClass.mainWindow.UCDefect.Visibility = Visibility.Collapsed;
                                    ClassesFolder.IDCourierClass.ID = -1;
                            }
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
            ClassesFolder.MainWindowClass.mainWindow.UCDefect.Visibility = Visibility.Collapsed;
            ClassesFolder.IDCourierClass.ID = -1;
        }

            #endregion

            #region Methods

            #endregion
        }
    }
