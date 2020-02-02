using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NelderMidWPF
{
    /// <summary>
    /// Логика взаимодействия для WindowOutput.xaml
    /// </summary>
    public partial class WindowOutput : Window
    {
        public WindowOutput(ref string str)
        {
            InitializeComponent();
            tbResult.Text = "";
            tbResult.Text += str;
        }

        private void btSaveResult_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
            sfd.DefaultExt = ".txt";
            sfd.Filter = "Text documents (.txt)|*.txt";
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    System.IO.File.WriteAllText(sfd.FileName, tbResult.Text);
                    MessageBox.Show("Файл сохранён!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка. Файл не сохранён!");
                }
            }
        }
    }
}
