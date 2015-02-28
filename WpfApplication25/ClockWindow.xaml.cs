using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Speech.Synthesis;

namespace WpfApplication25
{
    /// <summary>
    /// Interaction logic for ClockWindow.xaml
    /// </summary>
    public partial class ClockWindow : Window
    {
        /// <summary>
        /// FirstNumberSegments, SecondNumberSegments, ThirdNumberSegments, FourthNumberSegments
        ///     Data Type : Rectangle[]
        ///     Used For  : Hold the Segments of each number
        ///TimeTeller
        ///     Data Type : SpeechSynthesizer
        ///     Used For  : Telling the current system time
        /// </summary>
        Rectangle[] FirstNumberSegments, SecondNumberSegments, ThirdNumberSegments, FourthNumberSegments;
        SpeechSynthesizer TimeTeller;

        /// <summary>
        /// Default Constructor
        ///     - InitializeComponent() is called
        ///     - Initialize the TimeTeller object that will be used to tell the current time
        ///     - Initialize the Rectangle array that is supposed to hold the segments of the 4 numbers displayed in the clock face
        ///     - Assign the individual number's segments to their corresponding holder
        ///     - Initialize the Object ClockDisplayAnimator
        ///         - Time Interval : 1
        ///         - Create a new Tick function
        ///         - Start the timer
        /// </summary>
        public ClockWindow()
        {
            InitializeComponent();

            TimeTeller = new SpeechSynthesizer();
            TimeTeller.SelectVoiceByHints(VoiceGender.Female);
            TimeTeller.Rate = -1;

            FirstNumberSegments = new Rectangle[7];
            SecondNumberSegments = new Rectangle[7];
            ThirdNumberSegments = new Rectangle[7];
            FourthNumberSegments = new Rectangle[7];

            FirstNumberSegments[0] = NumberSegment11;
            FirstNumberSegments[1] = NumberSegment12;
            FirstNumberSegments[2] = NumberSegment13;
            FirstNumberSegments[3] = NumberSegment14;
            FirstNumberSegments[4] = NumberSegment15;
            FirstNumberSegments[5] = NumberSegment16;
            FirstNumberSegments[6] = NumberSegment17;

            SecondNumberSegments[0] = NumberSegment21;
            SecondNumberSegments[1] = NumberSegment22;
            SecondNumberSegments[2] = NumberSegment23;
            SecondNumberSegments[3] = NumberSegment24;
            SecondNumberSegments[4] = NumberSegment25;
            SecondNumberSegments[5] = NumberSegment26;
            SecondNumberSegments[6] = NumberSegment27;

            ThirdNumberSegments[0] = NumberSegment31;
            ThirdNumberSegments[1] = NumberSegment32;
            ThirdNumberSegments[2] = NumberSegment33;
            ThirdNumberSegments[3] = NumberSegment34;
            ThirdNumberSegments[4] = NumberSegment35;
            ThirdNumberSegments[5] = NumberSegment36;
            ThirdNumberSegments[6] = NumberSegment37;

            FourthNumberSegments[0] = NumberSegment41;
            FourthNumberSegments[1] = NumberSegment42;
            FourthNumberSegments[2] = NumberSegment43;
            FourthNumberSegments[3] = NumberSegment44;
            FourthNumberSegments[4] = NumberSegment45;
            FourthNumberSegments[5] = NumberSegment46;
            FourthNumberSegments[6] = NumberSegment47;

            System.Windows.Threading.DispatcherTimer ClockDisplayAnimator = new System.Windows.Threading.DispatcherTimer();
            ClockDisplayAnimator.Interval = System.TimeSpan.FromSeconds(1);
            ClockDisplayAnimator.Tick += ClockDisplayAnimator_Tick;
            ClockDisplayAnimator.Start();
        }


        /// <summary>
        /// Refreshes the Display every second with the updated time
        ///     - Get the System time
        ///     - Set the Opacity values for the AM/PM indicator depending on the value of HourNow/12
        ///     - Update the time by calling the SevenSegmentConverter(SegmentName, Number) function
        ///     - Make the ":" in the clock face blink every second 
        ///         - Making its Opactiy "1" when the second is an odd number and "0.3" when it is an even number
        ///     - Show the Tooltip with the content as the current date and time
        /// </summary>
        void ClockDisplayAnimator_Tick(object sender, EventArgs e)
        {
            int MinuteNow = DateTime.Now.Minute;
            int HourNow = DateTime.Now.Hour % 12 == 0 ? 12 : DateTime.Now.Hour % 12;
			
            if (DateTime.Now.Hour / 12 == 1)
            {
                PMIndicator.Opacity = 1;
                AMIndicator.Opacity = 0.3;
            }
            else
            {
                PMIndicator.Opacity = 0.3;
                AMIndicator.Opacity = 1;
            }

            SevenSegmentConverter(FirstNumberSegments, HourNow / 10);
            SevenSegmentConverter(SecondNumberSegments, HourNow % 10);
            SevenSegmentConverter(ThirdNumberSegments, MinuteNow / 10);
            SevenSegmentConverter(FourthNumberSegments, MinuteNow % 10);

            if (DateTime.Now.Second % 2 == 0)
            {
                TimeTickIndicator1.Opacity = 0.3;
                TimeTickIndicator2.Opacity = 0.3;
            }
            else
            {
                TimeTickIndicator1.Opacity = 1;
                TimeTickIndicator2.Opacity = 1;
            }

            this.ToolTip = DateTime.Now;
        }

        /// <summary>
        /// Converts the given Number into a seven segment display logic 
        ///     - Use the input Number to determine the logic outputs
        ///     - For all the Segments that need to be activated set Opacity to "1" otherwise set Opacity to "0.3"
        /// </summary>
        public void SevenSegmentConverter(Rectangle[] Segment, int Number)
        {
            switch (Number)
            {
                case 0:
                    Segment[0].Opacity = 1;
                    Segment[1].Opacity = 1;
                    Segment[2].Opacity = 1;
                    Segment[3].Opacity = 1;
                    Segment[4].Opacity = 1;
                    Segment[5].Opacity = 0.3;
                    Segment[6].Opacity = 1;
                    break;

                case 1:
                    Segment[0].Opacity = 0.3;
                    Segment[1].Opacity = 1;
                    Segment[2].Opacity = 0.3;
                    Segment[3].Opacity = 1;
                    Segment[4].Opacity = 0.3;
                    Segment[5].Opacity = 0.3;
                    Segment[6].Opacity = 0.3;
                    break;

                case 2:
                    Segment[0].Opacity = 0.3;
                    Segment[1].Opacity = 1;
                    Segment[2].Opacity = 1;
                    Segment[3].Opacity = 0.3;
                    Segment[4].Opacity = 1;
                    Segment[5].Opacity = 1;
                    Segment[6].Opacity = 1;
                    break;

                case 3:
                    Segment[0].Opacity = 0.3;
                    Segment[1].Opacity = 1;
                    Segment[2].Opacity = 0.3;
                    Segment[3].Opacity = 1;
                    Segment[4].Opacity = 1;
                    Segment[5].Opacity = 1;
                    Segment[6].Opacity = 1;
                    break;

                case 4:
                    Segment[0].Opacity = 1;
                    Segment[1].Opacity = 1;
                    Segment[2].Opacity = 0.3;
                    Segment[3].Opacity = 1;
                    Segment[4].Opacity = 0.3;
                    Segment[5].Opacity = 1;
                    Segment[6].Opacity = 0.3;
                    break;

                case 5:
                    Segment[0].Opacity = 1;
                    Segment[1].Opacity = 0.3;
                    Segment[2].Opacity = 0.3;
                    Segment[3].Opacity = 1;
                    Segment[4].Opacity = 1;
                    Segment[5].Opacity = 1;
                    Segment[6].Opacity = 1;
                    break;

                case 6:
                    Segment[0].Opacity = 1;
                    Segment[1].Opacity = 0.3;
                    Segment[2].Opacity = 1;
                    Segment[3].Opacity = 1;
                    Segment[4].Opacity = 1;
                    Segment[5].Opacity = 1;
                    Segment[6].Opacity = 1;
                    break;

                case 7:
                    Segment[0].Opacity = 0.3;
                    Segment[1].Opacity = 1;
                    Segment[2].Opacity = 0.3;
                    Segment[3].Opacity = 1;
                    Segment[4].Opacity = 1;
                    Segment[5].Opacity = 0.3;
                    Segment[6].Opacity = 0.3;
                    break;

                case 8:
                    Segment[0].Opacity = 1;
                    Segment[1].Opacity = 1;
                    Segment[2].Opacity = 1;
                    Segment[3].Opacity = 1;
                    Segment[4].Opacity = 1;
                    Segment[5].Opacity = 1;
                    Segment[6].Opacity = 1;
                    break;

                case 9:
                    Segment[0].Opacity = 1;
                    Segment[1].Opacity = 1;
                    Segment[2].Opacity = 0.3;
                    Segment[3].Opacity = 1;
                    Segment[4].Opacity = 1;
                    Segment[5].Opacity = 1;
                    Segment[6].Opacity = 1;
                    break;
            }
        }

        /// <summary>
        /// Closes the window
        ///     - Cancel all the asynchronous Speak Operations
        ///     - Call the Close() built-in function
        /// </summary>
        private void close_Click(object sender, RoutedEventArgs e)
        {
            TimeTeller.SpeakAsyncCancelAll();

            this.Close();
        }

        /// <summary>
        /// Re-positions the window
        ///     - Call the DragMove() built-in function
        /// </summary>
        private void DigiClk_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DigiClk.DragMove();
        }

        /// <summary>
        /// Tells the Current time
        ///     - Format hh:mm:(AM/Pm)
        ///         - If hour is "0" tell 12'o clock
        ///         - If minute is 0 skip it
        ///         - Use the Mod(hour) value to determine whether AM/PM
        /// </summary>
        private void DigiClk_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TimeTeller.SpeakAsync((DateTime.Now.Hour % 12 == 0 ? 12 : DateTime.Now.Hour % 12) + " " + (DateTime.Now.Minute == 0 ? " " : DateTime.Now.Minute + " ") + (DateTime.Now.Hour / 12 == 0 ? "A M" : "P M"));
        }
    }
}