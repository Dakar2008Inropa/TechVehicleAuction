using AuctionData.Models.UserModels;
using AuctionData.Models.VehicleModels;
using Microsoft.Data.SqlClient;
using System.Text;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        #region Properties
        private DbSettings Settings { get; set; }

        private readonly SqlConnection Sqlcon;

        public static Database Instance { get; } = new Database();
        #endregion

        #region Constructor
        public Database()
        {
            Settings = DbSettings.LoadSettings();
            Sqlcon = new SqlConnection(GetConnectionString(Settings!));
        }
        #endregion

        #region Connection
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

        private static string GetConnectionString(DbSettings settings)
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

        private static SqlConnection OpenNewConnection()
        {
            var connection = new SqlConnection(GetConnectionString(Instance.Settings!));
            connection.Open();
            return connection;
        }
        #endregion

        #region Login
        public bool LoginCheck(string username, string password)
        {
            SqlConnection sqlcon = new SqlConnection(GetLoginConnectionString(username, password, Settings));
            try
            {
                sqlcon.Open();
                return true;
            }
            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                sqlcon.Close();
            }
        }

        private static string GetLoginConnectionString(string username, string password, DbSettings settings)
        {
            SqlConnectionStringBuilder sb = new();
            sb.Clear();
            sb.DataSource = settings.Hostname;
            sb.InitialCatalog = settings.Database;
            sb.UserID = username;
            sb.TrustServerCertificate = true;
            sb.Password = password;

            return sb.ToString();
        }
        #endregion

        #region Tables
        public static void CreateTables()
        {
            using (SqlConnection con = OpenNewConnection())
            {
                CreateBaseTable();
                CreateUsersTable();
                CreatePrivateUserTable();
                CreateCorporateUserTable();
                CreateVehiclesTable();
                CreatePassengerCarTable();
                CreatePrivatePassengerCarTable();
                CreateProfessionalPassengerCarTable();
                CreateHeavyVehicleTable();
                CreateBusTable();
                CreateTruckTable();
                CreateAuctionsTable();
                CreateAuctionBidsTable();
                CreateVehicleImagesTable();
            }
        }

        public static bool IsTablesCreated()
        {
            using (SqlConnection con = OpenNewConnection())
            {
                con.Open();

                if (IsTableCreated(DatabaseTables.Base) &&
                    IsTableCreated(DatabaseTables.Users) &&
                    IsTableCreated(DatabaseTables.PrivateUser) &&
                    IsTableCreated(DatabaseTables.CorporateUser) &&
                    IsTableCreated(DatabaseTables.Vehicles) &&
                    IsTableCreated(DatabaseTables.PassengerCar) &&
                    IsTableCreated(DatabaseTables.PrivatePassengerCar) &&
                    IsTableCreated(DatabaseTables.ProfessionalPassengerCar) &&
                    IsTableCreated(DatabaseTables.HeavyVehicle) &&
                    IsTableCreated(DatabaseTables.Bus) &&
                    IsTableCreated(DatabaseTables.Truck) &&
                    IsTableCreated(DatabaseTables.Auctions) &&
                    IsTableCreated(DatabaseTables.AuctionBids) &&
                    IsTableCreated(DatabaseTables.VehicleImages))
                {
                    return true;
                }
                return false;
            }
        }

        private static bool IsTableCreated(string @base)
        {
            using (SqlConnection con = OpenNewConnection())
            {
                StringBuilder query = new StringBuilder();
                query.Append($@"SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{@base}'");

                using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }

        private static void CreateBaseTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.Base}')
            BEGIN
                CREATE TABLE {DatabaseTables.Base} (
                    {nameof(Models.Base.Id)} INT PRIMARY KEY IDENTITY(1,1), 
                    {nameof(Models.Base.CreatedAt)} DATETIME DEFAULT GETUTCDATE(),
                    {nameof(Models.Base.UpdatedAt)} DATETIME NULL,
                    {nameof(Models.Base.DeletedAt)} DATETIME NULL,
                    {nameof(Models.Base.Status)} INT
                );
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreateUsersTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.Users}')
            BEGIN
                CREATE TABLE {DatabaseTables.Users} (
                    {nameof(UserModels.User.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(UserModels.User.PostalCode)} NVARCHAR(50),
                    {nameof(UserModels.User.UserName)} NVARCHAR(255),
                    {nameof(UserModels.User.Discriminator)} NVARCHAR(50),
                    {nameof(UserModels.User.BaseId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Base}({nameof(Models.Base.Id)})
                );
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreatePrivateUserTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.PrivateUser}')
            BEGIN
                CREATE TABLE {DatabaseTables.PrivateUser} (
                    {nameof(Models.Base.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(PrivateUser.CPRNumber)} NVARCHAR(50),
                    {nameof(PrivateUser.UserId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Users}({nameof(UserModels.User.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreateCorporateUserTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.CorporateUser}')
            BEGIN
                CREATE TABLE {DatabaseTables.CorporateUser} (
                    {nameof(Models.Base.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(CorporateUser.Credit)} DECIMAL(18, 2),
                    {nameof(CorporateUser.CvrNumber)} NVARCHAR(50),
                    {nameof(CorporateUser.UserId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Users}({nameof(UserModels.User.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreateVehiclesTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.Vehicles}')
            BEGIN
                CREATE TABLE {DatabaseTables.Vehicles} (
                    {nameof(Models.Base.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(VehicleModels.Vehicle.Maker)} NVARCHAR(120),
                    {nameof(VehicleModels.Vehicle.Model)} NVARCHAR(120),
                    {nameof(VehicleModels.Vehicle.Mileage)} INT,
                    {nameof(VehicleModels.Vehicle.ModelYear)} INT,
                    {nameof(VehicleModels.Vehicle.EngineSize)} DECIMAL(18, 2),
                    {nameof(VehicleModels.Vehicle.EnergyClass)} INT,
                    {nameof(VehicleModels.Vehicle.FuelCapacity)} DECIMAL(18, 2),
                    {nameof(VehicleModels.Vehicle.FuelEconomy)} DECIMAL(18, 2),
                    {nameof(VehicleModels.Vehicle.FuelType)} INT,
                    {nameof(VehicleModels.Vehicle.LicensePlate)} NVARCHAR(50),
                    {nameof(VehicleModels.Vehicle.LicenseType)} INT,
                    {nameof(VehicleModels.Vehicle.Towinghitch)} BIT,
                    {nameof(VehicleModels.Vehicle.Discriminator)} NVARCHAR(50),
                    {nameof(VehicleModels.Vehicle.BaseId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Base}({nameof(Models.Base.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreatePassengerCarTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.PassengerCar}')
            BEGIN
                CREATE TABLE {DatabaseTables.PassengerCar} (
                    {nameof(VehicleModels.Vehicle.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(PassengerCar.RequireCommercialLicense)} BIT,
                    {nameof(PassengerCar.SeatCapacity)} INT,
                    {nameof(PassengerCar.TrunkHeight)} DECIMAL(18, 2),
                    {nameof(PassengerCar.TrunkLength)} DECIMAL(18, 2),
                    {nameof(PassengerCar.TrunkWidth)} DECIMAL(18, 2),
                    {nameof(PassengerCar.TrunkDimensions)} FLOAT,
                    {nameof(PassengerCar.VehicleId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Vehicles}({nameof(VehicleModels.Vehicle.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreatePrivatePassengerCarTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.PrivatePassengerCar}')
            BEGIN
                CREATE TABLE {DatabaseTables.PrivatePassengerCar} (
                    {nameof(PassengerCar.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(PrivatePassengerCar.IsofixMounts)} BIT,
                    {nameof(PrivatePassengerCar.PassengerCarId)} INT FOREIGN KEY REFERENCES {DatabaseTables.PassengerCar}({nameof(PassengerCar.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreateProfessionalPassengerCarTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.ProfessionalPassengerCar}')
            BEGIN
                CREATE TABLE {DatabaseTables.ProfessionalPassengerCar} (
                    {nameof(ProfessionalPassengerCar.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(ProfessionalPassengerCar.FireExtinguisher)} BIT,
                    {nameof(ProfessionalPassengerCar.LoadCapacity)} DECIMAL(18, 2),
                    {nameof(ProfessionalPassengerCar.RacingHarness)} BIT,
                    {nameof(ProfessionalPassengerCar.RollCage)} BIT,
                    {nameof(ProfessionalPassengerCar.PassengerCarId)} INT FOREIGN KEY REFERENCES {DatabaseTables.PassengerCar}({nameof(PassengerCar.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreateHeavyVehicleTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.HeavyVehicle}')
            BEGIN
                CREATE TABLE {DatabaseTables.HeavyVehicle} (
                    {nameof(HeavyVehicle.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(HeavyVehicle.Height)} DECIMAL(18, 2),
                    {nameof(HeavyVehicle.Length)} DECIMAL(18, 2),
                    {nameof(HeavyVehicle.VehicleId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Vehicles}({nameof(VehicleModels.Vehicle.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreateBusTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.Bus}')
            BEGIN
                CREATE TABLE {DatabaseTables.Bus} (
                    {nameof(Bus.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(Bus.SeatingCapacity)} INT,
                    {nameof(Bus.SleepingCapacity)} INT,
                    {nameof(Bus.Toilet)} BIT,
                    {nameof(Bus.HeavyVehicleId)} INT FOREIGN KEY REFERENCES {DatabaseTables.HeavyVehicle}({nameof(VehicleModels.HeavyVehicle.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreateTruckTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.Truck}')
            BEGIN
                CREATE TABLE {DatabaseTables.Truck} (
                    {nameof(Truck.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(Truck.LoadCapacity)} DECIMAL(18, 2),
                    {nameof(Truck.HeavyVehicleId)} INT FOREIGN KEY REFERENCES {DatabaseTables.HeavyVehicle}({nameof(VehicleModels.HeavyVehicle.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreateAuctionsTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.Auctions}')
            BEGIN
                CREATE TABLE {DatabaseTables.Auctions} (
                    {nameof(AuctionModels.Auction.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(AuctionModels.Auction.AuctionStatus)} INT,
                    {nameof(AuctionModels.Auction.EndDate)} DATETIME,
                    {nameof(AuctionModels.Auction.MinimumAmount)} DECIMAL(18, 2),
                    {nameof(AuctionModels.Auction.SellerId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Users}({nameof(AuctionModels.Auction.Id)}),
                    {nameof(AuctionModels.Auction.VehicleId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Vehicles}({nameof(AuctionModels.Auction.Id)}),
                    {nameof(AuctionModels.Auction.BaseId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Base}({nameof(Models.Base.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreateAuctionBidsTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.AuctionBids}')
            BEGIN
                CREATE TABLE {DatabaseTables.AuctionBids} (
                    {nameof(AuctionModels.AuctionBids.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(AuctionModels.AuctionBids.BidAmount)} DECIMAL(18, 2),
                    {nameof(AuctionModels.AuctionBids.BidderId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Users}({nameof(AuctionModels.AuctionBids.Id)}),
                    {nameof(AuctionModels.AuctionBids.AuctionId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Auctions}({nameof(AuctionModels.AuctionBids.Id)}),
                    {nameof(AuctionModels.AuctionBids.BaseId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Base}({nameof(Models.Base.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void CreateVehicleImagesTable()
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.VehicleImages}')
            BEGIN
                CREATE TABLE {DatabaseTables.VehicleImages} (
                    {nameof(VehicleModels.VehicleImage.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(VehicleModels.VehicleImage.Description)} NVARCHAR(255),
                    {nameof(VehicleModels.VehicleImage.Image)} VARCHAR(MAX),
                    {nameof(VehicleModels.VehicleImage.ImageHeight)} INT,
                    {nameof(VehicleModels.VehicleImage.ImageWidth)} INT,
                    {nameof(VehicleModels.VehicleImage.VehicleId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Vehicles}({nameof(VehicleModels.VehicleImage.Id)}),
                    {nameof(VehicleModels.VehicleImage.BaseId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Base}({nameof(Models.Base.Id)}));
            END
        ");

            ExecuteNonQuery(query.ToString());
        }

        private static void ExecuteNonQuery(string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, OpenNewConnection()))
            {
                cmd.ExecuteNonQuery();
            }
        }
        #endregion
    }
}