using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm;
using KpLibrary.View;
using KpLibrary.ViewModel.Base;

namespace KpLibrary.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region Private fields

        private Page Auth;

        #endregion

        #region Properties

        public string BackVisible { get; set; } = "Collapsed";
        public string ExitVisible { get; set; } = "Collapsed";

        #endregion
        
        #region Constructor

        public MainViewModel(Frame frame)
        {
            AppData.Frame = frame;
            AppData.Frame.Navigate(new AuthPage());
        }

        #endregion

        #region Commands

        /// <summary>
        /// Render content on Main Window.
        /// </summary>
        public ICommand ContentRender
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (AppData.CurrentUser == null)
                        ExitVisible = "Collapsed";
                    else
                        ExitVisible = "Visible";

                    if (AppData.Frame.CanGoBack)
                        BackVisible = "Visible";
                    else
                        BackVisible = "Collapsed";
                });
            }
        }

        /// <summary>
        /// Log out user from system.
        /// </summary>
        public ICommand Exit
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    while (AppData.Frame.CanGoBack)
                        AppData.Frame.GoBack();

                    ExitVisible = "Collapsed";
                    AppData.CurrentUser = null;
                });
            }
        }

        /// <summary>
        /// Go on previous page.
        /// </summary>
        public ICommand Back
        {
            get { return new DelegateCommand(() => { AppData.Frame.GoBack(); }); }
        }

        #endregion
    }
}