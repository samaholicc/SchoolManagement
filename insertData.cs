using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchoolManagement
{
    public class InsertData
    {
        private string mySqlDb = "Server=localhost;Database=system;User ID=root;Password=samia;";

        public string AddAccount(string userInsert, string passInsert, int insertIDStr, string insertROLE, int insertCLASS_ID)
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
                    INSERT INTO ACCOUNT (ID_LG, USER, PASSWORD, ROLE, Class_ID) 
                    VALUES (@id, @name, @password, @role, @class_id) 
                    
                ";

                        // Ensure all parameters are passed correctly as strings  
                        command.Parameters.AddWithValue("@id", insertIDStr); // insertIDStr is a string  
                        command.Parameters.AddWithValue("@name", userInsert); // userInsert is a string  
                        command.Parameters.AddWithValue("@password", Encrypt.HashString(passInsert)); // hashed password is a string  
                        command.Parameters.AddWithValue("@role", insertROLE); // insertROLE is a string  
                        command.Parameters.AddWithValue("@class_id", insertCLASS_ID); // insertCLASS_ID is a string

                        command.ExecuteNonQuery();
                        MessageBox.Show("Account information saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    return $"{insertIDStr} {userInsert} {passInsert} {insertROLE} {insertCLASS_ID}";
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

                    using (MySqlCommand command = new MySqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandText = @"
                        UPDATE ACCOUNT 
                        SET PASSWORD = @password 
                        WHERE USER = @name  
                    ";

                        command.Parameters.AddWithValue("@name", insertID);
                        command.Parameters.AddWithValue("@password", Encrypt.HashString(passInsert));

                        command.ExecuteNonQuery();
                        MessageBox.Show("Password updated", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    return $"{insertID} {passInsert}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

    }
}