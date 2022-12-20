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

namespace Canvas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnCE_Click(object sender, RoutedEventArgs e)
        {
            //
            if (sender.Equals(this.btn0))            
            {

                this.txtCalculo.Text += "0";
                
            }else if (sender.Equals(this.btn1))
            {

                this.txtCalculo.Text += "1";

            }
            else if (sender.Equals(this.btn2))
            {

                this.txtCalculo.Text += "2";

            }
            else if (sender.Equals(this.btn3))
            {

                this.txtCalculo.Text += "3";

            }
            else if (sender.Equals(this.btn4))
            {

                this.txtCalculo.Text += "4";

            }
            else if (sender.Equals(this.btn5))
            {

                this.txtCalculo.Text += "5";

            }
            else if (sender.Equals(this.btn6))
            {

                this.txtCalculo.Text += "6";

            }
            else if (sender.Equals(this.btn7))
            {

                this.txtCalculo.Text += "7";

            }
            else if (sender.Equals(this.btn8))
            {

                this.txtCalculo.Text += "8";

            }
            else if (sender.Equals(this.btn9))
            {

                this.txtCalculo.Text += "9";

            }
            else if(sender.Equals(this.btnDiv))
            {

                this.txtCalculo.Text += "/";

            }else if (sender.Equals(this.btnRes))
            {

                this.txtCalculo.Text += "-";

            }
            else if(sender.Equals(this.btnSum))
            {

                this.txtCalculo.Text += "+";

            }else if (sender.Equals(this.btnDec))
            {

                this.txtCalculo.Text += ",";

            }else if(sender.Equals(btnMult))
            {

                this.txtCalculo.Text += "x";
            }else if(sender.Equals(btnSupr))
            {

                this.txtCalculo.Text = null;
            }else if(sender.Equals(btnCE))
            {
                this.txtCalculo.Text = "0";
            }else if(sender.Equals(btnC))
            {

                string cadena = this.txtCalculo.Text.ToString();

                if(cadena.Length > 0)
                {
                    this.txtCalculo.Text = cadena.Remove(this.txtCalculo.Text.Length - 1);
                }
            }

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void calcular()
        {

            string cadena = this.txtCalculo.Text.ToString();

            if(cadena!=null && cadena != "")
            {
                this.txtCalculo.Text = checkCadena(cadena).ToString();
            }
        }

        private Double checkCadena(String c)
        {
            String[] cadResult = new String[1];
            Double num = 0, num1 = 0;

            cadResult[0] = c;

            if (c.Contains("-"))
            {

                cadResult = c.Split("-");

                //comprobamos si la cadena 0 aún contiene caracteres
                try
                {
                    num = Double.Parse(cadResult[0]);
                }
                catch (Exception e)
                {

                    num = checkCadena((String)cadResult[0]);
                }

                //comprobamos si aún quedan caracteres especiales en la cadena
                try
                {
                    num1 = Double.Parse(cadResult[1]);
                }
                catch (Exception e)
                {

                    num1 = checkCadena((String)cadResult[1]);
                }

                return num - num1;

            }
            else if (c.Contains("+"))
            {

                cadResult = c.Split("+");

                //comprobamos si la cadena 0 aún contiene caracteres
                try
                {
                    num = Double.Parse(cadResult[0]);
                }
                catch (Exception e)
                {

                    num = checkCadena((String)cadResult[0]);
                }

                //comprobamos si aún quedan caracteres especiales en la cadena
                try
                {
                    num1 = Double.Parse(cadResult[1]);
                }
                catch (Exception e)
                {

                    num1 = checkCadena((String)cadResult[1]);
                }

                return num + num1;
            }
            else if (c.Contains("x"))
            {

                cadResult = c.Split("x");

                //comprobamos si la cadena 0 aún contiene caracteres
                try
                {
                    num = Double.Parse(cadResult[0]);
                }
                catch (Exception e)
                {

                    num = checkCadena((String)cadResult[0]);
                }

                //comprobamos si aún quedan caracteres especiales en la cadena
                try
                {
                    num1 = Double.Parse(cadResult[1]);
                }
                catch (Exception e)
                {

                    num1 = checkCadena((String)cadResult[1]);
                }

                return num * num1;

            }else if (c.Contains("/"))
            {

                cadResult = c.Split("/");

                //comprobamos si la cadena 0 aún contiene caracteres
                try
                {
                    num = Double.Parse(cadResult[0]);
                }
                catch (Exception e)
                {

                    num = checkCadena((String)cadResult[0]);
                }

                //comprobamos si aún quedan caracteres especiales en la cadena
                try
                {
                    num1 = Double.Parse(cadResult[1]);
                }
                catch (Exception e)
                {

                    num1 = checkCadena((String)cadResult[1]);
                }

                return num / num1;

            }else
            {
                return Double.Parse(cadResult[0]);
            }

        }

        private void btnResultado_Click(object sender, RoutedEventArgs e)
        {
            calcular();
        }
    }
}
