﻿using System;
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
    /// Логика взаимодействия для ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {

        private SolidColorBrush orangeColor = new SolidColorBrush(Colors.Orange);

        List<Model.Report> reports = ClassesFolder.BDClass.bd.Reports.ToList();
        List<Model.Cartridge> cartridges = ClassesFolder.BDClass.bd.Cartridges.ToList();


        public ReportPage()
        {
            InitializeComponent();
            DGReport.ItemsSource = reports;
        }

        #region Events
        private void Bcreate_Click(object sender, RoutedEventArgs e)
        {

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
                            VereficationReceived();
                            ClassesFolder.BDClass.bd.SaveChanges();
                            MessageBox.Show("Данные добавленны", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                            DGReport.Items.Refresh();
                        }
                        else
                        {
                            MessageBox.Show("Есть отрицательные значения.\nСтрока с именем " + reports[ind].title, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else 
                    {
                        MessageBox.Show("Есть отрицательные значения.\nСтрока с именем " + reports[ind].title, "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }                   
                }
                else
                    MessageBox.Show("Количество полученных больше чем отправленных.\nСтрока с именем "+reports[ind].title ,"Информация", MessageBoxButton.OK, MessageBoxImage.Information);
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
        #endregion

        #region Methods
        private int VereficationSent()
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
        private void VereficationReceived()
        {
            for (int i = 0; i < reports.Count; i++)
            {
                if (reports[i].countSent - reports[i].countReceived > reports[i].countNotFill) // если убрали из "полученных"
                {
                    int indRoam = -1;
                    for (int y = 0; y < cartridges.Count; y++)
                    {
                        if (reports[i].title == cartridges[y].NNC)
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
                    }
                    reports[i].countNotFill = reports[i].countSent - reports[i].countReceived;
                    ClassesFolder.BDClass.bd.SaveChanges();                   
                }
                else if (reports[i].countSent - reports[i].countReceived < reports[i].countNotFill)// если добавили в "полученных"
                {
                    int indRoam = -1;
                    for (int y = 0; y < cartridges.Count; y++)
                    {
                        if (reports[i].title == cartridges[y].NNC)
                        {
                            indRoam = y;
                        }
                    }
                    cartridges[indRoam].countFull += reports[i].countNotFill-(reports[i].countSent - reports[i].countReceived);
                    reports[i].countNotFill = reports[i].countSent - reports[i].countReceived;
                    ClassesFolder.BDClass.bd.SaveChanges();
                }
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
