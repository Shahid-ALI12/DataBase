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
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
            ApplyPlaceholders();
            UpdateActionState();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!TryGetAdminValues(out var adminId, out var userId, out var username, out var password))
            {
                return;
            }

            const string commandText = "INSERT INTO ADMIN (AdminID, UserID, Username, Password) VALUES (@AdminID, @UserID, @Username, @Password)";
            var rows = DbHelper.ExecuteNonQuery(commandText,
                new SqlParameter("@AdminID", adminId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password));

            ShowResultMessage(rows, "added");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var adminId = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(adminId))
            {
                MessageBox.Show("Admin ID is required for deletion.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            const string commandText = "DELETE FROM ADMIN WHERE AdminID = @AdminID";
            var rows = DbHelper.ExecuteNonQuery(commandText, new SqlParameter("@AdminID", adminId));
            ShowResultMessage(rows, "deleted");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!TryGetAdminValues(out var adminId, out var userId, out var username, out var password))
            {
                return;
            }

            const string commandText = "UPDATE ADMIN SET UserID = @UserID, Username = @Username, Password = @Password WHERE AdminID = @AdminID";
            var rows = DbHelper.ExecuteNonQuery(commandText,
                new SqlParameter("@AdminID", adminId),
                new SqlParameter("@UserID", userId),
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password));

            ShowResultMessage(rows, "updated");
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

        private void ApplyPlaceholders()
        {
            UiHelpers.SetPlaceholder(textBox1, "ADM-001");
            UiHelpers.SetPlaceholder(textBox2, "USR-001");
            UiHelpers.SetPlaceholder(textBox3, "Username");
            UiHelpers.SetPlaceholder(textBox4, "Password");
        }

        private bool TryGetAdminValues(out string adminId, out string userId, out string username, out string password)
        {
            adminId = textBox1.Text.Trim();
            userId = textBox2.Text.Trim();
            username = textBox3.Text.Trim();
            password = textBox4.Text.Trim();

            if (string.IsNullOrWhiteSpace(adminId)
                || string.IsNullOrWhiteSpace(userId)
                || string.IsNullOrWhiteSpace(username)
                || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter all admin fields before saving.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ShowResultMessage(int rowsAffected, string action)
        {
            if (rowsAffected > 0)
            {
                MessageBox.Show($"Admin record {action} successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No matching admin record found.", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void UpdateActionState()
        {
            var hasAdminId = !string.IsNullOrWhiteSpace(textBox1.Text);
            var hasAllFields = hasAdminId
                && !string.IsNullOrWhiteSpace(textBox2.Text)
                && !string.IsNullOrWhiteSpace(textBox3.Text)
                && !string.IsNullOrWhiteSpace(textBox4.Text);

            button1.Enabled = hasAllFields;
            button2.Enabled = hasAdminId;
            button3.Enabled = hasAllFields;
        }
    }
}
