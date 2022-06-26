using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для SettingsControls.xaml
    /// </summary>
    public partial class SettingsControls : UserControl
    {
        public SettingsControls()
        {
            InitializeComponent();
            ClassesFolder.SettingsClass.Reader();
            switch (ClassesFolder.SettingsClass.theme)
            {
                case "Dark":
                    CBTheme.SelectedIndex = 0;
                    break;
                case "White":
                    CBTheme.SelectedIndex = 1;
                    break;
            }
            TBOXMail.Text = ClassesFolder.SettingsClass.mail;
        }

        #region Events
        private void GControls_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UCSettings.Visibility = Visibility.Collapsed;
        }

        private void BSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    ClassesFolder.SettingsClass.mail = TBOXMail.Text;
                    switch (CBTheme.SelectedIndex)
                    {
                        case 0:
                            ClassesFolder.SettingsClass.theme = "Dark";
                            break;
                        case 1:
                            ClassesFolder.SettingsClass.theme = "White";
                            break;
                        default:
                            ClassesFolder.SettingsClass.theme = "White";
                            break;
                    }
                    MessageBox.Show("Данные сохраннены", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                ClassesFolder.MainWindowClass.mainWindow.UCSettings.Visibility = Visibility.Collapsed;
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BCancel_Click(object sender, RoutedEventArgs e)
        {
            ClassesFolder.MainWindowClass.mainWindow.UCSettings.Visibility = Visibility.Collapsed;
        }
        private void BMail_Click(object sender, RoutedEventArgs e)
        {
            if (Verefication(TBOXMail.Text))
            {
                if (MailSend(TBOXMail.Text))
                {
                    MessageBox.Show("Письмо отправленно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                }                 
            }
        }
        private void CBTheme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (CBTheme.SelectedIndex)
            {
                case 0:
                    ChangeTheme('d');
                    break;
                case 1:
                    ChangeTheme('w');
                    break;
                default:
                    break;
            }
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

        private bool MailSend(string adressRecipient)
        {
            try
            {
                DateTime thisDay = DateTime.Today;
                MailAddress from = new MailAddress("chirko_v_d@mail.ru", "Danila");
                MailAddress to = new MailAddress(adressRecipient);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Тестовое письмо";
                m.Body = "Тело письма";
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

        private bool Verefication(string verStr)
        {
            try
            {
                if (verStr.Length == 0)
                {
                    MessageBox.Show("Где строка?", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
                bool isItEmail = Regex.IsMatch(verStr, emailPattern);
                if (!isItEmail)
                {
                    MessageBox.Show("Это не почта", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
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

