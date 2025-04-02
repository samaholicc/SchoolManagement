using ComponentFactory.Krypton.Toolkit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SchoolManagement.AdminProfile;

namespace SchoolManagement
{
    public partial class Login : KryptonForm
    {
        public static string ID { get; private set; } // User's last identity
        public static string TYPE_USER { get; private set; } // User type (Admin, Teacher, Student)
        private readonly IServiceProvider _serviceProvider;
        private readonly IAccountRepository _accountRepository;

        public Login(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _accountRepository = serviceProvider.GetService<IAccountRepository>();
            ConfigureForm();
        }

        #region Initialization
        private void ConfigureForm()
        {
            txtPassword.UseSystemPasswordChar = true; // Mask password input
            this.FormClosed += (s, e) => Application.Exit(); // Ensure app exits when form closes
        }
        #endregion

        #region Event Handlers
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsInputValid())
            {
                await AuthenticateUserAsync();
            }
        }

        private void BtnSwitch_Click(object sender, EventArgs e)
        {
            try
            {
                var currentLanguage = GetCurrentLanguage();
                var newLanguage = currentLanguage == "fr-FR" ? "en-US" : "fr-FR";
                var changeLanguage = new ChangeLanguage();
                changeLanguage.UpdateConfig("language", newLanguage);
                MessageBox.Show(GetLocalizedMessage("Language switched. Restarting application...", "Langue changée. Redémarrage de l'application..."));
                Application.Restart();
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("Error switching language: " + ex.Message, "Erreur lors du changement de langue : " + ex.Message));
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Additional initialization if needed
        }
        #endregion

        #region Authentication Methods
        private bool IsInputValid()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                error.Text = GetLocalizedMessage("Please enter your username and password.", "Veuillez entrer votre identifiant et votre mot de passe.");
                return false;
            }
            return true;
        }

        private async Task AuthenticateUserAsync()
        {
            try
            {
                var account = await _accountRepository.GetAccountByIdAsync(txtUsername.Text);
                if (account != null)
                {
                    await ValidateUserAsync(account);
                }
                else
                {
                    error.Text = GetLocalizedMessage("Invalid username or password.", "Identifiant ou mot de passe invalide.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(GetLocalizedMessage("An error occurred: " + ex.Message, "Une erreur est survenue : " + ex.Message),
                                GetLocalizedMessage("Error", "Erreur"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ValidateUserAsync(Account account)
        {
            string hashedEnteredPassword = Encrypt.HashString(txtPassword.Text);

            if (hashedEnteredPassword == account.Password)
            {
                ID = account.Id;
                TYPE_USER = account.Role;
                await OpenUserMenuAsync(account.Role);
            }
            else
            {
                error.Text = GetLocalizedMessage("Invalid username or password.", "Identifiant ou mot de passe invalide.");
            }
        }

        private async Task OpenUserMenuAsync(string userRole)
        {
            this.Hide();

            Form userMenu = null;
            switch (userRole.ToLower())
            {
                case "admin":
                    userMenu = _serviceProvider.GetService<MenuAdmin>();
                    break;
                case "teacher":
                    userMenu = _serviceProvider.GetService<TeacherMenu>();
                    break;
                case "student":
                    userMenu = _serviceProvider.GetService<StudentMenu>();
                    break;
                default:
                    MessageBox.Show(GetLocalizedMessage("Unknown user role.", "Rôle d'utilisateur inconnu."));
                    break;
            }

            if (userMenu != null)
            {
                userMenu.ShowDialog();
            }

            this.Close();
        }
        #endregion

        #region Helper Methods
        private string GetCurrentLanguage()
        {
            return ConfigurationManager.AppSettings["language"] ?? "en-US"; // Default to English if not set
        }

        private string GetLocalizedMessage(string englishMessage, string frenchMessage)
        {
            string currentCulture = CultureInfo.CurrentCulture.TwoLetterISOLanguageName.ToLower();
            return currentCulture.StartsWith("fr", StringComparison.OrdinalIgnoreCase) ? frenchMessage : englishMessage;
        }
        #endregion
    }
}