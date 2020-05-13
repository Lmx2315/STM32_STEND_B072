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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class DDS_form : Window
    {
        public DDS_form(string a)
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
                if (Name_this == "DDS") main.DDS_form[0] = false;
            }

        }

        private void button_init_dds_Click(object sender, RoutedEventArgs e)
        {
            //DAC_info
            string s1 = "";//FREQ
            string s2 = "";//FREQ_STEP
            string s3 = "";//FREQ_RATE
            string s4 = "";//N_impulse
            string s5 = "";//TYPE_impulse
            string s6 = "";//Interval_Ti
            string s7 = "";//Interval_Tp
            string s8 = "";//Tblank1
            string s9 = "";//Tblank2
            string sa = "";//spi4_sync
            UInt64 data = 0;
            double rezult = 0;
            double Fnco = 96_000_000;//тактовая частота DDS
            string[] Ar=new string[10];

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                //-----отсылаем команду кода частоты DDS 
                rezult = (Convert.ToDouble(textBox_freq.Text) * Convert.ToDouble(Math.Pow(2, 48)))*1_000_000/ Fnco;
                data = Convert.ToUInt64(rezult);
                s1 = " ~0 FREQ:" + Convert.ToString(data) + "; ";     //   

                rezult = (Convert.ToDouble(textBox_Freq_ramp.Text) * Convert.ToDouble(Math.Pow(2, 48))) * 1_000/ Fnco;
                data = Convert.ToUInt64(rezult);
                s2 = " ~0 FREQ_STEP:" + Convert.ToString(data) + "; ";//FREQ_STEP

                rezult = (Convert.ToDouble(textBox_Ramp_rate.Text));
                data = Convert.ToUInt64(rezult);
                s3 = " ~0 FREQ_RATE:" + Convert.ToString(data) + "; ";//

                rezult = (Convert.ToDouble(textBox_N_impulse.Text));
                data = Convert.ToUInt64(rezult);
                s4 = " ~0 N_impulse:" + Convert.ToString(data) + "; ";//N_impulse

                rezult = (Convert.ToDouble(textBox_TYPE_impulse.Text));
                data = Convert.ToUInt64(rezult);
                s5 = " ~0 TYPE_impulse:" + Convert.ToString(data) + "; ";//TYPE_impulse

                rezult = (Convert.ToDouble(textBox_Interval_Ti.Text));
                data = Convert.ToUInt64(rezult);
                s6 = " ~0 Interval_Ti:" + Convert.ToString(data) + "; ";//Interval_Ti

                rezult = (Convert.ToDouble(textBox_Interval_Tp.Text));
                data = Convert.ToUInt64(rezult);
                s7 = " ~0 Interval_Tp:" + Convert.ToString(data) + "; ";//Interval_Tp

                rezult = (Convert.ToDouble(textBox_Tblank1.Text));
                data = Convert.ToUInt64(rezult);
                s8 = " ~0 Tblank1:" + Convert.ToString(data) + "; ";//Tblank1

                rezult = (Convert.ToDouble(textBox_Tblank2.Text));
                data = Convert.ToUInt64(rezult);
                s9 = " ~0 Tblank2:" + Convert.ToString(data) + "; ";//Tblank2

                sa = " ~0 spi4_sync" + "; ";//spi4_sync  запускает синхронизацию в ПЛИС

                Ar[0] = s1;
                Ar[1] = s2;
                Ar[2] = s3;
                Ar[3] = s4;
                Ar[4] = s5;
                Ar[5] = s6;
                Ar[6] = s7;
                Ar[7] = s8;
                Ar[8] = s9;
                Ar[9] = sa;

                main.UART_TX(Ar);
               
            }
        }
    }
}
