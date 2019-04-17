using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm;
using KpLibrary.Enums;
using KpLibrary.Model;
using KpLibrary.View.BookViews;
using KpLibrary.ViewModel.Base;
using Microsoft.Win32;

namespace KpLibrary.ViewModel.BookViewModels
{
    public class BookManageViewModel : BaseViewModel
    {
        #region Constructor

        #endregion

        #region Properies

        public ObservableCollection<Book> Books { get; set; }
        public ObservableCollection<Book> StaticBooks { get; set; }
        public Book SelectedBook { get; set; }
        public string KeyWord { get; set; } = "";
        public bool IsLoad { get; set; } = true;

        #endregion

        #region Commands

        public ICommand PageLoaded
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (ApiQuery.GetBooks() == null)
                    {
                        MessageBox.Show("Кинги не завезли.", "Внимание", MessageBoxButton.OK,
                            MessageBoxImage.Information);
                    }
                    else
                    {
                        Books = new ObservableCollection<Book>(ApiQuery.GetBooks().Values.OrderBy(x => x.Name)
                            .ToList());
                        StaticBooks = Books;
                    }
                });
            }
        }

        public ICommand AddBook
        {
            get { return new DelegateCommand(() => { AppData.Frame.Navigate(new BookAddChangePage(false, null)); }); }
        }

        public ICommand ChangeBook
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    AppData.Frame.Navigate(new BookAddChangePage(true,
                        SelectedBook));
                }, () => SelectedBook != null);
            }
        }

        public ICommand DeleteBook
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    IsLoad = false;

                    if (ApiQuery.GetUserBooks() != null)
                        foreach (var key in ApiQuery.GetUserBooks().Where(x => x.Value.BookUid == SelectedBook.Uid))
                            try
                            {
                                await FBClient.GetInstance().DeleteAsync($"{FirebaseTable.UserBook}/{key.Key}");
                            }
                            catch
                            {
                                MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                            }

                    try
                    {
                        await FBClient.GetInstance().DeleteAsync($"{FirebaseTable.Book}/{SelectedBook.Uid}");
                    }
                    catch
                    {
                        MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    Books.Remove(SelectedBook);
                    MessageBox.Show("Книга удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    IsLoad = true;
                }, () => SelectedBook != null);
            }
        }

        public ICommand Search
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (KeyWord == "")
                        Books = StaticBooks;
                    else
                        Books = new ObservableCollection<Book>(
                            StaticBooks.Where(x => x.Name.Contains(KeyWord)).ToList());
                });
            }
        }

        public ICommand SaveQR
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var dlg = new SaveFileDialog();
                    dlg.FileName = SelectedBook.Name; // Default file name
                    dlg.DefaultExt = ".png"; // Default file extension
                    dlg.Filter = "Image documents (.png)|*.png"; // Filter files by extension

                    // Show save file dialog box
                    var result = dlg.ShowDialog();

                    // Process save file dialog box results
                    if (result == true)
                    {
                        // Save document
                        var filename = dlg.FileName;

                        var stream = new FileStream(filename, FileMode.Create);

                        var size = new Size(500, 500);

                        var res = new RenderTargetBitmap((int) size.Width, (int) size.Height, 96, 96,
                            PixelFormats.Pbgra32);

                        var drawingvisual = new DrawingVisual();
                        using (var context = drawingvisual.RenderOpen())
                        {
                            context.DrawRectangle(new ImageBrush(SelectedBook.QrCode), null,
                                new Rect(new Point(), size));
                            context.Close();
                        }

                        res.Render(drawingvisual);


                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(res));
                        encoder.Save(stream);

                        stream.Close();
                    }
                }, () => SelectedBook != null);
            }
        }

        #endregion
    }
}