using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using Firebase.Auth;
using KpLibrary.Enums;
using KpLibrary.ViewModel.Base;
using User = KpLibrary.Model.User;

namespace KpLibrary.ViewModel.UserViewModels
{
    public class UserAddViewModel : BaseViewModel
    {
        #region Constructor

        #endregion

        #region Commands

        public ICommand AddUser
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(ApiQuery._apiKey));
                    try
                    {
                        IsNotLoad = false;

                        var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email,
                            AppData.PasswordBox.Password);

                        var uid = Guid.NewGuid().ToString("N");

                        var user = new User
                        {
                            Name = UserName,
                            Uid = uid,
                            Email = Email,
                            Status = UserStatus.Accepted,
                            Group = SelectedGroup,
                            Role = "User"
                        };

                        var setResponse = await FBClient.GetInstance().SetAsync($"{FirebaseTable.User}/{uid}", user);

                        if (setResponse.StatusCode == HttpStatusCode.OK)
                            MessageBox.Show("Пользователь добавлен", "Успех", MessageBoxButton.OK,
                                MessageBoxImage.Information);
                        else
                            MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                        IsNotLoad = true;

                        AppData.Frame.GoBack();
                    }
                    catch (FirebaseAuthException e)
                    {
                        if (e.Reason == AuthErrorReason.InvalidEmailAddress)
                            MessageBox.Show("Неправильный email.", "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        else if (e.Reason == AuthErrorReason.EmailExists)
                            MessageBox.Show("Email уже занят.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                            MessageBox.Show("Неизвестная ошибка, обратитесь к администратору.", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);

                        IsNotLoad = true;
                    }
                }, () => SelectedGroup != null && UserName != "" && Password != "");
            }
        }

        #endregion

        #region Properties

        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string SelectedGroup { get; set; }
        public List<string> Groups { get; set; } = new List<string> {"ВИС-41", "ИСИП-13"};
        public bool IsNotLoad { get; set; } = true;

        #endregion
    }
}