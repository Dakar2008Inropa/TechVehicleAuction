using AuctionData.Models.UserModels;
using AuctionData.Models.VehicleModels;
using Microsoft.Data.SqlClient;
using System.Text;

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

        public static void CreateTables()
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString(Instance.Settings!)))
            {
                con.Open();

                CreateBaseTable(con);
                CreateUsersTable(con);
                CreatePrivateUserTable(con);
                CreateCorporateUserTable(con);
                CreateVehiclesTable(con);
                CreatePassengerCarTable(con);
                CreatePrivatePassengerCarTable(con);
                CreateProfessionalPassengerCarTable(con);
                CreateHeavyVehicleTable(con);
                CreateBusTable(con);
                CreateTruckTable(con);
                CreateAuctionsTable(con);
                CreateAuctionBidsTable(con);
                CreateVehicleImagesTable(con);

                con.Close();
            }
        }

        public static bool IsTablesCreated()
        {
            using (SqlConnection con = new SqlConnection(GetConnectionString(Instance.Settings!)))
            {
                con.Open();

                if (IsTableCreated(con, DatabaseTables.Base) &&
                    IsTableCreated(con, DatabaseTables.Users) &&
                    IsTableCreated(con, DatabaseTables.PrivateUser) &&
                    IsTableCreated(con, DatabaseTables.CorporateUser) &&
                    IsTableCreated(con, DatabaseTables.Vehicles) &&
                    IsTableCreated(con, DatabaseTables.PassengerCar) &&
                    IsTableCreated(con, DatabaseTables.PrivatePassengerCar) &&
                    IsTableCreated(con, DatabaseTables.ProfessionalPassengerCar) &&
                    IsTableCreated(con, DatabaseTables.HeavyVehicle) &&
                    IsTableCreated(con, DatabaseTables.Bus) &&
                    IsTableCreated(con, DatabaseTables.Truck) &&
                    IsTableCreated(con, DatabaseTables.Auctions) &&
                    IsTableCreated(con, DatabaseTables.AuctionBids) &&
                    IsTableCreated(con, DatabaseTables.VehicleImages))
                {
                    con.Close();
                    return true;
                }

                con.Close();
                return false;
            }
        }

        private static int InsertIntoBase(SqlConnection connection)
        {
            int baseId;

            StringBuilder Basequery = new StringBuilder();
            Basequery.Append($@"INSERT INTO {DatabaseTables.Base} ");
            Basequery.Append($@"({nameof(Base.CreatedAt)},");
            Basequery.Append($@"{nameof(Base.Status)})");
            Basequery.Append($@" VALUES ");
            Basequery.Append($@"(@{nameof(Base.CreatedAt)},");
            Basequery.Append($@"@{nameof(Base.Status)}); ");
            Basequery.Append($@"SELECT SCOPE_IDENTITY();");

            using (SqlCommand cmd = new SqlCommand(Basequery.ToString(), connection))
            {
                cmd.Parameters.AddWithValue($"@{nameof(Base.CreatedAt)}", DateTime.UtcNow);
                cmd.Parameters.AddWithValue($"@{nameof(Base.Status)}", (int)BaseStatus.Active);

                baseId = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return baseId;
        }

        private static bool IsTableCreated(SqlConnection con, string @base)
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

        private static void CreateBaseTable(SqlConnection con)
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.Base}')
            BEGIN
                CREATE TABLE {DatabaseTables.Base} (
                    {nameof(Base.Id)} INT PRIMARY KEY IDENTITY(1,1), 
                    {nameof(Base.CreatedAt)} DATETIME DEFAULT GETUTCDATE(),
                    {nameof(Base.UpdatedAt)} DATETIME NULL,
                    {nameof(Base.DeletedAt)} DATETIME NULL,
                    {nameof(Base.Status)} INT
                );
            END
        ");

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreateUsersTable(SqlConnection con)
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
                    {nameof(UserModels.User.BaseId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Base}({nameof(Base.Id)})
                );
            END
        ");

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreatePrivateUserTable(SqlConnection con)
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.PrivateUser}')
            BEGIN
                CREATE TABLE {DatabaseTables.PrivateUser} (
                    {nameof(Base.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(PrivateUser.CPRNumber)} NVARCHAR(50),
                    {nameof(PrivateUser.UserId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Users}({nameof(UserModels.User.Id)}));
            END
        ");

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreateCorporateUserTable(SqlConnection con)
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.CorporateUser}')
            BEGIN
                CREATE TABLE {DatabaseTables.CorporateUser} (
                    {nameof(Base.Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {nameof(CorporateUser.Credit)} DECIMAL(18, 2),
                    {nameof(CorporateUser.CvrNumber)} NVARCHAR(50),
                    {nameof(CorporateUser.UserId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Users}({nameof(UserModels.User.Id)}));
            END
        ");

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreateVehiclesTable(SqlConnection con)
        {
            StringBuilder query = new StringBuilder();
            query.Append($@"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.Vehicles}')
            BEGIN
                CREATE TABLE {DatabaseTables.Vehicles} (
                    {nameof(Base.Id)} INT PRIMARY KEY IDENTITY(1,1),
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
                    {nameof(VehicleModels.Vehicle.BaseId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Base}({nameof(Base.Id)}));
            END
        ");

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreatePassengerCarTable(SqlConnection con)
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

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreatePrivatePassengerCarTable(SqlConnection con)
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

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreateProfessionalPassengerCarTable(SqlConnection con)
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

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreateHeavyVehicleTable(SqlConnection con)
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

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreateBusTable(SqlConnection con)
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

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreateTruckTable(SqlConnection con)
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

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreateAuctionsTable(SqlConnection con)
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
                    {nameof(AuctionModels.Auction.BaseId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Base}({nameof(Base.Id)}));
            END
        ");

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreateAuctionBidsTable(SqlConnection con)
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
                    {nameof(AuctionModels.AuctionBids.BaseId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Base}({nameof(Base.Id)}));
            END
        ");

            ExecuteNonQuery(con, query.ToString());
        }

        private static void CreateVehicleImagesTable(SqlConnection con)
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
                    {nameof(VehicleModels.VehicleImage.BaseId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Base}({nameof(Base.Id)}));
            END
        ");

            ExecuteNonQuery(con, query.ToString());
        }

        private static void ExecuteNonQuery(SqlConnection con, string query)
        {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.ExecuteNonQuery();
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
    }
}