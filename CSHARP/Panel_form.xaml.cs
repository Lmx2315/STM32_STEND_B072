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
    /// Логика взаимодействия для Panel_form.xaml
    /// </summary>
    public partial class Panel_form : Window
    {
        public Panel_form(string a)//конструктор формы
        {
            InitializeComponent();
            this.Name_this = a;
            this.Title = a;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                Debug.WriteLine("Name_this:" + Name_this);
                if (Name_this == "Panel1")
                {
                   
                    Debug.WriteLine(" :");
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
                if (Name_this == "Panel1") main.P_form[0] = false;
            }

        }

        byte[] a = new byte[4];
        private void cb_ISPARV_geen_Click(object sender, RoutedEventArgs e)
        {
            int z = 0;
            string s1 = "";
            byte cmd = 201;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (cb_ISPARV_geen.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.ISPRAV_GREEN = 1;

                        a[3] = main.ISPRAV_GREEN;

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 led_ispr:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.ISPRAV_GREEN = 0;

                        a[3] = main.ISPRAV_GREEN;

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 led_ispr:" + Convert.ToString(a[3]) + "; ";

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

        private void cb_ISPARV_red_Click(object sender, RoutedEventArgs e)
        {
            int z = 0;
            string s1 = "";
            byte cmd = 201;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (cb_ISPARV_red.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.ISPRAV_RED = 4;

                        a[3] = main.ISPRAV_RED;

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 led_ispr:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.ISPRAV_RED = 0;

                        a[3] = main.ISPRAV_RED;

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 led_ispr:" + Convert.ToString(a[3]) + "; ";

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

        private void chbox_lin_svyaz_green_Click(object sender, RoutedEventArgs e)
        {
            int z = 0;
            string s1 = "";
            byte cmd = 202;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (chbox_lin_svyaz_green.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.LIN_SVYAZ_GREEN = 1;

                        a[3] = main.LIN_SVYAZ_GREEN;

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 led_ispr:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.LIN_SVYAZ_GREEN = 0;

                        a[3] = main.LIN_SVYAZ_GREEN;

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 led_ispr:" + Convert.ToString(a[3]) + "; ";

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

        private void chbox_lin_svyaz_red_Click(object sender, RoutedEventArgs e)
        {
            int z = 0;
            string s1 = "";
            byte cmd = 202;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (chbox_lin_svyaz_red.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.LIN_SVYAZ_RED = 1;

                        a[3] = main.LIN_SVYAZ_RED;

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 led_ispr:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.LIN_SVYAZ_RED = 0;

                        a[3] = main.LIN_SVYAZ_RED;

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 led_ispr:" + Convert.ToString(a[3]) + "; ";

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

        private void chbox_PRM1_K1_Click(object sender, RoutedEventArgs e)
        {
            int z = 0;
            string s1 = "";
            byte cmd = 203;
            int data = 0;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (chbox_PRM1_K1.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRM1_K1 = 1;
                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 CN0led:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRM1_K1 = 0;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 CN0led:" + Convert.ToString(a[3]) + "; ";

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

        private void chbox_PRD1_K1_Click(object sender, RoutedEventArgs e)
        {
            int z = 0;
            string s1 = "";
            byte cmd = 203;
            int data = 0;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (chbox_PRD1_K1.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRD1_k1 = 1;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 CN1led:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRD1_k1 = 0;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 CN1led:" + Convert.ToString(a[3]) + "; ";

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

        private void chbox_PRD2_K1_Click(object sender, RoutedEventArgs e)
        {
            int z = 0;
            string s1 = "";
            byte cmd = 203;
            int data = 0;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (chbox_PRD2_K1.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRD2_k1 = 1;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 CN2led:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRD2_k1 = 0;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 CN2led:" + Convert.ToString(a[3]) + "; ";

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

        private void chbox_PRM2_K1_Click(object sender, RoutedEventArgs e)
        {
            int z = 0;
            string s1 = "";
            byte cmd = 203;
            int data = 0;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (chbox_PRM2_K1.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRM2_k1 = 1;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 CN3led:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRM2_k1 = 0;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 CN3led:" + Convert.ToString(a[3]) + "; ";

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

        private void chbox_PRM1_K2_Click(object sender, RoutedEventArgs e)
        {
            int z = 0;
            string s1 = "";
            byte cmd = 203;
            int data = 0;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (chbox_PRM1_K2.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRM1_K2 = 1;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 CN4led:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRM1_K2 = 0;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 CN4led:" + Convert.ToString(a[3]) + "; ";

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

        private void chbox_PRD1_K2_Click(object sender, RoutedEventArgs e)
        {

            int z = 0;
            string s1 = "";
            byte cmd = 203;
            int data = 0;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (chbox_PRD1_K2.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRD1_k2 = 1;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 CN5led:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRD1_k2 = 0;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 CN5led:" + Convert.ToString(a[3]) + "; ";

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

        private void chbox_PRD2_K2_Click(object sender, RoutedEventArgs e)
        {

            int z = 0;
            string s1 = "";
            byte cmd = 203;
            int data = 0;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (chbox_PRD2_K2.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRD2_k2 = 1;

                        data =   ((main.PRM1_K1 & 0x01) << 7) |
                                 ((main.PRM2_k1 & 0x01) << 6) |
                                 ((main.PRD1_k1 & 0x01) << 5) |
                                 ((main.PRD2_k1 & 0x01) << 4) |
                                 ((main.PRM1_K2 & 0x01) << 3) |
                                 ((main.PRM2_k2 & 0x01) << 2) |
                                 ((main.PRD1_k2 & 0x01) << 1) |
                                 ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 CN6led:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRD2_k2 = 0;

                        data =   ((main.PRM1_K1 & 0x01) << 7) |
                                 ((main.PRM2_k1 & 0x01) << 6) |
                                 ((main.PRD1_k1 & 0x01) << 5) |
                                 ((main.PRD2_k1 & 0x01) << 4) |
                                 ((main.PRM1_K2 & 0x01) << 3) |
                                 ((main.PRM2_k2 & 0x01) << 2) |
                                 ((main.PRD1_k2 & 0x01) << 1) |
                                 ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 CN6led:" + Convert.ToString(a[3]) + "; ";

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

        private void chbox_PRM2_K2_Click(object sender, RoutedEventArgs e)
        {
            int z = 0;
            string s1 = "";
            byte cmd = 203;
            int data = 0;

            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
                if (chbox_PRM2_K2.IsChecked == true)
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRM2_k2 = 1;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND
                            (
                            cmd, //команда управление светодиодом "ИСПРАВ"
                            a,   //данные
                            4,   //число данных в байтах
                            0    //время исполнения , 0 - значит немедленно как сможешь.
                            );

                        s1 = " ~0 CN7led:" + Convert.ToString(a[3]) + "; ";
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
                else
                {
                    if (Name_this == "Panel1")
                    {
                        main.PRM2_k2 = 0;

                        data =  ((main.PRM1_K1 & 0x01) << 7) |
                                ((main.PRM2_k1 & 0x01) << 6) |
                                ((main.PRD1_k1 & 0x01) << 5) |
                                ((main.PRD2_k1 & 0x01) << 4) |
                                ((main.PRM1_K2 & 0x01) << 3) |
                                ((main.PRM2_k2 & 0x01) << 2) |
                                ((main.PRD1_k2 & 0x01) << 1) |
                                ((main.PRD2_k2 & 0x01) << 0);


                        a[3] = Convert.ToByte(data);

                        main.UDP_SEND(cmd, a, 4, 0);

                        s1 = " ~0 CN7led:" + Convert.ToString(a[3]) + "; ";

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
    }
}
