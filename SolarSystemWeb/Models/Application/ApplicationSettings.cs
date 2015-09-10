using System.Configuration;

namespace SolarSystemWeb.Models.Application
{
    public class ApplicationSettings
    {
        private static volatile ApplicationSettings _instance;
        private static readonly object SyncRoot = new object();

        private ApplicationSettings() { }

        public static ApplicationSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new ApplicationSettings();
                    }
                }

                return _instance;
            }
        }

        /// <summary>
        /// Нужна ли оптимизация скриптов и стилей
        /// </summary>
        public bool OptimizeScriptsAndStyles
        {
            get
            {
                string rawValue = ConfigurationManager.AppSettings["OptimizeScriptsAndStyles"]; ;
                bool res;
                return bool.TryParse(rawValue, out res) && res;

            }
        }
        
        /// <summary>
        /// Максимальный размер загружаемого изображения
        /// </summary>
        public int MaxImageSize
        {
            get
            {
                string rawValue = ConfigurationManager.AppSettings["MaxImageSize"]; 
                int res;
                bool parseRes = int.TryParse(rawValue, out res);

                return parseRes ? res : 0;
            }
        }
    }
}