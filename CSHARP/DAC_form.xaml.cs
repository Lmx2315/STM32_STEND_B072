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
    /// Логика взаимодействия для DAC_form.xaml
    /// </summary>
    public partial class DAC_form : Window
    {
        public DAC_form(string a)
        {
            InitializeComponent();
            this.Name_this = a;
            this.Title = a;

            Timer1.Tick += new EventHandler(Timer1_Tick);
            Timer1.Interval = new TimeSpan(0, 0, 0, 0, 2000);
            Timer1.Start();//запускаю таймер 
        }

        string Name_this = "";

        System.Windows.Threading.DispatcherTimer Timer1 = new System.Windows.Threading.DispatcherTimer();

        private void Timer1_Tick(object sender, EventArgs e)
        {//-------------Тут по таймеру шлём запрос состояния блока по UDP---------------------

            if (checkBox_DAC_reset.IsChecked ?? true) checkBox_DAC_reset.IsChecked = false;
           
        }

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

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
           

        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            //DAC powerdown
            string s1 = "";
            byte cmd = 203;
            int data = 0;
            byte [] a = new byte[4];

            Timer1.Start();

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                cmd = main.CMD_DAC_PWRDN;
                if (Name_this == "DAC0") main.DAC0.PWRDN = (bool)checkBox.IsChecked;
                else
                if (Name_this == "DAC1") main.DAC1.PWRDN = (bool)checkBox.IsChecked;

                data = Convert.ToByte((bool)checkBox.IsChecked);

                //для кого шлём команду
                if (Name_this == "DAC0") a[0] = 1;//первый цап
                else
                if (Name_this == "DAC1") a[0] = 2;//второй цап

                a[3] = Convert.ToByte(data);

                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда управление светодиодом "ИСПРАВ"
                           a,   //данные
                           4,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                if (Name_this == "DAC0") s1 = " ~0 DAC1_PWRDN:" + Convert.ToString(a[3]) + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 DAC2_PWRDN:" + Convert.ToString(a[3]) + "; ";

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

        private void checkBox_DAC_reset_Click(object sender, RoutedEventArgs e)
        {
            //DAC reset
            string s1 = "";
            byte cmd = 203;
            int data = 0;
            byte[] a = new byte[4];

            Timer1.Start();

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                cmd = main.CMD_DAC_RST ;
                if (Name_this == "DAC0") main.DAC0.RST = (bool)checkBox_DAC_reset.IsChecked;
                else
                if (Name_this == "DAC1") main.DAC1.RST = (bool)checkBox_DAC_reset.IsChecked;

                data = Convert.ToByte((bool)checkBox_DAC_reset.IsChecked);

                //для кого шлём команду
                if (Name_this == "DAC0") a[0] = 1;//первый цап
                else
                if (Name_this == "DAC1") a[0] = 2;//второй цап

                a[3] = Convert.ToByte(data);
                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда управление светодиодом "ИСПРАВ"
                           a,   //данные
                           4,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                if (Name_this == "DAC0") s1 = " ~0 dac1_reset:" + Convert.ToString(a[3]) + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 dac2_reset:" + Convert.ToString(a[3]) + "; ";

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

        private void checkBox2_Click(object sender, RoutedEventArgs e)
        {
            //DAC init
            string s1 = "";
            byte cmd = 203;
            int data = 0;
            byte[] a = new byte[4];

            Timer1.Start();

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                cmd = main.CMD_DAC_init ;
                if (Name_this == "DAC0") main.DAC0.INIT = (bool)checkBox2.IsChecked;
                else
                if (Name_this == "DAC1") main.DAC1.INIT = (bool)checkBox2.IsChecked;

                data = Convert.ToByte((bool)checkBox2.IsChecked);

                //для кого шлём команду
                if (Name_this == "DAC0") a[0] = 1;//первый цап
                else
                if (Name_this == "DAC1") a[0] = 2;//второй цап

                a[3] = Convert.ToByte(data);
                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда управление светодиодом "ИСПРАВ"
                           a,   //данные
                           4,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                if (Name_this == "DAC0") s1 = " ~0 dac1_init:" + Convert.ToString(a[3]) + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 dac2_init:" + Convert.ToString(a[3]) + "; ";

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

        private void checkBox_DAC_DSP_init_Click(object sender, RoutedEventArgs e)
        {
            //DAC_DDS_init
            string s1 = "";
            byte cmd = 203;
            int data = 0;
            byte[] a = new byte[4];

            Timer1.Start();

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                cmd = main.CMD_DAC_DDS_init;
                if (Name_this == "DAC0") main.DAC0.DDS_init = (bool)checkBox_DAC_DSP_init.IsChecked;
                else
                if (Name_this == "DAC1") main.DAC1.DDS_init = (bool)checkBox_DAC_DSP_init.IsChecked;

                data = Convert.ToByte((bool)checkBox_DAC_DSP_init.IsChecked);

                //для кого шлём команду
                if (Name_this == "DAC0") a[0] = 1;//первый цап
                else
                if (Name_this == "DAC1") a[0] = 2;//второй цап

                a[3] = Convert.ToByte(data);
                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда управление светодиодом "ИСПРАВ"
                           a,   //данные
                           4,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                if (Name_this == "DAC0") s1 = " ~0 dac1_dsp_init:" + Convert.ToString(a[3]) + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 dac2_dsp_init:" + Convert.ToString(a[3]) + "; ";

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

        private void button_jesd_info_Click(object sender, RoutedEventArgs e)
        {
            //DAC_jesd_info
            string s1 = "";
            byte cmd = 203;
            int data = 0;
            byte[] a = new byte[4];

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                cmd = main.CMD_DAC_JESD_info;
                data = 1;

                //для кого шлём команду
                if (Name_this == "DAC0") a[0] = 1;//первый цап
                else
                if (Name_this == "DAC1") a[0] = 2;//второй цап

                a[3] = Convert.ToByte(data);
                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда управление светодиодом "ИСПРАВ"
                           a,   //данные
                           4,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                if (Name_this == "DAC0") s1 = " ~0 jesd_dac1_info:" + Convert.ToString(a[3]) + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 jesd_dac1_info:" + Convert.ToString(a[3]) + "; ";

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

        private void button_dac_info_Click(object sender, RoutedEventArgs e)
        {
            //DAC_info
            string s1 = "";
            byte cmd = 203;
            int data = 0;
            byte[] a = new byte[4];

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                cmd = main.CMD_DAC_info ;
                data = 1;

                //для кого шлём команду
                if (Name_this == "DAC0") a[0] = 1;//первый цап
                else
                if (Name_this == "DAC1") a[0] = 2;//второй цап

                a[3] = Convert.ToByte(data);
                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда управление светодиодом "ИСПРАВ"
                           a,   //данные
                           4,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                if (Name_this == "DAC0") s1 = " ~0 dac1_info:" + Convert.ToString(a[3]) + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 dac2_info:" + Convert.ToString(a[3]) + "; ";

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

        private void button_dac_phy__info_Click(object sender, RoutedEventArgs e)
        {
            //DAC_info
            string s1 = "";
            byte cmd = 203;
            int data = 0;
            byte[] a = new byte[4];

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                cmd = main.CMD_DAC_PHY_info;
                data = 1;

                //для кого шлём команду
                if (Name_this == "DAC0") a[0] = 1;//первый цап
                else
                if (Name_this == "DAC1") a[0] = 2;//второй цап

                a[3] = Convert.ToByte(data);
                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда управление светодиодом "ИСПРАВ"
                           a,   //данные
                           4,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                if (Name_this == "DAC0") s1 = " ~0 dac1_phy_info:" + Convert.ToString(a[3]) + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 dac2_phy_info:" + Convert.ToString(a[3]) + "; ";

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

        private void button_init_dac_Click(object sender, RoutedEventArgs e)
        {
            //DAC_info
            string s1 = "";
            byte cmd = 203;
            double result = 0;
            UInt64 data = 0;
            byte[] a = new byte[7];
            double Fnco = 1536_000_000;//тактовая частота DAC

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                //для кого шлём команду
                //в первом байте данных команды размещаем адресат получения команды
                if (Name_this == "DAC0") a[0] = 1;//первый цап
                else
                if (Name_this == "DAC1") a[0] = 2;//второй цап

                //-----------------DAC_coarse_dac----------------------------------------
                cmd = main.CMD_DAC_coarse_dac;//длина команды 16 бит

                data = Convert.ToUInt16 (textBox_Coarse_dac.Text);

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
                if (Name_this == "DAC0") s1 = " ~0 DAC1_coarse_dac:" + data.ToString() + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 DAC2_coarse_dac:" + data.ToString() + "; ";

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
                //----------------------DAC_QMC_gain-----------------------------------
                cmd = main.CMD_DAC_QMC_gain;//длина команды 16 бит

                data = Convert.ToUInt16(textBox_QMC_gain.Text);

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
                if (Name_this == "DAC0") s1 = " ~0 DAC1_QMC_gain:" + data.ToString() + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 DAC2_QMC_gain:" + data.ToString() + "; ";

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


                //-----отсылаем команду кода частоты DDS внутри DAC--------
                cmd = main.CMD_DAC_DDS_freq;//длина команды 48 бит

                result = (Convert.ToDouble(textBox_freq.Text) *  Convert.ToDouble(Math.Pow(2, 48)))/ Fnco;
                data   =  Convert.ToUInt64(result);

                a[1] = Convert.ToByte(data >> 40 & 0xff);
                a[2] = Convert.ToByte(data >> 32 & 0xff);
                a[3] = Convert.ToByte(data >> 24 & 0xff);
                a[4] = Convert.ToByte(data >> 16 & 0xff);
                a[5] = Convert.ToByte(data >>  8 & 0xff);
                a[6] = Convert.ToByte(data >>  0 & 0xff);

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
                cmd = main.CMD_DAC_DDS_phase;//длина команды 16 бит

                data = (Convert.ToUInt64(textBox_phase.Text) * Convert.ToUInt64(Math.Pow(2, 16)))/360;

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

                //-----отсылаем команду кода амплитуды DDS внутри DAC        --------
                cmd = main.CMD_DAC_DDS_amp;//длина команды 16 бит

                data = Convert.ToUInt64(textBox_phase.Text);

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

                //-----отсылаем команду кода задержки сигнала DDS внутри DAC        --------
                cmd = main.CMD_DAC_delay;//длина команды 16 бит

                data = Convert.ToUInt64(textBox_phase.Text);

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

        private void checkBox_DAC_mixer_gain_Click(object sender, RoutedEventArgs e)
        {
            //DAC_mixer_gain
            string s1 = "";
            byte cmd = 203;
            int data = 0;
            byte[] a = new byte[4];

            Timer1.Start();

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                cmd = main.CMD_DAC_mixer_gain;//команда

                if (Name_this == "DAC0") main.DAC0.DAC_mixer_gain = (bool)checkBox_DAC_mixer_gain.IsChecked;
                else
                if (Name_this == "DAC1") main.DAC1.DAC_mixer_gain = (bool)checkBox_DAC_mixer_gain.IsChecked;

                data = Convert.ToByte((bool)checkBox_DAC_mixer_gain.IsChecked);

                //для кого шлём команду
                if (Name_this == "DAC0") a[0] = 1;//первый цап
                else
                if (Name_this == "DAC1") a[0] = 2;//второй цап

                a[3] = Convert.ToByte(data);
                main.FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
                main.UDP_SEND
                           (
                           cmd, //команда управление светодиодом "ИСПРАВ"
                           a,   //данные
                           4,   //число данных в байтах
                           0    //время исполнения , 0 - значит немедленно как сможешь.
                           );

                if (Name_this == "DAC0") s1 = " ~0 DAC1_mixer_gain:" + Convert.ToString(a[3]) + "; "; //команда заставляет мк установить ресет и снять его!!!
                else
                if (Name_this == "DAC1") s1 = " ~0 DAC2_mixer_gain:" + Convert.ToString(a[3]) + "; ";

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
