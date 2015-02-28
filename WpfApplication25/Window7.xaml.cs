using System;
using System.Windows;
using System.Windows.Input;

namespace WpfApplication25
{
    /// <summary>
    /// Interaction logic for Window7.xaml
    ///     - Can be used to drop files, executables and other folders
    ///     - Capable of opening such dropped files
    ///     - And then it saves the data into the KAppspot's Database
    /// </summary>
    public partial class Window7 : Window
    {
        /// <summary>
        /// Default Constructor
        ///     - Call InitializeComponent
        /// </summary>
        public Window7()
        {
            InitializeComponent();
        }

        /// <summary>
        /// This opens the files and other folders dropped into the window and also stores the successful logs
        ///     - Use the built-in Function to get the address of all the objects dropped into the window and store it in a string array
        ///     - foreach object in the dropped objects
        ///         - Create and Open the DataProvider and assign it to a reader
        ///             - Source    : Resources\FileHistory.kappspot
        ///             - Mode      : Open 
        ///             - Access    : ReadWrite
        ///         - Check whether the file already exists in the database or not
        ///             - Initially set FileAlreadyExists to be "false"
        ///             - If so then set FileAlreadyExists to "true"
        ///             - Close the Reader and the FileStream
        ///         - try and Open the Object
        ///             - Use Process.Start() to start the process
        ///             - If this succeeds and FileAlreadyExists is "false" 
        ///                 - Write the log into the datebase
        ///         - catch the execption
        ///             - Show the error message
        /// </summary>
        private void Window_Drop(object sender, DragEventArgs e)
        {
            string[] ObjectLocation = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string i in ObjectLocation)
            {
                System.IO.FileStream DataProvider = new System.IO.FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\FileHistory.kappspot", System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite);
                System.IO.StreamReader Reader = new System.IO.StreamReader(DataProvider);

                bool FileAlreadyExists = false;
                while (Reader.EndOfStream != true)
                {
                    if (Reader.ReadLine() == i)
                    {
                        FileAlreadyExists = true;
                    }
                }
                Reader.Close();
                DataProvider.Close();

                try
                {
                    System.Diagnostics.Process.Start(i);
                    if (FileAlreadyExists == false)
                    {
                        DataProvider = new System.IO.FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\FileHistory.kappspot", System.IO.FileMode.Append, System.IO.FileAccess.Write);
                        System.IO.StreamWriter writer = new System.IO.StreamWriter(DataProvider);
                        writer.WriteLine(i);
                        writer.Flush();
                        DataProvider.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to Open the Requested File. . . \n\n\tPlease Check whether you have a valid application to open your File", "Unable to Open");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            System.Speech.Synthesis.SpeechSynthesizer EventInformer = new System.Speech.Synthesis.SpeechSynthesizer();
            EventInformer.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Female);
            try
            {
                EventInformer.SpeakAsync("Opening Recent Places");
                System.Diagnostics.Process.Start("shell:::{22877a6d-37a1-461a-91b0-dbda5aaebc99}");
            }
            catch
            {
                EventInformer.SpeakAsync("Error: Your Operating System does not support this feature");
                MessageBox.Show("Error: Your O/S does not support this feature", "Unsupported O/S");
            }
        }

        /// <summary>
        /// This moves the window from one location to the other
        ///     - Use the built-in function DragMove()
        /// </summary>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
