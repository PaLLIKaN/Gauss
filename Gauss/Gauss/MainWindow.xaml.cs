// This is an open source non-commercial project. Dear PVS-Studio, please check it.

// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Gauss
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string vars = "xyzijmnk";

        int count_variable = 2;
        Grid grid_system = new Grid();
        TextBox[,] matrix = new TextBox[9, 9];
        string result = "Empty";
        TextBlock res;


        public MainWindow()
        {
            InitializeComponent();
            init_tab_expression();
        }

        private void init_tab_expression()
        {
            for (int i = 0; i < 25; i++)
            {
                Logic.ColumnDefinitions.Add(new ColumnDefinition());
                Logic.RowDefinitions.Add(new RowDefinition());


                Solution.ColumnDefinitions.Add(new ColumnDefinition());
                Solution.RowDefinitions.Add(new RowDefinition());
            }

            ScrollViewer cont = new ScrollViewer();

            TextBlock sol = new TextBlock();
            sol.Text = result;
            sol.Text = result;
            sol.FontSize = 13;
            res = sol;
            Grid.SetColumn(cont, 0);
            Grid.SetRow(cont, 0);
            Grid.SetColumnSpan(cont, 25);
            Grid.SetRowSpan(cont, 25);
            cont.Content = res;
            Solution.Children.Add(cont);

            TextBlock txt1 = new TextBlock();
            txt1.Text = "Система уравнений";
            txt1.FontSize = 13;
            Grid.SetColumn(txt1, 6);
            Grid.SetRow(txt1, 0);
            Grid.SetColumnSpan(txt1, 6);


            TextBlock txt2 = new TextBlock();
            txt2.Text = "Количетство переменных";
            txt2.FontSize = 12;
            Grid.SetColumn(txt2, 16);
            Grid.SetRow(txt2, 0);
            Grid.SetColumnSpan(txt2, 10);

            Button btn = new Button();
            btn.Click += handler_button;
            btn.Content = "Решить систему уравнений";
            
            Grid.SetRow(btn, 23);
            Grid.SetRowSpan(btn, 2);
            Grid.SetColumn(btn, 9);
            Grid.SetColumnSpan(btn, 6);

            Button btn2 = new Button();
            btn2.Click += handler_button_clear;
            btn2.Content = "Отчистить";

            Grid.SetRow(btn2, 23);
            Grid.SetRowSpan(btn2, 2);
            Grid.SetColumn(btn2, 20);
            Grid.SetColumnSpan(btn2, 3);

            ComboBox cmb = new ComboBox();
            cmb.SelectionChanged += handler_count_variable;
            init_grid_system();
            for (int i = 2; i < 9; i++)
                cmb.Items.Add(Convert.ToString(i));
            Grid.SetRow(cmb, 0);
            Grid.SetColumn(cmb, 21);
            Grid.SetColumnSpan(cmb, 2);
            Grid.SetRowSpan(cmb, 2);
            cmb.FontSize = 13;
            cmb.SelectedIndex = 0;



            Logic.Children.Add(cmb);
            Logic.Children.Add(txt1);
            Logic.Children.Add(txt2);
            Logic.Children.Add(btn);
            Logic.Children.Add(btn2);



        }

        private void init_grid_system()
        {
            grid_system = new Grid();
            for (int i = 0; i < 10; i++)
                grid_system.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 20; i++)
                grid_system.ColumnDefinitions.Add(new ColumnDefinition());

            Grid.SetRow(grid_system, 3);
            Grid.SetColumn(grid_system, 0);
            Grid.SetColumnSpan(grid_system, 20);
            Grid.SetRowSpan(grid_system, 10);
            layout_system();
            Logic.Children.Add(grid_system);

        }
        private void layout_system()
        {

            for (int i = 0; i < count_variable; i++)
            {
                for (int j = 1; j <= count_variable; j++)
                {
                    TextBox temp = new TextBox(); //entry field
                    Grid.SetColumn(temp, j * 2);
                    Grid.SetRow(temp, i);
                    temp.FontSize = 13;
                    temp.MaxLength = 4;

                    TextBlock temp2 = new TextBlock();
                    Grid.SetColumn(temp2, j * 2 + 1);
                    Grid.SetRow(temp2, i);

                    matrix[i, j - 1] = temp;
                    temp.Text = "";

                    if (j != count_variable)
                        temp2.Text = "*" + Convert.ToString(vars[j - 1]) + " + ";
                    else
                    {
                        temp2.Text = "*" + Convert.ToString(vars[j - 1]) + " = ";
                        TextBox res = new TextBox();
                        Grid.SetColumn(res, j * 2 + 2);
                        Grid.SetRow(res, i);
                        grid_system.Children.Add(res);
                        matrix[i, j] = res;
                        res.Text = "";
                    }
                    temp2.FontSize = 13;


                    grid_system.Children.Add(temp2);

                    grid_system.Children.Add(temp);
                }


            }
        }



        private void handler_count_variable(object b, RoutedEventArgs ev)
        {
            count_variable = Convert.ToInt32(((ComboBox)ev.Source).SelectedValue.ToString());
            Logic.Children.Remove(grid_system);
            init_grid_system();
        }

        private void handler_button(object b, RoutedEventArgs ev)
        {
            Matrix temp = new Matrix(count_variable, count_variable + 1);
            bool validation = true;
            for (int row = 0; row < count_variable; row++)
            {
                for (int col = 0; col < count_variable + 1; col++)
                {
                    if (matrix[row, col].Text == "")
                    {
                        temp[row, col] = 0;
                    }
                    else if (double.TryParse(matrix[row, col].Text, out var number))
                    {
                        temp[row, col] = Convert.ToDouble(matrix[row, col].Text);
                    }
                    else
                    {
                        validation = false;
                        matrix[row, col].Text = "";
                    }
                }
            }
            if (validation == false)
            {
                MessageBox.Show("Введены некоректные данные: введите данные снова.", "Недопускаемые значения", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                res.Text = "Не введена система уравнений";
                return;
            }
            Tb.SelectedIndex = 2;
            Tb.SelectedItem = sol;
            sol.IsSelected= true;
            temp.gauss();
            result = temp.Work;
            res.Text = result;
        }

        private void handler_button_clear(object b, RoutedEventArgs ev)
        {
            for (int row = 0; row < count_variable; row++)
            {
                for (int col = 0; col < count_variable + 1; col++)
                {
                    matrix[row, col].Text = "";
                }
            }
        }
        private void Tb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

