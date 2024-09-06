using Microsoft.Data.SqlClient;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static Database Instance { get; } = new Database();

        private DbSettings Settings { get; set; }


        public Database()
        {
            Settings = DbSettings.LoadSettings();
        }

        public bool TestConnection()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString(Settings!)))
                {
                    con.Open();
                    return true;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private string GetConnectionString(DbSettings settings)
        {
            SqlConnectionStringBuilder sb = new();
            sb.Clear();
            sb.DataSource = settings.Hostname;
            sb.InitialCatalog = settings.Database;
            sb.UserID = settings.Username;
            sb.TrustServerCertificate = true;
            sb.Password = settings.Password;
            return sb.ToString();
        }
    }
}