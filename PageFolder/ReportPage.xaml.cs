using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using fd = System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Cartridge.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {

        private SolidColorBrush orangeColor = new SolidColorBrush(Colors.Orange);
        private Model.Report reportDel = new Model.Report();

        List<Model.Report> reports = ClassesFolder.BDClass.bd.Reports.ToList();
        List<Model.Cartridge> cartridges = ClassesFolder.BDClass.bd.Cartridges.ToList();


        public ReportPage()
        {
            InitializeComponent();
            if (reports.Count == 0)
            {
                Gmain.Opacity = 0;
            }
            else
            {
                Gmain.Opacity = 1;
            }
            DGReport.ItemsSource = reports;

        }

        #region Events
        private void Bcreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fd.FolderBrowserDialog folderBrowserDialog = new fd.FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == fd.DialogResult.OK)
                {
                    
                }
            }
            catch (Exception z)
            {

            }
        }

        private void Bauto_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < reports.Count; i++)
            {
                if (reports[i].countReceived == 0
                && reports[i].countSent == 0 && reports[i].price == 0)
                {
                }
                else if (reports[i].countReceived != reports[i].countSent)
                {
                    reports[i].countReceived = reports[i].countSent;
                }           
            }
            if (VereficationReceived())
            {
                MessageBox.Show("Данные добавленны", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            DGReport.Items.Refresh();
            ClassesFolder.BDClass.bd.SaveChanges();
        }

            private void Badd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ind = VereficationSent();
                if (ind == -1)
                {
                    ind = NegativeReceived();
                    if (ind == -1)
                    {
                        ind = NegativePrice();
                        if (ind == -1)
                        {
                            if (VereficationDefected())
                            {
                                if (VereficationReceived())
                                {
                                    ClassesFolder.BDClass.bd.SaveChanges();
                                    MessageBox.Show("Данные добавленны", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                                    DGReport.Items.Refresh();
                                }
                            }
                        }
                        else
                        {
                            reports[ind].price = cartridges[reports[ind].idCantridges-1].price;
                            DGReport.Items.Refresh();
                            MessageBox.Show("Есть отрицательные значения.\nСтрока с именем " + reports[ind].title, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        reports[ind].countReceived = 0;
                        VereficationReceived();
                        DGReport.Items.Refresh();
                        MessageBox.Show("Есть отрицательные значения.\nСтрока с именем " + reports[ind].title, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    reports[ind].countReceived = 0;
                    VereficationReceived();
                    DGReport.Items.Refresh();
                    MessageBox.Show("Количество полученных больше чем отправленных.\nСтрока с именем " + reports[ind].title, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                   
            }
            catch
            {
                MessageBox.Show("Ошибка добавления в БД", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
        private void DGReport_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Model.Report cells = (Model.Report)e.Row.DataContext;
            if (cells.countReceived ==0 
                && cells.countSent == 0 && cells.price == 0)// делаем оранжевым если картридж их другого отчёта
            {
                e.Row.Background = orangeColor;
                e.Row.IsEnabled = false;
            }
            if (cells.price > 0 && cells.countSent > 0)// считаем полную сумму
            {
                cells.priceAll = cells.price * cells.countSent;
                ClassesFolder.BDClass.bd.SaveChanges();
            }
            if (cells.countReceived == 0
                && cells.countSent == 0 && cells.price == 0
                && cells.countDefects == 0)// убираем пустые строки
            {
                reports.Remove(cells);
                DGReport.Items.Refresh();
                ClassesFolder.BDClass.bd.Reports.Remove(cells);
                ClassesFolder.BDClass.bd.SaveChanges();
            }
        }
        private void DGReport_Loaded(object sender, RoutedEventArgs e)
        {
            //ClassesFolder.BDClass.bd.SaveChanges();
        }
        private void DGReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Model.Report cells = (Model.Report)DGReport.SelectedItem;
            reportDel = cells;
            Bclose.Visibility = Visibility.Visible;
        }
        private void Bclose_Click(object sender, RoutedEventArgs e)
        {

            if (reportDel != null)
            {
                try
                {
                    for (int i = 0; i < cartridges.Count; i++)
                    {
                        if (cartridges[i].id == reportDel.idCantridges)
                        {
                            if (reportDel.countReceived > 0)
                            {
                                cartridges[i].countEmpty += (reportDel.countSent - reportDel.countReceived);
                                cartridges[i].countFull += reportDel.countDefects;
                            }
                            else 
                            {
                                cartridges[i].countEmpty += reportDel.countSent;
                                cartridges[i].countFull += reportDel.countDefects;
                            }
                            
                        }
                    }
                    reports.Remove(reportDel);
                    ClassesFolder.BDClass.bd.Reports.Remove(reportDel);
                    ClassesFolder.BDClass.bd.SaveChanges();
                    DGReport.Items.Refresh();
                    if (reports.Count == 0)
                    {
                        Gmain.Opacity = 0;
                    }
                    MessageBox.Show("Данные удалены ", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception z)
                {
                    MessageBox.Show("Ошибка удаления: "+z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Ошибка удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Methods
        private int VereficationSent()// срабатывает когда количество полученных больше чем отправленных
        {
            int ind = -1;
            for (int i = 0; i < reports.Count; i++)
            {
                if (reports[i].countSent < reports[i].countReceived)
                {
                    return ind= i;
                }
            }
            return ind;
        }
        private bool VereficationReceived()
        {
            for (int i = 0; i < reports.Count; i++)
            {
                if (reports[i].countSent - reports[i].countReceived > reports[i].countNotFill) // если убрали из "полученных"
                {
                    int indRoam = -1;
                    for (int y = 0; y < cartridges.Count; y++)
                    {
                        if (reports[i].idCantridges == cartridges[y].id)
                        {
                            indRoam = y;
                        }
                    }
                    if (cartridges[indRoam].countFull >= ((reports[i].countSent - reports[i].countReceived) - reports[i].countNotFill))
                    {
                        cartridges[indRoam].countFull -= (reports[i].countSent - reports[i].countReceived) - reports[i].countNotFill;
                    }
                    else
                    {
                        MessageBox.Show("Нехватка заполненных катриджей id = "+ indRoam, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }
                    reports[i].countNotFill = reports[i].countSent - reports[i].countReceived;
                    ClassesFolder.BDClass.bd.SaveChanges();                   
                }
                else if (reports[i].countSent - reports[i].countReceived < reports[i].countNotFill)// если добавили в "полученных"
                {
                    int indRoam = -1;
                    for (int y = 0; y < cartridges.Count; y++)
                    {
                        if (reports[i].idCantridges == cartridges[y].id)
                        {
                            indRoam = y;
                        }
                    }
                    cartridges[indRoam].countFull += reports[i].countNotFill-(reports[i].countSent - reports[i].countReceived);
                    reports[i].countNotFill = reports[i].countSent - reports[i].countReceived;
                    ClassesFolder.BDClass.bd.SaveChanges();
                }
            }
            return true;
        }
        private bool VereficationDefected()
        {
            try
            {
                for (int i = 0; i < reports.Count; i++)
                {
                    var cart = cartridges.Where(x => x.id == reports[i].idCantridges).ToList();
                    if (reports[i].countDefects < 0)
                    {
                        MessageBox.Show("Есть отрицптельные значения\nНаименование: " + reports[i].title, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        reports[i].countDefects = reports[i].countNotDef;
                        DGReport.Items.Refresh();
                        return false;
                    }
                    if (reports[i].countDefects != reports[i].countNotDef)
                    {
                            if (reports[i].countDefects - reports[i].countNotDef <= cart[0].countFull)
                            {
                            if (reports[i].countDefects > reports[i].countNotDef)
                            {
                                cartridges[cart[0].id - 1].countFull -= (reports[i].countDefects - reports[i].countNotDef);
                                reports[i].countNotDef = reports[i].countDefects;
                            }
                            else
                            {
                                cartridges[cart[0].id - 1].countFull += (reports[i].countNotDef - reports[i].countDefects);
                                reports[i].countNotDef = reports[i].countDefects;
                            }
                            ClassesFolder.BDClass.bd.SaveChanges();
                        }
                        else
                        {
                            MessageBox.Show("Недостаточно заполненных катриджей для увеличение брака\nНаименование: " + reports[i].title, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            reports[i].countDefects = reports[i].countNotDef;
                            DGReport.Items.Refresh();
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка сохранения " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            
        }
        private int NegativeReceived()
        {
            int ind = -1;
            for (int i = 0; i < reports.Count; i++)
            {
                if (reports[i].countReceived <0)
                {
                    return ind = i;
                }
            }
            return ind;
        }
        private int NegativePrice()
        {
            int ind = -1;
            for (int i = 0; i < reports.Count; i++)
            {
                if (reports[i].price < 0)
                {
                    return ind = i;
                }
            }
            return ind;
        }



        #endregion

}
    
}
