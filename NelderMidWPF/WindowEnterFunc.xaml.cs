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
    /// Логика взаимодействия для WindowEnterFunc.xaml
    /// </summary>
    public partial class WindowEnterFunc : Window
    {
        public WindowEnterFunc(string str = "", bool enteringFunc = false)
        {
            InitializeComponent();
            if(str.Length > 0)
            {
                tbEnterFunc.Text = str;
            }
            if(str.Length > 0 && enteringFunc)
            {
                tbEnterFunc.Text = str;
                this.enterFunc();
            }
        }

        private void tbAllowSymbols_TextChanged(object sender, TextChangedEventArgs e)
        {
            e.Handled = true;
        }
        public string strFunc = "";
        private void btEnterFunc_Click(object sender, RoutedEventArgs e)
        {
            this.enterFunc();
        }
        private void enterFunc()
        {
            try
            {
                NelderMid.answer = NelderMid.exp.parseExpression(tbEnterFunc.Text);

                if (NelderMid.answer != null && NelderMid.answer.result == -1)
                {
                    strFunc = tbEnterFunc.Text;
                    this.Close();
                }
                else
                {
                    if (NelderMid.answer.result == -2)
                    {
                        MessageBox.Show("Ошибка. Есть незакрытые скобки");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Ошибка. Введён неправильный символ по номером " + NelderMid.answer.result);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
