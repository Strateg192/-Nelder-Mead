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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NelderMidWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public class MyGrid : Grid
    {
        public MyGrid()
        {
            this.MaxWidth = 50;
            this.Margin = new Thickness(1, 0, 1, 0);
            this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30.0, GridUnitType.Auto)});
            this.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(30.0, GridUnitType.Auto) });
            Label lb = new Label() { HorizontalAlignment = HorizontalAlignment.Center, Tag = "1" };
            Grid.SetRow(lb, 0);
            this.Children.Add(lb);
            TextBox tb = new TextBox() { TextWrapping = TextWrapping.NoWrap, TextAlignment = TextAlignment.Center, Tag = "2" };
            Grid.SetRow(tb, 1);
            this.Children.Add(tb);
        }
    }
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btEnterFunc_Click(object sender, RoutedEventArgs e)
        {
            WindowEnterFunc win = new WindowEnterFunc(textBlockEnteredFunc.Text);
            win.ShowDialog();
            if (NelderMid.answer != null && NelderMid.answer.result == -1)
            {
                this.textBlockEnteredFunc.Text = win.strFunc;               
                List<string> list = new List<string>();
                list.AddRange(NelderMid.answer.values.Keys);
                list.Sort();
                this.createCoordinates(ref spKoordinates, ref list);
            }
        }
        private void createCoordinates(ref StackPanel sp, ref List<string> listNames)
        {
            sp.Children.Clear();
            for (int i = 0; i < listNames.Count; ++i)
            {
                MyGrid my = new MyGrid();
                for (int j = 0; j < my.Children.Count; ++j)
                {
                    try
                    {
                        if (((Label)my.Children[j]).Tag.ToString() == "1")
                        {
                            ((Label)my.Children[j]).Content = listNames[i];
                        }
                        if (((TextBox)my.Children[j]).Tag.ToString() == "2")
                        {
                            ((TextBox)my.Children[j]).Text = (0).ToString();
                        }
                    }
                    catch
                    {

                    }
                }
                sp.Children.Add(my);
            }
        }

        private void btCalculate_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 1; i < spOptions.Children.Count; i+=2)
            {
                TextBox tb = (TextBox)spOptions.Children[i];
                if(tb.Text.Length == 0)
                {
                    string tmp_str = ((Label)spOptions.Children[i - 1]).Content.ToString();
                    MessageBox.Show("Ошибка! Не заполнено поле " + tmp_str.Remove(tmp_str.Length - 1));
                    return;
                }
            }
            if(NelderMid.answer != null && NelderMid.answer.values != null && NelderMid.answer.values.Count > 1)
            {
                List<double> ld = new List<double>();
                for (int i = 0; i < NelderMid.answer.values.Count; ++i)
                {
                    try
                    {
                        if (((TextBox)((Grid)spKoordinates.Children[i]).Children[1]).Tag.ToString() == "2")
                        {
                            string tmp = ((TextBox)((Grid)spKoordinates.Children[i]).Children[1]).Text;
                            if (tmp.Length == 0)
                            {
                                MessageBox.Show("Ошибка. Не введена " + i + " координата!");
                            }
                            else
                            {
                                try
                                {
                                    ld.Add(Convert.ToDouble(tmp));
                                }
                                catch
                                {
                                    MessageBox.Show("Ошибка. Неверно введена " + i + " координата!");
                                    return;
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                try
                {
                    NelderMid.Go_Click(ld.ToArray(), Convert.ToInt32(tbMaxK.Text), Convert.ToDouble(tbEpsilon.Text), Convert.ToDouble(tbLambda.Text),
                        Convert.ToDouble(tbAlpha.Text), Convert.ToDouble(tbBeta.Text), Convert.ToDouble(tbGamma.Text), Convert.ToDouble(tbCR.Text),
                        textBlockEnteredFunc.Text, cbCountSymbols.SelectedIndex);
                    WindowOutput win = new WindowOutput(ref NelderMid.outputStr);
                    win.ShowDialog();
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Ошибка. Неправильно введены данные на верхней панели!" + ex.Message);
                }
            }
        }

        private void btSaveData_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sv = new Microsoft.Win32.SaveFileDialog();
            sv.DefaultExt = ".txt";
            sv.Filter = "Text documents (.txt)|*.txt";
            if (sv.ShowDialog() == true)
            {
                string tmpStr = "";
                tmpStr += textBlockEnteredFunc.Text + "\n";
                for (int i = 0; i < NelderMid.answer.values.Count; ++i)
                {
                    try
                    {
                        string tmp = ((TextBox)((Grid)spKoordinates.Children[i]).Children[1]).Text;
                        if (tmp.Length == 0)
                        {
                            MessageBox.Show("Ошибка. Не введена " + i + " координата!");
                        }
                        else
                        {
                            try
                            {
                                Convert.ToDouble(tmp);
                                tmpStr += tmp + "\n";
                            }
                            catch
                            {
                                MessageBox.Show("Ошибка. Неверно введена " + i + " координата!");
                                return;
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                try
                {
                    Convert.ToInt32(tbMaxK.Text);
                    Convert.ToDouble(tbEpsilon.Text);
                    Convert.ToDouble(tbLambda.Text);
                    Convert.ToDouble(tbAlpha.Text);
                    Convert.ToDouble(tbBeta.Text);
                    Convert.ToDouble(tbGamma.Text);
                    Convert.ToDouble(tbCR.Text);
                }
                catch
                {
                    MessageBox.Show("Ошибка. Неверно введены данные на верхней панели!");
                    return;
                }
                tmpStr += tbMaxK.Text + "\n";
                tmpStr += tbEpsilon.Text + "\n";
                tmpStr += tbLambda.Text + "\n";
                tmpStr += tbAlpha.Text + "\n";
                tmpStr += tbBeta.Text + "\n";
                tmpStr += tbGamma.Text + "\n";
                tmpStr += tbCR.Text + "\n";
                tmpStr += cbCountSymbols.SelectedIndex + "\n";
                try
                {
                    System.IO.File.WriteAllText(sv.FileName, tmpStr);
                    System.IO.FileInfo file = new System.IO.FileInfo(sv.FileName);
                    file.Attributes = file.Attributes | System.IO.FileAttributes.ReadOnly;
                    MessageBox.Show("Файл сохранён!");
                }
                catch
                {
                    MessageBox.Show("Ошибка. Файл не сохранён!");
                    return;
                }
                tmpStr = null;
            }
        }

        private void btLoadData_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog sv = new Microsoft.Win32.OpenFileDialog();
            sv.DefaultExt = ".txt";
            sv.Filter = "Text documents (.txt)|*.txt";
            if (sv.ShowDialog() == true)
            {
                System.IO.StreamReader stream = new System.IO.StreamReader(sv.FileName);
                textBlockEnteredFunc.Text = stream.ReadLine();
                WindowEnterFunc win = new WindowEnterFunc(textBlockEnteredFunc.Text, true);
                win = null;
                if (NelderMid.answer == null || NelderMid.answer.result != -1)
                {
                    MessageBox.Show("Ошибка. Загрузочная функция введена не верно!");
                    textBlockEnteredFunc.Text = "";
                    return;
                }
                else
                {
                    List<string> list = new List<string>();
                    list.AddRange(NelderMid.answer.values.Keys);
                    list.Sort();
                    this.createCoordinates(ref spKoordinates, ref list);
                    for (int i = 0; i < NelderMid.answer.values.Count; ++i)
                    {
                        try
                        {
                            ((TextBox)((Grid)spKoordinates.Children[i]).Children[1]).Text = stream.ReadLine();
                        }
                        catch
                        {

                        }
                    }
                }
                tbMaxK.Text = stream.ReadLine();
                tbEpsilon.Text = stream.ReadLine();
                tbLambda.Text = stream.ReadLine();
                tbAlpha.Text = stream.ReadLine();
                tbBeta.Text = stream.ReadLine();
                tbGamma.Text = stream.ReadLine();
                tbCR.Text = stream.ReadLine();
                cbCountSymbols.Text = stream.ReadLine();
                stream.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
            "MaxK - Максимальное допустимое количество итераций\n" +
            "ε - Предельное значение длины ребра симплекса\n" +
            "λ - Начальная длина ребра симплекса\n" +
            "α - Коэффициент отражения симплекса\n" +
            "β - Коэффициент сжатия симплекса\n" +
            "γ - Коэффициент растяжения симплекса\n" +
            "cR - Коэффициент редукции симплекса\n", "Справка", MessageBoxButton.OK
            );
        }
    }
}
