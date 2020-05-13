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
using System.IO.Ports;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace stnd_72_v2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    
    public struct ADC
    {
        public string   Name;
        public bool     PWRDN;
        public bool     init;
        public uint     test;
    }

    public struct DAC
    {
        public string Name;
        public bool PWRDN;
        public bool init;
        public bool RST;
        public int faza;
    }

    public struct MAC
    {
        public string Name;
        public bool PWRDN;
        public bool init;
        public bool RST;
    }

    public partial class MainWindow : Window
    {
        public byte ISPRAV_GREEN;   //переменная состояния светодиода "ИСПРАВ"      зелёный с лицевой панели
        public byte ISPRAV_RED;     //переменная состояния светодиода "ИСПРАВ"      красный с лицевой панели
        public byte LIN_SVYAZ_GREEN;//переменная состояния светодиода "ЛИН СВЯЗИ"   зелёный с лицевой панели
        public byte LIN_SVYAZ_RED;  //переменная состояния светодиода "ЛИН СВЯЗИ"   красный с лицевой панели

        public byte PRM1_K1;
        public byte PRM2_k1;
        public byte PRD1_k1;
        public byte PRD2_k1;

        public byte PRM1_K2;
        public byte PRM2_k2;
        public byte PRD1_k2;
        public byte PRD2_k2;



        public class DAC
        {
            public bool PWRDN;
            public bool RST;
            public bool INIT;
            public bool DDS_init;
            public bool DAC_mixer_gain;
            public UInt16 DAC_coarse_dac;
            public UInt16 DAC_QMC_gain;

            public string Name;
            public DAC(string n)
            {
                Name = n;
            }
        }

        public DAC DAC0 = new DAC("DAC0");
        public DAC DAC1 = new DAC("DAC1");

        public ADC ADC0_072;
        public ADC ADC1_072;
        public DAC DAC0_072;
        public DAC DAC1_072;
        public MAC MAC0_072;//переменная состояния блока МАС ethernet в 072
        public MAC MAC1_072;//переменная состояния блока МАС ethernet в 072

        public byte SWITCH_072 = 0xff;//переменная состояния переключателей 072
        public uint DDS_072;        //переменная состояния кода частоты модуля DDS в блоке тест на DAC
        public uint ATT_072;        //переменная состояния аттенюаторов 072

        int MSG_CMD_OK = 3;//квитанция о том что команда выполненна
        int MSG_ID_CH1 = 101;
        int MSG_ID_CH2 = 102;
        int MSG_ID_CH3 = 103;
        int MSG_ID_CH4 = 104;
        int MSG_ID_CH5 = 105;
        int MSG_ID_CH6 = 106;
        int MSG_ID_CH7 = 107;
        int MSG_ID_CH8 = 108;

        public byte CMD_DAC_PWRDN = 50;//команда вкл/выкл DAC 
        public byte CMD_DAC_RST = 51;//команда reset DAC
        public byte CMD_DAC_init = 52;
        public byte CMD_DAC_DDS_init = 53;
        public byte CMD_DAC_JESD_info = 54;
        public byte CMD_DAC_info = 55;
        public byte CMD_DAC_PHY_info = 56;
        public byte CMD_DAC_DDS_freq = 57;
        public byte CMD_DAC_DDS_phase = 58;
        public byte CMD_DAC_DDS_amp = 59;
        public byte CMD_DAC_delay = 60;
        public byte CMD_DAC_mixer_gain = 61;
        public byte CMD_DAC_coarse_dac = 62;
        public byte CMD_DAC_QMC_gain = 63;
        public byte CMD_LMK_init = 64;
        public byte CMD_DDS_freq = 65;//команда установки частоты DDS в ПЛИС
        public byte CMD_DDS_phase = 66;
        public byte CMD_DDS_freq_ramp = 67;
        public byte CMD_DDS_ramp_rate = 68;
        public byte CMD_CHANNEL = 69;//команда переключения каналов в 072 (между ЦАП-АЦП и ЦАП-Выход)


        public byte FLAG_ETH_request = 0;//флаг отосланного запроса на кассету, проверяется на предмет ответа в таймере

        const uint MSG_TEMP_CH1 = 111;
        const uint MSG_TEMP_CH2 = 112;
        const uint MSG_TEMP_CH3 = 113;
        const uint MSG_TEMP_CH4 = 114;
        const uint MSG_TEMP_CH5 = 115;
        const uint MSG_TEMP_CH6 = 116;
        const uint MSG_TEMP_CH7 = 117;
        const uint MSG_TEMP_CH8 = 118;

        const uint MSG_I_CH1 = 131;
        const uint MSG_I_CH2 = 132;
        const uint MSG_I_CH3 = 133;
        const uint MSG_I_CH4 = 134;
        const uint MSG_I_CH5 = 135;
        const uint MSG_I_CH6 = 136;
        const uint MSG_I_CH7 = 137;
        const uint MSG_I_CH8 = 138;

        const uint MSG_P_CH1 = 141;
        const uint MSG_P_CH2 = 142;
        const uint MSG_P_CH3 = 143;
        const uint MSG_P_CH4 = 144;
        const uint MSG_P_CH5 = 145;
        const uint MSG_P_CH6 = 146;
        const uint MSG_P_CH7 = 147;
        const uint MSG_P_CH8 = 148;

        const uint MSG_U_CH1 = 121;
        const uint MSG_U_CH2 = 122;
        const uint MSG_U_CH3 = 123;
        const uint MSG_U_CH4 = 124;
        const uint MSG_U_CH5 = 125;
        const uint MSG_U_CH6 = 126;
        const uint MSG_U_CH7 = 127;
        const uint MSG_U_CH8 = 128;

        const uint MSG_PWR_CHANNEL = 150;


        public SerialPort serialPort1 = new SerialPort();
        static bool _continue;
        Byte[] RCV = new byte[64000];
        int sch_packet = 0;

        //----------ETH------------
        UdpClient _server = null;
        IPEndPoint _client = null;
        Thread _listenThread = null;
        private bool _isServerStarted = false;

        //-------------------eth-------------------------
        private void Start()
        {
            IPAddress my_ip;
            UInt16 my_port;

            my_ip = IPAddress.Parse(textBox_my_ip.Text);
            my_port = UInt16.Parse(textBox_my_port.Text);

            //Create the server.
            IPEndPoint serverEnd = new IPEndPoint(my_ip, my_port);
            try
            {
                _server = new UdpClient(serverEnd);
                _server.Client.ReceiveBufferSize = 8192 * 200;//увеличиваем размер приёмного буфера!!!
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show("Нет абонента!");
            }

            //Start listening.

            Thread listenThread = new Thread(new ThreadStart(Listening));
            listenThread.Start();

            //Change state to indicate the server starts.
            _isServerStarted = true;

            Debug.WriteLine("Waiting for a client...");
        }

        int FLAG_UDP_RCV = 0;
        int RCV_size = 0;
        string COMMAND_FOR_SERVER = "HOMEWORLD!!!\n";

        private void Listening()
        {
            //Listening loop.
            try
            {
                while (true)
                {
                    //receieve a message form a client.
                    byte[] data = _server.Receive(ref _client);
                    Debug.WriteLine("UDP rcv");
                    if (FLAG_UDP_RCV == 0)
                    {
                        //  receivedMsg = Encoding.ASCII.GetString(data, 0, data.Length);
                        Array.Copy(data, RCV, data.Length);//копируем массив отсчётов в форму обработки                    
                                                           //      FLAG_UDP_RCV = 1;
                        RCV_size = data.Length;
                        UDP_BUF_DESCRIPT();
                    }
                    sch_packet++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void Stop()
        {
            try
            {
                //Stop listening.
                _listenThread.Join();
                Debug.WriteLine("Server stops.");
                _server.Close();
                //Changet state to indicate the server stops.
                _isServerStarted = false;
            }
            catch (Exception excp)
            { }
        }


        System.Windows.Threading.DispatcherTimer Timer2 = new System.Windows.Threading.DispatcherTimer();

        int FLAG_TIMER_1 = 0;
        int FLAG_SYS_INIT;

        private void Timer2_Tick(object sender, EventArgs e)
        {//-------------Тут по таймеру шлём запрос состояния блока по UDP---------------------

            if (FLAG_TIMER_1 > 2)
            {
                button_udp_init_072.Background = new SolidColorBrush(Colors.Red);
                FLAG_SYS_INIT = 0;//нет связи с блоком 
            }
            else
            {
                if (button_udp_init_072.Background != Brushes.Green) button_udp_init_072.Background = Brushes.Green;
                FLAG_SYS_INIT = 1;//есть связь с блоком , можно запрашивать состояние
            }
            FLAG_TIMER_1++;

            byte[] a = new byte[1];

            a[0] = Convert.ToByte(100);
            UDP_SEND(
                100,//команда 100 - CMD_STATUS
                a,  //данные
                1,  //число данных в байтах
                0   //время исполнения 0 - значит сразу как сможешь
                );
        }

        Frame MSG1 = new Frame();
        public uint FLAG_NEW_DATA = 0; //флаг что пришли новые данные о состоянии блока

        void UDP_BUF_DESCRIPT()
        {
            int i = 0;
            int j = 0;
            int offset = 0;
            int tmp = 0;
            byte[] a = new byte[4];

            FLAG_NEW_DATA = 1;

            MSG1.MSG.CMD.A = new byte[4];
            //    Debug.WriteLine("------------------");

            MSG1.Frame_size = Convert.ToUInt16((RCV[0 + tmp] << 8) + RCV[1 + tmp]);
            MSG1.Frame_number = Convert.ToUInt16((RCV[2 + tmp] << 8) + RCV[3 + tmp]);
            MSG1.Stop_bit = Convert.ToUInt16(1);
            MSG1.Msg_uniq_id = Convert.ToUInt32((RCV[4 + tmp] << 24) + (RCV[5 + tmp] << 16) + (RCV[6 + tmp] << 8) + (RCV[7 + tmp] << 0));
            MSG1.Sender_id = Convert.ToUInt64((RCV[8 + tmp] << 56) + (RCV[9 + tmp] << 48) + (RCV[10 + tmp] << 40) + (RCV[11 + tmp] << 32) + (RCV[12 + tmp] << 24) + (RCV[13 + tmp] << 16) + (RCV[14 + tmp] << 8) + (RCV[15 + tmp] << 0));
            MSG1.Receiver_id = Convert.ToUInt64((RCV[16] << 56) + (RCV[17] << 48) + (RCV[18] << 40) + (RCV[19] << 32) + (RCV[20] << 24) + (RCV[21] << 16) + (RCV[22] << 8) + (RCV[23] << 0));
            MSG1.MSG.Msg_size = Convert.ToUInt32((RCV[24] << 24) + (RCV[25] << 16) + (RCV[26] << 8) + (RCV[27] << 0));
            MSG1.MSG.Msg_type = Convert.ToUInt32((RCV[28] << 24) + (RCV[29] << 16) + (RCV[30] << 8) + (RCV[31] << 0));
            MSG1.MSG.Num_cmd_in_msg = Convert.ToUInt64((RCV[32] << 56) + (RCV[33] << 48) + (RCV[34] << 40) + (RCV[35] << 32) + (RCV[36] << 24) + (RCV[37] << 16) + (RCV[38] << 8) + (RCV[39] << 0));


            Debug.WriteLine("    Frame_size:" + MSG1.Frame_size);
            Debug.WriteLine("  Frame_number:" + MSG1.Frame_number);
            Debug.WriteLine("   Msg_uniq_id:" + MSG1.Msg_uniq_id);
            Debug.WriteLine("     Sender_id:" + MSG1.Sender_id);
            Debug.WriteLine("   Receiver_id:" + MSG1.Receiver_id);
            Debug.WriteLine("      Msg_size:" + MSG1.MSG.Msg_size);
            Debug.WriteLine("      Msg_type:" + MSG1.MSG.Msg_type);
            Debug.WriteLine("Num_cmd_in_msg:" + MSG1.MSG.Num_cmd_in_msg);

            offset = 40;

            for (i = 0; i < Convert.ToInt32(MSG1.MSG.Num_cmd_in_msg); i++)
            {
                MSG1.MSG.CMD.Cmd_size = Convert.ToUInt32((RCV[offset + 0] << 24) + (RCV[offset + 1] << 16) + (RCV[offset + 2] << 8) + (RCV[offset + 3] << 0));
                MSG1.MSG.CMD.Cmd_type = Convert.ToUInt32((RCV[offset + 4] << 24) + (RCV[offset + 5] << 16) + (RCV[offset + 6] << 8) + (RCV[offset + 7] << 0));
                MSG1.MSG.CMD.Cmd_id = Convert.ToUInt64((RCV[offset + 8] << 56) + (RCV[offset + 9] << 48) + (RCV[offset + 10] << 40) + (RCV[offset + 11] << 32) + (RCV[offset + 12] << 24) + (RCV[offset + 13] << 16) + (RCV[offset + 14] << 8) + (RCV[offset + 15] << 0));
                MSG1.MSG.CMD.Cmd_time = Convert.ToUInt64((RCV[offset + 16] << 56) + (RCV[offset + 17] << 48) + (RCV[offset + 18] << 40) + (RCV[offset + 19] << 32) + (RCV[offset + 20] << 24) + (RCV[offset + 21] << 16) + (RCV[offset + 22] << 8) + (RCV[offset + 23] << 0));

                if (MSG1.MSG.CMD.Cmd_type == MSG_CMD_OK)
                {
                    FLAG_TIMER_1 = 0;
                }

                Debug.WriteLine("Cmd_size:" + MSG1.MSG.CMD.Cmd_size);
                Debug.WriteLine("Cmd_type:" + MSG1.MSG.CMD.Cmd_type);
                Debug.WriteLine("Cmd_id  :" + MSG1.MSG.CMD.Cmd_id);
                Debug.WriteLine("Cmd_time:" + MSG1.MSG.CMD.Cmd_time);
                //---------------------------------------------------------------------------------

                for (j = 0; j < Convert.ToInt32(MSG1.MSG.CMD.Cmd_size); j++)
                {
                    MSG1.MSG.CMD.Cmd_data = Convert.ToString(RCV[offset + 24 + j]);
                    MSG1.MSG.CMD.A[j] = RCV[offset + 24 + j];
                    a[3 - j] = RCV[offset + 24 + j];
                    //if (MSG1.MSG.CMD.Cmd_type == MSG_TEMP_CH1)  Debug.WriteLine("A[j]:" + MSG1.MSG.CMD.A[j]);
                }

                switch (MSG1.MSG.CMD.Cmd_type)
                {
                    case 0: break;
                }

                // TEMP_channel(MSG1.MSG.CMD.A);

                offset = offset + 24 + j;
            }
        }


        public MainWindow()
        {
            InitializeComponent();

            Timer2.Tick += new EventHandler(Timer2_Tick);
            Timer2.Interval = new TimeSpan(0, 0, 0, 0, 250);
            //  Timer2.Start();//запускаю таймер проверяющий приём по UDP          
        }

        private void button_comport_send_Click(object sender, RoutedEventArgs e)
        {
            string command1 = " ~0 freq:";
            SolidColorBrush myBrush = new SolidColorBrush(Colors.Red);

            try
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();
                }
                Debug.WriteLine("шлём:" + command1);
                button_comport_send.Background = Brushes.Green;
                serialPort1.Write(textBox_comport_message.Text);
                //  serialPort1.Write(command2);
                // здесь может быть код еще...
            }
            catch (Exception ex)
            {

                // что-то пошло не так и упало исключение... Выведем сообщение исключения
                Console.WriteLine(string.Format("Port:'{0}' Error:'{1}'", serialPort1.PortName, ex.Message));
                button_comport_send.Background = myBrush;
            }

        }


        private void button_comport_open_Click(object sender, RoutedEventArgs e)
        {

            SolidColorBrush myBrush = new SolidColorBrush(Colors.Red);

            if (serialPort1.IsOpen == false)
            {
                // Allow the user to set the appropriate properties.
                serialPort1.PortName = textBox_comport.Text;
                serialPort1.BaudRate = 256000;
                serialPort1.DataReceived += OnDataReceived;

                // Set the read/write timeouts
                serialPort1.ReadTimeout = 500;
                serialPort1.WriteTimeout = 500;

                try
                {
                    if (serialPort1.IsOpen == false)
                    {
                        serialPort1.Open();
                    }
                    Debug.WriteLine("open");
                    button_comport_open.Content = "close";
                    button_comport_send.Background = Brushes.Green;
                    // здесь может быть код еще...
                }
                catch (Exception ex)
                {
                    // что-то пошло не так и упало исключение... Выведем сообщение исключения
                    Console.WriteLine(string.Format("Port:'{0}' Error:'{1}'", serialPort1.PortName, ex.Message));
                    button_comport_open.Background = myBrush;
                }
            } else
            {
                serialPort1.Close();
                button_comport_open.Content = "open";
                button_comport_send.Background = Brushes.Black;
            }

        }

        public string console_text = "";
        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var serialDevice = sender as SerialPort;
            var buffer = new byte[serialDevice.BytesToRead];
            serialDevice.Read(buffer, 0, buffer.Length);
            int i = 0;

            string z = Encoding.GetEncoding(1251).GetString(buffer);//чтобы видеть русский шрифт!!!

            // for (i = 0; i < buffer.Length; i++) z = z + Convert.ToString(buffer[i]);

            // process data on the GUI thread
            Application.Current.Dispatcher.Invoke(
                new Action(() =>
                {
                    //   Debug.WriteLine("чё-то принято!");
                    console_text = console_text + z;

                    Debug.WriteLine(":" + z);
                    /*
                ... do something here ...
                */
                }));
        }
        string config = "";
        private void button_Click(object sender, RoutedEventArgs e)
        {
            string filename;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                filename = openFileDialog.FileName;
                config = File.ReadAllText(filename);
            }

        }

        struct Command
        {
            public UInt32 Cmd_size;
            public UInt32 Cmd_type;
            public UInt64 Cmd_id;
            public UInt64 Cmd_time;
            public string Cmd_data;
            public byte[] A;
        }

        struct Message
        {
            public UInt32 Msg_size;
            public UInt32 Msg_type;
            public UInt64 Num_cmd_in_msg;
            public Command CMD;
        }

        struct Frame
        {
            public UInt16 Frame_size;
            public UInt16 Frame_number;
            public UInt16 Stop_bit;
            public UInt32 Msg_uniq_id;
            public UInt64 Sender_id;
            public UInt64 Receiver_id;
            public Message MSG;
        }

        Frame FRAME;

        private void button_udp_init_072_Click(object sender, RoutedEventArgs e)
        {
            byte[] UDP_packet = new byte[1440];
            int DATA_lenght = 0;
            int i = 0;
            string MSG = "";
            UInt64 sch_cmd = 0;
            MSG = config;

            Start();//запускаю сервер UDP
                    //    Timer2.Start();//запускаю таймер проверяющий приём по UDP  

            try
            {
                FRAME.Frame_size = Convert.ToUInt16(DATA(MSG, "Frame_size"));
                FRAME.Frame_number = Convert.ToUInt16(DATA(MSG, "Frame_number"));
                FRAME.Stop_bit = Convert.ToUInt16(DATA(MSG, "Stop_bit"));
                FRAME.Msg_uniq_id = Convert.ToUInt32(DATA(MSG, "Msg_uniq_id"));
                FRAME.Sender_id = DATA(MSG, "Sender_id");
                FRAME.Receiver_id = DATA(MSG, "Receiver_id");
                FRAME.MSG.Msg_size = Convert.ToUInt32(DATA(MSG, "Msg_size"));
                FRAME.MSG.Msg_type = Convert.ToUInt32(DATA(MSG, "Msg_type"));
                FRAME.MSG.Num_cmd_in_msg = DATA(MSG, "Num_cmd_in_msg");
                sch_cmd = FRAME.MSG.Num_cmd_in_msg;//считаем число команд в файле

                //-------------------фреймовая часть пакета-----------------------
                UDP_packet[0] = Convert.ToByte((FRAME.Frame_size >> 8) & 0xff);
                UDP_packet[1] = Convert.ToByte((FRAME.Frame_size >> 0) & 0xff);

                UDP_packet[2] = Convert.ToByte((FRAME.Frame_number >> 8) & 0xff);
                UDP_packet[3] = Convert.ToByte((FRAME.Frame_number >> 0) & 0xff);//

                UDP_packet[4] = Convert.ToByte((FRAME.Msg_uniq_id >> 24) & 0xff);
                UDP_packet[5] = Convert.ToByte((FRAME.Msg_uniq_id >> 16) & 0xff);
                UDP_packet[6] = Convert.ToByte((FRAME.Msg_uniq_id >> 8) & 0xff);
                UDP_packet[7] = Convert.ToByte((FRAME.Msg_uniq_id >> 0) & 0xff);

                UDP_packet[8] = Convert.ToByte((FRAME.Sender_id >> 56) & 0xff);
                UDP_packet[9] = Convert.ToByte((FRAME.Sender_id >> 48) & 0xff);
                UDP_packet[10] = Convert.ToByte((FRAME.Sender_id >> 40) & 0xff);
                UDP_packet[11] = Convert.ToByte((FRAME.Sender_id >> 32) & 0xff);
                UDP_packet[12] = Convert.ToByte((FRAME.Sender_id >> 24) & 0xff);
                UDP_packet[13] = Convert.ToByte((FRAME.Sender_id >> 16) & 0xff);
                UDP_packet[14] = Convert.ToByte((FRAME.Sender_id >> 8) & 0xff);
                UDP_packet[15] = Convert.ToByte((FRAME.Sender_id >> 0) & 0xff);

                UDP_packet[16] = Convert.ToByte((FRAME.Receiver_id >> 56) & 0xff);
                UDP_packet[17] = Convert.ToByte((FRAME.Receiver_id >> 48) & 0xff);
                UDP_packet[18] = Convert.ToByte((FRAME.Receiver_id >> 40) & 0xff);
                UDP_packet[19] = Convert.ToByte((FRAME.Receiver_id >> 32) & 0xff);
                UDP_packet[20] = Convert.ToByte((FRAME.Receiver_id >> 24) & 0xff);
                UDP_packet[21] = Convert.ToByte((FRAME.Receiver_id >> 16) & 0xff);
                UDP_packet[22] = Convert.ToByte((FRAME.Receiver_id >> 8) & 0xff);
                UDP_packet[23] = Convert.ToByte((FRAME.Receiver_id >> 0) & 0xff);

                UDP_packet[24] = Convert.ToByte((FRAME.MSG.Msg_size >> 24) & 0xff);
                UDP_packet[25] = Convert.ToByte((FRAME.MSG.Msg_size >> 16) & 0xff);
                UDP_packet[26] = Convert.ToByte((FRAME.MSG.Msg_size >> 8) & 0xff);
                UDP_packet[27] = Convert.ToByte((FRAME.MSG.Msg_size >> 0) & 0xff);

                UDP_packet[28] = Convert.ToByte((FRAME.MSG.Msg_type >> 24) & 0xff);
                UDP_packet[29] = Convert.ToByte((FRAME.MSG.Msg_type >> 16) & 0xff);
                UDP_packet[30] = Convert.ToByte((FRAME.MSG.Msg_type >> 8) & 0xff);
                UDP_packet[31] = Convert.ToByte((FRAME.MSG.Msg_type >> 0) & 0xff);

                UDP_packet[32] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 56) & 0xff);
                UDP_packet[33] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 48) & 0xff);
                UDP_packet[34] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 40) & 0xff);
                UDP_packet[35] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 32) & 0xff);
                UDP_packet[36] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 24) & 0xff);
                UDP_packet[37] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 16) & 0xff);
                UDP_packet[38] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 8) & 0xff);
                UDP_packet[39] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 0) & 0xff);
                //-----------------------------------------------------------------------------------------
                int j = 0;
                int pos1 = 0;
                DATA_lenght = 40;//это число байт из упаковки выше 39+1
                while (sch_cmd > 0)
                {
                    FRAME.MSG.CMD.Cmd_size = Convert.ToUInt32(DATA(MSG, "Cmd_size"));
                    FRAME.MSG.CMD.Cmd_type = Convert.ToUInt32(DATA(MSG, "Cmd_type"));
                    FRAME.MSG.CMD.Cmd_id = Convert.ToUInt64(DATA(MSG, "Cmd_id"));
                    FRAME.MSG.CMD.Cmd_time = Convert.ToUInt64(DATA(MSG, "Cmd_time"));

                    var value = data_finder(MSG, "Cmd_time", 0);//координаты разделителя для слова указанного в поиске 
                    FRAME.MSG.CMD.Cmd_data = MSG.Substring(value.Item2 + 3, Convert.ToInt32(FRAME.MSG.CMD.Cmd_size));
                    //               Debug.WriteLine("Cmd_data:" + FRAME.MSG.CMD.Cmd_data);
                    pos1 = Convert.ToInt32(value.Item2 + 3 + FRAME.MSG.CMD.Cmd_size);

                    UDP_packet[40 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_size >> 24) & 0xff);
                    UDP_packet[41 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_size >> 16) & 0xff);
                    UDP_packet[42 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_size >> 8) & 0xff);
                    UDP_packet[43 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_size >> 0) & 0xff);

                    UDP_packet[44 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_type >> 24) & 0xff);
                    UDP_packet[45 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_type >> 16) & 0xff);
                    UDP_packet[46 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_type >> 8) & 0xff);
                    UDP_packet[47 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_type >> 0) & 0xff);

                    UDP_packet[48 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 56) & 0xff);
                    UDP_packet[49 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 48) & 0xff);
                    UDP_packet[50 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 40) & 0xff);
                    UDP_packet[51 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 32) & 0xff);
                    UDP_packet[52 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 24) & 0xff);
                    UDP_packet[53 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 16) & 0xff);
                    UDP_packet[54 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 8) & 0xff);
                    UDP_packet[55 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 0) & 0xff);

                    UDP_packet[56 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_time >> 56) & 0xff);
                    UDP_packet[57 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_time >> 48) & 0xff);
                    UDP_packet[58 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_time >> 40) & 0xff);
                    UDP_packet[59 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_time >> 32) & 0xff);
                    UDP_packet[60 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_time >> 24) & 0xff);
                    UDP_packet[61 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_time >> 16) & 0xff);
                    UDP_packet[62 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_time >> 8) & 0xff);
                    UDP_packet[63 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_time >> 0) & 0xff);

                    for (i = 0; i < FRAME.MSG.CMD.Cmd_data.Length; i++) UDP_packet[64 + i + j] = Convert.ToByte(Convert.ToInt32(FRAME.MSG.CMD.Cmd_data[i]) - 0x30);

                    DATA_lenght = DATA_lenght + FRAME.MSG.CMD.Cmd_data.Length + 24;
                    //  Debug.WriteLine("DATA_lenght:" + DATA_lenght);
                    MSG = MSG.Remove(0, pos1);//удаляем ранее обработанные данные из строки

                    // Debug.WriteLine("MSG:\r\n" + MSG);
                    //                Debug.WriteLine("sch_cmd:" + sch_cmd);
                    sch_cmd--;
                    j = j + FRAME.MSG.CMD.Cmd_data.Length + 24;
                }

                //-----шлём данные по UDP--------------
                string ip_dest = textBox_dest_ip.Text;
                int port_dest = Convert.ToInt32(textBox_dest_port.Text);

                UdpClient client = new UdpClient();
                client.Connect(ip_dest, port_dest);
                int number_bytes = client.Send(UDP_packet, DATA_lenght);
                //           Debug.WriteLine("DATA_lenght                  :" + DATA_lenght);
                //           Debug.WriteLine("FRAME.MSG.CMD.Cmd_data.Length:" + FRAME.MSG.CMD.Cmd_data.Length);
                //           Debug.WriteLine("FRAME.MSG.CMD.Cmd_data       :" + FRAME.MSG.CMD.Cmd_data);
                client.Close();
            }
            catch
            {

            }
        }

        //---------------поиск строк в стринге-----------------
        public UInt64 DATA(string a, string b)
        {
            var value = data_finder(a, b, 0);
            //      Debug.WriteLine(b + " " + value.Item1);
            return value.Item1;
        }

        public Tuple<UInt64, int> data_finder(string MSG, string CMD, int pos)
        {
            int i = 0;
            int a1, a2;
            UInt64 data = 0;
            int index;
            i = pos;
            while ((MSG.Substring(i, CMD.Length) != CMD) && (i < (MSG.Length - 1)))   //ищем команду
            {
                i = i + 1;
            }

            i = i + CMD.Length;

            while (((MSG.Substring(i, 1) == " ") || (MSG.Substring(i, 1) == "\t")) && (i < (MSG.Length - 1)))   //ищем ДАННЫЕ
            {
                i = i + 1;
            }
            a1 = i;
            while ((MSG.Substring(i, 1) != ";") && (i < (MSG.Length - 1)))  //ищем первый разделиетель
            {
                i = i + 1;
            }
            a2 = i - a1;
            //    Debug.WriteLine("a1:" + a1);
            //    Debug.WriteLine("a2:" + a2);
            index = i;
            //    Debug.WriteLine(">" + MSG.Substring(a1, a2));

            try
            {
                data = Convert.ToUInt64(MSG.Substring(a1, a2));
            }
            catch
            {
                MessageBox.Show("Введите правильные данные");
                index = 999999;
            }
            return Tuple.Create(data, index);
        }
        //-----------------------------------------------------

        public bool[] A_form = new bool[2];
        public bool[] D_form = new bool[2];
        public bool[] P_form = new bool[1];
        public bool[] DDS_form = new bool[1];
        public bool[] Console_form = new bool[1];

        private void button_adc0_Click(object sender, RoutedEventArgs e)
        {
            if (A_form[0] == false)
            {
                ADC_form newForm = new ADC_form("ADC0");
                A_form[0] = true;
                newForm.Show();
                newForm.Owner = this;
            }
        }

        private void button_adc1_Click(object sender, RoutedEventArgs e)
        {
            if (A_form[1] == false)
            {
                ADC_form newForm = new ADC_form("ADC1");
                A_form[1] = true;
                newForm.Show();
                newForm.Owner = this;
            }
        }

        private void button_dac0_Click(object sender, RoutedEventArgs e)
        {
            if (D_form[0] == false)
            {
                DAC_form newForm = new DAC_form("DAC0");
                D_form[0] = true;
                newForm.Show();
                newForm.Owner = this;
            }
        }

        private void button_dac1_Click(object sender, RoutedEventArgs e)
        {
            if (D_form[1] == false)
            {
                DAC_form newForm = new DAC_form("DAC1");
                D_form[1] = true;
                newForm.Show();
                newForm.Owner = this;
            }
        }



        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(":" + Console_form[0]);
            if (Console_form[0] == false)
            {
                form_consol1 newForm = new form_consol1("console1");
                Console_form[0] = true;
                newForm.Show();
                newForm.Owner = this;

            }
        }

        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            if (P_form[0] == false)
            {
                Panel_form newForm = new Panel_form("Panel1");
                P_form[0] = true;
                newForm.Show();
                newForm.Owner = this;
            }
        }

        UInt32 CMD_ID = 0;
        public void UDP_SEND(uint CMD_type, byte[] CMD_data, uint CMD_size, ulong CMD_time)
        {
            byte[] UDP_packet = new byte[1440];
            int DATA_lenght = 0;
            int i = 0;

            UInt64 sch_cmd = 0;
            try
            {
                FRAME.Frame_size = 0;
                FRAME.Frame_number = 0;
                FRAME.Stop_bit = 1;
                FRAME.Msg_uniq_id = 1;
                FRAME.Sender_id = 1;
                FRAME.Receiver_id = 2;
                FRAME.MSG.Msg_size = 10;
                FRAME.MSG.Msg_type = 1;
                FRAME.MSG.Num_cmd_in_msg = 1;
                sch_cmd = 1;//считаем число команд в файле

                //-------------------фреймовая часть пакета-----------------------
                UDP_packet[0] = Convert.ToByte((FRAME.Frame_size >> 8) & 0xff);
                UDP_packet[1] = Convert.ToByte((FRAME.Frame_size >> 0) & 0xff);

                UDP_packet[2] = Convert.ToByte((FRAME.Frame_number >> 8) & 0xff);
                UDP_packet[3] = Convert.ToByte((FRAME.Frame_number >> 0) & 0xff);//

                UDP_packet[4] = Convert.ToByte((FRAME.Msg_uniq_id >> 24) & 0xff);
                UDP_packet[5] = Convert.ToByte((FRAME.Msg_uniq_id >> 16) & 0xff);
                UDP_packet[6] = Convert.ToByte((FRAME.Msg_uniq_id >> 8) & 0xff);
                UDP_packet[7] = Convert.ToByte((FRAME.Msg_uniq_id >> 0) & 0xff);

                UDP_packet[8] = Convert.ToByte((FRAME.Sender_id >> 56) & 0xff);
                UDP_packet[9] = Convert.ToByte((FRAME.Sender_id >> 48) & 0xff);
                UDP_packet[10] = Convert.ToByte((FRAME.Sender_id >> 40) & 0xff);
                UDP_packet[11] = Convert.ToByte((FRAME.Sender_id >> 32) & 0xff);
                UDP_packet[12] = Convert.ToByte((FRAME.Sender_id >> 24) & 0xff);
                UDP_packet[13] = Convert.ToByte((FRAME.Sender_id >> 16) & 0xff);
                UDP_packet[14] = Convert.ToByte((FRAME.Sender_id >> 8) & 0xff);
                UDP_packet[15] = Convert.ToByte((FRAME.Sender_id >> 0) & 0xff);

                UDP_packet[16] = Convert.ToByte((FRAME.Receiver_id >> 56) & 0xff);
                UDP_packet[17] = Convert.ToByte((FRAME.Receiver_id >> 48) & 0xff);
                UDP_packet[18] = Convert.ToByte((FRAME.Receiver_id >> 40) & 0xff);
                UDP_packet[19] = Convert.ToByte((FRAME.Receiver_id >> 32) & 0xff);
                UDP_packet[20] = Convert.ToByte((FRAME.Receiver_id >> 24) & 0xff);
                UDP_packet[21] = Convert.ToByte((FRAME.Receiver_id >> 16) & 0xff);
                UDP_packet[22] = Convert.ToByte((FRAME.Receiver_id >> 8) & 0xff);
                UDP_packet[23] = Convert.ToByte((FRAME.Receiver_id >> 0) & 0xff);

                UDP_packet[24] = Convert.ToByte((FRAME.MSG.Msg_size >> 24) & 0xff);
                UDP_packet[25] = Convert.ToByte((FRAME.MSG.Msg_size >> 16) & 0xff);
                UDP_packet[26] = Convert.ToByte((FRAME.MSG.Msg_size >> 8) & 0xff);
                UDP_packet[27] = Convert.ToByte((FRAME.MSG.Msg_size >> 0) & 0xff);

                UDP_packet[28] = Convert.ToByte((FRAME.MSG.Msg_type >> 24) & 0xff);
                UDP_packet[29] = Convert.ToByte((FRAME.MSG.Msg_type >> 16) & 0xff);
                UDP_packet[30] = Convert.ToByte((FRAME.MSG.Msg_type >> 8) & 0xff);
                UDP_packet[31] = Convert.ToByte((FRAME.MSG.Msg_type >> 0) & 0xff);

                UDP_packet[32] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 56) & 0xff);
                UDP_packet[33] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 48) & 0xff);
                UDP_packet[34] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 40) & 0xff);
                UDP_packet[35] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 32) & 0xff);
                UDP_packet[36] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 24) & 0xff);
                UDP_packet[37] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 16) & 0xff);
                UDP_packet[38] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 8) & 0xff);
                UDP_packet[39] = Convert.ToByte((FRAME.MSG.Num_cmd_in_msg >> 0) & 0xff);
                //-----------------------------------------------------------------------------------------
                int j = 0;
                DATA_lenght = 40;//это число байт из упаковки выше 39+1
                while (sch_cmd > 0)
                {
                    FRAME.MSG.CMD.Cmd_size = Convert.ToUInt16(1 + CMD_data.Length);
                    FRAME.MSG.CMD.Cmd_id = CMD_ID++;

                    UDP_packet[40 + j] = Convert.ToByte((CMD_size >> 24) & 0xff);
                    UDP_packet[41 + j] = Convert.ToByte((CMD_size >> 16) & 0xff);
                    UDP_packet[42 + j] = Convert.ToByte((CMD_size >> 8) & 0xff);
                    UDP_packet[43 + j] = Convert.ToByte((CMD_size >> 0) & 0xff);

                    UDP_packet[44 + j] = Convert.ToByte((CMD_type >> 24) & 0xff);
                    UDP_packet[45 + j] = Convert.ToByte((CMD_type >> 16) & 0xff);
                    UDP_packet[46 + j] = Convert.ToByte((CMD_type >> 8) & 0xff);
                    UDP_packet[47 + j] = Convert.ToByte((CMD_type >> 0) & 0xff);

                    UDP_packet[48 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 56) & 0xff);
                    UDP_packet[49 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 48) & 0xff);
                    UDP_packet[50 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 40) & 0xff);
                    UDP_packet[51 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 32) & 0xff);
                    UDP_packet[52 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 24) & 0xff);
                    UDP_packet[53 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 16) & 0xff);
                    UDP_packet[54 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 8) & 0xff);
                    UDP_packet[55 + j] = Convert.ToByte((FRAME.MSG.CMD.Cmd_id >> 0) & 0xff);

                    UDP_packet[56 + j] = Convert.ToByte((CMD_time >> 56) & 0xff);
                    UDP_packet[57 + j] = Convert.ToByte((CMD_time >> 48) & 0xff);
                    UDP_packet[58 + j] = Convert.ToByte((CMD_time >> 40) & 0xff);
                    UDP_packet[59 + j] = Convert.ToByte((CMD_time >> 32) & 0xff);
                    UDP_packet[60 + j] = Convert.ToByte((CMD_time >> 24) & 0xff);
                    UDP_packet[61 + j] = Convert.ToByte((CMD_time >> 16) & 0xff);
                    UDP_packet[62 + j] = Convert.ToByte((CMD_time >> 8) & 0xff);
                    UDP_packet[63 + j] = Convert.ToByte((CMD_time >> 0) & 0xff);

                    for (i = 0; i < CMD_size; i++) UDP_packet[64 + i + j] = CMD_data[i];

                    DATA_lenght = DATA_lenght + Convert.ToInt32(CMD_size) + 24;

                    sch_cmd--;
                    j = j + Convert.ToInt32(CMD_size) + 24;
                }

                //-----шлём данные по UDP--------------
                string ip_dest = textBox_dest_ip.Text;
                int port_dest = Convert.ToInt32(textBox_dest_port.Text);

                UdpClient client = new UdpClient();
                client.Connect(ip_dest, port_dest);
                int number_bytes = client.Send(UDP_packet, DATA_lenght);
                //           Debug.WriteLine("DATA_lenght                  :" + DATA_lenght);
                //           Debug.WriteLine("FRAME.MSG.CMD.Cmd_data.Length:" + FRAME.MSG.CMD.Cmd_data.Length);
                //           Debug.WriteLine("FRAME.MSG.CMD.Cmd_data       :" + FRAME.MSG.CMD.Cmd_data);
                client.Close();

            }
            catch
            {

            }
        }

        private void button_init_lmk_Click(object sender, RoutedEventArgs e)
        {
            //DAC_info
            string s1 = "";
            byte cmd = 203;
            int data = 0;
            byte[] a = new byte[4];

            cmd = CMD_LMK_init;
            data = 1;

            a[3] = 1;
            FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
            UDP_SEND
                       (
                       cmd, //команда 
                       a,   //данные
                       4,   //число данных в байтах
                       0    //время исполнения , 0 - значит немедленно как сможешь.
                       );

            s1 = " ~0 init_lmk:" + Convert.ToString(a[3]) + "; "; //

            try
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();
                }
                Debug.WriteLine("шлём:" + s1);
                serialPort1.Write(s1);

            }
            catch (Exception ex)
            {
                // что-то пошло не так и упало исключение... Выведем сообщение исключения
                Console.WriteLine(string.Format("Port:'{0}' Error:'{1}'", serialPort1.PortName, ex.Message));
            }

        }

        private void button_DDS_Click(object sender, RoutedEventArgs e)
        {
            if (DDS_form[0] == false)
            {
                DDS_form newForm = new DDS_form("DDS");
                DDS_form[0] = true;
                newForm.Show();
                newForm.Owner = this;
            }
        }

        private void checkBox_ch1_Click(object sender, RoutedEventArgs e)
        {   //
            string s1;
            byte cmd;
            byte[] a = new byte[4];

            if (checkBox_ch1.IsChecked ?? true) SWITCH_072 = Convert.ToByte(SWITCH_072 | 0x01);
            else SWITCH_072 = Convert.ToByte(SWITCH_072 & (~0x01));

            cmd = CMD_CHANNEL;

            a[3] = SWITCH_072;
            FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
            UDP_SEND
                       (
                       cmd, //команда управление светодиодом "ИСПРАВ"
                       a,   //данные
                       4,   //число данных в байтах
                       0    //время исполнения , 0 - значит немедленно как сможешь.
                       );

            s1 = " ~0 upr_switch:" + Convert.ToString(a[3]) + "; "; //команда по уарту

            try
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();
                }
                Debug.WriteLine("шлём:" + s1);
                serialPort1.Write(s1);
            }
            catch (Exception ex)
            {
                // что-то пошло не так и упало исключение... Выведем сообщение исключения
                Console.WriteLine(string.Format("Port:'{0}' Error:'{1}'", serialPort1.PortName, ex.Message));
            }

        }

        private void checkBox_ch2_Click(object sender, RoutedEventArgs e)
        {
            string s1;
            byte cmd;
            byte[] a = new byte[4];

            if (checkBox_ch2.IsChecked ?? true) SWITCH_072 = Convert.ToByte(SWITCH_072 | 0x02);
            else SWITCH_072 = Convert.ToByte(SWITCH_072 & (~0x02));

            cmd = CMD_CHANNEL;

            a[3] = SWITCH_072;
            FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
            UDP_SEND
                       (
                       cmd, //команда управление светодиодом "ИСПРАВ"
                       a,   //данные
                       4,   //число данных в байтах
                       0    //время исполнения , 0 - значит немедленно как сможешь.
                       );

            s1 = " ~0 upr_switch:" + Convert.ToString(a[3]) + "; "; //команда по уарту

            try
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();
                }
                Debug.WriteLine("шлём:" + s1);
                serialPort1.Write(s1);
            }
            catch (Exception ex)
            {
                // что-то пошло не так и упало исключение... Выведем сообщение исключения
                Console.WriteLine(string.Format("Port:'{0}' Error:'{1}'", serialPort1.PortName, ex.Message));
            }

        }

        private void checkBox_ch3_Click(object sender, RoutedEventArgs e)
        {
            string s1;
            byte cmd;
            byte[] a = new byte[4];

            if (checkBox_ch3.IsChecked ?? true) SWITCH_072 = Convert.ToByte(SWITCH_072 | 0x04);
            else SWITCH_072 = Convert.ToByte(SWITCH_072 & (~0x04));

            cmd = CMD_CHANNEL;

            a[3] = SWITCH_072;
            FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
            UDP_SEND
                       (
                       cmd, //команда управление светодиодом "ИСПРАВ"
                       a,   //данные
                       4,   //число данных в байтах
                       0    //время исполнения , 0 - значит немедленно как сможешь.
                       );

            s1 = " ~0 upr_switch:" + Convert.ToString(a[3]) + "; "; //команда по уарту

            try
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();
                }
                Debug.WriteLine("шлём:" + s1);
                serialPort1.Write(s1);
            }
            catch (Exception ex)
            {
                // что-то пошло не так и упало исключение... Выведем сообщение исключения
                Console.WriteLine(string.Format("Port:'{0}' Error:'{1}'", serialPort1.PortName, ex.Message));
            }

        }

        private void checkBox_ch4_Click(object sender, RoutedEventArgs e)
        {
            string s1;
            byte cmd;
            byte[] a = new byte[4];

            if (checkBox_ch4.IsChecked ?? true) SWITCH_072 = Convert.ToByte(SWITCH_072 | 0x08);
            else SWITCH_072 = Convert.ToByte(SWITCH_072 & (~0x08));

            cmd = CMD_CHANNEL;

            a[3] = SWITCH_072;
            FLAG_ETH_request = 1;//поднимаем флаг запроса по ETH с поделкой , для контроля обмена по сети
            UDP_SEND
                       (
                       cmd, //команда управление светодиодом "ИСПРАВ"
                       a,   //данные
                       4,   //число данных в байтах
                       0    //время исполнения , 0 - значит немедленно как сможешь.
                       );

            s1 = " ~0 upr_switch:" + Convert.ToString(a[3]) + "; "; //команда по уарту

            try
            {
                if (serialPort1.IsOpen == false)
                {
                    serialPort1.Open();
                }
                Debug.WriteLine("шлём:" + s1);
                serialPort1.Write(s1);
            }
            catch (Exception ex)
            {
                // что-то пошло не так и упало исключение... Выведем сообщение исключения
                Console.WriteLine(string.Format("Port:'{0}' Error:'{1}'", serialPort1.PortName, ex.Message));
            }
        }

        //---------------------------------------------------------------------------
        private static bool FLAG_UART_TX = true;
        private static System.Timers.Timer aTimer;
        private string S_uart = "";
        private static void SetTimer(int t)
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(t);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            FLAG_UART_TX = true;
            aTimer.Stop();
        //  Console.WriteLine("time {0:HH:mm:ss.fff}",e.SignalTime);
        }
         public uint UART_TX(string[] Array)
        {
            uint i = 0;
            string s1 = "";
            SetTimer(50);
            while (i < (Array.Length-1))
              {
                if (FLAG_UART_TX)
                {
                    FLAG_UART_TX = false;
                    i++;
                    s1 = Array[i];
                    S_uart = s1;
                    UART(S_uart);
                    aTimer.Start();
                    Debug.WriteLine("шлём:" + s1);
                }                                     
               }          
            return 1;
        }

         void UART (string s1)
        {
            try
            {
               if (serialPort1.IsOpen == false)
               {
                serialPort1.Open();
               }
                serialPort1.Write(s1);
            }
            catch (Exception ex)
            {
                // что-то пошло не так и упало исключение... Выведем сообщение исключения
                Console.WriteLine(string.Format("Port:'{0}' Error:'{1}'", serialPort1.PortName, ex.Message));
                MessageBox.Show("Закрыт компорт!");
            }
        }
        

    }
}
