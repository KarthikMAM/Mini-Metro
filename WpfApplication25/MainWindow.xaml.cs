using System;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;

namespace WpfApplication25
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    ///     - Used to Create a SplashScreen window and animate its properties
    ///     - And start the Side Strip as an Idle Window
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// AnimatronicsTimer
        ///     - DataType  : DispatcherTimer
        ///     - Used For  : Implement Animations to the splashscreen
        /// ElapsedTime
        ///     - DataType  : int
        ///     - Used For  : To know which stage the Animation is in at any particular instant
        /// Letters
        ///     - DataType  : Label[]
        ///     - Used For  : Used as a holder for Label Objects
        /// </summary>
        System.Windows.Threading.DispatcherTimer AnimatronicsTimer;
        int ElapsedTime;
        Label[] Letters;
        SpeechSynthesizer Speaker;

        /// <summary>
        /// This initializes the new window of the SplashScreen which is also the MainWindow - Default Constuctor
        ///     - The Elapsed time is set to the default value 0
        ///     - The Individual letters of kappspot which are Labels are assgined to the Label Array
        ///     - The DispatcherTimer Object that implements the animations is initialized here and started
        ///         - Time Interval = 10 Milliseconds
        ///     - A new speech synthesizer object is created and a female voice has been selected. It is made to speak the welcome text asynchronously
        ///         - Voice : Female
        /// </summary>
        public MainWindow()
        {

            InitializeComponent();

            ElapsedTime = 0;

            Letters = new Label[8] { letter0, letter1, letter2, letter3, letter4, letter5, letter6, letter7 };

            AnimatronicsTimer = new System.Windows.Threading.DispatcherTimer();
            AnimatronicsTimer.Interval = TimeSpan.FromMilliseconds(10);
            AnimatronicsTimer.Tick += AnimatronicsTimer_Tick;
            AnimatronicsTimer.Start();

            Speaker = new SpeechSynthesizer();
            Speaker.SelectVoiceByHints(VoiceGender.Female);
        }

        /// <summary>
        /// This Implements the Animation in the SplashScreen
        ///     - Implement the Fading in and Fading out of the Splash Screen
        ///         - Increment the ElapsedTime by "10"
        ///         - Welcome note : Kappspot welcomes you
        ///             - If the ElapsedTime is equal to 50 call the speaker's SpeakAsync() with "Kappspot Wecomes you!" as a function parameter
        ///         - If the Opacity is greater than "0"
        ///             - If the ElapsedTime is less than "1500"
        ///                 - Increment the Opacity by 0.02 provided the Opacity is less the "0.92"
        ///             - Otherwise 
        ///                 - Decrement Opacity by "0.02"
        ///         - Otherwise
        ///             - Stop the Asynchronous Timer
        ///             - Create and Show the IdleWindow
        ///                 - Create and Initialize the WindowObject
        ///                 - Set the Left and the Right Margin
        ///             - Close the Current Window
        ///         - If the ElapsedTime is less than "250"
        ///             - Contract the Logo Size by "2"
        ///         - Otherwise 
        ///             - If the ElapsedTime is less than "500"
        ///                 - Expand the Logo Size by "2"
        ///             - Otherwise if the Opacity of the Letters is greater than "0"
        ///                 - Decrease their Opacity by "0.03" 
        /// </summary>
        void AnimatronicsTimer_Tick(object sender, EventArgs e)
        {
            ElapsedTime += 10;

            if(ElapsedTime == 50)
            {
                string UserName = "";
                foreach (char i in Environment.UserName)
                {
                    if(i != '.')
                    {
                        UserName += i;
                    }
                    else
                    {
                        UserName += ' ';
                    }
                }
                Speaker.SpeakAsync("Welcome " + UserName);
            }

            if (this.Opacity > 0)
            {
                if (ElapsedTime < 1500)
                {
                    if (this.Opacity < 0.92)
                    {
                        this.Opacity += 0.02;
                    }
                }
                else
                {
                    this.Opacity -= 0.02;
                }
            }
            else
            {
                AnimatronicsTimer.Stop();

                new Window9().Show();
                this.Close();
            }

            if (ElapsedTime < 250)
            {
                Logo.Height -= 2;
                Logo.Width -= 2;
            }
            else if (ElapsedTime < 500)
            {
                Logo.Height += 2;
                Logo.Width += 2;
            } 
            else if (Letters[0].Opacity > 0)
            {
                for (int i = 0; i < 8; i++)
                {
                    Letters[i].Opacity -= 0.03;
                }
            }
        }
    }
}