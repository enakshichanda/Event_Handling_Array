 /*Creating an application for TeamBuilder Ltd to update the application developed previously by adding new 
 * functionality, and refactoring some of its existing functionality.Due to recent successful trading, the company has
 * decided to increase its event offerings from 5 to ten, while retaining the same five locations as before.
 */

using System;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace Assignment5_F
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }

        // Declaration of the array
        string[] eventName, eventLocation;
        int[] eventDays;
        decimal[] eventFee, lodgingFee;
        decimal[] mealPrice;
        string[] mealNames;
        int[,] placesAvailable;
        decimal totalFee = 0;
        int eventIndex, locationIndex, mealIndex;
        int temp = 0, j = 0;
        int[] eventIndexCalculate = new int[10], locationIndexCalculate = new int[10], mealIndexCalculate = new int[10], quantityCalculate = new int[10];
        decimal[] totalFeeCount = new decimal[10];
        string[] uniqueId = new string[10];

        private void Form1_Load(object sender, EventArgs e)
        {
            mealListBox.Enabled = false;
            locationListBox.Enabled = false;
            addBookingButton.Enabled = false;
            quantityTextBox.Enabled = false;
            reportButton.Enabled = false;
            confirmBookingButton.Enabled = false;
            eventName = new string[10] { "Murder Mystery Weekend", "CSI Weekends          ", "The Great Outdoors    ",
                                      "The Chase                 ",
                                         "Digital Refresh            ","Action Photography    ",
                                         "Team Ryder Cup        ", "Abselling                  ","War Games             ",
                                          "Find Wally               "};
            mealNames = new string[4] { "Full Meal", "Half Meal", "Breakfast", "No Meal" };
            eventDays = new int[10] { 2, 3, 4, 6, 2, 5, 3, 2, 6, 5 };
            mealPrice = new decimal[4] { 49.50m, 37.50m, 12m, 0m };
            eventFee = new decimal[10] { 600m, 1000m, 1500m, 1800m, 599m, 999m, 619m, 499m, 1999m, 799m };
            eventLocation = new string[5] { "Cork", "Dublin", "Galway", "Belmullet", "Belfast" };
            lodgingFee = new decimal[5] { 250m, 165m, 225m, 305m, 95m };
            placesAvailable = new int[5, 10]{{35,28,23,12,2,1,16,3,45,0},{67,3,2,6,0,8,24,7,12,0},
                                            {12,34,6,9,7,4,40,45,56,3},{77,12,4,4,5,7,4,3,12,0},{32,7,3,6,8,4,12,3,23,0}};

            //checks the seat availability in the file
            if (Directory.Exists("data"))
            {
                var directory = new DirectoryInfo("data");
                var myFile = directory.GetFiles()
                 .OrderByDescending(f => f.LastWriteTime)
                 .First().ToString();
                var lastLine = File.ReadLines("data/" + myFile).Last();
                String[] numbers1 = lastLine.Split(':');
                int k = 0, l = 0;
                foreach (var item in numbers1)
                {
                    
                    placesAvailable[k, l] = Convert.ToInt32(item.ToString());
                    l++;
                    if (l % 10 == 0) { l = 0; k++; }
                }
            }
            // Initialization of array
            for (int i = 0; i < 10; i++)
                eventsListBox.Items.Add(eventName[i] + "\t" + eventDays[i] + "\t" + eventFee[i]);
            for (int j = 0; j < 5; j++)
                locationListBox.Items.Add(eventLocation[j] + "\t\t" + lodgingFee[j]);
            for (int k = 0; k < 4; k++)
                mealListBox.Items.Add(mealNames[k] + "\t\t" + mealPrice[k]);

        }


        private void eventsListBox_MouseClick(object sender, MouseEventArgs e)
        {
            //displays event details in appropriate control
            locationListBox.Enabled = true;
            eventNameLabel.Text = eventName[eventsListBox.SelectedIndex];
            //function call to perform event price calculations
            calValue();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            //create directory if already a directory doesn't exist
            if (!Directory.Exists("data"))
                Directory.CreateDirectory("data");
            
            //save all details in file after exit button is pressed
            string result = "";
            for (int i = 1; i < temp + 1; i++)
            {
                
                result += "\nUnique ID for This Transaction is " + uniqueId[i] + "\nEvent selected:" + eventName[eventIndexCalculate[i]] + "\nLocation selected:" + eventLocation[locationIndexCalculate[i]] + "\nMeal selected:" +
                          mealNames[mealIndexCalculate[i]] +
                         "\nSeats booked:" + quantityCalculate[i] + "\nTotal Price" + totalFeeCount[i].ToString() + "\n\n";
              
            }
            //keeps a count for the available places in the file where we fetch the data from
            for (int k = 0; k < 5; k++)
                for (int l = 0; l < 10; l++)
                    result += placesAvailable[k, l] + ":";

            //each file is saved with the current date format
            string str = DateTime.Now.ToString("yyyyMMdd");

            //writes data into file
            TextWriter textFile = new StreamWriter("data/" + str, append: true);
            result = result.Remove(result.Length - 1, 1);
            textFile.Write(result);
            textFile.Close();
//exits the application
            this.Close();
            System.Windows.Forms.Application.ExitThread();

        }

        private void quantityTextBox_MouseHover(object sender, EventArgs e)
        {
            //mouse hover to enter the required number of seats
            ToolTip t1 = new ToolTip();
            t1.Active = true;
            t1.AutoPopDelay = 2000;
            t1.InitialDelay = 400;
            t1.IsBalloon = true;
            t1.ToolTipIcon = ToolTipIcon.Info;
            t1.SetToolTip(quantityTextBox, "Please enter the required number of seats");

        }

        private void eventsListBox_MouseHover(object sender, EventArgs e)
        {
            //mouse hover to select an event frrom the list
            ToolTip t2 = new ToolTip();
            t2.Active = true;
            t2.AutoPopDelay = 2000;
            t2.InitialDelay = 400;
            t2.IsBalloon = true;
            t2.ToolTipIcon = ToolTipIcon.Info;
            t2.SetToolTip(eventsListBox, "Please select your preferred event");
        }

        private void locationListBox_MouseHover(object sender, EventArgs e)
        {
            //mouse hove to choose location
            ToolTip t3 = new ToolTip();
            t3.Active = true;
            t3.AutoPopDelay = 2000;
            t3.InitialDelay = 400;
            t3.IsBalloon = true;
            t3.ToolTipIcon = ToolTipIcon.Info;
            t3.SetToolTip(locationListBox, "Select your preferred location for the event");
        }

        private void mealListBox_MouseHover(object sender, EventArgs e)
        {
            //mouse hover to select meal
            ToolTip t4 = new ToolTip();
            t4.Active = true;
            t4.AutoPopDelay = 2000;
            t4.InitialDelay = 400;
            t4.IsBalloon = true;
            t4.ToolTipIcon = ToolTipIcon.Info;
            t4.SetToolTip(mealListBox, "Choose your preferred meal");

        }



        private void reportButton_Click(object sender, EventArgs e)
        {
            //shows the number of seats left after each confirmed booking
            string result = "";
            if (Directory.Exists("data"))
            {
                var directory = new DirectoryInfo("data");
                var myFile = directory.GetFiles()
                 .OrderByDescending(f => f.LastWriteTime)
                 .First().ToString();
                
                // reads the data in the file till last line
                var lastLine = File.ReadLines("data/" + myFile).Last();
                String[] numbers1 = lastLine.Split(':');
                int k = 0, l = 0;
                foreach (var item in numbers1)
                {
                    placesAvailable[k, l] = Convert.ToInt32(item.ToString());
                    l++;
                    if (l % 10 == 0)
                    { l = 0; k++; }
                }
            }
            //displays the available seats along with event and location
            for (int k = 0; k < 5; k++)
                for (int l = 0; l < 10; l++)
                    result += eventLocation[k] + "\t" + eventName[l] + "\t" + placesAvailable[k, l] + "\n";
            MessageBox.Show(result);
        }

    private void exitButton_MouseHover(object sender, EventArgs e)
        {
            //mouss hover to exit the application
            ToolTip t6 = new ToolTip();
            t6.Active = true;
            t6.AutoPopDelay = 2000;
            t6.InitialDelay = 400;
            t6.IsBalloon = true;
            t6.ToolTipIcon = ToolTipIcon.Info;
            t6.SetToolTip(exitButton, "Click to close the application");

        }

        private void reportButton_MouseHover(object sender, EventArgs e)
        {
            //mouse hover to check seat availability
            ToolTip t5 = new ToolTip();
            t5.Active = true;
            t5.AutoPopDelay = 2000;
            t5.InitialDelay = 400;
            t5.IsBalloon = true;
            t5.ToolTipIcon = ToolTipIcon.Info;
            t5.SetToolTip(reportButton, "Click to check the available seats");
        }

        private void locationListBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //displays location value in appropriate control
                mealListBox.Enabled = true;
                LocDisplayLabel.Text = eventLocation[locationListBox.SelectedIndex];
                //function call for performing location calculations
                calValue();
            }
            catch { }
        }

        private void mealListBox_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                //displays meal value in appropriate control
                mealDisplayLabel.Text = mealNames[mealListBox.SelectedIndex];
                quantityTextBox.Enabled = true;

                //function call for performing meal calculations
                calValue();
            }
            catch { }
        }

        private void quantityTextBox_TextChanged(object sender, EventArgs e)
        {
            //calculating total value as per the number of available seats
            if (quantityTextBox.Text != "")
            {
                addBookingButton.Enabled = true;
                priceDisplayLabel.Text = (totalFee * Convert.ToInt32(quantityTextBox.Text)).ToString();
            }
        }

        private void confirmBookingButton_Click(object sender, EventArgs e)
        {
            exitButton.Enabled = true;
            confirmBookingButton.Enabled = false;
            string result = "";
            
            decimal finalAmount = 0;
            
            //generation of unique transaction ID using date-time format
            for (int i = 1+j; i < temp+1; i++)
            {
                uniqueId[i] = DateTime.Now.ToString("MMddmmssff");
                //displaying each event with the details
                result += "\nUnique ID for This Transaction is " + uniqueId[i]+"\nEvent selected:" + eventName[eventIndexCalculate[i]] + "\nLocation selected:" + eventLocation[locationIndexCalculate[i]] + "\nMeal selected:" + 
                          mealNames[mealIndexCalculate[i]] + 
                         "\nSeats booked:" + quantityCalculate[i]+"\nTotal Price:"+ totalFeeCount[i].ToString()+"\n\n";
                 finalAmount += totalFeeCount[i];
                
            }
            //displaying total value for a booking
            MessageBox.Show(result + "\n\n Total booking Ammount: "+finalAmount);
            j=temp;

        }

        private void quantityTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //validation check to enter only numeric value
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
             (e.KeyChar == '.'))
            {
                e.Handled = true;
            }
        }

        private void addBookingButton_Click(object sender, EventArgs e)
        {
            reportButton.Enabled = true;
            exitButton.Enabled = false;
            confirmBookingButton.Enabled = true;
            // validation check to see the quantity and places available for booking
            if ((Convert.ToInt32(quantityTextBox.Text)>0) &&(Convert.ToInt32(quantityTextBox.Text) <= placesAvailable[locationIndex, eventIndex]))
            {
               //calculation performed as per the number of seats selected and then add to booking
                temp++;
                eventIndexCalculate[temp] = eventIndex;
                locationIndexCalculate[temp] = locationIndex;
                mealIndexCalculate[temp] = mealIndex;
                quantityCalculate[temp] = Convert.ToInt32(quantityTextBox.Text);
                totalFeeCount[temp] = totalFee * Convert.ToInt32(quantityTextBox.Text);
                placesAvailable[locationIndex, eventIndex] -= quantityCalculate[temp];
                
                
                MessageBox.Show("You booking has been added.");
                eventsListBox.ClearSelected();
                locationListBox.ClearSelected();
                mealListBox.ClearSelected();
                quantityTextBox.Text = "";
                priceDisplayLabel.Text = "";
                eventNameLabel.Text = "";
                LocDisplayLabel.Text = "";
                mealDisplayLabel.Text = "";
                totalPriceLabel.Text = "";
                quantityTextBox.Enabled = false;
                mealListBox.Enabled = false;
                locationListBox.Enabled = false;
                addBookingButton.Enabled = false;
 }
            else
            {
                MessageBox.Show("Sorry, booking is not possible as"+placesAvailable[locationIndex, eventIndex].ToString()+" seats left for booking", "Info",MessageBoxButtons.OK, MessageBoxIcon.Information);
                quantityTextBox.Text = placesAvailable[locationIndex, eventIndex].ToString();
            }
        }

        public void calValue()
        {
            //function created to add the values of event, location and meal.
            totalFee = 0;
            eventIndex = eventsListBox.SelectedIndex;
            locationIndex = locationListBox.SelectedIndex;
            mealIndex = mealListBox.SelectedIndex;
           
            if (eventIndex != -1)
            {
                totalFee += eventFee[eventIndex];
                }
            if (locationIndex != -1)
            {
                totalFee += lodgingFee[locationIndex] * eventDays[eventIndex];
            }
            if (mealIndex != -1)
            {
                totalFee += mealPrice[mealIndex]*eventDays[eventIndex];
            }
            totalPriceLabel.Text = totalFee.ToString();

        }
    }
}

