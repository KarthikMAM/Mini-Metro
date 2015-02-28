using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApplication25
{
    /// <summary>
    /// Interaction logic for Window8.xaml
    /// </summary>
    public partial class Window8 : Window
    {
        double ScreenHeight, ScreenWidth;
        DispatcherTimer WindowSizeAnimation;

        public Window8()
        {
            InitializeComponent();

            ScreenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            ScreenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;

            WindowSizeAnimation = new DispatcherTimer(DispatcherPriority.Send);
            WindowSizeAnimation.Interval = TimeSpan.FromMilliseconds(15);
            WindowSizeAnimation.Tick += WindowSizeAnimation_Tick;
            WindowSizeAnimation.Start();
        }

        void WindowSizeAnimation_Tick(object sender, EventArgs e)
        {
            if(Border.Height < 400)
            {
                Border.Height += 5;
                Border.Width += 5;
            }
            else
            {
                WindowSizeAnimation.Stop();
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
