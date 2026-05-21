using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Database_Project
{
    public partial class FlightForm : Form
    {
        public FlightForm()
        {
            InitializeComponent();
            ApplyPlaceholders();
            UpdateActionState();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!TryGetFlightValues(out var flightId, out var flightName, out var arrivalCity, out var departureCity))
            {
                return;
            }

            const string commandText = "INSERT INTO FLIGHT (FlightID, FlightName, ArrivalCity, DepartureCity) VALUES (@FlightID, @FlightName, @ArrivalCity, @DepartureCity)";
            var rows = DbHelper.ExecuteNonQuery(commandText,
                new SqlParameter("@FlightID", flightId),
                new SqlParameter("@FlightName", flightName),
                new SqlParameter("@ArrivalCity", arrivalCity),
                new SqlParameter("@DepartureCity", departureCity));

            ShowResultMessage(rows, "added");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var flightId = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(flightId))
            {
                MessageBox.Show("Flight ID is required for deletion.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string commandText = "DELETE FROM FLIGHT WHERE FlightID = @FlightID";
            var rows = DbHelper.ExecuteNonQuery(commandText, new SqlParameter("@FlightID", flightId));
            ShowResultMessage(rows, "deleted");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!TryGetFlightValues(out var flightId, out var flightName, out var arrivalCity, out var departureCity))
            {
                return;
            }

            const string commandText = "UPDATE FLIGHT SET FlightName = @FlightName, ArrivalCity = @ArrivalCity, DepartureCity = @DepartureCity WHERE FlightID = @FlightID";
            var rows = DbHelper.ExecuteNonQuery(commandText,
                new SqlParameter("@FlightID", flightId),
                new SqlParameter("@FlightName", flightName),
                new SqlParameter("@ArrivalCity", arrivalCity),
                new SqlParameter("@DepartureCity", departureCity));

            ShowResultMessage(rows, "updated");
        }

        private void ApplyPlaceholders()
        {
            UiHelpers.SetPlaceholder(textBox1, "FLT-001");
            UiHelpers.SetPlaceholder(textBox2, "Flight name");
            UiHelpers.SetPlaceholder(textBox3, "Arrival city");
            UiHelpers.SetPlaceholder(textBox4, "Departure city");
        }

        private bool TryGetFlightValues(out string flightId, out string flightName, out string arrivalCity, out string departureCity)
        {
            flightId = textBox1.Text.Trim();
            flightName = textBox2.Text.Trim();
            arrivalCity = textBox3.Text.Trim();
            departureCity = textBox4.Text.Trim();

            if (string.IsNullOrWhiteSpace(flightId)
                || string.IsNullOrWhiteSpace(flightName)
                || string.IsNullOrWhiteSpace(arrivalCity)
                || string.IsNullOrWhiteSpace(departureCity))
            {
                MessageBox.Show("Please enter all flight fields before saving.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ShowResultMessage(int rowsAffected, string action)
        {
            if (rowsAffected > 0)
            {
                MessageBox.Show($"Flight record {action} successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Record not found.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateActionState()
        {
            var hasFlightId = !string.IsNullOrWhiteSpace(textBox1.Text);
            var hasAllFields = hasFlightId
                && !string.IsNullOrWhiteSpace(textBox2.Text)
                && !string.IsNullOrWhiteSpace(textBox3.Text)
                && !string.IsNullOrWhiteSpace(textBox4.Text);

            button1.Enabled = hasAllFields;
            button3.Enabled = hasFlightId;
            button4.Enabled = hasAllFields;
        }
    }
}
