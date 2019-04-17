using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using KpLibrary.Enums;
using KpLibrary.Model;
using KpLibrary.ViewModel.Base;
using Microsoft.Win32;

namespace KpLibrary.ViewModel.BookViewModels
{
    public class BookAddChangeModelView : BaseViewModel
    {
        #region Constructor

        public BookAddChangeModelView(bool isChange, Book book)
        {
            if (isChange)
            {
                _book = book;
                AddVisible = "Collapsed";
                GetTitle = "Библиотека - изменение книги";
                BookName = book.Name;
                BookUrl = book.Url;
                BookImageUrl = book.ImageUrl == "null" ? null : book.ImageUrl;
            }
            else
            {
                ChangeVisible = "Collapsed";
                GetTitle = "Библиотека - добавление книги";
            }
        }

        #endregion

        #region Private fields

        private readonly Book _book;
        private string _bookUrl;
        private string _bookImageUrl;

        #endregion

        #region Properties

        public string ChangeVisible { get; set; }
        public string AddVisible { get; set; }
        public string BookName { get; set; } = "";
        public string BookUrl { get; set; } = "";
        public string BookImageUrl { get; set; }
        public string GetTitle { get; set; }
        public bool IsNotLoad { get; set; } = true;

        #endregion

        #region Commands

        public ICommand ChangeBook
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    IsNotLoad = false;

                    if (!_book.Url.Contains("https://firebasestorage.googleapis.com/v0/b/kotlinlibrary"))
                        _book.Url = await ApiQuery.Upload(_bookUrl);

                    if (!_book.ImageUrl.Contains("https://firebasestorage.googleapis.com/v0/b/kotlinlibrary"))
                    {
                        _book.ImageUrl = _bookImageUrl;

                        if (_book.ImageUrl == null)
                            _book.ImageUrl = "null";
                        else
                            _book.ImageUrl = await ApiQuery.Upload(_bookImageUrl);
                    }

                    var book = new Book
                    {
                        Name = BookName,
                        Uid = _book.Uid,
                        Url = _book.Url,
                        ImageUrl = _book.ImageUrl
                    };

                    var setResponse =
                        await FBClient.GetInstance().UpdateAsync($"{FirebaseTable.Book}/{_book.Uid}", book);

                    if (setResponse.StatusCode == HttpStatusCode.OK)
                        MessageBox.Show("Книга изменена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    IsNotLoad = true;

                    AppData.Frame.GoBack();
                }, () => BookName != "" && BookUrl != "");
            }
        }

        public ICommand AddBook
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    IsNotLoad = false;

                    var bookUrl = await ApiQuery.Upload(_bookUrl);

                    var pic = _bookImageUrl;

                    if (pic == null)
                        pic = "null";
                    else
                        pic = await ApiQuery.Upload(_bookImageUrl);

                    var uid = Guid.NewGuid().ToString("N");

                    var book = new Book
                    {
                        Name = BookName,
                        Uid = uid,
                        Url = bookUrl,
                        ImageUrl = pic
                    };

                    var setResponse = await FBClient.GetInstance().SetAsync($"{FirebaseTable.Book}/{uid}", book);

                    if (setResponse.StatusCode == HttpStatusCode.OK)
                        MessageBox.Show("Книга добавлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Произошла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                    IsNotLoad = true;
                    AppData.Frame.GoBack();
                }, () => BookName != "" && BookUrl != "");
            }
        }

        public ICommand BrowseBook
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var openFileDialog = new OpenFileDialog {Filter = "Pdf files (*.pdf)|*.pdf"};

                    if (openFileDialog.ShowDialog() == true)
                    {
                        _bookUrl = openFileDialog.FileName;
                        var values = openFileDialog.FileName.Split('\\');
                        BookUrl = values[values.Length - 1];
                    }
                });
            }
        }

        public ICommand BrowsePic
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    var openFileDialog = new OpenFileDialog
                    {
                        Filter =
                            "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
                    };

                    if (openFileDialog.ShowDialog() == true)
                    {
                        _bookImageUrl = openFileDialog.FileName;
                        var values = openFileDialog.FileName.Split('\\');
                        BookImageUrl = values[values.Length - 1];
                    }
                });
            }
        }

        #endregion
    }
}