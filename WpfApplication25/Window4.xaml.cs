using System;
using System.Diagnostics;
using System.IO;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfApplication25
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        //The function of the dispatcher timer is to 
        DispatcherTimer HeightIncreamentor, WidthIncreamentor, HeightDecreamentor, WidthDecreamentor;
        bool[] OperationInProgress;
        enum Operation { WidthIncrease, WidthDecrease, HeightIncrease, HeightDecrease };
        bool CanExitApplication;
        int PrimaryScreenWidth;

        private SpeechSynthesizer EventInformer;

        /*System.Windows.Threading.DispatcherTimer t, s, window_expander;
        bool got_mouse_focus;
        bool can_enable_minimize_button;
        bool window_expansion_state;
        bool can_window_size_change;*/

        public Window4()
        {
            InitializeComponent();

            /*got_mouse_focus = true;
            can_enable_minimize_button = true;
            window_expansion_state = false;
            can_window_size_change = true;*/

            //Using seperate timers for each window property manipulation   
            HeightIncreamentor = new DispatcherTimer(DispatcherPriority.Send);
            HeightDecreamentor = new DispatcherTimer(DispatcherPriority.Send);
            WidthIncreamentor = new DispatcherTimer(DispatcherPriority.Send);
            WidthDecreamentor = new DispatcherTimer(DispatcherPriority.Send);

            //Give an interval for each timer function
            HeightDecreamentor.Interval = HeightIncreamentor.Interval = TimeSpan.FromMilliseconds(0.2);
            WidthIncreamentor.Interval = WidthDecreamentor.Interval = TimeSpan.FromMilliseconds(1);

            //Tick event creation
            HeightIncreamentor.Tick += HeightIncrease_Tick;
            HeightDecreamentor.Tick += HeightDecrease_Tick;
            WidthDecreamentor.Tick += WidthDecrease_Tick;
            WidthIncreamentor.Tick += WidthIncrease_Tick;

            //This is used to find which operation is being performed
            //This also enables us to prevent the simultaneous execution of the same operation
            //This also helps us to call the complementary functions for each operation at the required time only
            //This gives us an idea about the working model of the animator
            OperationInProgress = new bool[4];

            OperationInProgress[(int)Operation.WidthIncrease] = false;
            OperationInProgress[(int)Operation.WidthDecrease] = false;
            OperationInProgress[(int)Operation.HeightIncrease] = false;
            OperationInProgress[(int)Operation.HeightDecrease] = false;

            EventInformer = new SpeechSynthesizer();
            EventInformer.SelectVoiceByHints(VoiceGender.Female);

            PrimaryScreenWidth = (int)SystemParameters.PrimaryScreenWidth;

            this.Left = PrimaryScreenWidth;
            this.Top = 20;

            //This will check whether the files required for the data exist and if they do not exist then they are created

            //FileStream FileToBeCleared = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\FileHistory.kappspot", FileMode.OpenOrCreate, FileAccess.Read);
            //FileToBeCleared.Close();

            FileStream FileToBeCleared = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\history_data_provider.kappspot", FileMode.OpenOrCreate, FileAccess.Read);
            FileToBeCleared.Close();

            FileToBeCleared = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\kappspot_notes.kappspot", FileMode.OpenOrCreate, FileAccess.Read);
            FileToBeCleared.Close();

            FileToBeCleared = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\SearchHistory.kappspot", FileMode.OpenOrCreate, FileAccess.Read);
            FileToBeCleared.Close();

            if (File.Exists(Environment.CurrentDirectory + @"\Resources\Apps\Hang Man\Log.txt") == false)
            {
                StreamWriter Writer;
                FileToBeCleared = new FileStream(Environment.CurrentDirectory + @"\Resources\Apps\Hang Man\Log.txt", FileMode.OpenOrCreate, FileAccess.Write);
                Writer = new StreamWriter(FileToBeCleared);
                Writer.WriteLine("\n\n\n-----------------------------------------------------------------------");
                Writer.WriteLine("|   PLAYER 1 | SCORE |   PLAYER 2 | SCORE | TURNS |   GAME STATISTICS |");
                Writer.WriteLine("-----------------------------------------------------------------------");
                Writer.Flush();
                FileToBeCleared.Close();
            }

        }

        // use the increase timer if the required property's value is less. . .if it not then end the timer and assign the default value
        void WidthIncrease_Tick(object sender, EventArgs e)
        {
            if (this.Left > PrimaryScreenWidth - 504)
            {
                OperationInProgress[(int)Operation.WidthIncrease] = true;
                this.Left -= 10;
            }
            else
            {
                this.Left = PrimaryScreenWidth - 514.2;
                OperationInProgress[(int)Operation.WidthIncrease] = false;
                GridOfDock.IsEnabled = true;
                WidthIncreamentor.Stop();
            }
        }

        void WidthDecrease_Tick(object sender, EventArgs e)
        {
            if (this.Left < PrimaryScreenWidth - 40)
            {
                OperationInProgress[(int)Operation.WidthDecrease] = true;
                this.Left += 10;
            }
            else
            {
                OperationInProgress[(int)Operation.WidthDecrease] = false;
                WidthDecreamentor.Stop();

                if (CanExitApplication == false)
                {
                    new Window9().Show();
                }
                //if (FileHistory.SelectedIndex > -1)
                //{
                //    try
                //    {
                //        Process.Start(FileHistory.SelectedItem.ToString());
                //        EventInformer.SpeakAsync("Your request has been processed successfully.");
                //    }
                //    catch
                //    {
                //        EventInformer.SpeakAsync("Unable to Process your request. Please, check whether it is a valid file path");
                //        MessageBox.Show("Unable to Open the Requested File. . . \n\n  1. Check whether you have a valid application to open your File \n\n  2. Check whether you have deleted the file", "Notification: Unable to Open");
                //    }
                //}

                this.Close();
            }
        }

        void HeightIncrease_Tick(object sender, EventArgs e)
        {
            if (this.Height < 263)
            {
                OperationInProgress[(int)Operation.HeightIncrease] = true;
                this.Height += 5;
            }
            else
            {
                this.Height = 262;
                OperationInProgress[(int)Operation.HeightIncrease] = false;
                HeightIncreamentor.Stop();
            }
        }

        void HeightDecrease_Tick(object sender, EventArgs e)
        {
            if (this.Height > 159)
            {
                OperationInProgress[(int)Operation.HeightDecrease] = true;
                this.Height -= 5;
            }
            else
            {
                this.Height = 158;
                OperationInProgress[(int)Operation.HeightDecrease] = false;
                HeightDecreamentor.Stop();
            }
        }


        //Check whether the same operation is being executed
        //If it is not being executed stop the complementary process and then start the process
        private void widget_MouseEnter(object sender, MouseEventArgs e)
        {
            this.Focus();
            if (OperationInProgress[(int)Operation.WidthIncrease] == false)
            {
                OperationInProgress[(int)Operation.WidthDecrease] = false;
                WidthDecreamentor.Stop();
                WidthIncreamentor.Start();
            }
            if (OperationInProgress[(int)Operation.HeightIncrease] == false && this.Height > 159)
            {
                OperationInProgress[(int)Operation.HeightDecrease] = false;
                HeightDecreamentor.Stop();
                HeightIncreamentor.Start();
            }
        }

        private void widget_MouseLeave(object sender, MouseEventArgs e)
        {
            if(OperationInProgress[(int)Operation.WidthDecrease] == false)
            {
                OperationInProgress[(int)Operation.WidthIncrease] = false;
                WidthIncreamentor.Stop();
                WidthDecreamentor.Start();
            }
            if (OperationInProgress[(int)Operation.HeightDecrease] == false && this.Height > 158) 
            {
                OperationInProgress[(int)Operation.HeightIncrease] = false;
                HeightIncreamentor.Stop();
                HeightDecreamentor.Start();
            }
        }

        private void kappspot_productions_Click(object sender, RoutedEventArgs e)
        {
            if(OperationInProgress[(int)Operation.HeightIncrease] == false && this.Height > 158)
            {
                OperationInProgress[(int)Operation.HeightIncrease] = false;
                HeightIncreamentor.Stop();
                HeightDecreamentor.Start();
            }
            if (OperationInProgress[(int)Operation.HeightIncrease] == false && this.Height < 262)
            {
                OperationInProgress[(int)Operation.HeightDecrease] = false;
                HeightDecreamentor.Stop();
                HeightIncreamentor.Start();
            }
        }

        private void widget_Loaded(object sender, RoutedEventArgs e)
        {
            WidthIncreamentor.Start();
        }

        
        
        private void close_Click(object sender, RoutedEventArgs e)
        {
            CanExitApplication = true;
            this.Close();
        }
                
        private void switch_window_Click(object sender, RoutedEventArgs e)
        {
            EventInformer.SpeakAsyncCancelAll();
            try
            {
                Process.Start("shell:::{3080F90E-D7AD-11D9-BD98-0000947B0257}");
                EventInformer.SpeakAsync("Opening Window Switcher");
            }
            catch
            {
                EventInformer.SpeakAsync("Error: Your Operating System does not support this feature");
                MessageBox.Show("Error: Your O/S does not support this feature", "Unsupported O/S");
            }
        }
        
        private void recent_Click(object sender, RoutedEventArgs e)
        {
            EventInformer.SpeakAsyncCancelAll();
            try
            {
                Process.Start("shell:::{22877a6d-37a1-461a-91b0-dbda5aaebc99}");
                EventInformer.SpeakAsync("Opening Recent Places");
            }
            catch
            {
                EventInformer.SpeakAsync("Error: Your Operating System does not support this feature");
                MessageBox.Show("Error: Your O/S does not support this feature", "Unsupported O/S");
            }
        }

        //Shortcuts to various services are launched here
        private void ShortcutApps_Click(object sender, MouseButtonEventArgs e)
        {
            EventInformer.SpeakAsyncCancelAll();
            try
            {
                switch ((sender as Image).Name)
                {
                    case "MyComputer":
                        Process.Start("::{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
                        EventInformer.SpeakAsync("Opening My Computer");
                        break;
                    case "Facebook":
                        Process.Start("https://www.facebook.com/");
                        EventInformer.SpeakAsync("Opening Facebook.com");
                        break;
                    case "WindowsMediaPlayer":
                        Process.Start("mplayer2.exe");
                        EventInformer.SpeakAsync("Opening Windows Media Player");
                        break;
                    case "RecycleBin":
                        Process.Start("::{645FF040-5081-101B-9F08-00AA002F954E}");
                        EventInformer.SpeakAsync("Opening Recycle Bin");
                        break;
                    case "AdobeReader":
                        Process.Start("AcroRd32.exe");
                        EventInformer.SpeakAsync("Opening Adobe Reader");
                        break;
                    case "WinStore":
                        Process.Start(new ProcessStartInfo("ms-windows-store:"));
                        EventInformer.SpeakAsync("Opening Windows App Store");
                        break;
                    case "People":
                        Process.Start(new ProcessStartInfo("wlpeople:"));
                        EventInformer.SpeakAsync("Opening People App");
                        break;
                    case "Mail":
                        Process.Start("mailto:");
                        EventInformer.SpeakAsync("Opening Mail");
                        break;
                    case "Chat":
                        Process.Start(new ProcessStartInfo("mschat:"));
                        EventInformer.SpeakAsync("Opening Microsoft Chat");
                        break;
                    case "Videos":
                        Process.Start(new ProcessStartInfo("microsoftvideo:"));
                        EventInformer.SpeakAsync("Opening Microsoft Video");
                        break;
                    case "MicrosoftMusic":
                        Process.Start(new ProcessStartInfo("microsoftmusic:"));
                        EventInformer.SpeakAsync("Opening Microsoft Music");
                        break;
                    case "Libraries":
                        Process.Start("shell:::{031E4825-7B94-4dc3-B131-E946B44C8DD5}");
                        EventInformer.SpeakAsync("Opening Libraries");
                        break;
                }
            }
            catch
            {
                EventInformer.SpeakAsync("Oops, there seems to be a problem with your request. Please check whether you have the app or try reinstalling it again");
                Process.Start("Error: \n\t1. This works only with Windows 8 and above \n\t2. You might have uninstalled the App", "App Not Found");
            }
        }

        //Search Widgets launchers
        private void SearchAppsClick(object sender, MouseButtonEventArgs e)
        {
            EventInformer.SpeakAsyncCancelAll();
            switch((sender as Image).Name)
            {
                case "WindowsSearch":
                    {
                        string SearchTerm = " ";
                        int SearchTermLetterCount = 0;
                        foreach (char i in AddressBar.Text)
                        {
                            if (SearchTerm == " ")
                            {
                                SearchTerm = i.ToString();
                            }
                            else
                            {
                                if (AddressBar.Text[SearchTermLetterCount] != ' ')
                                {
                                    SearchTerm += i.ToString();
                                }
                                else
                                {
                                    SearchTerm += "%20";
                                }
                            }
                            SearchTermLetterCount++;
                        }
                        if (AddressBar.Text == "Browse or Search" || AddressBar.Text == "")
                        {
                            EventInformer.SpeakAsync("Opening Microsoft Search");
                            Process.Start("search-ms:");
                        }
                        else
                        {
                            EventInformer.SpeakAsync("Searching for " + AddressBar.Text + " in Explorer Window");
                            Process.Start("search-ms:displayname=Search%20Results%20in%20Computer&crumb=System.Generic.String%3A(" + SearchTerm + ")&crumb=location:%3A%3A{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
                            AddressBarHistorySourceUpdator(AddressBar.Text);
                        }
                    }
                    break;
                case "WikipediaSearch":
                    {
                        if (AddressBar.Text == "Browse or Search" || AddressBar.Text == "")
                        {
                            EventInformer.SpeakAsync("Opening Wikipedia");
                            Process.Start("https://en.WikipediaSearch.org");
                        }
                        else
                        {
                            string WikiSearchNameString = "";
                            foreach (char WikiSearchStringCharacter in AddressBar.Text)
                            {
                                if (WikiSearchStringCharacter != ' ')
                                {
                                    WikiSearchNameString += WikiSearchStringCharacter;
                                }
                                else
                                {
                                    WikiSearchNameString += '+';
                                }
                            }
                            EventInformer.SpeakAsync("Searching  " + AddressBar.Text + "in Wikipedia");
                            Process.Start("http://en.WikipediaSearch.org/wiki/Special:Search?search=" + WikiSearchNameString + "&go=Go");
                            AddressBarHistorySourceUpdator(AddressBar.Text);
                        }
                        AddressBar.Text = "Browse or Search";
                        AddressBar.FontStyle = FontStyles.Italic;
                    }
                    break;
                case "WebsiteLauncher":
                    {
                        ExplorerSearch();
                    }
                    break;
                case "google":
                    {
                        GoogleSearch();
                    }
                    break;
            }
        }

        private void ExplorerSearch()
        {
            EventInformer.SpeakAsyncCancelAll();
            bool IsAValidURL = false;
            int NumberOfFullStops = 0;
            if (AddressBar.Text == "Browse or Search" || AddressBar.Text == "")
            {
                Process.Start("iexplore.exe");
                EventInformer.SpeakAsync("Opening Internet Explorer");
            }
            else
            {
                foreach (char CharacterOfOriginalURL in AddressBar.Text)
                {
                    if (CharacterOfOriginalURL == '.')
                    {
                        NumberOfFullStops++;
                        if (string.Compare(AddressBar.Text.ToString(), 0, "http://", 0, 7, true) != 0 && string.Compare(AddressBar.Text, 0, "https://", 0, 8, true) != 0)
                        {
                            if ((NumberOfFullStops == 1 && (string.Compare(AddressBar.Text.ToString(), 0, "www", 0, 3, true) != 0) || (NumberOfFullStops == 2)))
                            {
                                Process.Start("http://" + AddressBar.Text.ToString().ToLower());
                                EventInformer.SpeakAsync("Opening " + AddressBar.Text.ToString().ToLower());
                                IsAValidURL = true;
                                AddressBarHistorySourceUpdator(AddressBar.Text);
                                break;
                            }
                        }
                        else
                        {
                            if ((string.Compare(AddressBar.Text.ToString(), 0, "http://www", 0, 10, true) != 0 && string.Compare(AddressBar.Text.ToString(), 0, "https://www", 0, 12, true) != 0) || NumberOfFullStops == 2)
                            {
                                Process.Start(AddressBar.Text.ToString().ToLower());
                                EventInformer.SpeakAsync("Opening " + AddressBar.Text.ToString().ToLower());
                                IsAValidURL = true;
                                AddressBarHistorySourceUpdator(AddressBar.Text);
                                break;
                            }
                        }
                    }
                }
                if (IsAValidURL == false)
                {
                    EventInformer.SpeakAsync("This Entry seems to be invalid");
                    MessageBox.Show("The URL \"" + AddressBar.Text + "\" seems to be invalid", "Error");
                }
            }
            AddressBar.Text = "Browse or Search";
            AddressBar.FontStyle = FontStyles.Italic;
        }

        private void GoogleSearch()
        {
            EventInformer.SpeakAsyncCancelAll();
            /*if (AddressBar.Text == "Browse or Search" || AddressBar.Text == "")
            {
                EventInformer.SpeakAsync("Opening Google.com");
                Process.Start("https://www.google.com/");
            }
            else
            {
                EventInformer.SpeakAsync("Searching  " + AddressBar.Text + "in Google.com");
                Process.Start("https://www.google.com/search?q=" + AddressBar.Text.ToString());
                AddressBarHistorySourceUpdator(AddressBar.Text);
            }
            AddressBar.Text = "Browse or Search";
            AddressBar.FontStyle = FontStyles.Italic;*/

            if (AddressBar.Text == "Browse or Search" || AddressBar.Text == "")
            {
                EventInformer.SpeakAsync("Opening Google.com");
                Process.Start("https://www.google.com/");
            }
            else
            {
                EventInformer.SpeakAsync("Searching  " + AddressBar.Text + "in Google.com");
                string SearchQuery = "";
                int SearchQueryLength = AddressBar.Text.Length;
                for (int i = 0; i < SearchQueryLength; i++)
                {
                    if (AddressBar.Text[i] != ' ')
                    {
                        SearchQuery += AddressBar.Text[i].ToString();
                    }
                    else
                    {
                        SearchQuery += '+';
                    }
                }
                Process.Start("https://www.google.com/search?q=" + SearchQuery);
                AddressBarHistorySourceUpdator(AddressBar.Text);
            }
            AddressBar.Text = "Browse or Search";
            AddressBar.FontStyle = FontStyles.Italic;
        }


        //Various apps by Kappspot is launched using the following code
        private void KappspotApps_Click(object sender, MouseButtonEventArgs e)
        {
            EventInformer.SpeakAsyncCancelAll();
            try
            {
                switch ((sender as Image).Name)
                {
                    //Various apps by Kappspot is launched using the following code
                    case "AnalogClock":
                        EventInformer.SpeakAsync("Opening Analog Clock");
                        new Window5().Show();
                        break;
                    case "DigitalClock":
                        EventInformer.SpeakAsync("Opening Digital Clock");
                        new ClockWindow().Show();
                        break;
                    case "CalanderWidget":
                        EventInformer.SpeakAsync("Opening Calender");
                        new Window2().Show();
                        break;
                    case "SearchWidget":
                        EventInformer.SpeakAsync("Opening Search widget");
                        new Window1().Show();
                        break;
                    case "NotesTaker":
                        EventInformer.SpeakAsync("Opening Notes Taker");
                        new Window3().Show();
                        break;
                    //case "DropObject":
                    //    EventInformer.SpeakAsync("Opening Drop and Save Agent");
                    //    new Window7().Show();
                    //    break;
                    case "PaintWidget":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\PaintWidget\";
                            App.FileName = "PaintWidget.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Opening Express Stroke");
                            EventInformer.SpeakAsync("Take notes or draw");
                        }
                        break;
                    case "CodeGen":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\CodeGen\";
                            App.FileName = "CodeGen.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Opening CodeGen");
                            EventInformer.SpeakAsync("Just enter the menu you want for your menu driven program to get the template code");
                        }
                        break;
                    case "Jumper":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\Jumper\";
                            App.FileName = "Jumper.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Opening jumper");
                            EventInformer.SpeakAsync("Use space bar to jump and save your life");
                        }
                        break;
                    case "PianoTiles":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\TapTheTiles\";
                            App.FileName = "TapTheTiles.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Tap the black tiles or use the number keys 1 2 3 4");
                        }
                        break;
                    case "_2048":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\2048\";
                            App.FileName = "2048.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Opening 2 0 4 8 Gaming Window");
                            EventInformer.SpeakAsync("Use the arrow keys to move the tiles in the required direction");
                        }
                        break;
                    case "WordamentSolver":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\WordamentSolver\";
                            App.FileName = "WordamentSolver.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Opening Wordament Solver");
                        }
                        break;
                    case "HangMan":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\Hang Man\";
                            App.FileName = "Hang Man.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Opening Hang Man");
                            EventInformer.SpeakAsync("Use the clue to find the correct word within the given number of chances");
                        }
                        break;
                    case "FlappyBird":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\FlappyBird\";
                            App.FileName = "FlappyBird.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Opening Flappy Bird");
                            EventInformer.SpeakAsync("Use space bar or tap the screen to fly up");
                        }
                        break;
                    case "Shooting":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\ShootingGame\";
                            App.FileName = "ShootingGame.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Opening shooter game");
                            EventInformer.SpeakAsync("Tap the balls to shoot them");
                        }
                        break;
                    case "SnakeLadders":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\Snake and Ladders\";
                            App.FileName = "SnakeAndLadders.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Play the classical Snake and ladders game");
                        }
                        break;
                    case "PhotoFrame":
                        {
                            ProcessStartInfo App = new ProcessStartInfo();
                            App.WorkingDirectory = Environment.CurrentDirectory + @"\Resources\Apps\PictureSlide\";
                            App.FileName = "PictureSlide.exe";
                            Process.Start(App);
                            EventInformer.SpeakAsync("Opening Picture frame the Photo album");
                        }
                        break;
                    case "Boxo":
                        {
                            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Kappspot\Boxo The Explorer"))
                            {
                                ProcessStartInfo App = new ProcessStartInfo();
                                App.WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + @"\Kappspot\Boxo The Explorer\";
                                App.FileName = "Boxo The Explorer.exe";
                                Process.Start(App);
                                EventInformer.SpeakAsync("Opening boxoo the Explorer");
                            }
                            else
                            {
                                EventInformer.SpeakAsync("Please! download and install Boxo the explorer");
                                Process.Start("http://kappspot.yolasite.com/");
                            }
                        }
                        break;
                }
            }
            catch
            {
                EventInformer.SpeakAsync("Oops! it seems that the application is not avilable anymore.. Please reinstall to repair this problem");
            }
        }


        /*This is used to clear the data files of their content and restore it to their Factory set state*/
        private void ClearHistory_Click(object sender, MouseButtonEventArgs e)
        {
            EventInformer.SpeakAsyncCancelAll();
            EventInformer.SpeakAsync("K app spot has cleared you user history");

            //FileStream FileToBeCleared = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\FileHistory.kappspot", FileMode.Truncate, FileAccess.Write);
            //FileToBeCleared.Close();

            FileStream FileToBeCleared = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\history_data_provider.kappspot", FileMode.Truncate, FileAccess.Write);
            FileToBeCleared.Close();

            FileToBeCleared = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\kappspot_notes.kappspot", FileMode.Truncate, FileAccess.Write);
            FileToBeCleared.Close();

            FileToBeCleared = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\SearchHistory.kappspot", FileMode.Truncate, FileAccess.Write);
            FileToBeCleared.Close();

            FileToBeCleared = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\Log.txt", FileMode.Truncate, FileAccess.Write);
            StreamWriter Writer = new StreamWriter(FileToBeCleared);
            Writer.WriteLine("\n\n\n-----------------------------------------------------------------------");
            Writer.WriteLine("|   PLAYER 1 | SCORE |   PLAYER 2 | SCORE | TURNS |   GAME STATISTICS |");
            Writer.WriteLine("-----------------------------------------------------------------------");
            Writer.Flush();
            FileToBeCleared.Close();

            MessageBox.Show("All your User Data has been Cleared from the Kappspot Cache. . .", "History Cleared Notification");
        }

        ////Files dropped into to grabber are stored and retrived and opened here
        //private void FileHistory_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    FileStream DataProvider = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\FileHistory.kappspot", FileMode.Open, FileAccess.Read);
        //    StreamReader Reader = new StreamReader(DataProvider);
        //    while (Reader.EndOfStream != true)
        //    {
        //        FileHistory.Items.Add(Reader.ReadLine());
        //    }
        //    Reader.Close();
        //    DataProvider.Close();
        //}

        //private void FileHistory_KeyUp(object sender, KeyEventArgs e)
        //{
        //    FileHistory.FontStyle = FontStyles.Normal;
        //    if (e.Key == Key.Enter)
        //    {
        //        try
        //        {
        //            Process.Start(FileHistory.Text);
        //            System.IO.FileStream DataProvider = new System.IO.FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\FileHistory.kappspot", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
        //            System.IO.StreamReader Reader = new System.IO.StreamReader(DataProvider);
        //            bool FilePathAlreadyExists = false;
        //            while (Reader.EndOfStream != true)
        //            {
        //                if (Reader.ReadLine() == FileHistory.Text)
        //                {
        //                    FilePathAlreadyExists = true;
        //                }
        //            }
        //            Reader.Close();
        //            DataProvider.Close();
        //            if (FilePathAlreadyExists == false)
        //            {
        //                DataProvider = new System.IO.FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\FileHistory.kappspot", System.IO.FileMode.Append, System.IO.FileAccess.Write);
        //                System.IO.StreamWriter Writer = new System.IO.StreamWriter(DataProvider);
        //                Writer.WriteLine(FileHistory.Text);
        //                Writer.Flush();
        //                DataProvider.Close();
        //            }
        //            EventInformer.SpeakAsync("Your request has been processed successfully.");
        //        }
        //        catch
        //        {
        //            EventInformer.SpeakAsync("Unable to Process your request. Please, check whether it is a valid file path");
        //            MessageBox.Show("Unable to Open the Requested File. . . \n\n  1. Check whether you have a valid application to open your File \n\n  2. Check whether you have deleted the file", "Notification: Unable to Open");
        //        }
        //        FileHistory.Text = "Select an Item to Open";
        //        FileHistory.FontStyle = FontStyles.Italic;
        //    }
        //}


        /// <summary>
        /// The following code is utilized to give the user a fluid search or browse experience
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddressBar_KeyUp(object sender, KeyEventArgs e)
        {
            AddressBar.FontStyle = FontStyles.Normal;
            if (e.Key != Key.Enter && e.Key != Key.Down)
            {
                AddressBarHistory.Items.Clear();
                FileStream DataProvider = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\history_data_provider.kappspot", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader Reader = new StreamReader(DataProvider);
                while (Reader.EndOfStream != true)
                {
                    string TempHolder = Reader.ReadLine();
                    /*for (int i = 0; i <= TempHolder.Length - AddressBar.Text.Length; i++)
                    {
                        if (string.Compare(AddressBar.Text, 0, TempHolder, i, AddressBar.Text.Length, true) == 0 || AddressBar.Text == "Browse or Search" || AddressBar.Text.Length == 0)
                        {
                            AddressBarHistory.Items.Add(TempHolder);
                            break;
                        }
                    }*/
                    if (string.Compare(AddressBar.Text, 0, TempHolder, 0, AddressBar.Text.Length, true) == 0 || AddressBar.Text == "Browse or Search" || AddressBar.Text.Length == 0)
                    {
                        AddressBarHistory.Items.Add(TempHolder);
                    }
                }
                Reader.Close();
                DataProvider.Close();
                if(AddressBarHistory.Items.Count == 0)
                {
                    AddressBarHistory.IsDropDownOpen = false;
                }
                else
                {
                    if (e.Key != Key.Escape)
                    {
                        AddressBarHistory.IsDropDownOpen = true;
                    }
                    else
                    {
                        AddressBarHistory.IsDropDownOpen = false;
                    }
                }
            }
            else
            {
                if (e.Key == Key.Down)
                {
                    AddressBarHistory.Focus();
                    AddressBarHistory.IsDropDownOpen = true;
                    if(AddressBarHistory.SelectedIndex > -1)
                    {
                        AddressBarHistory.ToolTip = AddressBarHistory.Text.ToString();
                    }
                }

                if(e.Key == Key.Enter)
                {
                    bool IsAddressBarEntryUrl = false;
                    int AddressBarLetterCount = 0;

                    AddressBarHistory.IsDropDownOpen = false;
                    
                    foreach (char AddressBarCharacter in AddressBar.Text)
                    {
                        if (AddressBarCharacter == '.' || AddressBarCharacter == ':')
                        {
                            IsAddressBarEntryUrl = true;
                        }
                        AddressBarLetterCount++;
                    }

                    if (AddressBarLetterCount > 0)
                    {
                        if (IsAddressBarEntryUrl)
                        {
                            ExplorerSearch();
                        }
                        else
                        {
                            GoogleSearch();
                        }
                    }
                    AddressBar.Text = "Browse or Search";
                }
            }
        }

        private void AddressBarHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddressBarHistory.SelectedIndex != -1)
            {
                AddressBar.Text = AddressBarHistory.SelectedItem.ToString();
                AddressBar.Focus();
                //AddressBar.SelectAll();
                AddressBar.Select(AddressBar.Text.Length, 0);
            }
        }

        public void AddressBarHistorySourceUpdator(string AddressBarEntry)
        {
            bool IsEntryAlreadyPresent = false;
            FileStream DataProvider = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\history_data_provider.kappspot", FileMode.Open, FileAccess.ReadWrite);
            StreamReader Reader = new StreamReader(DataProvider);
            while (Reader.EndOfStream != true)
            {
                if (AddressBarEntry.ToUpper() == Reader.ReadLine().ToUpper())
                {
                    IsEntryAlreadyPresent = true;
                    break;
                }
            }
            if (IsEntryAlreadyPresent != true && AddressBarEntry.Length > 0)
            {
                AddressBarHistory.Items.Add(AddressBarEntry);
                StreamWriter writer = new StreamWriter(DataProvider);
                writer.WriteLine(AddressBarEntry);
                writer.Flush();
            }
            DataProvider.Close();
        }

        private void AddressBar_GotFocus(object sender, RoutedEventArgs e)
        {
            if(AddressBar.Text == "Browse or Search")
            {
                AddressBar.Clear();
            }
        }

        private void AddressBarHistory_DropDownOpened(object sender, EventArgs e)
        {
            if(AddressBar.Text.Length == 0 || AddressBar.Text == "Browse or Search")
            {
                AddressBarHistory.Items.Clear();
                FileStream DataProvider = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\history_data_provider.kappspot", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader Reader = new StreamReader(DataProvider);
                while(Reader.EndOfStream != true)
                {
                    AddressBarHistory.Items.Add(Reader.ReadLine());
                }
            }
        }

        private void AddressBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(AddressBar.Text == "Browse or Search")
            {
                AddressBar.FontStyle = FontStyles.Italic;
            }
            else
            {
                AddressBar.FontStyle = FontStyles.Normal;
            }
        }

        private void Logo_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventInformer.SpeakAsyncCancelAll();
            EventInformer.SpeakAsync("You are being redirected to K App Spot");
            Process.Start("http://kappspot.yolasite.com/");
        }

        //private void widget_MouseEnter(object sender, MouseEventArgs e)
        // {
        //     widget.Opacity = 1;
        // }

        // private void widget_MouseLeave(object sender, MouseEventArgs e)
        // {
        //     if (got_mouse_focus && can_enable_minimize_button)
        //     {
        //         if (FileHistory.SelectedIndex > -1)
        //         {
        //             Process.Start(FileHistory.SelectedItem.ToString());
        //         }
        //         if (window_expansion_state == true)
        //         {
        //             kappspot_productions_Click(sender, e);
        //         }
        //         t = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Send);
        //         t.Interval = TimeSpan.FromMilliseconds(5);
        //         t.Tick += t_Tick;
        //         got_mouse_focus = false;
        //         t.Start();
        //     }
        // }

        // void t_Tick(object sender, EventArgs e)
        // {
        //     if (widget.got_mouse_focus == false)
        //     {
        //         if (widget.Width <= 100)
        //         {
        //             t.IsEnabled = false;
        //             Window IdleWindow = new Window7();
        //             IdleWindow.Left = this.Left;
        //             IdleWindow.Top = this.Top;
        //             IdleWindow.Show();
        //             this.Close();
        //         }
        //         else
        //         {
        //             widget.Width -= 10;
        //         }
        //     }
        // }

        // private void widget_Loaded(object sender, RoutedEventArgs e)
        // {
        //     widget.Width = 0;
        //     s = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Send);
        //     s.Interval = TimeSpan.FromMilliseconds(5);
        //     s.Tick += s_Tick;
        //     s.Start();
        // }

        // void s_Tick(object sender, EventArgs e)
        // {
        //     if (widget.got_mouse_focus == true)
        //     {
        //         if (widget.Width >= 514)
        //         {
        //             widget.Width = 514;
        //             s.Stop();
        //         }
        //         else
        //         {
        //             widget.Width += 10;
        //         }
        //     }
        // }

        // private void kappspot_productions_Click(object sender, RoutedEventArgs e)
        // {
        //     if (can_window_size_change)
        //     {
        //         can_window_size_change = false;
        //         window_expander = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Send);
        //         window_expander.Interval = TimeSpan.FromMilliseconds(15);
        //         window_expander.Tick += window_expander_Tick;
        //         window_expander.Start();
        //     }
        // }

        // void window_expander_Tick(object sender, EventArgs e)
        // {
        //     if (window_expansion_state)
        //     {
        //         if (this.Height > 158)
        //         {
        //             this.Height -= 10;
        //             return;
        //         }
        //         else
        //         {
        //             this.Height = 158;
        //             window_expander.Stop();
        //             can_window_size_change = true;
        //             window_expansion_state = false;
        //             return;
        //         }
        //     }
        //     else
        //     {
        //         if (this.Height < 262)
        //         {
        //             this.Height += 10;
        //             return;
        //         }
        //         else
        //         {
        //             this.Height = 262;
        //             window_expander.Stop();
        //             can_window_size_change = true;
        //             window_expansion_state = true;
        //             return;
        //         }
        //     }
        // }*/

        // private void url_GotFocus(object sender, RoutedEventArgs e)
        // {
        //     if (AddressBar.Text == "Browse or Search" || AddressBar.Text == "")
        //     {
        //         AddressBar.Text = "";
        //         AddressBar.FontStyle = FontStyles.Normal;
        //         SearchTerm = AddressBar.Text;
        //         AddressBar.Foreground = Brushes.Black;
        //     }
        // }

        // private void url_KeyDown(object sender, KeyEventArgs e)
        // {
        //     string search_or_url = AddressBar.Text;
        //     bool is_url = false;
        //     int wordcount = 0;
        //     if (e.Key == Key.Enter && AddressBar.Text != "Browse or Search")
        //     {
        //         AddressBarHistory.IsDropDownOpen = false;
        //         AddressBarHistorySourceUpdator(search_or_url);
        //         foreach (char i in search_or_url)
        //         {
        //             if (i == '.' || i == ':')
        //             {
        //                 is_url = true;
        //             }
        //             wordcount++;
        //         }
        //         if (wordcount > 0)
        //         {
        //             if (is_url)
        //             {
        //                 explorer_Click(sender, e);
        //             }
        //             else
        //             {
        //                 google_Click(sender, e);
        //             }
        //         }
        //         AddressBar.Text = "Browse or Search";
        //         AddressBar.FontStyle = FontStyles.Italic;
        //         AddressBar.Text = "";
        //     }
        //     else
        //     {
        //         if ((e.Key == Key.BrowserBack || e.Key == Key.Delete || e.Key == Key.A || e.Key == Key.B || e.Key == Key.C || e.Key == Key.D || e.Key == Key.E || e.Key == Key.F || e.Key == Key.G || e.Key == Key.H || e.Key == Key.I || e.Key == Key.J || e.Key == Key.K || e.Key == Key.L || e.Key == Key.M || e.Key == Key.N || e.Key == Key.O || e.Key == Key.P || e.Key == Key.Q || e.Key == Key.R || e.Key == Key.S || e.Key == Key.T || e.Key == Key.U || e.Key == Key.V || e.Key == Key.W || e.Key == Key.X || e.Key == Key.Y || e.Key == Key.Z || e.Key == Key.D0 || e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4 || e.Key == Key.D5 || e.Key == Key.D5 || e.Key == Key.D6 || e.Key == Key.D7 || e.Key == Key.D8 || e.Key == Key.D9) && AddressBar.Text.Length > 0)
        //         {
        //             AddressBarHistory.Items.Clear();
        //             length = AddressBar.Text.Length;
        //             FileStream DataProvider = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\history_data_provider.kappspot", FileMode.OpenOrCreate, FileAccess.Read);
        //             StreamReader Reader = new StreamReader(DataProvider);
        //             while ((Reader.EndOfStream != true))
        //             {
        //                 string temp_string = Reader.ReadLine();
        //                 for (int i = 0; i <= temp_string.Length - search_or_url.Length; i++)
        //                 {
        //                     if (string.Compare(search_or_url, 0, temp_string, i, search_or_url.Length, true) == 0 || search_or_url.Length == 0)
        //                     {
        //                         AddressBarHistory.Items.Add(temp_string);
        //                         break;
        //                     }
        //                 }
        //             }
        //             if(AddressBarHistory.HasItems)
        //             {
        //                AddressBarHistory.IsDropDownOpen = true;
        //             }
        //             else
        //             {
        //                AddressBarHistory.IsDropDownOpen = false;
        //             }
        //             Reader.Close();
        //             DataProvider.Close();
        //         }
        //     }
        //     if (e.Key == Key.Up || e.Key == Key.Down && AddressBarHistory.HasItems)
        //     {
        //         AddressBarHistory.SelectedIndex = 0;
        //         AddressBarHistory.Focus();
        //         AddressBar.IsEnabled = false;
        //     }
        // }


        // private void url_text_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        // {
        //     AddressBar.IsEnabled = true;
        //     AddressBar.Focus();
        //     AddressBarHistory.IsDropDownOpen = false;
        // }

        // private void url_SelectionChanged(object sender, SelectionChangedEventArgs e)
        // {
        //     if (AddressBarHistory.SelectedIndex > -1)
        //     {
        //         AddressBar.Text = AddressBarHistory.SelectedItem.ToString();
        //         AddressBarHistory.Text = "";
        //     }
        // }

        // private void url_KeyUp_1(object sender, KeyEventArgs e)
        // {
        //     AddressBar.Text = "";
        //     if(e.Key == Key.Escape || e.Key == Key.Enter || e.Key == Key.Left || e.Key == Key.Right)
        //     {
        //         AddressBar.IsEnabled = true;
        //         AddressBar.Foreground = Brushes.Black;
        //         AddressBar.Focus();
        //     }
        // }*/

        ////The various apps, utilities and pages by Microsoft and other publishers are launched here
        //private void my_computer_Click(object sender, MouseButtonEventArgs e)
        //{
        //    System.Diagnostics.Process.Start("::{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
        //    EventInformer.SpeakAsync("Opening My Computer");
        //}

        //private void fb_Click(object sender, MouseButtonEventArgs e)
        //{
        //    Process.Start("https://www.facebook.com/");
        //    EventInformer.SpeakAsync("Opening Facebook.com");
        //}

        //private void windows_media_player_Click(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Process.Start("mplayer2.exe");
        //        EventInformer.SpeakAsync("Opening Windows Media Player");
        //    }
        //    catch
        //    {
        //        EventInformer.SpeakAsync("Please Check Whether you have the latest version of windows media player installed on your computer");
        //        MessageBox.Show("Error: Check whether you have the latest version of Windows Media Player", "Media Player Not Found");
        //    }
        //}

        //private void recycle_bin_Click(object sender, MouseButtonEventArgs e)
        //{
        //    Process.Start("::{645FF040-5081-101B-9F08-00AA002F954E}");
        //    EventInformer.SpeakAsync("Opening Recycle Bin");
        //}

        //private void Adobe_reader_Click(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Process.Start("AcroRd32.exe");
        //        EventInformer.SpeakAsync("Opening Adobe Reader");
        //    }
        //    catch
        //    {
        //        EventInformer.SpeakAsync("Check whether you have adobe reader installed on your system");
        //        MessageBox.Show("Error: \n\t1. Check whether you have installed Adobe Reader \n\t2. If not download from https://www.filehippo.com/", "Adobe Reader Not Found");
        //    }
        //}

        //private void ms_store_Click(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Process.Start("ms-windows-store:");
        //        EventInformer.SpeakAsync("Opening Windows App Store");
        //    }
        //    catch
        //    {
        //        EventInformer.SpeakAsync("Please Check whether you have windows 8 or above");
        //        Process.Start("Error: This works only with Windows 8 and above", "Unsupported O/S");
        //    }
        //}

        //private void people_Click(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Process.Start("wlpeople:");
        //        EventInformer.SpeakAsync("Opening People App");
        //    }
        //    catch
        //    {
        //        EventInformer.SpeakAsync("Error You might have uninstalled the App or You don't have an operating system that supports this app");
        //        Process.Start("Error: \n\t1. This works only with Windows 8 and above \n\t2. You might have uninstalled the App", "App Not Found");
        //    }
        //}

        //private void mail_Click(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Process.Start("mailto:");
        //        EventInformer.SpeakAsync("Opening Mail");
        //    }
        //    catch
        //    {
        //        EventInformer.SpeakAsync("Error: You might have uninstalled the App or You don't have an operating system that supports this app");
        //        Process.Start("Error: \n\t1. This works only with Windows 8 and above \n\t2. You might have uninstalled the App", "App Not Found");
        //    }
        //}

        //private void chat_Click(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Process.Start("mschat:");
        //        EventInformer.SpeakAsync("Opening Microsoft Chat");
        //    }
        //    catch
        //    {
        //        EventInformer.SpeakAsync("Error: You might have uninstalled the App or You don't have an operating system that supports this app");
        //        Process.Start("Error: \n\t1. This works only with Windows 8 and above \n\t2. You might have uninstalled the App", "App Not Found");
        //    }
        //}

        //private void videos_Click(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Process.Start("microsoftvideo:");
        //        EventInformer.SpeakAsync("Opening Microsoft Video");
        //    }
        //    catch
        //    {
        //        EventInformer.SpeakAsync("Error: You might have uninstalled the App or You don't have an operating system that supports this app");
        //        Process.Start("Error: \n\t1. This works only with Windows 8 and above \n\t2. You might have uninstalled the App", "App Not Found");
        //    }
        //}

        //private void music_Click(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Process.Start("microsoftmusic:");
        //        EventInformer.SpeakAsync("Opening Microsoft Music");
        //    }
        //    catch
        //    {
        //        EventInformer.SpeakAsync("Error: You might have uninstalled the app or you have an Unsupported Operating System");
        //        Process.Start("Error: \n\t1. This works only with Windows 8 and above \n\t2. You might have uninstalled the App", "App Not Found");
        //    }
        //}

        //private void library_Click(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        Process.Start("shell:::{031E4825-7B94-4dc3-B131-E946B44C8DD5}");
        //        EventInformer.SpeakAsync("Opening Libraries");
        //    }
        //    catch
        //    {
        //        EventInformer.SpeakAsync("Error: Unsupported Operating System");
        //        MessageBox.Show("Error: Unsupported O/S. . .Please install Windows 8 or above", "O/S Unsupported");
        //    }
        //}
        //private void start_menu_search_Click(object sender, MouseButtonEventArgs e)
        //{
        //    string SearchTerm = " ";
        //    int SearchTermLetterCount = 0;
        //    foreach (char i in AddressBar.Text)
        //    {
        //        if (SearchTerm == " ")
        //        {
        //            SearchTerm = i.ToString();
        //        }
        //        else
        //        {
        //            if (AddressBar.Text[SearchTermLetterCount] != ' ')
        //            {
        //                SearchTerm += i.ToString();
        //            }
        //            else
        //            {
        //                SearchTerm += "%20";
        //            }
        //        }
        //        SearchTermLetterCount++;
        //    }
        //    if (AddressBar.Text == "Browse or Search" || AddressBar.Text == "")
        //    {
        //        EventInformer.SpeakAsync("Opening Microsoft Search");
        //        Process.Start("search-ms:");
        //    }
        //    else
        //    {
        //        EventInformer.SpeakAsync("Searching for " + AddressBar.Text + " in Explorer Window");
        //        Process.Start("search-ms:displayname=Search%20Results%20in%20Computer&crumb=System.Generic.String%3A(" + SearchTerm + ")&crumb=location:%3A%3A{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
        //        AddressBarHistorySourceUpdator(AddressBar.Text);
        //    }
        //}

        //private void wikipedia_Click(object sender, MouseButtonEventArgs e)
        //{
        //    if (AddressBar.Text == "Browse or Search" || AddressBar.Text == "")
        //    {
        //        EventInformer.SpeakAsync("Opening Wikipedia");
        //        Process.Start("https://en.WikipediaSearch.org");
        //    }
        //    else
        //    {
        //        string WikiSearchNameString = "";
        //        foreach (char WikiSearchStringCharacter in AddressBar.Text)
        //        {
        //            if (WikiSearchStringCharacter != ' ')
        //            {
        //                WikiSearchNameString += WikiSearchStringCharacter;
        //            }
        //            else
        //            {
        //                WikiSearchNameString += '+';
        //            }
        //        }
        //        EventInformer.SpeakAsync("Searching  " + AddressBar.Text + "in Wikipedia");
        //        Process.Start("http://en.WikipediaSearch.org/wiki/Special:Search?search=" + WikiSearchNameString + "&go=Go");
        //        AddressBarHistorySourceUpdator(AddressBar.Text);
        //    }
        //    AddressBar.Text = "Browse or Search";
        //    AddressBar.FontStyle = FontStyles.Italic;
        //}

        //private void explorer_Click(object sender, MouseButtonEventArgs e)
        //{
        //    ExplorerSearch();
        //}

        //private void google_Click(object sender, MouseButtonEventArgs e)
        //{
        //    GoogleSearch();
        //}

        //private void _2048_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening 2 0 4 8 Gaming Window");
        //    EventInformer.SpeakAsync("Use the arrow keys to move the tiles in the required direction");
        //    Process.Start(@"Resources\_2048.exe");
        //}

        //private void clock_analog_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening Analog Clock");
        //    new Window5().Show();
        //}

        //private void clock_digital_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening Digital Clock");
        //    new ClockWindow().Show();
        //}

        //private void calander_app_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening Calender");
        //    new Window2().Show();
        //}

        //private void search_app_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening K app spot Search");
        //    new Window1().Show();
        //}

        //private void notes_taker_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening Notes Taker");
        //    new Window3().Show();
        //}

        //private void wordament_app_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening Wordament Solver");
        //    EventInformer.SpeakAsync("Type the letters in the game linearly and hit enter button and select on a letter to list the combinations");
        //    ProcessStartInfo App = new ProcessStartInfo();
        //    App.WorkingDirectory = System.IO.Directory.GetCurrentDirectory() + @"\Resources";
        //    App.UseShellExecute = true;
        //    App.FileName = "wordament.exe";
        //    Process.Start(App);
        //}

        //private void paint_app_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening Express Stroke");
        //    EventInformer.SpeakAsync("Take notes or draw");
        //    Process.Start(@"Resources\Paint.exe");
        //}

        //private void Hang_man_app_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening Hang Man");
        //    EventInformer.SpeakAsync("Use the clue to find the correct word within the given number of chances");
        //    ProcessStartInfo App = new ProcessStartInfo();
        //    App.WorkingDirectory = System.IO.Directory.GetCurrentDirectory() + @"\Resources";
        //    App.UseShellExecute = true;
        //    App.FileName = "Hang_Man_Source.exe";
        //    Process.Start(App);
        //}

        //private void Flappy_Smiley_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening Flappy smiley");
        //    EventInformer.SpeakAsync("Use space bar to fly up");
        //    Process.Start(@"Resources\Flappy Smiley.exe");
        //}

        //private void Jumper_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening jumper");
        //    EventInformer.SpeakAsync("Use space bar to jump and save your life");
        //    Process.Start(@"Resources\JUMPER.exe");
        //}

        //private void Shooting_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening shooter game");
        //    EventInformer.SpeakAsync("Use w a s d to move and Space bar to shoot");
        //    Process.Start(@"Resources\shooting.exe", @"Resources\coord.txt");
        //}

        //private void CodeGen_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening CodeGen");
        //    EventInformer.SpeakAsync("Just enter the menu you want for your menu driven program to get the template code");
        //    Process.Start(@"Resources\CodeGen.exe");
        //}

        //private void DropObject_Click(object sender, MouseButtonEventArgs e)
        //{
        //    EventInformer.SpeakAsync("Opening Drop and Save Agent");
        //    new Window7().Show();
        //}
    }
}
