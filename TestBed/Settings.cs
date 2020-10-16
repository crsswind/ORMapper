using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace TestBed
{
    public class Settings
    {
        #region Constructors

        private Settings()
        {
            ServerName = @".\SQL2019";
            Database = "ORM";
            UserName = @"sa";
            Password = @"###################";
        }

        #endregion

        #region Properties

        public string ServerName { get; set; }

        public string Database { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public static Settings Instance => LoadSettings();

        #endregion

        #region  Public Methods

        public static Settings LoadSettings()
        {
            string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Settings.xml";

            var serializer = new XmlSerializer(typeof(Settings));
            Settings setting;

            if (File.Exists(filePath))
            {
                FileStream s = File.OpenRead(filePath);
                setting = (Settings)serializer.Deserialize(s);
                s.Close();
            }
            else
            {
                setting = new Settings();
                FileStream s = File.Create(filePath);
                serializer.Serialize(s, setting);
                s.Close();
            }

            return setting;
        }

        #endregion
    }
}