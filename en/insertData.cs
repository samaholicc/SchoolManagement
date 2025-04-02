using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchoolManagement
{
    public class InsertData
    {
        private string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

        public string AddAccount(string userInsert, string passInsert, string insertIDStr, string insertROLE)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();

                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandText = @"
                            INSERT INTO ACCOUNT (ID, FULL_NAME, PASSWORD, ROLE) 
                            VALUES (@id, @name, @password, @role) 
                        ";

                        // Ensure all parameters are passed correctly as strings  
                        command.Parameters.AddWithValue("@id", insertIDStr); // insertIDStr is the student ID  
                        command.Parameters.AddWithValue("_Lingobit_@name", userInsert); // userInsert is a string  
                        command.Parameters.AddWithValue("@password", Encrypt.HashString(passInsert)); // hashed password  
                        command.Parameters.AddWithValue("@role", insertROLE); // insertROLE is a string  

                        command.ExecuteNonQuery();
                        MessageBox.Show("Account information saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    return $"{insertIDStr} {userInsert}{passInsert} {insertROLE} ";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        public string UpdatePassword(string insertID, string passInsert)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();

                    // Check if the ID exists before updating
                    using (MySqlCommand checkCmd = new MySqlCommand("SELECT COUNT(*) FROM ACCOUNT WHERE ID = @id", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@id", insertID);
                        int recordCount = Convert.ToInt32(checkCmd.ExecuteScalar()); // Get the number of records with this ID

                        if (recordCount == 0)
                        {
                            MessageBox.Show($"No records found with ID: {insertID}. Please check the ID.", "_Lingobit_Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return "Failed: ID not found";
                        }
                    }

                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandText = @"
                    UPDATE ACCOUNT 
                    SET PASSWORD = @password 
                    WHERE ID = @id
                ";

                        // Add parameters with values
                        command.Parameters.AddWithValue("@id", insertID);
                        command.Parameters.AddWithValue("@password", passInsert);  

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("_Lingobit_Password updated successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return "Success";
                        }
                        else
                        {
                            MessageBox.Show("No records were updated. Please check the ID or password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return "Failed: No rows affected";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;  // Return null if there's an exception
            }
        }





        public void UpdateUserIdBasedOnRole(string username, string userRole)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(mySqlDb))
                {
                    conn.Open();

                    string newId = string.Empty;
                    if (userRole == "Student")
                    {
                        // Get the student ID from the studentstable based on the FULL_NAME
                        string studentQuery = "SELECT student_id FROM studentstable WHERE full_name = @username";
                        using (MySqlCommand cmd = new MySqlCommand(studentQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            newId = cmd.ExecuteScalar()?.ToString();  // Get the student ID as string
                        }
                    }
                    else if (userRole == "Teacher")
                    {
                        // Get the teacher ID from the teacher table based on the FULL_NAME
                        string teacherQuery = "SELECT teacher_id FROM teacher WHERE full_name = @username";
                        using (MySqlCommand cmd = new MySqlCommand(teacherQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@username", username);
                            newId = cmd.ExecuteScalar()?.ToString();  // Get the teacher ID as string
                        }
                    }

                    if (!string.IsNullOrEmpty(newId))
                    {
                        // Update the ACCOUNT table with the new ID (student or teacher ID)
                        string updateQuery = "UPDATE ACCOUNT SET ID = @newId WHERE FULL_NAME = @username";
                        using (MySqlCommand cmd = new MySqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@newId", newId);
                            cmd.Parameters.AddWithValue("@username", username);
                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("_Lingobit_User ID updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("No matching student or teacher found.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
