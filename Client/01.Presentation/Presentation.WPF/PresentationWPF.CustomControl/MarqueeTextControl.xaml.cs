﻿using System;
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
using System.Windows.Media.Animation;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Interaction logic for MarqueeTextControl.xaml
    /// </summary>
    public partial class MarqueeTextControl : UserControl
    {
        MarqueeType _marqueeType;

        public MarqueeType MarqueeType
        {
            get { return _marqueeType; }
            set { _marqueeType = value; }
        }       

        public String MarqueeContent
        {
            set { tbmarquee.Text = value; }
        }

        private double _marqueeTimeInSeconds;

        public double MarqueeTimeInSeconds
        {
            get { return _marqueeTimeInSeconds; }
            set { _marqueeTimeInSeconds = value; }
        }


        public MarqueeTextControl()
        {
            InitializeComponent();
            canMain.Height = this.Height;
            canMain.Width = this.Width;
            this.Loaded += new RoutedEventHandler(MarqueeText_Loaded);
        }

        void MarqueeText_Loaded(object sender, RoutedEventArgs e)
        {
            StartMarqueeing(_marqueeType);
        }

        public void StartMarqueeing(MarqueeType marqueeType)
        {
            if (marqueeType == MarqueeType.LeftToRight)
            {
                LeftToRightMarquee();
            }
            else if (marqueeType == MarqueeType.RightToLeft)
            {
                RightToLeftMarquee();
            }
            else if (marqueeType == MarqueeType.TopToBottom)
            {
                TopToBottomMarquee();
            }
            else if (marqueeType == MarqueeType.BottomToTop)
            {
                BottomToTopMarquee();
            }
        }

        private void LeftToRightMarquee()
        {
            double height = canMain.ActualHeight - tbmarquee.ActualHeight;
            tbmarquee.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tbmarquee.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(_marqueeTimeInSeconds));
            tbmarquee.BeginAnimation(Canvas.LeftProperty, doubleAnimation);
        }
        private void RightToLeftMarquee()
        {
            double height = canMain.ActualHeight - tbmarquee.ActualHeight;
            tbmarquee.Margin = new Thickness(0, height / 2, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tbmarquee.ActualWidth;
            doubleAnimation.To = canMain.ActualWidth;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(_marqueeTimeInSeconds));
            tbmarquee.BeginAnimation(Canvas.RightProperty, doubleAnimation);
        }
        private void TopToBottomMarquee()
        {
            double width = canMain.ActualWidth - tbmarquee.ActualWidth;
            tbmarquee.Margin = new Thickness(width / 2, 0, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tbmarquee.ActualHeight;
            doubleAnimation.To = canMain.ActualHeight;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(_marqueeTimeInSeconds));
            tbmarquee.BeginAnimation(Canvas.TopProperty, doubleAnimation);
        }
        private void BottomToTopMarquee()
        {
            double width = canMain.ActualWidth - tbmarquee.ActualWidth;
            tbmarquee.Margin = new Thickness(width / 2, 0, 0, 0);
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = -tbmarquee.ActualHeight;
            doubleAnimation.To = canMain.ActualHeight;
            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(_marqueeTimeInSeconds));
            tbmarquee.BeginAnimation(Canvas.BottomProperty, doubleAnimation);
        }
    }
    public enum MarqueeType
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }
    
}
