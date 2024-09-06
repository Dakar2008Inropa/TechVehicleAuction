using Microsoft.Data.SqlClient;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        private DbSettings Settings { get; set; }

        private readonly SqlConnection Sqlcon;

        public static Database Instance { get; } = new Database();



        public Database()
        {
            Settings = DbSettings.LoadSettings();
            Sqlcon = new SqlConnection(GetConnectionString(Settings!));
        }

        public void OpenConnection()
        {
            Sqlcon.Open();
        }

        public async Task OpenConnectionAsync()
        {
            await Sqlcon.OpenAsync();
        }

        public void CloseConnection()
        {
            Sqlcon.Close();
        }

        public async Task CloseConnectionAsync()
        {
            await Sqlcon.CloseAsync();
        }

        public SqlConnection GetConnection()
        {
            return Sqlcon;
        }

        public bool TestConnection()
        {
            try
            {
                OpenConnection();
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                CloseConnection();
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