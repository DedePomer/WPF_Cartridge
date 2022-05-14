using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace WPF_Cartridge
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool mainWindowState = false;
        public MainWindow()
        {
            InitializeComponent();
            ClassesFolder.BDClass.bd = new Model.AppContex();
            ChangeTheme('w');
            ClassesFolder.MainWindowClass.mainWindow = this;
            ClassesFolder.ControlsClass.fillControl = UT;
            ClassesFolder.ControlsClass.nullConrol = UCnull;
            ClassesFolder.ControlsClass.useControl = UCuse;
            ClassesFolder.ControlsClass.deffectedControls = UCDefect;
            Fmain.Navigate(new PageFolder.TablePage());
        }


        #region Events
        private void BClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void BWindow_Click(object sender, RoutedEventArgs e)
        {
            if (!mainWindowState)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
                mainWindowState = true;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
                mainWindowState = false;
            }
        }
        private void BCollapse_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void BView_Click(object sender, RoutedEventArgs e)
        {
            Fmain.Navigate(new PageFolder.TablePage());
        }
        private void BCreate_Click(object sender, RoutedEventArgs e)
        {
            Fmain.Navigate(new PageFolder.ReportPage());
        }
        private void BCatrigesTable_Click(object sender, RoutedEventArgs e)
        {
            Fmain.Navigate(new PageFolder.MainTablePage());
        }


        private void BSettings_Click(object sender, RoutedEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UCSettings.Visibility = Visibility.Visible;
        }
        #endregion

        #region Methods
        private void ChangeTheme(char theme)
        {
            string commPath = "ThemeFolder\\CommonTheme.xaml";
            string pathTheme = "";
            if (theme == 'd')
            {
                pathTheme = "ThemeFolder\\DarkTheme.xaml";
            }
            else
            {
                pathTheme = "ThemeFolder\\LightTheme.xaml";
            }
            // добавляем темы в ресурсы приложения
            var uriTheme = new Uri(pathTheme, UriKind.Relative);
            var commApp = new Uri(commPath, UriKind.Relative);
            Application.Current.Resources.Clear();
            ResourceDictionary resourceDictTheme = Application.LoadComponent(uriTheme) as ResourceDictionary;
            ResourceDictionary resourceDictApp = Application.LoadComponent(commApp) as ResourceDictionary;
            Application.Current.Resources.MergedDictionaries.Add(resourceDictTheme);
            Application.Current.Resources.MergedDictionaries.Add(resourceDictApp);
        }
        #endregion

    }
}
