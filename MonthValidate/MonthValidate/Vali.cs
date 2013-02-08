using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace wach.Validate
{
    public class Validate
    {
        private int year;

        public Validate()
        {
            year = DateTime.Now.Year;
        }

       private int leapYear(int year)
       {
            int leapYear = 0;

            if (year % 4 == 0 && year % 100 != 0 || year % 400 == 0)
                leapYear = 1;

            return leapYear;
       }

       // Antal dagar för respektive månad returneras	
        private int daysInMonth(string month, int year)
        {
            switch (month)
            {          
                case "Feb": return 28 + leapYear(year);
                case "Apr":
                case "Jun": 
                case "Sep":
                case "Nov": return 30; 
     
                default: return 31;
           
            }//switch
        }//daysInMonth

        public void addDaysToComboBox(string month, ref System.Windows.Forms.ComboBox cb)
        {
           int numberOfMonth = daysInMonth(month,year);

           cb.Items.Clear();

           for (int i = 1; i <= numberOfMonth; i++)
           {
               cb.Items.Add(i);
           }
        }//addDaysToComboBox

        public bool validateTxtBox(System.Windows.Forms.TextBox txt, string message)
        {
            bool validateOk = true;

            if ("".Equals(txt.Text))
            {
                MessageBox.Show("" + message, "Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                validateOk = false;
            }

            validateOk = isValidAlpha(txt.Text);

            if (!validateOk)
                MessageBox.Show("Error input", "Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            return validateOk;
        }

        public bool validateComboBox(ComboBox cb, string message)
        {

            if (string.IsNullOrEmpty(cb.Text))
            {
                MessageBox.Show("" + message, "Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        public bool isTextboxEmpty(TextBox txt, string message)
        {

            if (string.IsNullOrEmpty(txt.Text))
            {
                MessageBox.Show("" + message, "Important Note", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        public bool isValidAlpha(String str)
        {
            return Regex.IsMatch(str, @"^[a-zA-ZåäöÅÄÖ \""]+$");
        }
    }//class
}//namespace
