using System.Linq;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using Firebase.Auth;
using KpLibrary.Properties;
using KpLibrary.View;
using KpLibrary.ViewModel.Base;
using User = KpLibrary.Model.User;

namespace KpLibrary.ViewModel
{
    public class AuthViewModel : BaseViewModel
    {
        #region Properties

        public string Email { get; set; }
        public bool IsActive { get; set; }

        #endregion

        #region Constructor

        public AuthViewModel()
        {
            if (Settings.Default.Email != "")
                Email = Settings.Default.Email;

            IsActive = true;
        }

        #endregion

        #region Commands

        /// <summary>
        /// L
        /// </summary>
        public ICommand LogIn
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiQuery._apiKey));
                    try
                    {
                        IsActive = false;

                        var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email,
                            AppData.PasswordBox.Password);

                        var user = new User
                        {
                            Email = auth.User.Email
                        };

                        var users = ApiQuery.GetUsers().Values.ToList();

                        if (users.FirstOrDefault(x => x.Email == user.Email)?.Role == "Admin")
                        {
                            AppData.CurrentUser = user;
                            AppData.Frame.Navigate(new MenuPage());

                            Settings.Default.Email = Email;
                            Settings.Default.Save();
                        }
                        else
                        {
                            MessageBox.Show("У вас недостаточно прав.", "Внимание", MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                        }

                        IsActive = true;
                    }
                    catch (FirebaseAuthException e)
                    {
                        if (e.Reason == AuthErrorReason.WrongPassword)
                            MessageBox.Show("Не правильный пароль.", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        else if (e.Reason == AuthErrorReason.InvalidEmailAddress)
                            MessageBox.Show("Неправильный email.", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        else if (e.Reason == AuthErrorReason.UnknownEmailAddress)
                            MessageBox.Show("Неправильный email.", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        else if (e.Reason == AuthErrorReason.MissingPassword)
                            MessageBox.Show("Введите пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                            MessageBox.Show("Неизвестная ошибка, обратитесь к администратору.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);

                        IsActive = true;
                    }
                });
            }
        }

        #endregion
    }
}