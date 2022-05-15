using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Cartridge.ClassesFolder
{
    class SettingsClass
    {

        private static string bufStr = "";
        public static string fileName = "Settings.txt";
        private static string[] masStr;

        public static string mail {
            get
            {
                if (Reader())
                {
                    return masStr[0];                   
                }
                return "";
            }

            set
            {
                masStr[0] = value;
                Writer(masStr);
            }
        }
        public static string theme
        {
            get
            {
                if (Reader())
                {
                    return masStr[1];
                }
                return "";
            }

            set
            {
                masStr[1] = value;
                Writer(masStr);
            }
        }

        private static bool Separatore() //подгатавливает данные для чтения
        {
            try
            {
                masStr = bufStr.Split('\n');
                for (int i = 0; i < masStr.Length; i++)
                {                    
                    int numChar = masStr[i].IndexOf(':');
                    masStr[i] = masStr[i].Substring(numChar + 1);
                    return true;
                }
                return false;
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        } //устарели
        private static bool Collectre() //собирает строку для записи
        {
            try
            {
                bufStr = "";
                for (int i = 0; i < masStr.Length; i++)
                {
                    bufStr += ":" + masStr[i]+"\n";
                }    
                    return false;
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static bool Reader()
        {
            try
            {
                masStr = File.ReadAllLines(fileName);
                return true;
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        public static bool Writer(string [] str)
        {
            try
            {
                File.WriteAllLines(fileName, str);  
                return true;
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
