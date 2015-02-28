using System;
using System.Diagnostics;
using System.IO;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfApplication25
{

    /// <summary>
    /// Interaction logic for Window1.xaml
    ///     - Used to Manipulate the search string of the windows explorer
    ///     - Search by User's Preferance and History
    ///     - It is like a Customized Search Widget
    /// </summary>
    public partial class Window1 : Window
    {
        /// <summary>
        /// Default Constructor
        ///     - Call InitializeComponent()
        ///     - Let the ToolTip be the purpose of the widget
        ///     - Add the various options into their respective drop-down list boxes
        ///         - DateSpecs
        ///         - KindSpecs
        ///         - SizeSpecs
        /// </summary>
        public Window1()
        {
            InitializeComponent();

            this.ToolTip = "Search and Find at ease";

            DateSpecs.Items.Add("None");
            DateSpecs.Items.Add("Today");
            DateSpecs.Items.Add("Yesterday");
            DateSpecs.Items.Add("This week");
            DateSpecs.Items.Add("Last week");
            DateSpecs.Items.Add("This month");
            DateSpecs.Items.Add("Last month");
            DateSpecs.Items.Add("This year");
            DateSpecs.Items.Add("Last year");

            KindSpecs.Items.Add("None");
            KindSpecs.Items.Add("Calendar");
            KindSpecs.Items.Add("Communication");
            KindSpecs.Items.Add("Contact");
            KindSpecs.Items.Add("Documnet");
            KindSpecs.Items.Add("E-mail");
            KindSpecs.Items.Add("Feed");
            KindSpecs.Items.Add("Folder");
            KindSpecs.Items.Add("Game");
            KindSpecs.Items.Add("Instant Message");
            KindSpecs.Items.Add("Journal");
            KindSpecs.Items.Add("Link");
            KindSpecs.Items.Add("Movie");
            KindSpecs.Items.Add("Music");
            KindSpecs.Items.Add("Note");
            KindSpecs.Items.Add("Picture");
            KindSpecs.Items.Add("Playlist");
            KindSpecs.Items.Add("Program");
            KindSpecs.Items.Add("Recorded TV");
            KindSpecs.Items.Add("Saved Search");
            KindSpecs.Items.Add("Task");
            KindSpecs.Items.Add("Video");
            KindSpecs.Items.Add("Web History");

            SizeSpecs.Items.Add("None");
            SizeSpecs.Items.Add("Empty");
            SizeSpecs.Items.Add("Tiny");
            SizeSpecs.Items.Add("Small");
            SizeSpecs.Items.Add("Medium");
            SizeSpecs.Items.Add("Large");
            SizeSpecs.Items.Add("Huge");
            SizeSpecs.Items.Add("Gigantic");
        }

        /// <summary>
        /// Get the data from the user and search for the file in the explorer
        ///     - If the AddressBar is "Not Empty" or is equal to "Type Here"
        ///         - Create the Speaker Object that will inform the user of the search query
        ///             - Voice Gender : Female
        ///         - If TypeSpecs is in the correct format like ".mp3"
        ///             - Save the Search Object in the history database
        ///             - Inform the FileName using the SpeechSynthesizer object
        ///             - The default value for searching in my computer is set in the SearchString
        ///             - Convert the FileName to the required format 
        ///                 - Eg: karthik m a m is converted into (karthik%20m%20a%20m)
        ///                 - Let the default value in the loader be space
        ///                 - The initial number of characters is assumed to be zero
        ///                 - If the FileName is a valid name
        ///                     - foreach char i in the FileName
        ///                         - If the FileName holder is empty
        ///                             - If the Letter is not a space
        ///                                 - Add the Letter to the SearchObject
        ///                         - Otherwise
        ///                             - If the Letter is not a space
        ///                                 - Add the letter to the SearchObject
        ///                             - Otherwise
        ///                                 - Add "%20" which is a representation of the " "
        ///                 - Add the Paranthesis in the appropriate place
        ///                 - Append the SearchObject to the SearchString
        ///             - Convert the DateSpecs to the required format
        ///                 - Format : "%20datemodified%3A" + DateSpecsOption
        ///                 - If a valid option is selected
        ///                     - Append the DateSpecs to the SearchString
        ///                         - If there is a space inbetween remove them by using the special keyword "%20"
        ///                              - For each char in the DateSpecsOption
        ///                                 - If it is ' ' 
        ///                                     - Append "%20" to the SearchString
        ///                                 - Otherwise 
        ///                                     - Append that character to the SearchString
        ///                     - Inform the Option selected
        ///             - Convert the KindSpecs to the required format
        ///                 - Format : "%20kind%3A%3D" + KindSpecsOption
        ///                 - If a valid option is selected
        ///                     - Assign it to a temporary string
        ///                     - Append the KindSpecs format to the SearchString
        ///                     - Inform the Option selected
        ///                     - SpecialCases
        ///                         - Instant Message   = instant%20message
        ///                         - E-Mail            = email
        ///                         - Recorded TV       = recorded%20tv
        ///                         - Saved Search      = saved%20search
        ///                         - Web History       = web%20history
        ///                     - Convert all upper case letters to lower case and then append the search term
        ///             - Convert the SizeSpecs to the required format
        ///                 - Format : "%20datemodified%3A" + (SizeSpecsOption -> Equivalent size interms of bytes)
        ///                 - Store the Option in a string holder
        ///                 - If a valid option is selected
        ///                     - Inform the Option selected
        ///                     - Append the DateSpecs format to the SearchString
        ///                     - Convert and add the Option to the SearchString
        ///                 - If the type length is greater than "0" 
        ///                     - Append the Type format "%20type%3A" along with the user's choice
        ///                     - Inform the type entered
        ///                 - Append the last portion of the SearchString and using Process.Start() start the Explorer search i.e., like adding the GUID
        ///                 - Reset all the fields to their default values
        ///         - Otherwise
        ///             - Inform that the FileType is invalid
        /// </summary>
        private void Search_files_Click(object sender, RoutedEventArgs e)
        {
            if (AddressBar.Text != "Type here" && AddressBar.Text.Length > 0)
            {
                SpeechSynthesizer Speaker = new SpeechSynthesizer();
                Speaker.SelectVoiceByHints(VoiceGender.Female);

                if (TypeSpecs.Text == "" || TypeSpecs.Text[0] == '.')
                {
                    AddressBarHistorySourceUpdator(AddressBar.Text);

                    Speaker.SpeakAsync("You are searching for File Name : " + AddressBar.Text);

                    string SearchString = "displayname=Search%20Results%20in%20Computer&crumb=System.Generic.String%3A(";

                    string SearchObject = " ";
                    int count = 0;
                    if (AddressBar.Text != "Type Here" || AddressBar.Text.Length > 0)
                    {
                        foreach (char i in AddressBar.Text)
                        {
                            if (SearchObject == " ")
                            {
                                if (i != ' ')
                                {
                                    SearchObject = i.ToString();
                                }
                            }
                            else
                            {
                                if (i != ' ')
                                {
                                    SearchObject += i.ToString();
                                }
                                else
                                {
                                    SearchObject += "%20";
                                }
                            }
                            count++;
                        }
                    }
                    SearchString += SearchObject + ")";

                    if (DateSpecs.SelectedIndex != -1 && DateSpecs.SelectedItem.ToString() != "None")
                    {
                        string DateModified = DateSpecs.SelectedItem.ToString().ToLower();

                        Speaker.SpeakAsync("Date modified : " + DateModified);

                        SearchString += "%20datemodified%3A";
                       
                        if(DateModified[4] == ' ')
                        {

                            SearchString += DateModified.Substring(0, 4) + "%20" + DateModified.Substring(5);
                        }
                        else
                        {
                            SearchString += DateModified.ToLower();
                        }

                    }

                    if (KindSpecs.SelectedIndex != -1 && KindSpecs.SelectedItem.ToString() != "None")
                    {
                        string Kind = KindSpecs.SelectedItem.ToString();

                        Speaker.SpeakAsync("Kind : " + Kind);

                        SearchString += "%20kind%3A%3D";

                        if (Kind == "Instant Message")
                        {
                            SearchString += "instant%20message";
                        }
                        else if (Kind == "E-mail")
                        {
                            SearchString += "email";
                        }
                        else if (Kind == "Recorded TV")
                        {
                            SearchString += "recorded%20tv";
                        }
                        else if (Kind == "Saved Search")
                        {
                            SearchString += "saved%20search";
                        }
                        else if (Kind == "Web History")
                        {
                            SearchString += "web%20history";
                        }
                        else
                        {
                            SearchString += Kind.ToLower();
                        }
                    }


                    //This block's purpose is to get the option for the size of the file field and convert it into the respected AddressBar
                    //If no field is selected then there will be no addition to the AddressBar code
                    if (SizeSpecs.SelectedIndex != -1 && SizeSpecs.SelectedItem.ToString() != "None")
                    {
                        string Size = SizeSpecs.SelectedItem.ToString();

                        Speaker.SpeakAsync("File Size : " + Size);

                        SearchString += "%20size%3A";
                        if (Size == "Empty")
                        {
                            SearchString += "(>%3D0%20<1)";
                        }
                        else if (Size == "Tiny")
                        {
                            SearchString += "(>%3D1%20<10241)";
                        }
                        else if (Size == "Small")
                        {
                            SearchString += "(>%3D10241%20<102401)";
                        }
                        else if (Size == "Medium")
                        {
                            SearchString += "(>%3D102401%20<1048577)";
                        }
                        else if (Size == "Large")
                        {
                            SearchString += "(>%3D1048577%20<16777217)";
                        }
                        else if (Size == "Huge")
                        {
                            SearchString += "(>%3D16777217%20<134217729)";
                        }
                        else if (Size == "Gigantic")
                        {
                            SearchString += ">%3D134217729";
                        }
                    }

                    if (TypeSpecs.Text.Length > 0)
                    {
                        Speaker.SpeakAsync("File Type : " + TypeSpecs.Text);

                        SearchString += "%20type%3A" + TypeSpecs.Text.ToString().ToLower();
                    }

                    SearchString += "&crumb=location:%3A%3A{20D04FE0-3AEA-1069-A2D8-08002B30309D}";
                    Process.Start("search-ms:" + SearchString);

                    AddressBar.Text = "Type here";
                    DateSpecs.SelectedIndex = -1;
                    KindSpecs.SelectedIndex = -1;
                    SizeSpecs.SelectedIndex = -1;
                    TypeSpecs.Text = "";
                    AddressBar.SelectAll();
                }
                else
                {
                    Speaker.SpeakAsync("Invalid File Type : " + TypeSpecs.Text);
                }
            }
        }

        /// <summary>
        /// This closes the window
        ///     - Call the built-in function Close();
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// This drag moves the Window
        ///     - Call the built-in function DragMove()
        /// </summary>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }


        /// <summary>
        /// This gives the user a fluid search or browse experience
        ///     - If the key is not Enter or DownArrow
        ///         - Clear the Items of the AddressBarHistory
        ///         - Create and open the FileStream and its StreamReader
        ///             - Source    : Resources\SearchHistory.kappspot
        ///             - Mode      : OpenOrCreate 
        ///             - Access    : Read
        ///         - Until the end of the Stream is not reached
        ///             - Save the current line pointed by the reader temporarily
        ///             - If the user's search string is a part of that line add it to the AddressBarHistory
        ///                 - Using for loop and Compare <This Procedure for Searching seems to be Undesirable/>
        ///                     - If it is present or if the content is default
        ///                         - Add the item to the AddressBarHistory
        ///                         - Break from the loop 
        ///                 - Add Search History Terms which are only a part starting from the begining of the Search Term <Alternative Procedure/>
        ///                    - If the User's Search String is found to be a sub string of the History Content starting from the 0th Element 
        ///                         - Add it to the AddressBarHistory
        ///                    - Otherwise
        ///                         - Proceed to the next element
        ///         - Close the FileStream and the Reader
        ///         - If the number of items added is greater than 0
        ///             - Open the drop down box
        ///         - Otherwise 
        ///             - close it
        ///     Otherwise
        ///         - If the key is down
        ///             - Focus the addressBar
        ///             - Open the drop down box
        ///         - If the key is enter 
        ///             - Close the drop down box
        ///             - Use the SearchButtonClick to start searching if the user input is valid
        /// </summary>
        private void AddressBar_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter && e.Key != Key.Down)
            {
                AddressBarHistory.Items.Clear();

                FileStream DataProvider = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\SearchHistory.kappspot", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader Reader = new StreamReader(DataProvider);

                while (Reader.EndOfStream != true)
                {
                    string TempHolder = Reader.ReadLine();
                    /*for (int i = 0; i <= TempHolder.Length - AddressBar.Text.Length; i++)
                    {
                        if (string.Compare(AddressBar.Text, 0, TempHolder, i, AddressBar.Text.Length, true) == 0 || AddressBar.Text == "Type here" || AddressBar.Text.Length == 0)
                        {
                            AddressBarHistory.Items.Add(TempHolder);
                            break;
                        }
                    }*/
                    if (string.Compare(AddressBar.Text, 0, TempHolder, 0, AddressBar.Text.Length, true) == 0 || AddressBar.Text == "Type here" || AddressBar.Text.Length == 0)
                    {
                        AddressBarHistory.Items.Add(TempHolder);
                    }
                }

                Reader.Close();
                DataProvider.Close();

                if (AddressBarHistory.Items.Count == 0)
                {
                    AddressBarHistory.IsDropDownOpen = false;
                }
                else
                {
                    AddressBarHistory.IsDropDownOpen = true;
                }
            }
            else
            {
                if (e.Key == Key.Down)
                {
                    AddressBarHistory.Focus();
                    AddressBarHistory.IsDropDownOpen = true;
                }

                if (e.Key == Key.Enter)
                {
                    AddressBarHistory.IsDropDownOpen = false;

                    if (AddressBar.Text.Length > 0 && AddressBar.Text != "Type here")
                    {
                        Search_files_Click(sender, e);
                    }
                }
            }
        }

        /// <summary>
        /// Used to select an item from the history
        ///     - If an item is selected
        ///         - Set the AddressBar's Text to the Content of the SelectedItem
        ///         - Focus the AddressBar
        ///         - Using select navigate the last character
        /// </summary>
        private void AddressBarHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AddressBarHistory.SelectedIndex != -1)
            {
                AddressBar.Text = AddressBarHistory.SelectedItem.ToString();
                AddressBar.Focus();
                AddressBar.Select(AddressBar.Text.Length,0);
            }
        }

        /// <summary>
        /// Updates the SearchHistoryDataBase with the search string
        ///     - Assume that the entry is not already present
        ///     - Create the FileStream and the Reader Objects to read the file
        ///             - Source    : Resources\SearchHistory.kappspot
        ///             - Mode      : Open
        ///             - Access    : ReadWriter
        ///     - While the end if the stream is not reached
        ///         - If the entry is present
        ///             - Set IsEntryAlreadyPresent to "true"
        ///             - break the for loop
        ///     - If the entry is not null and not already present
        ///         - Add it to the SearchHistory
        ///         - Flush the Data from the buffer to the text file
        ///     - Close the Reader
        ///     - Close the FileStream
        /// </summary>
        public void AddressBarHistorySourceUpdator(string AddressBarEntry)
        {
            bool IsEntryAlreadyPresent = false;

            FileStream DataProvider = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\SearchHistory.kappspot", FileMode.Open, FileAccess.ReadWrite);
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
                StreamWriter Writer = new StreamWriter(DataProvider);
                Writer.WriteLine(AddressBarEntry);
                Writer.Flush();
            }

            Reader.Close();
            DataProvider.Close();
        }

        /// <summary>
        /// Makes the visual indication that the item got focus
        ///     - If the AddressBar text is "Type Here"
        ///         - Navigate to the last character
        ///         - Set the fontstyle as normal
        /// </summary>
        private void AddressBar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AddressBar.Text == "Type here")
            {
                AddressBar.Select(AddressBar.Text.Length, 0);
                AddressBar.FontStyle = FontStyles.Normal;
            }
        }

        /// <summary>
        /// Opens the drop down with all the user history when the AddressBar is empty or "Type here"
        ///     - If the AddressBar is empty or has the text "Type here"
        ///         - Clear the contents of the DropDownBox
        ///         - Create and Open the FileStream and the Reader Object
        ///             - Source    : Resources\SearchHistory.kappspot
        ///             - Mode      : OpenOrCreate 
        ///             - Access    : Read
        ///         - While the end of the file is not reached
        ///             - Add the items to the DropDownBox
        ///         - Close the Reader and the FileStream Objects
        /// </summary>
        private void AddressBarHistory_DropDownOpened(object sender, EventArgs e)
        {
            if (AddressBar.Text == "" && AddressBar.Text == "Type here")
            {
                AddressBarHistory.Items.Clear();
                FileStream DataProvider = new FileStream(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Kappspot\MiniMetro\Resources\SearchHistory.kappspot", FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader Reader = new StreamReader(DataProvider);
                while (Reader.EndOfStream != true)
                {
                    AddressBarHistory.Items.Add(Reader.ReadLine());
                }
                Reader.Close();
                DataProvider.Close();
            }
        }
    }
}
