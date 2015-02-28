using System;
using System.Windows;
using System.Windows.Input;

namespace WpfApplication25
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    ///     - Calender Widget
    ///     - Uses the built-in calender with transparent background and thereby making the image behind it as the background
    /// </summary>
    public partial class Window2 : Window
    {
        /// <summary>
        /// Default Constructor
        ///     - Calls InitializeComponent
        /// </summary>
        public Window2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closes the window
        ///     - Call the built-in function Close()
        /// </summary>
        private void close_Click(object sender, RoutedEventArgs e)
        {
            calender.Close();
        }

        /// <summary>
        /// Drag Moves the Calender window
        ///     - Call the built-in DragMove() function
        /// </summary>
        private void calender_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /// <summary>
        /// Sets the ToolTIp to show the current date and time
        ///     - Use the DateTime.Now to assign the Date and Time to the Calender
        /// </summary>
        private void calender_MouseEnter(object sender, MouseEventArgs e)
        {
            this.ToolTip = DateTime.Now;
        }
    }
}
