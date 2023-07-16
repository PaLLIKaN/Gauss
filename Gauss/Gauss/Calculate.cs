// This is an open source non-commercial project. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Matrix
{

    private double[,] matrix;
    private int row;
    private int column;
    private string work = "";

    public Matrix() { }

    public Matrix(double[,] B)
    {
        matrix = B;
    }
    public Matrix(int row, int column)
    {
        this.row = row;
        this.column = column;
        matrix = new double[this.row, this.column];
    }

    public double this[int i, int j]
    {
        get
        {
            return matrix[i, j];
        }
        set
        {
            matrix[i, j] = value;
        }
    }

    public string print(double[,] matrix, int n)
    { // Метод вывода всей матрицы строкой
        string temp = "";
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < n + 1; j++)
            {
                temp += matrix[i, j] < 0 ? "" : " ";
                temp += Math.Round(matrix[i, j], 2) + "\t";
            }

            temp += "\n";
        }
        return temp;
    }
    public string Work { get => work; set => work = value; } // Строка хранящая в себе метод Гаусса
    public void gauss()
    {

        Work = $"Перепишем систему уравнений в матричном виде и решим его методом Гаусса: \n" + print(matrix, row);

        int t = 0;
        int r = 0;
        int k = 0;
        int flag = 1;

        Work += "С помощью преобразований делаем треугольную матрицу:\n";
        for (int i = 0; i < row; ++i)
        {
            Work += "Проверяем элемент главной диагонали на значение нуля:\n ";
            flag = checking_zero_main_diagonal(row, column, matrix, i, k);
            if (flag >= 0)
            {
                if (flag == 1)
                {
                    Work += print(matrix, row);
                }
                else
                {
                    Work += "Т.к. элемент на главной диагонале не нулевой, то оставляем матрицу без изменений.\n" + print(matrix, row);
                }
                if (matrix[k, i] != 0)
                {
                    Work += "Приводим элемент главной диагонали к единице:\n";
                    Work += $"Делим все элементы строки {k + 1} на: {Math.Round(matrix[k, i], 2)} \n";
                    reduction_main_diagonal(matrix, column, i, k); //Reduction of the main diagonal element to one
                    Work += print(matrix, row);
                    if (t > r)
                    {
                        converting_rows_main_element(row, column, matrix, i, k); //Converting rows below the main element
                    }
                    else
                        converting_rows_main_diagonal(row, column, matrix, i); //Converting rows below the main diagonal

                }
                k = i + 1;
            }
            else
            {
                Work += "Т.к. элемент на главной диагонали равен нулю и все элементы ниже него равны нулю, переходим к следующему столбцу.\n\n";
                k = i;
                r = t;
                t++;
            }

        }

        if (t > 0)
        {
            bool Checking = checking_for_incompatibility(matrix, row, column, t); //Checking for incompatibility of SLOWS
            if (Checking == false)
            {
                Work += $"Решения нет. Так как 0 ≠ {Math.Round(matrix[row - 1, column - 1], 2)}";
                return;
            }
            else
            {
                output_transformed_system_more(matrix, row, column);
                final_system_more(matrix, row, t);
                return;
            }

        }
        Work += "Конечная матрица: \n" + print(matrix, row);
        output_transformed_system_single(matrix, row, column);
        final_system_single(matrix, row);
    }
    public void reduction_main_diagonal(double[,] matrix, int m, int i, int k)
    {
        double koef = matrix[k, i];

        for (int j = i; j < m; ++j)
            matrix[k, j] /= koef;

    }
    public bool checking_for_incompatibility(double[,] matrix, int n, int m, int t)
    {
        for (int i = n - 1; i > n - 1 - t; i--)
        {
            if (matrix[i, m - 1] != 0)
            {
                for (int j = 0; j < m; ++j)
                {
                    if (matrix[i, j] != 0)
                        return true;
                    else
                        return false;
                }
            }


        }
        return true;
    }
    public int checking_zero_main_diagonal(int n, int m, double[,] matrix, int i, int k)
    {
        if (matrix[k, i] == 0)
        {
            for (int j = k + 1; j < n; j++)
            {
                if (matrix[j, i] != 0)
                {
                    Work += $"Меняем местами строку {k} со строкой {j}, так как элемент на главной диагонале равен нулю: \n";

                    for (int j1 = 0; j1 < m; j1++)
                    {
                        double x = matrix[k, j1];
                        matrix[k, j1] = matrix[j, j1];
                        matrix[j, j1] = x;

                    }
                    return 1;
                }

            }

            return -1;
        }
        else
            return 0;

    }
    public void converting_rows_main_diagonal(int n, int m, double[,] matrix, int i)
    {
        for (int j = i + 1; j < n; j++)
        {
            int p = 0;
            double koef;
            koef = matrix[j, i] / matrix[i, i];
            if (koef != 0)
            {
                Work += $"от {j + 1} строки отнимаем {i + 1} строку, умноженную на {Math.Round(koef, 2)}; ";
                for (int k = i; k < m; k++)
                {
                    matrix[j, k] -= matrix[i, k] * koef;
                }
                p++;
            }
            Work += "\n";
            if (p > 0)
                Work += print(matrix, row);
        }


    }

    public void converting_rows_main_element(int n, int m, double[,] matrix, int i, int k)
    {
        for (int j = i; j < n; j++)
        {
            int p = 0;
            double koef;
            koef = matrix[j, i] / matrix[k, i];
            if (koef != 0)
            {
                Work += $"от {j + 1} строки отнимаем {k + 1} строку, умноженную на {Math.Round(koef, 2)}; ";
                for (int l = i; l < m; l++)
                {
                    matrix[j, l] -= matrix[k, l] * koef;
                }
                p++;
            }
            Work += "\n";
            if (p > 0)
                Work += print(matrix, row);
        }

    }

    public void final_system_single(double[,] matrix, int n)
    {
        List<string> variables = new List<string>() { "x", "y", "z", "i", "j", "m", "n", "k" };
        double[] Score = new double[n];
        for (int i = n - 1; i >= 0; i--)
        {
            Score[i] = matrix[i, n];
            for (int j = n - 1; j > i; j--)
                Score[i] -= Score[j] * matrix[i, j];

        }
        Work += "В результате получим:\n ";
        string total = "{(";
        for (int i = 0; i < n; i++)
        {
            Work += $"{variables[i]} = {Math.Round(Score[i], 2) }\n";
            total += $" {Math.Round(Score[i], 2) }";
            if (i < n - 1)
                total += ";";
        }
        total += ")}";
        Work += "Ответ: " + total;
    }

    public void output_transformed_system_single(double[,] matrix, int n, int m)
    {
        List<string> variables = new List<string>() { "x", "y", "z", "i", "j", "m", "n", "k" };
        string[] Score = new string[n];
        Work += "Система имеет единственное решений: \n";
        Work += "Преобразовываем обратно в систему: \n";
        for (int i = 0; i < n; i++)
        {

            for (int j = 0; j < m; j++)
            {
                if (matrix[i, j] != 0 || j == n)
                {
                    Score[i] += $"";
                    if (i - j == 0)
                        Score[i] += $"{Math.Round(matrix[i, j], 2)}*{variables[j]} ";
                    else if (j <= n - 1)
                        Score[i] += $"+ {Math.Round(matrix[i, j], 2)}*{variables[j]} ";
                    else
                        Score[i] += $"= {Math.Round(matrix[i, j], 2)} ";
                }
            }
            Work += Score[i] + "\n";
        }
    }

    public void output_transformed_system_more(double[,] matrix, int n, int m)
    {
        List<string> variables = new List<string>() { "x", "y", "z", "i", "j", "m", "n", "k" };
        string[] Score = new string[n];
        Work += "Система имеет множество решений: \n";
        Work += "Преобразовываем обратно в систему: \n";
        for (int i = 0; i < n; i++)
        {

            for (int j = 0; j < m; j++)
            {
                if (matrix[i, j] != 0)
                {
                    Score[i] += $" {Math.Round(matrix[i, j], 2)}";
                    if (j == n - 1)
                        Score[i] += $"*{variables[j]} = ";
                    else if (j < n - 1)
                        Score[i] += $"*{variables[j]} +";
                    else
                        Score[i] += $"\n";
                }
            }
            Work += Score[i] ;
        }
    }
    public void final_system_more(double[,] matrix, int n, int t)
    {
        List<string> variables = new List<string>() { "x", "y", "z", "i", "j", "m", "n", "k" };
        List<string> constants = new List<string>() { "α", "β", "γ", "δ", "ε", "λ", "τ", "ρ" };
        string[] Score = new string[n];
        for (int i = n - 1; i >= n - t; i--)
        {
            Score[i] = $" {variables[i]} = {constants[i]}, {constants[i]} = const";
            

        }
        for(int i = n - t; i < n; i++)
        {
            Work += Score[i] + "\n";
        }
    }
}
