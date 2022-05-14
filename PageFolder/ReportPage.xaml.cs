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
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

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
                int ind = VereficationSent(); //сохранение перед формированием отчёта
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
                                    DGReport.Items.Refresh();
                                }
                            }
                        }
                        else
                        {
                            reports[ind].price = cartridges[reports[ind].idCantridges - 1].price;
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

            fd.FolderBrowserDialog folderBrowserDialog = new fd.FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == fd.DialogResult.OK)
                {
                    string path = folderBrowserDialog.SelectedPath;
                    path += "\\Отчёт.xlsx";
                    Excel.Application xlApp = new Excel.Application();
                    Excel.Workbook wbNewExcel = xlApp.Workbooks.Add();
                    Excel.Worksheet wsNewExcel = wbNewExcel.Sheets[1];



                    wsNewExcel.Range["A1"].Value = "Название"; // задаём заначения внутри ячеек
                    wsNewExcel.Range["B1"].Value = "Количество отправленных";
                    wsNewExcel.Range["C1"].Value = "Количество полученных";
                    wsNewExcel.Range["D1"].Value = "Цена";
                    wsNewExcel.Range["E1"].Value = "Общая цена";

                    StyleEX(wsNewExcel, "A1"); //Добавляем стили ячейки
                    StyleEX(wsNewExcel, "B1");
                    StyleEX(wsNewExcel, "C1");
                    StyleEX(wsNewExcel, "D1");
                    StyleEX(wsNewExcel, "E1");
                    wsNewExcel.Range["A1:E1"].Columns.ColumnWidth = 35; 

                    wsNewExcel.Range["A1:E1"].Interior.ColorIndex = 22; // сеняем цвета 20 - светло голубой, 21 - тёмно бардовый, 22 - кораловый, 23 - ультрамариновый, 24 - серенивый, 25 - фиолетовый 
                    wsNewExcel.Range["A1:E1"].Interior.PatternColorIndex = Excel.Constants.xlAutomatic;



                    List <Model.Report> reportsEX = ClassesFolder.BDClass.bd.Reports.Where(x => x.countSent > 0).ToList();
                    for (int i = 0; i < reportsEX.Count; i++)
                    {
                            wsNewExcel.Range["A" + (i + 2)].Value = reportsEX[i].title;
                            wsNewExcel.Range["B" + (i + 2)].Value = reportsEX[i].countSent;
                            wsNewExcel.Range["C" + (i + 2)].Value = reportsEX[i].countReceived;
                            wsNewExcel.Range["D" + (i + 2)].Value = reportsEX[i].price;
                            wsNewExcel.Range["E" + (i + 2)].Value = reportsEX[i].priceAll;                                           
                    }

                    wsNewExcel.Range["E" + (reportsEX.Count + 2)].FormulaLocal = "=СУММ(E2:E"+ (reportsEX.Count + 1)+")";
                    wsNewExcel.Range["E" + (reportsEX.Count + 2)].Borders.Weight = Excel.XlBorderWeight.xlThick;

                    List<Model.Report> reportsEXv = ClassesFolder.BDClass.bd.Reports.Where(x => x.countSent == 0 || x.countDefects > 0).ToList();// таблица с браком
                    for (int i = 0; i < reportsEXv.Count; i++)
                    {
                        wsNewExcel.Range["A" + (i + reportsEX.Count+4)].Value = reportsEXv[i].title;
                        wsNewExcel.Range["B" + (i + reportsEX.Count+4)].Value = reportsEXv[i].countDefects;
                    }

                    wsNewExcel.Range["A"+ (reportsEX.Count + 3)].Value = "Название"; 
                    wsNewExcel.Range["B"+ (reportsEX.Count + 3)].Value = "Количество брака";
                    StyleEX(wsNewExcel, "A" + (reportsEX.Count + 3));
                    StyleEX(wsNewExcel, "B"+ (reportsEX.Count + 3));
                    wsNewExcel.Range["A"+ (reportsEX.Count + 3) + ":B"+ (reportsEX.Count + 3)].Interior.ColorIndex = 24;
                    wsNewExcel.Range["A1:F1"].Interior.PatternColorIndex = Excel.Constants.xlAutomatic;

                    wbNewExcel.SaveAs(path);
                    wbNewExcel.Close(true);
                    xlApp.Quit();
                    MessageBox.Show("Документ сохранён", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);

                    // очистка интерфейса
                    ClassesFolder.BDClass.bd.Reports.RemoveRange(reports);
                    ClassesFolder.BDClass.bd.SaveChanges();
                    reports = ClassesFolder.BDClass.bd.Reports.ToList();
                    DGReport.Items.Refresh();
                    if (reports.Count == 0)
                    {
                        Gmain.Opacity = 0;
                    }

                    DGReport.ItemsSource = reports;

                    switch (MessageBox.Show("Хотите получить отчёт на почту", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Information))
                    {
                        case MessageBoxResult.Yes:
                            if (MailSend(path))
                            {
                                MessageBox.Show("Документ отправлен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }


                }
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка формирования отчёта: "+z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            int a = 0;
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
                                cartridges[i].countEmpty += (reportDel.countSent);
                                cartridges[i].countFull -= reportDel.countReceived;
                                cartridges[i].countFull += reportDel.countDefects;
                            }
                            else if (reportDel.countReceived == 0)
                            {
                                cartridges[i].countEmpty += reportDel.countSent;
                                cartridges[i].countFull += reportDel.countDefects;
                            }
                            else
                            {
                                MessageBox.Show("Отрицательное значение ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                a++;
                            }                            
                        }
                    }
                    if (a == 0)
                    {
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


        private void StyleEX(Excel.Worksheet wsNewExcel,string cell)
        {
            try
            {
                wsNewExcel.Range[cell].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                wsNewExcel.Range[cell].VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                wsNewExcel.Range[cell].Font.Size = 16;               
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка применения стиля  " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private bool MailSend(string path)
        {
            try
            {
                DateTime thisDay = DateTime.Today;
                MailAddress from = new MailAddress("chirko_v_d@mail.ru", "Danila");
                MailAddress to = new MailAddress("danilatchirckov@yandex.ru");
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Отчёт за "+ thisDay.Date;
                m.Body = "А тут могла быть ваша реклама";
                m.Attachments.Add(new Attachment(path));
                m.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient("smtp.mail.ru", 587);
                smtp.Credentials = new NetworkCredential("chirko_v_d@mail.ru", "Vp4zjzN5xMbitzkWcSy0");
                smtp.EnableSsl = true;
                smtp.Send(m);
                return true; 
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
