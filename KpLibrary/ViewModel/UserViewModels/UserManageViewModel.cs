using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using KpLibrary.Enums;
using KpLibrary.Model;
using KpLibrary.View.UserViews;
using KpLibrary.ViewModel.Base;

namespace KpLibrary.ViewModel.UserViewModels
{
    public class UserManageViewModel : BaseViewModel
    {
        #region Constructor

        #endregion

        #region Properies

        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<User> StaticUsers { get; set; }
        public User SelectedUser { get; set; }
        public string KeyWord { get; set; } = "";
        public bool IsLoad { get; set; } = true;

        #endregion

        #region Commands

        public ICommand UpdatePage
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (ApiQuery.GetUsers() == null)
                    {
                        MessageBox.Show("У вас нет пользователей.", "Внимание", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        Users = new ObservableCollection<User>(ApiQuery.GetUsers()
                            .Values.Where(x => x.Role == UserRole.User)
                            .OrderBy(x => x.Name)
                            .ToList());

                        StaticUsers = Users;
                    }
                });
            }
        }

        public ICommand AddUser
        {
            get { return new DelegateCommand(() => { AppData.Frame.Navigate(new UserAddPage()); }); }
        }

        public ICommand BlockUser
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    IsLoad = false;

                    SelectedUser.Status = UserStatus.Blocked;

                    var setResponse = await FBClient.GetInstance()
                        .UpdateAsync($"{FirebaseTable.User}/{SelectedUser.Uid}", SelectedUser);

                    if (setResponse.StatusCode == HttpStatusCode.OK)
                        MessageBox.Show("Пользователь заблокирован", "Успех", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    else
                        MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    UpdatePage.Execute(null);

                    IsLoad = true;
                }, () => SelectedUser != null && SelectedUser.Status != UserStatus.Blocked);
            }
        }

        public ICommand AcceptUser
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    IsLoad = false;

                    SelectedUser.Status = UserStatus.Accepted;

                    var setResponse = await FBClient.GetInstance()
                        .UpdateAsync($"{FirebaseTable.User}/{SelectedUser.Uid}", SelectedUser);

                    if (setResponse.StatusCode == HttpStatusCode.OK)
                        MessageBox.Show("Пользователь подтверждён", "Успех", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    else
                        MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    UpdatePage.Execute(null);

                    IsLoad = true;
                }, () => SelectedUser != null && SelectedUser.Status != UserStatus.Accepted);
            }
        }

        public ICommand Search
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (KeyWord == "")
                        Users = StaticUsers;
                    else
                        Users = new ObservableCollection<User>(StaticUsers.Where(x =>
                            x.Name.Contains(KeyWord) || x.Group.Contains(KeyWord) || x.Email.Contains(KeyWord) ||
                            x.Status.Contains(KeyWord)).ToList());
                });
            }
        }

        #endregion
    }
}