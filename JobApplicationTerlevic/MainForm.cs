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
using System.Globalization;

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

                if(reader == null)
                {
                    MessageBox.Show("No data was found", "Data error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                while (reader.Read())
                {
                    dataGridView.Rows.Clear();
                    dataGridView.Rows.Add(new object[] {
                    reader.GetValue(1),  
                    reader.GetValue(2),
                    reader.GetValue(3),
                    reader.GetValue(4),
                    reader.GetValue(5),
                    reader.GetValue(6),
                    reader.GetValue(7),
                    reader.GetValue(8),
                    reader.GetValue(9),
                    reader.GetValue(10),
                    });
                }

            }
            else
            {
                Console.Out.WriteLine("Boo!");
            }
        }
    }
}
