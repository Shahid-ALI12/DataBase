using System;
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
    public partial class PaymentForm : Form
    {
        public PaymentForm()
        {
            InitializeComponent();
            ApplyPlaceholders();
            UpdateActionState();
        }

        private void PaymentForm_Load(object sender, EventArgs e)
        {

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

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!TryGetPaymentValues(out var paymentId, out var bookingId, out var amount, out var paymentMethodId, out var paymentDate))
            {
                return;
            }

            if (paymentDate.HasValue)
            {
                const string commandText = "INSERT INTO PAYMENT (PaymentID, BookingID, Amount, PaymentMethodID, PaymentDate) VALUES (@PaymentID, @BookingID, @Amount, @PaymentMethodID, @PaymentDate)";
                var rows = DbHelper.ExecuteNonQuery(commandText,
                    new SqlParameter("@PaymentID", paymentId),
                    new SqlParameter("@BookingID", bookingId),
                    new SqlParameter("@Amount", amount),
                    new SqlParameter("@PaymentMethodID", paymentMethodId),
                    new SqlParameter("@PaymentDate", paymentDate.Value));

                ShowResultMessage(rows, "added");
                return;
            }

            const string commandTextWithoutDate = "INSERT INTO PAYMENT (PaymentID, BookingID, Amount, PaymentMethodID) VALUES (@PaymentID, @BookingID, @Amount, @PaymentMethodID)";
            var rowsWithoutDate = DbHelper.ExecuteNonQuery(commandTextWithoutDate,
                new SqlParameter("@PaymentID", paymentId),
                new SqlParameter("@BookingID", bookingId),
                new SqlParameter("@Amount", amount),
                new SqlParameter("@PaymentMethodID", paymentMethodId));

            ShowResultMessage(rowsWithoutDate, "added");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!TryGetPaymentValues(out var paymentId, out var bookingId, out var amount, out var paymentMethodId, out var paymentDate))
            {
                return;
            }

            if (paymentDate.HasValue)
            {
                const string commandText = "UPDATE PAYMENT SET BookingID = @BookingID, Amount = @Amount, PaymentMethodID = @PaymentMethodID, PaymentDate = @PaymentDate WHERE PaymentID = @PaymentID";
                var rows = DbHelper.ExecuteNonQuery(commandText,
                    new SqlParameter("@PaymentID", paymentId),
                    new SqlParameter("@BookingID", bookingId),
                    new SqlParameter("@Amount", amount),
                    new SqlParameter("@PaymentMethodID", paymentMethodId),
                    new SqlParameter("@PaymentDate", paymentDate.Value));

                ShowResultMessage(rows, "updated");
                return;
            }

            const string commandTextWithoutDate = "UPDATE PAYMENT SET BookingID = @BookingID, Amount = @Amount, PaymentMethodID = @PaymentMethodID WHERE PaymentID = @PaymentID";
            var rowsWithoutDate = DbHelper.ExecuteNonQuery(commandTextWithoutDate,
                new SqlParameter("@PaymentID", paymentId),
                new SqlParameter("@BookingID", bookingId),
                new SqlParameter("@Amount", amount),
                new SqlParameter("@PaymentMethodID", paymentMethodId));

            ShowResultMessage(rowsWithoutDate, "updated");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var paymentId = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(paymentId))
            {
                MessageBox.Show("Payment ID is required for deletion.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string commandText = "DELETE FROM PAYMENT WHERE PaymentID = @PaymentID";
            var rows = DbHelper.ExecuteNonQuery(commandText, new SqlParameter("@PaymentID", paymentId));
            ShowResultMessage(rows, "deleted");
        }

        private void ApplyPlaceholders()
        {
            UiHelpers.SetPlaceholder(textBox1, "PAY-001");
            UiHelpers.SetPlaceholder(textBox2, "BKG-001");
            UiHelpers.SetPlaceholder(textBox3, "0.00");
            UiHelpers.SetPlaceholder(textBox4, "PM-001");
            UiHelpers.SetPlaceholder(textBox5, "YYYY-MM-DD (optional)");
        }

        private bool TryGetPaymentValues(out string paymentId, out string bookingId, out decimal amount, out string paymentMethodId, out DateTime? paymentDate)
        {
            paymentId = textBox1.Text.Trim();
            bookingId = textBox2.Text.Trim();
            paymentMethodId = textBox4.Text.Trim();

            if (string.IsNullOrWhiteSpace(paymentId)
                || string.IsNullOrWhiteSpace(bookingId)
                || string.IsNullOrWhiteSpace(paymentMethodId))
            {
                MessageBox.Show("Please enter all payment fields before saving.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                amount = 0;
                paymentDate = null;
                return false;
            }

            if (!decimal.TryParse(textBox3.Text.Trim(), out amount))
            {
                MessageBox.Show("Please enter a valid amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                paymentDate = null;
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox5.Text))
            {
                paymentDate = null;
                return true;
            }

            if (!DateTime.TryParse(textBox5.Text.Trim(), out var parsedDate))
            {
                MessageBox.Show("Please enter a valid payment date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                paymentDate = null;
                return false;
            }

            paymentDate = parsedDate;
            return true;
        }

        private void ShowResultMessage(int rowsAffected, string action)
        {
            if (rowsAffected > 0)
            {
                MessageBox.Show($"Payment record {action} successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Record not found.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateActionState()
        {
            var hasPaymentId = !string.IsNullOrWhiteSpace(textBox1.Text);
            var hasRequiredFields = hasPaymentId
                && !string.IsNullOrWhiteSpace(textBox2.Text)
                && !string.IsNullOrWhiteSpace(textBox3.Text)
                && !string.IsNullOrWhiteSpace(textBox4.Text);

            button1.Enabled = hasRequiredFields;
            button2.Enabled = hasRequiredFields;
            button3.Enabled = hasPaymentId;
        }
    }
}
