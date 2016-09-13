using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using DAL;

namespace JobApplicationTerlevic
{
    public partial class MainForm : Form
    {

        
        public MainForm()
        {
            InitializeComponent();
            this.comboBoxCurrency.SelectedIndex = 0;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {

            if (Helper.ValidateSearch(this.textBoxDeparture.Text.Trim(), this.textBoxDestination.Text.Trim(),
                           this.dateTimePickerDeparture.Value, this.dateTimePickerReturn.Value,
                           this.textBoxPassengers.Text.Trim(), this.comboBoxCurrency.SelectedItem.ToString()))
            {
                Console.Out.WriteLine("Yay!");

                //In case the DB isn't set up yet
                DataManager dbManager = new DataManager();
                dbManager.CreateTables();
                SQLiteDataReader reader = dbManager.ProcessQuery(this.textBoxDeparture.Text.Trim(), this.textBoxDestination.Text.Trim(),
                           this.dateTimePickerDeparture.Value, this.dateTimePickerReturn.Value,
                           this.textBoxPassengers.Text.Trim(), this.comboBoxCurrency.SelectedItem.ToString());



            }
            else
            {
                Console.Out.WriteLine("Boo!");
            }
        }
    }
}
