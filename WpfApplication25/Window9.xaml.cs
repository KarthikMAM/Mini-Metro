using System;
using System.Windows;
using System.Windows.Input;

namespace WpfApplication25
{
    /// <summary>
    /// Interaction logic for Window9.xaml
    ///     - Side strip will open the Dock Window and it will close
    ///     - It will re-position itself when the apps and the windows explorer are opened side-by-side
    /// </summary>
    public partial class Window9 : Window
    {
        /// <summary>
        /// AlignmentMaintainer
        ///     DataType : DispatcherTimer
        ///     Used for : Maintaining the Alignment of the Side Strip when the windows becomes a pane in the metro mode
        /// </summary>
        System.Windows.Threading.DispatcherTimer AlignmentMaintainer;


        /// <summary>
        /// Default Constructor
        ///     - InitializeComponent();
        ///     - Initializing AlignementAdjuster
        ///         - DispatcherPriority = ApplicationIdle 
        ///         - TimeInterval       = 5 MilliSeconds
        ///         - Creation of a new Tick event and Starting the timer
        ///     - Set the Left and the Top margin of this window
        /// </summary>
        public Window9()
        {
            InitializeComponent();

            AlignmentMaintainer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.ApplicationIdle);
            AlignmentMaintainer.Interval = TimeSpan.FromSeconds(5);
            AlignmentMaintainer.Tick += AlignmentMaintainer_Tick;
            AlignmentMaintainer.Start();

            this.Top = 20;
            this.Left = SystemParameters.PrimaryScreenWidth - 40;
        }


        /// <summary>
        /// Alignment Maintenance
        ///     - Sets the Alignment of the Side Strip's Left Margin realtive to the Primary screen's width
        /// </summary>
        void AlignmentMaintainer_Tick(object sender, EventArgs e)
        {
            this.Left = SystemParameters.PrimaryScreenWidth - 40;
        }


        /// <summary>
        /// Closes the current window and opens the Dock
        ///     - Create, Open and Show the NewWindow
        ///     - Close the current window
        /// </summary>
        private void Window_MouseEnter(object sender, MouseEventArgs e)
        {
                new Window4().Show();
                this.Close();
        }
    }
}
