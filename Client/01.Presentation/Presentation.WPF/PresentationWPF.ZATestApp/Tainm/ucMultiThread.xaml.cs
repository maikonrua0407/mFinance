using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

namespace PresentationWPF.ZATestApp.Tainm
{
    /// <summary>
    /// Interaction logic for ucMultiThread.xaml
    /// </summary>
    public partial class ucMultiThread : UserControl
    {
        DispatcherTimer timeone = new DispatcherTimer();
        DispatcherTimer timetwo = new DispatcherTimer();
        System.Threading.Thread threadOne = null;
        System.Threading.Thread threadTwo = null;
        int idThreadOne = 0;
        int idThreadTwo = 0;
        public ucMultiThread()
        {
            InitializeComponent();
            threadOne = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadOne));
            threadOne.Name = "LAN PHUONG";
            idThreadOne = threadOne.ManagedThreadId;
            threadTwo = new System.Threading.Thread(new System.Threading.ThreadStart(ThreadTwo));
            idThreadTwo = threadTwo.ManagedThreadId;
            threadOne.Start();
            threadTwo.Start();
        }



        public void ThreadOne()
        {
            timeone.Tick += new EventHandler(timeone_Tick);
            timeone.Interval = new System.TimeSpan(0, 0, 1);
            timeone.Start();
        }

        void timeone_Tick(object sender, EventArgs e)
        {
            TimeOne.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        public void ThreadTwo()
        {
            timetwo.Tick += new EventHandler(timetwo_Tick);
            timetwo.Interval = new System.TimeSpan(0, 0, 1);
            timetwo.Start();
        }

        void timetwo_Tick(object sender, EventArgs e)
        {
            TimeTwo.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void AboutOne_Click(object sender, RoutedEventArgs e)
        {
            Process AboutCollection = Process.GetCurrentProcess();
            foreach (Thread About in AboutCollection.Threads)
            {
                if (About.ManagedThreadId == idThreadOne)
                {
                    About.Abort();
                    timeone.Stop();
                    
                }
            }
        }

        private void AboutTwo_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread About = Process.GetCurrentProcess().Threads.Cast<Thread>().Single(q => q.ManagedThreadId == idThreadTwo);
            if (About.IsAlive)
            {
                timetwo.Stop();
                threadTwo.Abort();
            }
        }
    }
}
