using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.IO;

namespace SchoolManagement
{
    public static class DatabaseInitializer
    {
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        public static void InitializeDatabase()
        {
            try
            {
                // Extraire la chaîne de connexion sans le nom de la base de données pour vérifier son existence
                MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder(connectionString);
                string databaseName = builder.Database;
                builder.Database = null; // Supprimer le nom de la base pour se connecter au serveur uniquement
                string serverConnectionString = builder.ToString();

                using (MySqlConnection conn = new MySqlConnection(serverConnectionString))
                {
                    conn.Open();

                    // Vérifier si la base de données SYSTEM existe
                    MySqlCommand checkDbCmd = new MySqlCommand("SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = @DatabaseName", conn);
                    checkDbCmd.Parameters.AddWithValue("@DatabaseName", databaseName);
                    object result = checkDbCmd.ExecuteScalar();

                    if (result == null) // La base de données n'existe pas
                    {
                        Console.WriteLine("La base de données SYSTEM n'existe pas. Création en cours...");

                        // Lire le fichier SQL
                        string sqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "init_database.sql");
                        if (!File.Exists(sqlFilePath))
                        {
                            throw new FileNotFoundException("Le fichier init_database.sql est introuvable dans le répertoire Resources.");
                        }

                        string sqlScript = File.ReadAllText(sqlFilePath);

                        // Exécuter le script SQL
                        MySqlCommand createDbCmd = new MySqlCommand(sqlScript, conn);
                        createDbCmd.ExecuteNonQuery();

                        Console.WriteLine("Base de données SYSTEM créée avec succès.");
                    }
                    else
                    {
                        Console.WriteLine("La base de données SYSTEM existe déjà.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'initialisation de la base de données : " + ex.Message);
                throw; // Relancer l'exception pour que l'application ne continue pas si la base n'est pas créée
            }
        }
    }
}