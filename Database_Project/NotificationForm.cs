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
    public partial class NotificationForm : Form
    {
        public NotificationForm()
        {
            InitializeComponent();
            ApplyPlaceholders();
            UpdateActionState();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void NotificationForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!TryGetNotificationValues(out var notificationId, out var passengerId, out var message, out var notificationDate))
            {
                return;
            }

            if (notificationDate.HasValue)
            {
                const string commandText = "INSERT INTO NOTIFICATION (NotificationID, PassengerID, Message, NotificationDate) VALUES (@NotificationID, @PassengerID, @Message, @NotificationDate)";
                var rows = DbHelper.ExecuteNonQuery(commandText,
                    new SqlParameter("@NotificationID", notificationId),
                    new SqlParameter("@PassengerID", passengerId),
                    new SqlParameter("@Message", message),
                    new SqlParameter("@NotificationDate", notificationDate.Value));

                ShowResultMessage(rows, "added");
                return;
            }

            const string commandTextWithoutDate = "INSERT INTO NOTIFICATION (NotificationID, PassengerID, Message) VALUES (@NotificationID, @PassengerID, @Message)";
            var rowsWithoutDate = DbHelper.ExecuteNonQuery(commandTextWithoutDate,
                new SqlParameter("@NotificationID", notificationId),
                new SqlParameter("@PassengerID", passengerId),
                new SqlParameter("@Message", message));

            ShowResultMessage(rowsWithoutDate, "added");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!TryGetNotificationValues(out var notificationId, out var passengerId, out var message, out var notificationDate))
            {
                return;
            }

            if (notificationDate.HasValue)
            {
                const string commandText = "UPDATE NOTIFICATION SET PassengerID = @PassengerID, Message = @Message, NotificationDate = @NotificationDate WHERE NotificationID = @NotificationID";
                var rows = DbHelper.ExecuteNonQuery(commandText,
                    new SqlParameter("@NotificationID", notificationId),
                    new SqlParameter("@PassengerID", passengerId),
                    new SqlParameter("@Message", message),
                    new SqlParameter("@NotificationDate", notificationDate.Value));

                ShowResultMessage(rows, "updated");
                return;
            }

            const string commandTextWithoutDate = "UPDATE NOTIFICATION SET PassengerID = @PassengerID, Message = @Message WHERE NotificationID = @NotificationID";
            var rowsWithoutDate = DbHelper.ExecuteNonQuery(commandTextWithoutDate,
                new SqlParameter("@NotificationID", notificationId),
                new SqlParameter("@PassengerID", passengerId),
                new SqlParameter("@Message", message));

            ShowResultMessage(rowsWithoutDate, "updated");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var notificationId = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(notificationId))
            {
                MessageBox.Show("Notification ID is required for deletion.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string commandText = "DELETE FROM NOTIFICATION WHERE NotificationID = @NotificationID";
            var rows = DbHelper.ExecuteNonQuery(commandText, new SqlParameter("@NotificationID", notificationId));
            ShowResultMessage(rows, "deleted");
        }

        private void ApplyPlaceholders()
        {
            UiHelpers.SetPlaceholder(textBox1, "NOT-001");
            UiHelpers.SetPlaceholder(textBox2, "PAS-001");
            UiHelpers.SetPlaceholder(textBox3, "Message");
            UiHelpers.SetPlaceholder(textBox4, "YYYY-MM-DD (optional)");
        }

        private bool TryGetNotificationValues(out string notificationId, out string passengerId, out string message, out DateTime? notificationDate)
        {
            notificationId = textBox1.Text.Trim();
            passengerId = textBox2.Text.Trim();
            message = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(notificationId)
                || string.IsNullOrWhiteSpace(passengerId)
                || string.IsNullOrWhiteSpace(message))
            {
                MessageBox.Show("Please enter all notification fields before saving.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                notificationDate = null;
                return false;
            }

            if (string.IsNullOrWhiteSpace(textBox4.Text))
            {
                notificationDate = null;
                return true;
            }

            if (!DateTime.TryParse(textBox4.Text.Trim(), out var parsedDate))
            {
                MessageBox.Show("Please enter a valid notification date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                notificationDate = null;
                return false;
            }

            notificationDate = parsedDate;
            return true;
        }

        private void ShowResultMessage(int rowsAffected, string action)
        {
            if (rowsAffected > 0)
            {
                MessageBox.Show($"Notification record {action} successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Record not found.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateActionState()
        {
            var hasNotificationId = !string.IsNullOrWhiteSpace(textBox1.Text);
            var hasRequiredFields = hasNotificationId
                && !string.IsNullOrWhiteSpace(textBox2.Text)
                && !string.IsNullOrWhiteSpace(textBox3.Text);

            button1.Enabled = hasRequiredFields;
            button2.Enabled = hasRequiredFields;
            button3.Enabled = hasNotificationId;
        }
    }
}
