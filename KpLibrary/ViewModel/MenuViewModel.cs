using System.Windows.Input;
using DevExpress.Mvvm;
using KpLibrary.View.BookViews;
using KpLibrary.View.UserViews;
using KpLibrary.ViewModel.Base;

namespace KpLibrary.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        #region Constructor
        
        public MenuViewModel() { }

        #endregion

        #region Commands

        /// <summary>
        ///     Navigate to Book Manage page.
        /// </summary>
        public ICommand GoBook
        {
            get { return new DelegateCommand(() => { AppData.Frame.Navigate(new BookManagePage()); }); }
        }

        /// <summary>
        ///     Navigate to User Manage page.
        /// </summary>
        public ICommand GoUser
        {
            get { return new DelegateCommand(() => { AppData.Frame.Navigate(new UserManagePage()); }); }
        }

        #endregion
    }
}