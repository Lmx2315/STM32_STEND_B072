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
using System.Diagnostics;

namespace stnd_72_v2
{
    /// <summary>
    /// Логика взаимодействия для ADC_form.xaml
    /// </summary>
    public partial class ADC_form : Window
    {
      
        public ADC_form(string a)//конструктор формы
        {
            InitializeComponent();
            this.Name_this = a;
            this.Title = a;
            
            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                Debug.WriteLine("Name_this:" + Name_this);
                if (Name_this == "ADC0")
                {
                    checkBox_adc_pwrdn.IsChecked = main.ADC0_072.PWRDN;
                    checkBox_ADC_init.IsChecked  = main.ADC0_072.init;
                    Debug.WriteLine("main.ADC0_072.PWRDN:" + main.ADC0_072.PWRDN);
                    Debug.WriteLine("main.ADC0_072.init :" + main.ADC0_072.init);
                }
                if (Name_this == "ADC1")
                {
                    checkBox_adc_pwrdn.IsChecked = main.ADC1_072.PWRDN;
                    checkBox_ADC_init.IsChecked  = main.ADC1_072.init;
                    Debug.WriteLine("main.ADC1_072.PWRDN:" + main.ADC1_072.PWRDN);
                    Debug.WriteLine("main.ADC1_072.init :" + main.ADC1_072.init);
                }
            }
                
        }      

        string Name_this = "";
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            // MessageBox.Show("Closing called");
            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (Name_this == "ADC0") main.A_form[0] = false;
                if (Name_this == "ADC1") main.A_form[1] = false;
            }

        }

        private void button_adc_phy_info_Click(object sender, RoutedEventArgs e)
        {

        }

        private void checkBox_adc_pwrdn_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        private void checkBox_adc_pwrdn_Click(object sender, RoutedEventArgs e)
        {
            string s1 = "";
            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (checkBox_adc_pwrdn.IsChecked == true)
                {
                    if (Name_this == "ADC0")
                    {
                        s1 = "~0 ADC1_PWRDN:1;";
                        main.ADC0_072.PWRDN = true;
                    }
                    if (Name_this == "ADC1")
                    {
                        s1 = "~0 ADC2_PWRDN:1;";
                        main.ADC1_072.PWRDN = true;
                    }
                    Debug.WriteLine(s1);
                } else
                {
                    if (Name_this == "ADC0")
                    {
                        s1 = "~0 ADC1_PWRDN:0;";
                        main.ADC0_072.PWRDN = false;
                    }
                    if (Name_this == "ADC1")
                    {
                        s1 = "~0 ADC2_PWRDN:0;";
                        main.ADC1_072.PWRDN = false;
                    }
                    Debug.WriteLine(s1);
                }
            }           

           
        }

        private void checkBox_ADC_init_Click(object sender, RoutedEventArgs e)
        {
            string s1 = "";
            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (checkBox_ADC_init.IsChecked == true)
                {
                    if (Name_this == "ADC0")
                    {
                        s1 = "~0 adc1_init:0;";
                        main.ADC0_072.init = true;
                    }
                    if (Name_this == "ADC1")
                    {
                        s1 = "~0 adc2_init:0;";
                        main.ADC1_072.init = true;
                    }
                    Debug.WriteLine(s1);
                }
                else
                {
                    if (Name_this == "ADC0")
                    {
                      main.ADC0_072.init = false;
                    }
                    if (Name_this == "ADC1")
                    {
                      main.ADC1_072.init = false;
                    }
                    Debug.WriteLine(s1);
                }
            }
        }


        private void checkBox_mac_init_Click(object sender, RoutedEventArgs e)
        {
            string s1 = "";
            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (checkBox_mac_init.IsChecked == true)
                {
                    if (Name_this == "ADC0")
                    {
                        s1 = "~0 mac_init:0;";
                        main.MAC0_072.init = true;
                    }
                    if (Name_this == "ADC1")
                    {
                        s1 = "~0 mac_init:1;";
                        main.MAC1_072.init = true;
                    }
                    Debug.WriteLine(s1);
                }
                else
                {
                    if (Name_this == "ADC0")
                    {
                        main.MAC0_072.init = false;
                    }
                    if (Name_this == "ADC1")
                    {
                        main.MAC1_072.init = false;
                    }
                    Debug.WriteLine(s1);
                }
            }
        }
    }
}
