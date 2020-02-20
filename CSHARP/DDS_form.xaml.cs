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
            string s1 = "";
            byte cmd = 203;
            long data = 0;
            byte[] a = new byte[7];
            long Fnco = 96_000_000;//тактовая частота DDS

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                //для кого шлём команду
                //в первом байте данных команды размещаем адресат получения команды
                if (Name_this == "DDS") a[0] = 1;//первый цап

                //-----отсылаем команду кода частоты DDS внутри DAC--------
                cmd = main.CMD_DDS_freq;//длина команды 48 бит

                data = (Convert.ToInt64(textBox_freq.Text) * Fnco) / Convert.ToInt64(Math.Pow(2, 48));

                a[1] = Convert.ToByte(data >> 40 & 0xff);
                a[2] = Convert.ToByte(data >> 32 & 0xff);
                a[3] = Convert.ToByte(data >> 24 & 0xff);
                a[4] = Convert.ToByte(data >> 16 & 0xff);
                a[5] = Convert.ToByte(data >> 8 & 0xff);
                a[6] = Convert.ToByte(data >> 0 & 0xff);

                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда 
                           a,   //данные
                           7,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );
                //--------------------------------------------------------------
                //-----отсылаем команду кода фазы DDS внутри DAC        --------
                cmd = main.CMD_DDS_phase;//длина команды 16 бит

                data = (Convert.ToInt64(textBox_phase.Text) * Convert.ToInt64(Math.Pow(2, 16))) / 360;

                a[2] = Convert.ToByte(data >> 8 & 0xff);
                a[3] = Convert.ToByte(data >> 0 & 0xff);

                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда 
                           a,   //данные
                           4,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                //-----отсылаем команду кода freq_ramp DDS внутри DAC        --------
                cmd = main.CMD_DDS_freq_ramp;//длина команды 48 бит

                data = Convert.ToInt64(textBox_Freq_ramp.Text);

                a[1] = Convert.ToByte(data >> 40 & 0xff);
                a[2] = Convert.ToByte(data >> 32 & 0xff);
                a[3] = Convert.ToByte(data >> 24 & 0xff);
                a[4] = Convert.ToByte(data >> 16 & 0xff);
                a[5] = Convert.ToByte(data >> 8 & 0xff);
                a[6] = Convert.ToByte(data >> 0 & 0xff);

                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда 
                           a,   //данные
                           7,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                //-----отсылаем команду кода Ramp rate сигнала DDS внутри DAC        --------
                cmd = main.CMD_DDS_ramp_rate;//длина команды 16 бит

                data = Convert.ToInt64(textBox_Ramp_rate.Text);

                a[2] = Convert.ToByte(data >> 8 & 0xff);
                a[3] = Convert.ToByte(data >> 0 & 0xff);

                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда 
                           a,   //данные
                           4,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                /*  не поддерживается при работе по уарту!!!
                if (Name_this == "DAC0") s1 = " ~0 dac1_dds_freq:" + Convert.ToString(a[3]) + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 dac2_dds_freq:" + Convert.ToString(a[3]) + "; ";

                try
                {
                    if (main.serialPort1.IsOpen == false)
                    {
                        main.serialPort1.Open();
                    }
                    Debug.WriteLine("шлём:" + s1);
                    main.serialPort1.Write(s1);

                }
                catch (Exception ex)
                {
                    // что-то пошло не так и упало исключение... Выведем сообщение исключения
                    Console.WriteLine(string.Format("Port:'{0}' Error:'{1}'", main.serialPort1.PortName, ex.Message));
                }
                */
            }
        }
    }
}
