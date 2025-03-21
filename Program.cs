using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchoolManagement
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                DatabaseInitializer.InitializeDatabase();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur fatale : {ex.Message}");
            }
            Console.ReadLine();
            var language = ConfigurationManager.AppSettings["language"];
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(language);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Login());


            
           
            }
        }

        

       





    }
