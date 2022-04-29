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
using a =Microsoft.Windows.Themes;


namespace WPF_Cartridge
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Fmain.Navigate(new PageFolder.TablePage());
        }


        #region Events
        private void BBlackTheme_Click(object sender, RoutedEventArgs e)
        {
            ChangeTheme('d');
        }

        private void BWhiteTheme_Click(object sender, RoutedEventArgs e)
        {
            ChangeTheme('w');
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
