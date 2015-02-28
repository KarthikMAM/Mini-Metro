using System;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApplication25
{
    /// <summary>
    /// Interaction logic for Window5.xaml
    /// </summary>
    public partial class Window5 : Window
    {
        RotateTransform MinuteHandRotator, SecondHandRotator, HourHandRotator;
        SpeechSynthesizer TimeTeller;

        public Window5()
        {
            InitializeComponent();
            
            TimeTeller = new SpeechSynthesizer();
            TimeTeller.SelectVoiceByHints(VoiceGender.Female);
            TimeTeller.Rate = -1;

            SecondHandRotator = new RotateTransform(6 * DateTime.Now.Second,0,0);
            MinuteHandRotator = new RotateTransform(6 * DateTime.Now.Minute + 0.1 * DateTime.Now.Second, 0, 0);
            HourHandRotator = new RotateTransform(30 * DateTime.Now.Hour + 0.5 * DateTime.Now.Minute, 0, 0);
            
            System.Windows.Threading.DispatcherTimer t = new System.Windows.Threading.DispatcherTimer();
            t.Interval = System.TimeSpan.FromSeconds(1);
            t.Tick += t_Tick;
            t.Start();        
        }

        void t_Tick(object sender, EventArgs e)
        {
            SecondHandRotator.Angle = 6 * DateTime.Now.Second;
            MinuteHandRotator.Angle = 6 * DateTime.Now.Minute + 0.1 * DateTime.Now.Second;
            HourHandRotator.Angle = 30 * DateTime.Now.Hour + 0.5 * DateTime.Now.Minute;

            minute.RenderTransform = MinuteHandRotator;
            hour.RenderTransform = HourHandRotator;
            second.RenderTransform = SecondHandRotator;

            minute.RenderTransformOrigin = new Point(0.5, 0.5);
            hour.RenderTransformOrigin = new Point(0.5, 0.5);
            second.RenderTransformOrigin = new Point(0.5, 0.5);

            Date.Text = " " + DateTime.Now.Day.ToString() + " ";
            switch(DateTime.Now.Month)
            {
                case 1: 
                    Date.Text += "JAN";
                    break;
                case 2:
                    Date.Text += "FEB";
                    break;
                case 3:
                    Date.Text += "MAR";
                    break;
                case 4:
                    Date.Text += "APR";
                    break;
                case 5:
                    Date.Text += "MAY";
                    break;
                case 6:
                    Date.Text += "JUN";
                    break;
                case 7:
                    Date.Text += "JUL";
                    break;
                case 8:
                    Date.Text += "AUG";
                    break;
                case 9:
                    Date.Text += "SEP";
                    break;
                case 10:
                    Date.Text += "OCT";
                    break;
                case 11:
                    Date.Text += "NOV";
                    break;
                case 12:
                    Date.Text += "DEC";
                    break;
            }

            switch(DateTime.Now.DayOfWeek.ToString())
            {
                case "Sunday":
                    Date.Text += " SUN";
                    break;
                case "Monday":
                    Date.Text += " MON";
                    break;
                case "Tuesday":
                    Date.Text += " TUE";
                    break;
                case "Wednesday":
                    Date.Text += " WED";
                    break;
                case "Thursday":
                    Date.Text += " THU";
                    break;
                case "Friday":
                    Date.Text += " FRI";
                    break;
                case "Saturday":
                    Date.Text += " SAT";
                    break;
            }

            //Add the purpose and the logo
            this.ToolTip = DateTime.Now;
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TimeTeller.SpeakAsync((DateTime.Now.Hour % 12 == 0 ? 12 : DateTime.Now.Hour % 12) + " " + (DateTime.Now.Minute == 0 ? " " : DateTime.Now.Minute + " ") + (DateTime.Now.Hour / 12 == 0 ? "A M" : "P M"));
        }
    }
}
