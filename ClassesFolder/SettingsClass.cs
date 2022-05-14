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
        private static string fileName = "Settings.txt";
        private static string[] masStr;

        public static string mail {
            get
            {
                if (Reader())
                {
                    if (Separatore())
                    {
                        return masStr[0];
                    }                    
                }
                return "";
            }

            set
            {
                //bufStr = ":4di-bor@ro.ru\n:White";
                //Writer();
                masStr[0] = value;
                if (Collectre())
                {
                    Writer();
                }
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
                if (Collectre())
                {
                    Writer();
                }
            }
        }

        public static bool Separatore() //подгатавливает данные для чтения
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
        }

        public static bool Collectre() //собирает строку для записи
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


        private static bool Reader()
        {
            try
            {
                using (FileStream fstream = File.OpenRead(fileName))
                {
                    // выделяем массив для считывания данных из файла
                    byte[] buffer = new byte[fstream.Length];
                    // считываем данные
                    fstream.ReadAsync(buffer, 0, buffer.Length);
                    // декодируем байты в строку
                    bufStr = Encoding.Default.GetString(buffer);
                }
                return true;
            }
            catch (Exception z)
            {
                MessageBox.Show("Ошибка: " + z, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private static bool Writer()
        {
            try
            {
                using (FileStream fstream = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    // преобразуем строку в байты
                    byte[] buffer = Encoding.Default.GetBytes(bufStr);
                    // запись массива байтов в файл
                    fstream.WriteAsync(buffer, 0, buffer.Length);
                }            
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
