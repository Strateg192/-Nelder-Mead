using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NelderMidWPF
{
    public class node
    {
        private node()
        {
            left = null;
            right = null;
            operat = null;
            value = null;
        }
        public static node createNode(string str) //воспринимает только значение без скобок
        {
            if (str.Length == 0)
                return null;
            node thisNode = new node();
            while (thisNode.deleteScobka(ref str))
            {
            }
            bool isHaveScobka = false;
            //string scobkaBuf = "";
            //bool swap = false;
            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] == '(' || str[i] == ')')
                {
                    isHaveScobka = true;
                    break;
                }
            }
            if (isHaveScobka)
            {
                int tmpCountScobok = 0; //количество скобок
                int minCountScobok = int.MaxValue; //минимальное количество за которыми стоит оператор
                int tmpPM = -1; //+, -
                int tmpUD = -1; //*, /
                int tmpV = -1; //^
                int tmpOperat = -1;

                int tmpFL = -1; //func left index
                int tmpFR = -1; //func right index
                string tmpBuf = "";
                for (int i = 0; i < str.Length; ++i)
                {
                    if (char.IsLetter(str[i]))
                    {
                        tmpBuf += str[i];
                        if (tmpBuf.Length == 0)
                        {
                            tmpFL = i;
                        }
                    }
                    else if (char.IsDigit(str[i]))
                    {
                        if (tmpBuf.Length > 0)
                        {
                            tmpBuf += str[i];
                        }
                    }
                    else if (str[i] == '(')
                    {
                        if (Operat.operators.IndexOf(tmpBuf) > -1)
                        {
                            tmpFR = i - 1;
                            tmpBuf = "";
                            minCountScobok = 0;
                        }
                        break;
                    }
                }
                for (int i = 0; i < str.Length; ++i)
                {
                    if (str[i] == '(')
                    {
                        ++tmpCountScobok;
                    }
                    else if (str[i] == ')')
                    {
                        --tmpCountScobok;
                    }
                    else if (str[i] == '^')
                    {
                        if (tmpCountScobok < minCountScobok)
                        {
                            tmpV = i;
                            minCountScobok = tmpCountScobok;
                        }
                        else if (tmpCountScobok == minCountScobok && tmpV == -1)
                        {
                            tmpV = i;
                            minCountScobok = tmpCountScobok;
                        }
                    }
                }
                for (int i = 0; i < str.Length; ++i)
                {
                    if (str[i] == '(')
                    {
                        ++tmpCountScobok;
                    }
                    else if (str[i] == ')')
                    {
                        --tmpCountScobok;
                    }
                    else if (str[i] == '*' || str[i] == '/')
                    {
                        if (tmpCountScobok < minCountScobok)
                        {
                            tmpUD = i;
                            minCountScobok = tmpCountScobok;
                        }
                        else if (tmpCountScobok == minCountScobok && tmpUD == -1)
                        {
                            tmpUD = i;
                            minCountScobok = tmpCountScobok;
                        }
                    }
                }
                tmpCountScobok = 0;
                for (int i = (str[0] == '-' ? 1 : 0); i < str.Length; ++i)
                {
                    if (str[i] == '(')
                    {
                        ++tmpCountScobok;
                    }
                    else if (str[i] == ')')
                    {
                        --tmpCountScobok;
                    }
                    else if (str[i] == '+' || str[i] == '-')
                    {
                        if (tmpCountScobok < minCountScobok)
                        {
                            tmpPM = i;
                            minCountScobok = tmpCountScobok;
                        }
                        else if (tmpCountScobok == minCountScobok && tmpPM == -1)
                        {
                            tmpPM = i;
                            minCountScobok = tmpCountScobok;
                        }
                    }
                }
                if (tmpPM != -1)
                {
                    tmpOperat = tmpPM;
                }
                else if (tmpUD != -1)
                {
                    tmpOperat = tmpUD;
                }
                else if (tmpV != -1)
                {
                    tmpOperat = tmpV;
                }
                else if (tmpFR != -1)
                {
                    tmpOperat = tmpFR;
                }
                string tmpLeft = "";
                string tmpRight = "";
                if (tmpOperat == tmpFR)
                {
                    tmpRight = str.Remove(0, tmpOperat + 1);
                    thisNode.operat = Operat.createOperat(str.Remove(tmpOperat + 1));
                }
                else
                {
                    tmpLeft = str.Remove(tmpOperat, str.Length - tmpOperat);
                    tmpRight = str.Remove(0, tmpOperat + 1);
                    thisNode.operat = Operat.createOperat(str[tmpOperat].ToString());
                }
                thisNode.left = tmpLeft.Length != 0 ? (node.createNode(tmpLeft)) : null;
                thisNode.right = tmpRight.Length != 0 ? (node.createNode(tmpRight)) : null;
            }
            else
            {
                int firstOperat = thisNode.findFirstOperat(ref str);
                if (firstOperat < 0)
                {
                    thisNode.value = str;
                }
                else
                {
                    string tmpOperat = str[firstOperat].ToString();
                    thisNode.operat = Operat.createOperat(tmpOperat);
                    if (thisNode.operat == null)
                    {
                        throw new Exception();
                    }
                    string tmpLeft = str.Remove(firstOperat, str.Length - firstOperat);
                    string tmpRight = str.Remove(0, firstOperat + 1);
                    if (tmpLeft.Length != 0)
                        thisNode.left = createNode(tmpLeft);
                    thisNode.right = node.createNode(tmpRight);
                }
            }
            return thisNode;
        }
        private bool deleteScobka(ref string str) //удаляет лишние скобки true - если что-то удалилось
        {
            if (str[0] != '(' || str[str.Length - 1] != ')')
            {
                return false;
            }

            int openScobka = 0;
            for (openScobka = str.Length - 1; openScobka >= 0; --openScobka)
            {
                if (str[openScobka] == '(')
                {
                    break;
                }
            }
            int closeScobka = 0;
            for (closeScobka = 0; closeScobka < str.Length; ++closeScobka)
            {
                if (str[closeScobka] == ')')
                {
                    break;
                }
            }
            if (openScobka >= closeScobka)
            {
                return false;
            }
            str = str.Remove(0, 1);
            str = str.Remove(str.Length - 1, 1);
            return true;
        }
        private int findFirstOperat(ref string str)
        {
            int pm = -1; //+, - обозначает на каком месте находится первый оператор
            int ud = -1; //*, / обозначает на каком месте находится первый оператор
            int v = -1; //^ обозначае на каком месте находится первый оператор
            for (int i = 1; i < str.Length; ++i)
            {
                if (str[i] == '(')
                {
                    break;
                }
                if (str[i] == '+' || str[i] == '-')
                {
                    pm = i;
                    break;
                }
            }
            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] == '(')
                {
                    break;
                }
                if (str[i] == '*' || str[i] == '/')
                {
                    ud = i;
                    break;
                }
            }
            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] == '(')
                {
                    break;
                }
                if (str[i] == '^')
                {
                    ud = i;
                    break;
                }
            }
            if (pm != -1)
            {
                return pm;
            }
            else if (ud != -1)
            {
                return ud;
            }
            else
            {
                return v;
            }
        }
        public double calculate(ref Dictionary<string, double> namesValues)
        {
            if (operat == null)
            {
                try
                {
                    return namesValues[this.value];
                }
                catch
                {

                }
                return Convert.ToDouble(this.value.Replace('.', ','));
            }
            else
            {
                if (left == null && right != null)
                {
                    double tmp = this.operat.calculate(this.right.calculate(ref namesValues));
                    //Console.WriteLine(tmp);
                    return tmp;
                }
                else if (left != null && right != null)
                {
                    double tmp = this.operat.calculate(this.left.calculate(ref namesValues), this.right.calculate(ref namesValues));
                    //Console.WriteLine(tmp);
                    return tmp;
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        public void output()
        {
            Console.Write((operat == null ? value : operat.getOutput()) + " ");
            if (left != null)
            {
                Console.Write((left.operat == null ? left.value : left.operat.getOutput()) + "l ");
            }
            if (right != null)
            {
                Console.Write((right.operat == null ? right.value : right.operat.getOutput()) + "r ");
            }
            Console.WriteLine();
            if (left != null)
                left.output();
            if (right != null)
                right.output();
        }
        private node left;
        private node right;
        private Operat operat;
        private string value;

    }
    public class Operat
    {
        public static Operat createOperat(string str)
        {
            int tmpType = str.Length > 0 ? Operat.operators.IndexOf(str) : -1;
            if (tmpType < 0)
            {
                return null;
            }
            else
            {
                return new Operat(tmpType);
            }
        }
        private Operat(int numberOperat)
        {
            type = numberOperat;
        }
        public static List<string> operators = (new string[] { "+", "-", "*", "/", "^", "ln" }).ToList<string>();
        private int? type = null;
        public double calculate(double _arg1, double? _arg2 = null)
        {
            double arg1 = _arg1;
            double arg2 = _arg2 != null ? Convert.ToDouble(_arg2) : 0;
            //Console.WriteLine(Operat.operators[Convert.ToInt32(type)] + " arg1 " + arg1.ToString() + (_arg2 != null ? (" arg2 " + arg2.ToString()) : ""));
            if (_arg2 != null)
            {
                if (this.type == 0)
                {
                    return arg1 + arg2;
                }
                else if (this.type == 1)
                {
                    return arg1 - arg2;
                }
                else if (this.type == 2)
                {
                    return arg1 * arg2;
                }
                else if (this.type == 3)
                {
                    return arg1 / arg2;
                }
                else if (this.type == 4)
                {
                    return Math.Pow(arg1, arg2);
                }
                else
                {
                    throw new Exception();
                }
            }
            else
            {
                if (this.type == 0)
                {
                    return arg1;
                }
                else if (this.type == 1)
                {
                    return -arg1;
                }
                else if (this.type == 5)
                {
                    return Math.Log(arg1);
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        public string getOutput()
        {
            return operators[Convert.ToInt32(type)];
        }
    }
    public class Answer
    {
        public Answer(int _result, Dictionary<string, double> _values)
        {
            result = _result;
            if (_values == null)
                return;
            foreach (var i in _values)
            {
                this.values.Add(i.Key, i.Value);
            }
        }
        public Answer(Answer x)
        {
            result = x.result;
            foreach (var i in x.values)
            {
                this.values.Add(i.Key, i.Value);
            }
        }
        public Answer umnj(double x)
        {
            Answer tmp = new Answer(this.result, this.values);
            List<string> tmpList = new List<string>();
            tmpList.AddRange(tmp.values.Keys.ToList<string>());
            for (int i = 0; i < tmpList.Count; ++i)
            {
                tmp.values[tmpList[i]] *= x;
            }
            return tmp;
        }
        public Answer del(double x)
        {
            Answer tmp = new Answer(this.result, this.values);
            List<string> tmpList = new List<string>();
            tmpList.AddRange(tmp.values.Keys.ToList<string>());
            for (int i = 0; i < tmpList.Count; ++i)
            {
                tmp.values[tmpList[i]] /= x;
            }
            return tmp;
        }
        public static Answer plus(Answer x, Answer y)
        {
            Answer tmp = new Answer(x.result, x.values);
            List<string> tmpList = new List<string>();
            tmpList.AddRange(tmp.values.Keys.ToList<string>());
            for (int i = 0; i < tmpList.Count; ++i)
            {
                tmp.values[tmpList[i]] += y.values[tmpList[i]];
            }
            return tmp;
        }
        public Dictionary<string, double> values = new Dictionary<string, double>();
        public int result = -10;
    }
    public class Expression
    {
        public Answer parseExpression(string _str)
        {
            string str = "";
            for (int i = 0; i < _str.Length; ++i)
            {
                if (_str[i] != ' ')
                {
                    str += _str[i];
                }
            }
            int result = analizExpression(ref str);
            if (result == -1)
            {
                //int st = 0; //состояние
                string buf = ""; //буфер в который будут накапливаться числа и имена переменных
                Dictionary<string, double> set = new Dictionary<string, double>();
                //алфавит 0-9, +, -, *, /, a-z, A-Z, (, ), .
                for (int i = 0; i < str.Length; ++i)
                {
                    if (Char.IsLetter(str[i]))
                    {
                        buf += str[i];
                        for (i += 1; i < str.Length; ++i)
                        {
                            if (Char.IsLetter(str[i]) || Char.IsDigit(str[i]))
                            {
                                buf += str[i];
                            }
                            else
                            {
                                if (Operat.operators.IndexOf(buf) > -1)
                                {
                                    buf = "";
                                    break;
                                }
                                if (!set.ContainsKey(buf))
                                {
                                    set.Add(buf, 1.0);
                                }
                                buf = "";
                                break;
                            }
                        }
                    }
                }
                if (buf.Length > 0 && Operat.operators.IndexOf(buf) < 0)
                {
                    set.Add(buf, 0.0);
                }
                root = node.createNode(str);
                root.output(); //////
                //Console.WriteLine(root.calculate(ref set));
                return new Answer(result, set);
            }
            else
                return new Answer(result, null);
        }
        public double calculate(Dictionary<string, double> _set)
        {
            return root.calculate(ref _set);
        }
        private node root = null;
        private int analizExpression(ref string str) //в случае ошибки возвращает номер первого ошибочного символа в строке, -1 если всё нормально, -2 есть незакрытые скобки
        {

            //алфавит 0-9, +, -, *, /, a-z, A-Z, (, ), .
            int countLeft = 0; //количество скобок слева которые ещё не закрыты
            int st = 0; //номер состояния
            int prevSt = 0; //номер предыдущего состояния
            string buf = "";
            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] == ' ')
                    continue;
                //Console.WriteLine("st = " + st);
                if (st == 0) //st = 1-5
                {
                    if (Char.IsDigit(str[i]))
                    {
                        st = 1;
                    }
                    else if (str[i] == '+' || str[i] == '-')
                    {
                        st = 2;
                    }
                    else if (char.IsLetter(str[i]))
                    {
                        st = 4;
                        buf += str[i];
                    }
                    else if (str[i] == '(')
                    {
                        st = 5;
                        ++countLeft;
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (st == 1) //st = 1, 6-9 если символ число
                {
                    if (Char.IsDigit(str[i]))
                    {
                        st = 1;
                    }
                    else if (str[i] == '+' || str[i] == '-' || str[i] == '*')
                    {
                        st = 6;
                    }
                    else if (str[i] == '/')
                    {
                        st = 9;
                    }
                    else if (str[i] == '^')
                    {
                        st = 18;
                    }
                    else if (str[i] == ')')
                    {
                        if (countLeft == 0)
                            return i;
                        else
                        {
                            --countLeft;
                            st = 16;
                        }
                    }
                    else if (str[i] == '.')
                    {
                        prevSt = st;
                        st = 17;
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (st == 2) //st = 1-12, если первый символ +, -
                {
                    if (Char.IsDigit(str[i]))
                    {
                        st = 1;
                    }
                    else if (char.IsLetter(str[i]))
                    {
                        st = 4;
                        buf += str[i];
                    }
                    else if (str[i] == '(')
                    {
                        st = 5;
                        countLeft += 1;
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (st == 4) //если буква
                {
                    if (Char.IsDigit(str[i]))
                    {
                        st = 4;
                        buf += str[i];
                    }
                    else if (str[i] == '+' || str[i] == '-' || str[i] == '*')
                    {
                        if (Operat.operators.IndexOf(buf) > 0)
                        {
                            return i;
                        }
                        else
                        {
                            buf = "";
                        }
                        st = 6;
                    }
                    else if (str[i] == '/')
                    {
                        if (Operat.operators.IndexOf(buf) > 0)
                        {
                            return i;
                        }
                        else
                        {
                            buf = "";
                        }
                        st = 9;
                    }
                    else if (str[i] == '^')
                    {
                        if (Operat.operators.IndexOf(buf) > 0)
                        {
                            return i;
                        }
                        else
                        {
                            buf = "";
                        }
                        st = 18;
                    }
                    else if (char.IsLetter(str[i]))
                    {
                        st = 4;
                        buf += str[i];
                    }
                    else if (str[i] == '(')
                    {
                        if (Operat.operators.IndexOf(buf) < 0)
                        {
                            return i;
                        }
                        else
                        {
                            buf = "";
                        }
                        ++countLeft;
                    }
                    else if (str[i] == ')')
                    {
                        if (Operat.operators.IndexOf(buf) > 0)
                        {
                            return i;
                        }
                        else
                        {
                            buf = "";
                        }
                        if (countLeft == 0)
                            return i;
                        else
                        {
                            --countLeft;
                            st = 16;
                        }
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (st == 5) //если открывающая скобка
                {
                    if (Char.IsDigit(str[i]))
                    {
                        st = 1;
                    }
                    else if (str[i] == '+' || str[i] == '-')
                    {
                        st = 2;
                    }
                    else if (char.IsLetter(str[i]))
                    {
                        st = 4;
                        buf += str[i];
                    }
                    else if (str[i] == '(')
                    {
                        st = 5;
                        ++countLeft;
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (st == 6) //если +, -, *
                {
                    if (Char.IsDigit(str[i]))
                    {
                        st = 1;
                    }
                    else if (char.IsLetter(str[i]))
                    {
                        st = 4;
                        buf += str[i];
                    }
                    else if (str[i] == '(')
                    {
                        st = 5;
                        ++countLeft;
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (st == 9) //если символ деления - /
                {
                    if (Char.IsDigit(str[i]))
                    {
                        if (str[i] == '0')
                            st = 15;
                        else
                            st = 1;
                    }
                    else if (char.IsLetter(str[i]))
                    {
                        st = 4;
                        buf += str[i];
                    }
                    else if (str[i] == '(')
                    {
                        st = 5;
                        ++countLeft;
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (st == 15)
                {
                    if (Char.IsDigit(str[i]))
                    {
                        if (str[i] == '0')
                            return i;
                        else
                            st = 1;
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (st == 16) //если до этого была закрывающая скобка
                {
                    if (str[i] == '+' || str[i] == '-' || str[i] == '*')
                    {
                        st = 6;
                    }
                    else if (str[i] == '/')
                    {
                        st = 9;
                    }
                    else if (str[i] == '^')
                    {
                        st = 18;
                    }
                    else if (str[i] == ')')
                    {
                        if (countLeft == 0)
                            return i;
                        else
                        {
                            st = 16;
                            --countLeft;
                        }
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (st == 17) //если предыдущий символ точка - .
                {
                    if (Char.IsDigit(str[i]))
                    {
                        st = prevSt;
                    }
                    else
                    {
                        return i;
                    }
                }
                else if (st == 18) //если символ деления - /
                {
                    if (Char.IsDigit(str[i]))
                    {
                        st = 1;
                    }
                    else if (char.IsLetter(str[i]))
                    {
                        st = 4;
                        buf += str[i];
                    }
                    else if (str[i] == '(')
                    {
                        st = 5;
                        ++countLeft;
                    }
                    else
                    {
                        return i;
                    }
                }
            }
            if (Char.IsDigit(str[str.Length - 1]) || str[str.Length - 1] == ')' ||
                Char.IsLetter(str[str.Length - 1]))
            {

            }
            else
            {
                return str.Length - 1;
            }
            if (countLeft != 0)
            {
                return -2;
            }
            return -1;
        }
    }
    public static class NelderMid
    {
        //public static int NP = 0; // NP - число аргументов функции
        //static double[,] simplex = null; // NP + 1 - число вершин симплекса
        static double[] FN = null;
        public static string outputStr = "";
        public static Expression exp = new Expression();
        public static Answer answer = null;
        private static int countSymbols = 5;
        static Dictionary<string, double> pairs = null;
        private static double F(double[] X, int NP) // Функциия Розенброка
        {
            List<string> keys = new List<string>();
            keys.AddRange(answer.values.Keys);
            keys.Sort();
            pairs = new Dictionary<string, double>();
            for (int i = 0; i < keys.Count; ++i)
            {
                pairs.Add(keys[i], X[i]);
            }
            double tmp = exp.calculate(pairs);
            //Console.WriteLine(tmp);
            return tmp;
        }
        // Создает из точки X регулярный симплекс с длиной ребра L и с NP + 1 вершиной
        // Формирует массив FN значений оптимизируемой функции F в вершинах симплекса
        private static double[,] makeSimplexMy(double[] X, double L, int NP)
        {
            double s1, s2; //sigma1, sigma2
            s1 = (Math.Sqrt(NP + 1) + NP - 1.0) / (NP * Math.Sqrt(2.0)) * L;
            s2 = (Math.Sqrt(NP + 1) - 1.0) / (NP * Math.Sqrt(2.0)) * L;
            double[,] simp = new double[NP, NP + 1];
            for (int i = 0; i < NP; ++i)
            {
                simp[i, 0] = X[i];
            }
            for (int j = 1; j < NP + 1; ++j)
            {
                for (int i = 0; i < NP; ++i)
                {
                    if (i == j - 1)
                    {
                        simp[i, j] = X[i] + s2;
                    }
                    else
                    {
                        simp[i, j] = X[i] + s1;
                    }
                }
            }
            return simp;
        }
        private static double[] center_of_gravity(double[,] simp, int k, int NP) // Центр тяжести симплекса
        {
            double[] xc = new double[NP];
            for(int i = 0; i < NP; ++i)
            {
                xc[i] = 0;
            }
            for(int j = 0; j < NP+1; ++j)
            {
                if (j == k)
                    continue;
                for(int i = 0; i < NP; ++i)
                {
                    xc[i] += simp[i, j];
                }
            }
            for (int i = 0; i < NP; ++i)
            {
                xc[i] /= NP;
            }
            return xc;
        }
        /*
        private static void reflection(int k, double cR, int NP) // Отражение вершины с номером k относительно центра тяжести
        {
            outputStr += "Отражение " + (k+1) + " вершины относительно центра тяжести\n";
            double[] xc = center_of_gravity(k, NP); // cR – коэффициент отражения
            outputStr += "Центр тяжести\n";
            outputCenterOfGravity(xc);
            for (int i = 0; i < NP; i++)
            {
                //simplex[i, k] = (1.0 + cR) * xc[i] - simplex[i, k];
                simplex[i, k] = (1.0 + cR) * xc[i] - cR*simplex[i, k];
            }
            List<double> list = new List<double>();
            outputStr += "Значение " + (k + 1) + " вершины после отражения\n";
            for(int i = 0; i < NP; ++i)
            {
                outputStr += string.Format("{0:N" + countSymbols + "}", simplex[i, k]) + " ";
                list.Add(simplex[i, k]);
            }
            outputStr += "\n";
            outputStr += "Значение функции в этой вершине\n";
            outputStr += string.Format("{0:N" + countSymbols + "}", F(list.ToArray(), NP));
            outputStr += "\n";
            //outputStr += "Симплекс после отражаения\n";
            //outputSimplex();
        }
        */
        private static double[] reflectionMy(double[] xc, double[] xh, double alpha, int NP) // Отражение вершины с номером k относительно центра тяжести
        {
            double[] xr = new double[NP];
            for(int i = 0; i< NP; ++i)
            {
                xr[i] = (1 + alpha) * xc[i] - alpha * xh[i];
            }
            return xr;
        }
        private static double[] calculateFuncsSimplex(double[,] simp, int NP)
        {
            double[] funcs = new double[NP+1];
            for(int j = 0; j < NP+1; ++j)
            {
                double[] tmpx = new double[NP];
                for(int i = 0;i < NP; ++i)
                {
                    tmpx[i] = simp[i, j];
                }
                funcs[j] = F(tmpx, NP);
            }
            return funcs;
        }
        private static double[,] reduction(double[,] simp, int indexXL, double kReduc, int NP) // Редукция симплекса к вершине k
        {
            double[,] simp2 = new double[NP, NP + 1];
            simp2 = simp;
            for(int j = 0; j < NP+1; ++j)
            {
                if (j == indexXL)
                    continue;
                for(int i = 0; i < NP; ++i)
                {
                    simp2[i, j] = simp2[i, indexXL] + 0.5 * (simp2[i, j] - simp2[i, indexXL]);
                }
            }
            return simp2;
        }
        private static double[] shrinking_expansion(double[] xc, double[] xr, double beta_gamma, int NP) // Сжатие/растяжение симплекса. alpha_beta – коэффициент растяжения/сжатия
        {
            double[] xe = new double[NP];
            for (int i = 0; i < NP; i++)
            {
                xe[i] = beta_gamma * xr[i] + (1 - beta_gamma) * xc[i];
            }
            return xe;
        }
        private static double findL(ref double[] X2, int NP) // Длиина ребра симплекса
        {
            double L = 0;
            for (int i = 0; i < NP; i++)
                L += X2[i] * X2[i];
            return Math.Sqrt(L);
        }
        private static bool alreadyStop(double [,] simp, double epsilon, int NP) // возвращает true, если sigma < epsilon
        {
            double[] tmpFuncs = new double[NP];
            tmpFuncs = calculateFuncsSimplex(simp, NP);
            //outputStr += "tmpFuncs:\n"; //out
            //outputMassive(ref tmpFuncs, NP+1); //out
            //outputStr += "\n"; //out

            double tmpSum = 0.0;
            for (int k = 0; k < NP+1; ++k)
            {
                tmpSum += tmpFuncs[k];
            }
            //outputStr += "tmpSum = " + tmpSum + "\n"; //out

            double sum = 0.0;
            for(int j = 0; j < NP+1; ++j)
            {
                double[] tmpx1 = new double[NP];
                for(int i = 0; i < NP; ++i)
                {
                    tmpx1[i] = simp[i, j];
                }
                double fxj = F(tmpx1, NP);
                //outputStr += "fxj = " + fxj + "\n"; //out
                double nef = 1.0 / (NP + 1.0) * tmpSum;
                //outputStr += "nef = " + nef + "\n"; //out
                sum += Math.Pow(fxj - nef, 2);
            }
            //outputStr += "sum = " + sum + "\n"; //out
            double sigma = Math.Sqrt(sum / (NP + 1.0));
            outputStr += "sigma = " + sigma + "\n";
            if(sigma < epsilon)
            {
                outputStr += "sigma < e\n";
                return true;
            }
            else
            {
                outputStr += "sigma > e\n";
                return false;
            }
        }
        // Выполняет поиск экстремума (минимума) функции F
        private static void nelMead(ref double[] X, int maxK, double L, double epsilon,
            double alpha, double beta, double gamma, double cR, int NP)
        {
            double[] answer = new double[NP];
            outputStr += "λ = " + L + "\n";
            outputStr += "ε = " + epsilon + "\n";
            outputStr += "cR = " + gamma + "\n";
            outputStr += "α = " + cR + "\n";
            outputStr += "β = " + beta + "\n";
            outputStr += "γ = " + alpha + "\n";
            double[,] simplex = new double[NP, NP + 1];
            simplex = makeSimplexMy(X, L, NP);
            int k = 0;
            while (k < maxK)
            {
                outputStr += "Итерация номер #" + (k+1) + "\n";
                outputStr += "Исходный симплекс\n";
                outputSimplex(ref simplex, NP);
                ++k;
                //шаг 1
                outputStr += "Шаг 1\n";
                double[] funcs = new double[NP + 1];
                funcs = calculateFuncsSimplex(simplex, NP);

                outputStr += "Значения функции в точках симплекса:\n"; //out
                outputMassive(ref funcs, NP + 1, true); //out

                double fh = funcs.ToList<double>().Max(), fg, fl = funcs.ToList<double>().Min();
                outputStr += "fh = " + outputDouble(fh) + "\n"; //out
                fg = fl;
                for (int i = 0; i < NP + 1; ++i)
                {
                    if (funcs[i] == fh)
                        continue;
                    if (funcs[i] > fg)
                    {
                        fg = funcs[i];
                    }
                }
                outputStr += "fg = " + outputDouble(fg) + "\n"; //out
                outputStr += "fl = " + outputDouble(fl) + "\n"; //out
                //шаг 2
                outputStr += "Шаг 2\n";
                double[] xh = new double[NP];
                for (int i = 0, j = funcs.ToList<double>().IndexOf(fh); i < NP; ++i)
                {
                    xh[i] = simplex[i, j];
                }
                outputStr += "xh:\n"; //out
                outputMassive(ref xh, NP); //out
                outputStr += "\n"; //out
                double[] xg = new double[NP];
                for (int i = 0, j = funcs.ToList<double>().IndexOf(fg); i < NP; ++i)
                {
                    xg[i] = simplex[i, j];
                }
                outputStr += "xg:\n"; //out
                outputMassive(ref xg, NP); //out
                outputStr += "\n"; //out
                double[] xl = new double[NP];
                for (int i = 0, j = funcs.ToList<double>().IndexOf(fl); i < NP; ++i)
                {
                    xl[i] = simplex[i, j];
                }
                outputStr += "xl:\n"; //out
                outputMassive(ref xl, NP); //out
                outputStr += "\n"; //out
                //шаг 3
                outputStr += "Шаг 3\n";
                double[] xc = new double[NP];
                xc = center_of_gravity(simplex, funcs.ToList<double>().IndexOf(fh), NP);
                outputStr += "xc:\n"; //out
                outputMassive(ref xc, NP); //out
                outputStr += "\n"; //out
                //шаг 4
                outputStr += "Шаг 4\n";
                double[] xr = new double[NP];
                xr = reflectionMy(xc, xh, alpha, NP);
                outputStr += "xr:\n"; //out
                outputMassive(ref xr, NP); //out
                outputStr += "\n"; //out
                double fr = F(xr, NP);
                outputStr += "fr = " + outputDouble(fr) + "\n"; //out

                if (fr < fl)
                {
                    outputStr += "fr < fl\n"; //out
                    double[] xe = new double[NP];
                    xe = shrinking_expansion(xc, xr, gamma, NP);
                    outputStr += "xe:\n"; //out
                    outputMassive(ref xe, NP); //out
                    outputStr += "\n"; //out
                    double fe = F(xe, NP);
                    outputStr += "fe = " + outputDouble(fe) + "\n"; //out
                    if (fe < fr)
                    //if (fe < fl)
                    {
                        outputStr += "fe < fr\n"; //out
                        for (int i = 0, j = funcs.ToList<double>().IndexOf(fh); i < NP; ++i)
                        {
                            simplex[i, j] = xe[i];
                        }
                        outputStr += "Симплекс после замены xh на xe:\n"; //out
                        outputSimplex(ref simplex, NP); //out
                        if (alreadyStop(simplex, epsilon, NP))
                        {
                            for (int i = 0; i < NP; ++i)
                            {
                                answer[i] = xe[i];
                            }
                            break;
                        }
                    }
                    else if (fe >= fr)
                    //else if (fe >= fl)
                    {
                        outputStr += "fe >= fr\n"; //out
                        for (int i = 0, j = funcs.ToList<double>().IndexOf(fh); i < NP; ++i)
                        {
                            simplex[i, j] = xr[i];
                        }
                        outputStr += "Симплекс после замены xh на xr:\n"; //out
                        outputSimplex(ref simplex, NP); //out
                        if (alreadyStop(simplex, epsilon, NP))
                        {
                            for (int i = 0; i < NP; ++i)
                            {
                                answer[i] = xr[i];
                            }
                            break;
                        }
                    }
                }
                else if (fl <= fr && fr <= fg)
                {
                    outputStr += "fl <= fr <= fg\n";
                    for (int i = 0, j = funcs.ToList<double>().IndexOf(fh); i < NP; ++i)
                    {
                        simplex[i, j] = xr[i];
                    }
                    outputStr += "Симплекс после замены xh на xr:\n"; //out
                    outputSimplex(ref simplex, NP); //out
                    if (alreadyStop(simplex, epsilon, NP))
                    {
                        for (int i = 0, j = funcs.ToList<double>().IndexOf(fl); i < NP; ++i)
                        {
                            answer[i] = simplex[i, j];
                        }
                        break;
                    }
                }
                else if (fg < fr)
                {
                    outputStr += "fg < fr\n"; //out
                    //шаг 6
                    outputStr += "Шаг 6\n"; //out
                    double[] xs = new double[NP];
                    if (fh < fr)
                    {
                        outputStr += "fh < fr\n"; //out
                        xs = shrinking_expansion(xc, xh, beta, NP);
                        outputStr += "xs:\n"; //out
                        outputMassive(ref xs, NP); //out
                        outputStr += "\n"; //out
                    }
                    else if (fh > fr)
                    {
                        outputStr += "fh > fr\n"; //out
                        xs = shrinking_expansion(xc, xr, beta, NP);
                        outputStr += "xs:\n"; //out
                        outputMassive(ref xs, NP); //out
                        outputStr += "\n"; //out
                    }
                    double fs = F(xs, NP);
                    outputStr += "fs = " + outputDouble(fs) + "\n"; //out
                    //шаг 7
                    outputStr += "Шаг 7\n";
                    if (fs < (fh < fr ? fh : fr))
                    //if (fs < fh)
                    {
                        outputStr += "fs < min(fh, fr)\n";
                        for (int i = 0, j = funcs.ToList<double>().IndexOf(fh); i < NP; ++i)
                        {
                            simplex[i, j] = xs[i];
                        }
                        outputStr += "Симплекс после замены xh на xs:\n"; //out
                        outputSimplex(ref simplex, NP); //out
                        if (alreadyStop(simplex, epsilon, NP))
                        {
                            for (int i = 0; i < NP; ++i)
                            {
                                answer[i] = xs[i];
                            }
                            break;
                        }
                    }
                    else if (fs > fh)
                    {
                        outputStr += "fs > fh\n"; //out
                        //шаг 8
                        outputStr += "Шаг 8\n"; //out
                        simplex = reduction(simplex, funcs.ToList<double>().IndexOf(fl), epsilon, NP);
                        outputStr += "Симплекс после редукции:\n"; //out
                        outputSimplex(ref simplex, NP); //out
                        double[] newFuncs = new double[NP];
                        newFuncs = calculateFuncsSimplex(simplex, NP);
                        double newFL = newFuncs.ToList<double>().Min();
                        if (alreadyStop(simplex, epsilon, NP))
                        {
                            for (int i = 0, j = newFuncs.ToList<double>().IndexOf(newFL); i < NP; ++i)
                            {
                                answer[i] = simplex[i, j];
                            }
                            break;
                        }
                    }
                }
                outputStr += "\n";
            }
            outputStr += "Число итераций: " + k + "\n";
            outputStr += "Результат: ";
            for(int i =0; i < NP; ++i)
            {
                outputStr += answer[i] + " ";
            }
            outputStr += "\nКонечный симплекс:\n";
            for (int j = 0; j < NP + 1; ++j)
            {
                for(int i = 0; i < NP; ++i)
                {
                    outputStr += simplex[i, j] + " ";
                }
                outputStr += "\n";
            }
        }
        private static void outputSimplex(ref double[,] simp, int NP)
        {
            for (int j = 0; j < NP + 1; ++j)
            {
                for (int i = 0; i < NP; ++i)
                {
                    outputStr += string.Format("{0:N" + countSymbols + "}", simp[i, j]) + " ";
                }
                outputStr += "\n";
            }
        }
        private static void outputMassive(ref double[] mas, int NP, bool perenos = false)
        {
            for (int i = 0; i < NP; ++i)
            {
                outputStr += string.Format("{0:N" + countSymbols + "}", mas[i]) + (perenos ? "\n" : " ");
            }
        }
        private static string outputDouble(double x)
        {
            return string.Format("{0:N" + countSymbols + "}", x);
        }
        public static void Go_Click(double[] _X, int maxK, double _epsilon, double _L, 
            double _alpha, double _beta, double _gamma, double _kReduc, string _strFunc = "", int _countSymbolsAfterDot = 0)
        {
            int NP = answer.values.Count;
            FN = new double[NP + 1];
            double[] X = _X; // Первая вершина начального симплекса (начальная точка)
            double L, L_thres, cR, alpha, beta, gamma;
            L = _L; // Начальная длина ребра симплекса
            L_thres = _epsilon; // Предельное значение длины ребра симплекса
            cR = _kReduc; // Коэффициент отражения симплекса
            alpha = _alpha; // Коэффициент растяжения симплекса
            beta = _beta; // Коэффициент сжатия симплекса
            gamma = _gamma; // Коэффициент редукции симплекса
            outputStr = null;
            outputStr = "";
            outputStr += _strFunc.Length > 0 ? ("Исходная функция\n" + _strFunc + "\n") : "";
            countSymbols = _countSymbolsAfterDot;
            outputStr += "Начальная точка:\n"; // Результат
            for (int i = 0; i < NP; i++)
            {
                outputStr += X[i]+"\n";
            }
            nelMead(ref X, maxK, L, L_thres, alpha, beta, gamma, _kReduc, NP); // Поиск минимума функции Розенброка
        }
    }
}
