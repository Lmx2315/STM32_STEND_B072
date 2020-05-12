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

            byte cmd = 203;
            UInt64  data = 0;
            UInt64 data_FREQ    = 0;
            UInt64 FREQ_STEP    = 0;
            UInt32 FREQ_RATE    = 0;
            UInt32 N_impulse    = 0;
            UInt32 TYPE_impulse = 0;
            UInt32 Interval_Ti  = 0;
            UInt32 Interval_Tp  = 0;
            UInt32 Tblank1      = 0;
            UInt32 Tblank2      = 0;

            double rezult = 0;
            byte[] a = new byte[7];
            double Fnco = 96_000_000;//тактовая частота DDS

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                //для кого шлём команду
                //в первом байте данных команды размещаем адресат получения команды
                if (Name_this == "DDS") a[0] = 1;//первый цап

                //-----отсылаем команду кода частоты DDS внутри DAC--------
                cmd = main.CMD_DDS_freq;//длина команды 48 бит
                rezult = (Convert.ToDouble(textBox_freq.Text) * Convert.ToDouble(Math.Pow(2, 48)))/ Fnco;
                data = Convert.ToUInt64(rezult);

                Debug.WriteLine("data:" + data);
                /*
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

                data = (Convert.ToUInt32 (textBox_phase.Text) * Convert.ToUInt32(Math.Pow(2, 16))) / 360;

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

                data = Convert.ToUInt64 (textBox_Freq_ramp.Text);

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

                data = Convert.ToUInt64(textBox_Ramp_rate.Text);

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
                */
 //отсылаем частоту DDS    по UART      
                s1 = " ~0 FREQ:" + Convert.ToString(a[3]) + "; "; //   
                s2 = "";//FREQ_STEP
                s3 = "";//FREQ_RATE
                s4 = "";//N_impulse
                s5 = "";//TYPE_impulse
                s6 = "";//Interval_Ti
                s7 = "";//Interval_Tp
                s8 = "";//Tblank1
                s9 = "";//Tblank2
                sa = "";//spi4_sync

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
               
            }
        }
    }
}
