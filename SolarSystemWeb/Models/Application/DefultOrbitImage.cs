using System.IO;

namespace SolarSystemWeb.Models.Application
{
    public sealed class DefaultOrbitImage
    {
        private static volatile DefaultOrbitImage _instance;
        private static readonly object SyncRoot = new object();

        public static string DefaultImagePath { get; set; }

        private DefaultOrbitImage()
        {
            var file = new FileStream(DefaultImagePath, FileMode.Open);
            ImageData = new byte[file.Length];
            file.Read(ImageData, 0, (int)file.Length);
        }

        public byte[] ImageData { get; }

        public static DefaultOrbitImage Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new DefaultOrbitImage();
                    }
                }

                return _instance;
            }
        }
    }
}