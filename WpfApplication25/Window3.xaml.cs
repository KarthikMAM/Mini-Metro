using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApplication25
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    ///     - Can be used to take notes
    ///     - Saves the Notes being taken asynchronously
    ///     - Can read the Contents of the editor
    /// </summary>
    public partial class Window3 : Window
    {
        ///<summary>
        /// FileIsBeingLooded
        ///     DataType : bool
        ///     Used For : This is to check whether the file is being loaded or not
        /// NotesReader
        ///     DataType : SpeechSynthesizer
        ///     Used For : This is also to prevent the write operation into the file along with the load operation
        ///</summary>
        bool FileIsBeingLoaded;
        System.Speech.Synthesis.SpeechSynthesizer NotesReader;

        /// <summary>
        /// Default Constructor
        ///     - Call the InitializeComponent() function
        ///     - Create a TextToSpeech Object to Read the contents of the notes
        ///         - Voice : Female
        ///     - Add Purpose of the Widget window as the ToolTip
        ///     - Load the Save Data
        ///         - Indicate that the file is being loaded by setting FileIsBeingLoded to "true"
        ///         - Create and Open the Filestream and the Reader objects for the data source
        ///             - Source    : Resources\kappspot_notes.kappspot
        ///             - Mode      : OpenOrCreate
        ///             - Access    : Read
        ///         - If the File is not empty
        ///             - Read the File to the end and assign it to the text field of the NotesEditor
        ///         - Otherwise
        ///             - Set that Field to "Type Here"
        ///         - Close the Reader and the FileStream Objects
        ///     - Navigate to the last Letter of the Text field
        ///     - Indicate the the Load operation is complete to allow the write operations to proceed
        /// </summary>
        public Window3()
        {
            InitializeComponent();

            NotesReader = new System.Speech.Synthesis.SpeechSynthesizer();
            NotesReader.SelectVoiceByHints(System.Speech.Synthesis.VoiceGender.Female);

            this.ToolTip = "Note Anytime and relieve your stress";

            FileIsBeingLoaded = true;

            FileStream NotesDataFile = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\kappspot_notes.kappspot", FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader Reader = new StreamReader(NotesDataFile);

            if (Reader.EndOfStream != true)
            {
                NotesEditor.Text = Reader.ReadToEnd();
            }
            else
            {
                NotesEditor.Text = "Type Here";
            }
            Reader.Close();
            NotesDataFile.Close();

            NotesEditor.Select(NotesEditor.Text.Length,0);

            FileIsBeingLoaded = false;
        }

        /// <summary>
        /// Closes the window
        ///     - Stop all the Operations taking place
        ///     - Call the built-in function Close()
        /// </summary>
        private void close_Click(object sender, RoutedEventArgs e)
        {
            NotesReader.SpeakAsyncCancelAll();
            this.Close();
        }


        /// <summary>
        /// This drag moves the window
        ///     - Use the built-in function DragMove()
        /// </summary>
        private void notes_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        ///<summary>
        /// Resets the Contents of the NotesEditor
        ///     - Clear the text by calling the built-in function Clear()
        ///     - Set the default text "Type Here"
        ///     - Navigate to the last end of the text field
        ///     - Note : Since the content of the text box changes the new default text will automatically be saved and there is no need to explicitly define snippets to save the new content
        ///</summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NotesEditor.Clear();
            NotesEditor.Text = "Type Here";
            NotesEditor.Select(NotesEditor.Text.Length,0);
        }

        /// <summary>
        /// Writes data as and when the changes are made asynchronously
        ///     - If the File is not being loded
        ///         - Create and Open the NotesData and the Writer Objects
        ///         - Divert and write the data from the NotesEditor to the text File
        ///         - Close the Writer and the NotesData
        /// </summary>
        private void notes_editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            //The if condition is to prevent the execution of the block when the file is being loaded into the memory
            //This avoids the conflict between the read and the write operations
            if (!FileIsBeingLoaded)
            {
                FileStream NotesData = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\kappspot_notes.kappspot", FileMode.Truncate, FileAccess.Write);
                StreamWriter Writer = new StreamWriter(NotesData);

                Writer.WriteLine(NotesEditor.Text.ToString());

                Writer.Close();
                NotesData.Close();
            }
        }

        /// <summary>
        /// Start Reading the Contents of the NotesEidtor
        ///     - Call the SpeakAsync() built-in function
        /// </summary>
        private void Reader_Click(object sender, RoutedEventArgs e)
        {
            NotesReader.SpeakAsync(NotesEditor.Text);
        }

        /// <summary>
        /// Cancel all the Read Operations
        ///     - Call the SpeakAsyncCancelAll() built-in function
        /// </summary>
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            NotesReader.SpeakAsyncCancelAll();
        }
    }
}
