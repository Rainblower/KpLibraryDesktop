using System.Windows.Controls;

namespace KpLibrary.View
{
    /// <summary>
    ///     Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();

            AppData.PasswordBox = Password;
        }
    }
}