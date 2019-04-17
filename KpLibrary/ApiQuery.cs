using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Firebase.Storage;
using FireSharp.Interfaces;
using KpLibrary.Enums;
using KpLibrary.Model;
using Newtonsoft.Json;

namespace KpLibrary
{
    public static class ApiQuery
    {
        private const string _storageBucket = "kotlinlibrary-5e1b7.appspot.com";
        public const string _apiKey = "AIzaSyCVc4Gf0Vo-nJdleBjJ34VdtChtYhugrN4";
        private static readonly IFirebaseClient _client = FBClient.GetInstance();

        /// <summary>
        ///     Get all users from Firebase.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, User> GetUsers()
        {
            return GetDataFromFirebase<User>(FirebaseTable.User);
        }

        /// <summary>
        ///     Get all book from Firebase.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Book> GetBooks()
        {
            return GetDataFromFirebase<Book>(FirebaseTable.Book);
        }

        /// <summary>
        ///     Get all user books from Firebase.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, UserBook> GetUserBooks()
        {
            return GetDataFromFirebase<UserBook>(FirebaseTable.UserBook);
        }

        /// <summary>
        ///     Get some data from Firebase.
        /// </summary>
        /// <typeparam name="T">Object type from model for json deserialize.</typeparam>
        /// <param name="path">Path to table in Firebase database.</param>
        /// <returns></returns>
        private static Dictionary<string, T> GetDataFromFirebase<T>(string path)
        {
            var firebaseResponse = _client.GetAsync(path);
            var json = firebaseResponse.Result.Body;
            return JsonConvert.DeserializeObject<Dictionary<string, T>>(json);
        }

        /// <summary>
        ///     Upload pdf and image files in storage Firebase.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<string> Upload(string path)
        {
            var stream = File.Open($@"{path}", FileMode.Open);

            var filename = path.Split('\\');

            FirebaseStorageTask task;

            if (filename[filename.Length - 1].Contains(".pdf"))
                task = new FirebaseStorage(_storageBucket).Child(StorageFolder.Book)
                    .Child(filename[filename.Length - 1]).PutAsync(stream);
            else
                task = new FirebaseStorage(_storageBucket).Child(StorageFolder.Image)
                    .Child(filename[filename.Length - 1]).PutAsync(stream);

            var downloadUrl = await task;

            return downloadUrl;
        }
    }
}