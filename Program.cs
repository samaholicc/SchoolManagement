using System;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace SchoolManagement
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var language = ConfigurationManager.AppSettings["language"] ?? "en-US";
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            try
            {

                ConfigureServices(services);
                DatabaseInitializer.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur fatale : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var loginForm = serviceProvider.GetRequiredService<Login>();
                Application.Run(loginForm);
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'MySqlConnection' not found in app.config.");
            }

            // Register IDbConnectionFactory
            services.AddSingleton<IDbConnectionFactory>(sp => new MySqlConnectionFactory(connectionString));

            // Register repositories with the connection string
            services.AddScoped<IDepartmentRepository>(sp => new DepartmentRepository(connectionString));
            services.AddScoped<IAccountRepository>(sp => new AccountRepository(sp.GetRequiredService<IDbConnectionFactory>()));
            services.AddScoped<IClassSectionRepository>(sp => new ClassSectionRepository(sp.GetRequiredService<IDbConnectionFactory>()));

            // Register services
            services.AddScoped<ClassSectionService>();

            // Register forms
            services.AddTransient<Login>();
            services.AddTransient<MenuAdmin>();
            services.AddTransient<AdminProfile>();
            services.AddTransient<StudentManager>();
            services.AddTransient<TeacherManager>();
            services.AddTransient<TeacherClassSection>();
            services.AddTransient<StudentsInClassSection>();
            services.AddTransient<StudentsInClass>();
            services.AddTransient<StudentGrade>();
            services.AddTransient<StudentClassSectionList>();
            services.AddTransient<StudentClassList>();
            services.AddTransient<Schedule>();
            services.AddTransient<ClassSectionManager>();
            services.AddTransient<DepartmentManager>();
            services.AddTransient<SubjectManager>();
            services.AddTransient<TeacherMenu>();
            services.AddTransient<TeacherProfile>();
            services.AddTransient<StudentMenu>();
            services.AddTransient<StudentProfile>();
        }
    }
}
