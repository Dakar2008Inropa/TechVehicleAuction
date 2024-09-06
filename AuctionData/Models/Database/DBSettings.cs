using Newtonsoft.Json;

namespace AuctionData.Models.Database
{
    public class DbSettings
    {
        private static string SettingsPath { get; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TechAuction");
        private static string SettingsFileName { get; } = "Settings.json";
        private static string SettingsFilePath { get; } = Path.Combine(SettingsPath, SettingsFileName);


        public string? Hostname { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Database { get; set; }

        public static DbSettings LoadSettings()
        {
            Directory.CreateDirectory(SettingsPath);
            try
            {
                var data = File.ReadAllText(SettingsFilePath);
                return JsonConvert.DeserializeObject<DbSettings>(data)!;
            }
            catch
            {
                DbSettings settings = new DbSettings();

                settings.Hostname = "h2sql.cloudprog.org,20001";
                settings.Username = "sa";
                settings.Password = "yourStrong(!)Password";
                settings.Database = "TechAuctionDB";

                return settings;
            }
        }

        public static void SaveSettings(DbSettings settings)
        {
            var data = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(SettingsFilePath, data);
        }
    }
}