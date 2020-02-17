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
using System.Windows.Shapes;
using System.ComponentModel; // CancelEventArgs

namespace stnd_72_v2
{
    /// <summary>
    /// Логика взаимодействия для DAC_form.xaml
    /// </summary>
    public partial class DAC_form : Window
    {
        public DAC_form(string a)
        {
            InitializeComponent();
            this.Name_this = a;
            this.Title = a;
        }

        string Name_this = "";

        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            // MessageBox.Show("Closing called");
            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (Name_this == "DAC0") main.D_form[0] = false;
                if (Name_this == "DAC1") main.D_form[1] = false;
            }

        }

        private void button_init_lmk_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
