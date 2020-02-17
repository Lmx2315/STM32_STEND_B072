﻿using System;
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
    /// Логика взаимодействия для form_consol1.xaml
    /// </summary>
    /// 
   
   
    public partial class form_consol1 : Window
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public form_consol1(string a)
        {
            InitializeComponent();
            this.Name_this = a;
            this.Title = a;

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            //          dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 25);
            dispatcherTimer.Start();
        }

        string Name_this = "";
        string list = "";
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            // MessageBox.Show("Closing called");
            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
               main.Console_form[0] = false;
            }

        }

        public void Clear_data ()
        {
            richTextBox.Document.Blocks.Clear();         
        }

        public void Show_data (string a)
        {
            richTextBox.Document.Blocks.Add(new Paragraph(new Run(a)));
            richTextBox.ScrollToEnd();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            MainWindow main = this.Owner as MainWindow;
            if (main != null)
            {
               if (main.console_text!="") this.Show_data(main.console_text);
                   main.console_text = "";
            }
        }
    }
}
