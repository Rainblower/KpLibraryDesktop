using System.Windows;
using KpLibrary.ViewModel;

namespace KpLibrary
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private IFirebaseClient _client;

        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel(Frame);

            //_client = FBClient.GetInstance();
        }


        
    }
}