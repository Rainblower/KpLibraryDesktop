using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;

namespace KpLibrary
{
    public class FBClient
    {
        private static FirebaseClient instance;

        private FBClient()
        {
        }

        public static FirebaseClient GetInstance()
        {
            if (instance == null)
            {
                IFirebaseConfig config = new FirebaseConfig
                {
                    BasePath = "https://kotlinlibrary-5e1b7.firebaseio.com/",
                    AuthSecret = "FxZ2TEdYgbloyOzVGCnG2Amu1sUyuBQsDkbu98fm"
                };

                instance = new FirebaseClient(config);
            }

            return instance;
        }
    }
}