using System.Windows.Controls;
using KpLibrary.Model;
using KpLibrary.ViewModel.BookViewModels;

namespace KpLibrary.View.BookViews
{
    /// <summary>
    ///     Логика взаимодействия для BookAddChangePage.xaml
    /// </summary>
    public partial class BookAddChangePage : Page
    {
        public BookAddChangePage(bool isChange, Book book)
        {
            InitializeComponent();

            DataContext = new BookAddChangeModelView(isChange, book);
        }
    }
}