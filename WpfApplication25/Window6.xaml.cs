using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Windows.Threading;

namespace WpfApplication25
{
    /// <summary>
    /// Interaction logic for Window6.xaml
    /// </summary>
    public partial class Window6 : Window
    {
       //Global variable declaration
        char[,] word_array;
        int error;
        FileStream errormsg;
        int selected_row, selected_column;
        int[,] defaultvalidity;
        int selection_validity;
        Button[,] buttons;
        DispatcherTimer timer, go_button_animation_timer;
        bool new_timer_started;
        int[,] display_sequence;
        int last_highlight_button, next_highlight_button;


        //Default Constructor
        //Contains operations to be performed before the main window opens
        public Window6()
        {
            //Built in function
            InitializeComponent();

            //to give default values to color identifier
            defaultvalidity = new int[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    defaultvalidity[i, j] = 0;
                }
            }

            //to check whether the app was restarted
            errormsg = new FileStream(@"Resources\errormsg.kappspot", FileMode.Open, FileAccess.Read);
            error = Convert.ToInt32(new StreamReader(errormsg).ReadLine());
            errormsg.Close();

            //first open instructions
            FileStream first_open = new FileStream(@"Resources\first_open.kappspot", FileMode.Open, FileAccess.ReadWrite);
            int firstopen = Convert.ToInt32(new StreamReader(first_open).Read());
            first_open.Close();

            //to show welcome address if the app was not restarted and when the app is not launched first
            //to show instructions when the app is launched for the first time
            if (firstopen == 0)
            {
                if (error == 0)
                {
                    MessageBox.Show("Kappspot Welcomes You!!!", "©Kappspot");
                }
            }
            else
            {
                MessageBox.Show("Kappspot Welcomes You!!! \n\nINSTRUCTIONS \n\n\t1. Type the word sequence in the text box as given in WORDAMENT. \n\t2. Hit the GO!!! button when it gets activated. \n\t3. Click on a Button with a letter below the text box to word list.\n\t4. Select a word in the list box to HIGHLIGHT the word in the sequence.\n\t5. Double click the same word to search its meaning in GOOGLE. ", "©Kappspot - Karthik M A M");
            }

            //to tell the computer whether the file has been opened before or not
            first_open = new FileStream(@"Resources\first_open.kappspot", FileMode.Open, FileAccess.ReadWrite);
            StreamWriter nextopen = new StreamWriter(first_open);
            nextopen.Write("0");
            nextopen.Flush();
            first_open.Close();

            //to arbitrarily tell that there will be no error in the app
            error = 0;
            errormsg = new FileStream(@"Resources\errormsg.kappspot", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter s1 = new StreamWriter(errormsg);
            s1.Write(error);
            s1.Flush();
            errormsg.Close();

            //to initialize the global variables
            word_array = new char[4, 4];

            //to disable the go button
            go.IsEnabled = false;

            //to clear all the data when the text box gets focus
            word_collector.GotFocus += word_collector_GotFocus;

            //Initialize the button array to store the button
            buttons = new Button[4, 4];
            buttons[0, 0] = Button01;
            buttons[0, 1] = Button02;
            buttons[0, 2] = Button03;
            buttons[0, 3] = Button04;
            buttons[1, 0] = Button05;
            buttons[1, 1] = Button06;
            buttons[1, 2] = Button07;
            buttons[1, 3] = Button08;
            buttons[2, 0] = Button09;
            buttons[2, 1] = Button10;
            buttons[2, 2] = Button11;
            buttons[2, 3] = Button12;
            buttons[3, 0] = Button13;
            buttons[3, 1] = Button14;
            buttons[3, 2] = Button15;
            buttons[3, 3] = Button16;

            //Assign individual button's properties using the button array
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    buttons[i, j].FontSize = 15;
                    buttons[i, j].FontWeight = FontWeights.Heavy;
                }
            }

            //Initialize the dispatcher timer object for the animation timer
            //Also initialize the sequence holder that holds the sequence in which the buttons must grow
            display_sequence = new int[4, 4];
            timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Render);
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += timer_Tick;

            //Data Initialization for go_button_animation
            go_button_animation_timer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Render);
            go_button_animation_timer.Interval = TimeSpan.FromMilliseconds(20);
            go_button_animation_timer.Tick += go_button_animation_timer_Tick;
        }


        //This is to increase the size of the buttons from zero for 
        void timer_Tick(object sender, EventArgs e)
        {
            if (new_timer_started)
            {
                //Set the objects height and width that need to be animated to 5 and the rest to their default values
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (display_sequence[i, j] == 0)
                        {
                            buttons[i, j].Height = 50;
                            buttons[i, j].Width = 50;
                        }
                        else
                        {
                            buttons[i, j].Width = 0;
                            buttons[i, j].Height = 0;
                        }
                    }
                }
                //The first instance of the timer is going to end.
                //So set the new_timer_started to false to inform the next instance that the current instance is not the first instance
                new_timer_started = false;
                next_highlight_button = 1;
            }
            else
            {
                //This is used to stop the outer for loop that finds the position of the element
                //When the inner for loop finds the position the outer for loop is given the command to be stopped
                int for_loop_stopper = 0;

                //If the button that is going to be highlighted doesn't have the terminating index the block in if gets executed
                if (next_highlight_button < last_highlight_button)
                {
                    //Scope is this block because i and j are needed outside the for loops
                    int i = 0, j = 0;

                    //This for loop is used to find the element's row and column index
                    for (i = 0; i < 4; i++)
                    {
                        for (j = 0; j < 4; j++)
                        {
                            if (display_sequence[i, j] == next_highlight_button)
                            {
                                for_loop_stopper = 1;
                                break;
                            }
                        }

                        //This simulates a double break from the inner as well as the outer
                        //The inner for loop breaks and the condition implemented there facilitates the breaking of the outer for loop
                        if (for_loop_stopper == 1)
                        {
                            break;
                        }
                    }

                    //If the total height is less than 50 increment its height till 50 and when that condition is reached go for the next button in the sequence
                    if (i < 4 && j < 4)
                    {
                        if (buttons[i, j].Height < 50)
                        {
                            buttons[i, j].Height += 5;
                            buttons[i, j].Width += 5;
                        }
                        else
                        {
                            next_highlight_button++;

                            //Play the sound file as an audio indicator

                        }
                    }
                }
                else
                {
                    //Stop the timer when the animation is complete...i.e., when all the highlighted buttons have grown fully
                    timer.Stop();
                }
            }
        }


        //when the text box gets mouse focus the contents are deleted and makes the text look normal
        void word_collector_GotFocus(object sender, RoutedEventArgs e)
        {
            word_collector.FontStyle = FontStyles.Normal;
            word_collector.Clear();
        }


        //Check whether the next letter of the word is present around the word's previous letter
        //The found letter must not be selected before
        public int nth_letter_finder(ref int m, ref int n, int[,] validity, char letter, ref int counter)
        {
            //To Assign the Position of the letter in the word
            validity[m, n] = counter++;

            //To find whether the next letter is around the last found letter
            for (int i = m - 1; i <= m + 1; i++)
            {
                for (int j = n - 1; j <= n + 1; j++)
                {
                    if (i < 4 && j < 4 && i > -1 && j > -1)
                    {
                        if (validity[i, j] == 0 && letter == word_array[i, j])
                        {
                            //if the next letter is present it returns 1 and its position and validity is obtained
                            m = i;
                            n = j;
                            validity[i, j] = 1;
                            return 1;
                        }
                    }
                }
            }

            //the next letter is not present so it returns 0
            return 0;
        }

        /*public static int reassign(char[,] word_array, ref int m, ref int n, char letter, int[,] validity, ref int counter)
        {
            int new_row = 0, new_column = 0;
            for (int i = m - 1; i < m + 1;i++ )
            {
                for(int j=n-1;j<n+1;j++)
                {
                    if(i < 4 && j < 4 && i > -1 && j > -1 && validity[m,n] - validity[i,j] == 1 && validity[i,j] !=0)
                    {
                        new_row = i;
                        new_column = j;
                    }
                }
            }
            return 0;
            x:
            {
                for(int i=new_row-1; i<new_row+1; i++)
                {
                    for(int j=new_column-1; j<new_column+1;j++)
                    {
                        if(i < 4 && j < 4 && i > -1 && j > -1 && validity[i,j] == 0 && letter == word_array[i,j])
                        {
                            validity[i, j] = counter;
                            validity[m, n] = 0;
                            m = i;
                            n = j;
                            return 1;
                        }
                    }
                }
            }
            return 0;
        }*/


        //Find the presence of a particular word
        //Calls nth_letter_finder repeatedly to detect the word's presence
        public int word_finder(string word, int m, int n)
        {
            //initially assign counter to 0
            int counter = 0;

            //assign validity of the first letter to 1 and the rest to 0
            int[,] validity = { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } };
            validity[m, n] = 1;

            //iteration for each letter in the word finding the next letter
            //return 0 if the letter is not found
            foreach (char i in word)
            {
                if (nth_letter_finder(ref m, ref n, validity, i, ref counter) == 0)
                {
                    //if(reassign(word_array, ref m, ref n, i, validity, ref counter) == 0)
                    return 0;
                }
            }

            //return 1 since the letter is found
            return 1;
        }

        //Select File which contains the words starting with a particular letter
        //Scan the file for the words present in the wordament array
        //Add the words present to the list box for display
        public void selection_display(int i, int j)
        {
            //declaration and initialization of the local variables
            string file, word;

            //append the extension to get the text file name
            file = @"Resources\" + word_array[i, j] + ".kappspot";

            //"FILE NOT FOUND EXCEPTION" handled here
            try
            {
                //Searches for the file having the words starting with a particular letter
                //if the file is found reader is assigned its starting address
                //streamer is given to stream through the file
                FileStream reader = new FileStream(file, FileMode.Open, FileAccess.Read);
                StreamReader streamer = new StreamReader(reader);

                //scans through the file
                //if a particular word is found in the wordament sequence it is added to the text box
                while ((word = streamer.ReadLine()) != null)
                {
                    if (word_finder(word, i, j) == 1)
                    {
                        list1.Items.Add(word);
                    }
                }

                //reader is closed to avoid damages to the original file
                reader.Close();
            }
            catch
            {
                //the user has entered an invalid character in the sequence
                //exception error is handled here

                //write in the text file that an error has occured
                error = 1;
                errormsg = new FileStream(@"Resources\errormsg.kappspot", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter s = new StreamWriter(errormsg);
                s.Write(error);
                s.Flush();
                errormsg.Close();

                //Show the error message in the Message Box
                if (word_array[i, j] < 'a' && word_array[i, j] > 'z')
                {
                    MessageBox.Show("Critical Files Missing. Re-extract the .rar File from Kappspot.", "ERROR");
                }
                else
                {
                    MessageBox.Show("Undesired Character Detected : \"" + word_array[i, j] + "\"\nPlease Reenter The Sequence!!!", "ERROR");
                }

                //Restart the application
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }


        //Assign the content field to each letter's Button using the word_array
        public void button_data()
        {
            //Assignment of Content field to each button

            Button01.Content = word_array[0, 0].ToString().ToUpper();
            Button02.Content = word_array[0, 1].ToString().ToUpper();
            Button03.Content = word_array[0, 2].ToString().ToUpper();
            Button04.Content = word_array[0, 3].ToString().ToUpper();
            Button05.Content = word_array[1, 0].ToString().ToUpper();
            Button06.Content = word_array[1, 1].ToString().ToUpper();
            Button07.Content = word_array[1, 2].ToString().ToUpper();
            Button08.Content = word_array[1, 3].ToString().ToUpper();
            Button09.Content = word_array[2, 0].ToString().ToUpper();
            Button10.Content = word_array[2, 1].ToString().ToUpper();
            Button11.Content = word_array[2, 2].ToString().ToUpper();
            Button12.Content = word_array[2, 3].ToString().ToUpper();
            Button13.Content = word_array[3, 0].ToString().ToUpper();
            Button14.Content = word_array[3, 1].ToString().ToUpper();
            Button15.Content = word_array[3, 2].ToString().ToUpper();
            Button16.Content = word_array[3, 3].ToString().ToUpper();
        }


        //Describe the general function of the individual letter's button
        public void button_function()
        {
            if (word_array[selected_row, selected_column] != '\0')
            {
                //To Clear the Contents of The List Box.
                //0 and 1 for selection validity is to prevent unidentified error and selecting next button
                //To Prevent the execution of the listbox selection changed event handler we set selection_validity to 0
                selection_validity = 0;
                list1.Items.Clear();
                selection_validity = 1;

                //Adds the found words to the list box
                selection_display(selected_row, selected_column);

                //Toggles the color of the old and new buttons
                defaultvalidity[selected_row, selected_column] = 1;
                button_highlighter(defaultvalidity);
                defaultvalidity[selected_row, selected_column] = 0;

                //Resets the button size...
                //This is used because the highlighted boxes resize themselves and then grow once the button is clicked. This is undesirable
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        buttons[i, j].Width = 50;
                        buttons[i, j].Height = 50;
                    }
                }
            }
        }


        //toggle the border color between coral and blueviolet
        //Blueviolet bordered button is a highlighted button
        public void button_highlighter(int[,] array)
        {
            //scan through the array
            //if the validity of the letter is 0 assign the color coral
            //if the validity of the letter is 1 assign the color blueviolet
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (array[i, j] != 0)
                    {
                        switch (i * 4 + j + 1)
                        {
                            case 1:
                                Button01.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 2:
                                Button02.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 3:
                                Button03.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 4:
                                Button04.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 5:
                                Button05.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 6:
                                Button06.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 7:
                                Button07.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 8:
                                Button08.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 9:
                                Button09.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 10:
                                Button10.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 11:
                                Button11.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 12:
                                Button12.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 13:
                                Button13.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 14:
                                Button14.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 15:
                                Button15.BorderBrush = Brushes.BlueViolet;
                                break;
                            case 16:
                                Button16.BorderBrush = Brushes.BlueViolet;
                                break;
                        }
                    }
                    else
                    {
                        switch (i * 4 + j + 1)
                        {
                            case 1:
                                Button01.BorderBrush = Brushes.Coral;
                                break;
                            case 2:
                                Button02.BorderBrush = Brushes.Coral;
                                break;
                            case 3:
                                Button03.BorderBrush = Brushes.Coral;
                                break;
                            case 4:
                                Button04.BorderBrush = Brushes.Coral;
                                break;
                            case 5:
                                Button05.BorderBrush = Brushes.Coral;
                                break;
                            case 6:
                                Button06.BorderBrush = Brushes.Coral;
                                break;
                            case 7:
                                Button07.BorderBrush = Brushes.Coral;
                                break;
                            case 8:
                                Button08.BorderBrush = Brushes.Coral;
                                break;
                            case 9:
                                Button09.BorderBrush = Brushes.Coral;
                                break;
                            case 10:
                                Button10.BorderBrush = Brushes.Coral;
                                break;
                            case 11:
                                Button11.BorderBrush = Brushes.Coral;
                                break;
                            case 12:
                                Button12.BorderBrush = Brushes.Coral;
                                break;
                            case 13:
                                Button13.BorderBrush = Brushes.Coral;
                                break;
                            case 14:
                                Button14.BorderBrush = Brushes.Coral;
                                break;
                            case 15:
                                Button15.BorderBrush = Brushes.Coral;
                                break;
                            case 16:
                                Button16.BorderBrush = Brushes.Coral;
                                break;
                        }
                    }
                }
            }
        }


        //Functions of the GO button
        //Asssgin the data to each button
        //Initialize the word array
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Get the Characters from the Text Box
            string letters = word_collector.Text;

            //To Clear the Contents of The List Box.
            //0 and 1 for selection validity is to prevent unidentified error and selecting next button
            selection_validity = 0;
            list1.Items.Clear();
            button_highlighter(defaultvalidity);
            selection_validity = 1;

            //Initialisation of Local Variables
            int i = 0, j = 0;

            //Convert all the letters to lower case
            letters = letters.ToLower();

            //takes each letter in the sequence and assigns it to the matrix
            foreach (char letter in letters)
            {
                if (letter != ' ')
                {
                    // nth element in 1d array = i * max_no_of_columns + j
                    //if statement is used to go to the next row's first column
                    if (j == 4)
                    {
                        i++;
                        j = 0;
                    }
                    word_array[i, j] = letter;
                    j++;
                }

                //Clears the text box for next iteration
                word_collector.Clear();

                //Assign the buttons their content
                button_data();
            }

            //Make the size of all the buttons to 0
            for (int ii = 0; ii < 4; ii++)
            {
                for (int jj = 0; jj < 4; jj++)
                {
                    buttons[ii, jj].Height = 0;
                    buttons[ii, jj].Width = 0;
                }
            }
            go_button_animation_timer.Start();

            //Reset the text box that collects the data from the user
            word_collector.Text = "Type the sequence here";
            word_collector.FontStyle = FontStyles.Italic;
            word_collector.SelectAll();
        }


        //Go button click's animation that makes all the buttons grow in size
        void go_button_animation_timer_Tick(object sender, EventArgs e)
        {
            if (buttons[0, 0].Height < 50)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        buttons[i, j].Height += 5;
                        buttons[i, j].Width += 5;
                    }
                }
            }
            else
            {
                go_button_animation_timer.Stop();
            }
        }

        //Individual letter's button clicks are handled here
        //Assign the row and the column details to the global variables
        //Call the button's function
        private void Button01_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 0;
            selected_column = 0;
            button_function();
        }
        private void Button02_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 0;
            selected_column = 1;
            button_function();
        }
        private void Button03_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 0;
            selected_column = 2;
            button_function();
        }
        private void Button04_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 0;
            selected_column = 3;
            button_function();
        }
        private void Button05_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 1;
            selected_column = 0;
            button_function();
        }
        private void Button06_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 1;
            selected_column = 1;
            button_function();
        }
        private void Button07_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 1;
            selected_column = 2;
            button_function();
        }
        private void Button08_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 1;
            selected_column = 3;
            button_function();
        }
        private void Button09_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 2;
            selected_column = 0;
            button_function();
        }
        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 2;
            selected_column = 1;
            button_function();
        }
        private void Button11_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 2;
            selected_column = 2;
            button_function();
        }
        private void Button12_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 2;
            selected_column = 3;
            button_function();
        }
        private void Button13_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 3;
            selected_column = 0;
            button_function();
        }
        private void Button14_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 3;
            selected_column = 1;
            button_function();
        }
        private void Button15_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 3;
            selected_column = 2;
            button_function();
        }
        private void Button16_Click(object sender, RoutedEventArgs e)
        {
            selected_row = 3;
            selected_column = 3;
            button_function();
        }


        //To Activate the go button
        //To Delete the Message
        //To get the Data and Transfer it to the Wordament Array
        private void word_collector_TextChanged(object sender, TextChangedEventArgs e)
        {
            //initialisation of the local variables
            int counter = 0;

            //to get the data from the text box
            string words = word_collector.Text;

            //find the number of spaces in the input data
            foreach (char letter in words)
            {
                if (letter == ' ')
                {
                    counter++;
                }
            }

            //check the condition to enable the go button
            //displays the deficient or excess letters in the go buttons content
            if (words.Length - counter < 16 || words.Length - counter > 16)
            {
                go.Content = words.Length - counter;
                go.IsEnabled = false;
            }
            else
            {
                go.Content = "GO!!!";
                go.IsEnabled = true;
            }
        }


        //Actions performed after the closing of the window
        //Thank you message is displayed
        private void Window_Closed(object sender, EventArgs e)
        {
            //if there is no error while closing display a thank you message
            if (error == 0)
            {
                MessageBox.Show("Thank You!!! Visit Again!!!", "©Kappspot");
            }
        }


        //Actions for the selection event in the list box
        //It highlights the sequence of the letters from the selected words in the list box
        private void list1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //selection_validity is to prevent the simultaneous execution of this event when its data is being cleared
            //if selection_validity is 0 the list is going to be cleared in the next statement
            if (selection_validity != 0)
            {
                //to get the selected item from the list box
                string selected_word = list1.SelectedItem.ToString();

                //declaration and initialisation of the variables
                int counter = 0;
                display_sequence = new int[4, 4];

                //assign the validity of the word's first letter to 1
                //assign the position of the first letter to m and n
                display_sequence[selected_row, selected_column] = 1;
                int m = selected_row, n = selected_column;

                //tosearch for the presence of the word in the wordament array and assign the validity in the increasing order
                //Drawback initializes the last letter's validity to 1
                //This is because I have used a post incrementation
                //Reason for not changing it to pre increment... It requires changes in many functions
                //So optimal solution is to assign the validity of the last word at the end of this for loop
                //This does not affect other functions
                //The word finder function can work even without assigning the last letter's validity to 1. Because it requires only to know the presence...
                //But this requires the sequence of the occurances
                foreach (char i in selected_word)
                {
                    if (nth_letter_finder(ref m, ref n, display_sequence, i, ref counter) == 0)
                    {
                        //if(reassign(word_array, ref m, ref n, i, validity, ref counter) == 0)
                        break;
                    }
                }
                display_sequence[m, n] = counter++;

                //Display the required Boxes With different background so as get a highlighted feel
                button_highlighter(display_sequence);

                //Set the first and the last buttons index value considering them to be an one dimensional array with base index as 1
                last_highlight_button = counter;
                next_highlight_button = 1;

                //Start the animation timer after intimating that this is going to be the first instance of the timer instance for the selected word
                new_timer_started = true;
                timer.Start();
            }
        }


        //slider to adjust the text size in the text box
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //set maximum and minimum values for the slider
            slider.Maximum = 300;
            slider.Minimum = 100;

            //divide the slider value by 10 to get a whole numer and assign it to the font size;
            list1.FontSize = slider.Value / 10;
        }


        //mouse double click event handler
        private void list1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (list1.SelectedItem != null)
            {
                //starts a new internet explorer window 
                //goes to the google site 
                //search for the selected word
                Process.Start(new ProcessStartInfo("iexplore.exe", "https://www.google.com/search?q=" + list1.SelectedItem.ToString() + "+meaning&expnd=1"));
            }
        }

        //To enable the enter button in the module
        private void wordament_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (go.IsEnabled == true)
            {
                if (e.Key == Key.Enter)
                {
                    Button_Click_1(sender, e);
                }
            }
        }
    }
}