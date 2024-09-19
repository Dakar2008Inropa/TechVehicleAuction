using AuctionData.Models.AuctionModels;
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
            }
        }

        private static void CreateBaseTable(SqlConnection con)
        {
            StringBuilder query = new StringBuilder();
            query.Append(@$"
            IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{DatabaseTables.Base}')
            BEGIN
                CREATE TABLE {DatabaseTables.Base} (
                    {Utility.GetPropertyName(() => new PrivateUser().Id)} INT PRIMARY KEY IDENTITY(1,1),
                    {Utility.GetPropertyName(() => new PrivateUser().CreatedAt)} DATETIME,
                    {Utility.GetPropertyName(() => new PrivateUser().UpdatedAt)} DATETIME,
                    {Utility.GetPropertyName(() => new PrivateUser().DeletedAt)} DATETIME,
                    {Utility.GetPropertyName(() => new PrivateUser().Status)} INT
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
                    {Utility.GetPropertyName(() => new PrivateUser().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.Base}({Utility.GetPropertyName(() => new PrivateUser().Id)}),
                    {Utility.GetPropertyName(() => new PrivateUser().PostalCode)} NVARCHAR(50),
                    {Utility.GetPropertyName(() => new PrivateUser().UserName)} NVARCHAR(255),
                    {Utility.GetPropertyName(() => new PrivateUser().Discriminator)} NVARCHAR(50)
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
                    {Utility.GetPropertyName(() => new PrivateUser().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.Users}({Utility.GetPropertyName(() => new PrivateUser().Id)}),
                    {Utility.GetPropertyName(() => new PrivateUser().CPRNumber)} NVARCHAR(50)
                );
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
                    {Utility.GetPropertyName(() => new PrivateUser().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.Users}({Utility.GetPropertyName(() => new PrivateUser().Id)}),
                    {Utility.GetPropertyName(() => new CorporateUser().Credit)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new CorporateUser().CvrNumber)} NVARCHAR(50)
                );
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
                    {Utility.GetPropertyName(() => new PrivateUser().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.Base}({Utility.GetPropertyName(() => new PrivateUser().Id)}),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().Maker)} NVARCHAR(120),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().Model)} NVARCHAR(120),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().Mileage)} INT,
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().ModelYear)} INT,
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().EngineSize)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().EnergyClass)} INT,
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().FuelCapacity)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().FuelEconomy)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().FuelType)} INT,
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().LicensePlate)} NVARCHAR(50),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().LicenseType)} INT,
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().Towinghitch)} BIT,
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().Discriminator)} NVARCHAR(50)
                );
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
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.Vehicles}({Utility.GetPropertyName(() => new PrivatePassengerCar().Id)}),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().RequireCommercialLicense)} BIT,
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().SeatCapacity)} INT,
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().TrunkHeight)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().TrunkLength)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().TrunkWidth)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().TrunkDimensions)} FLOAT
                );
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
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.PassengerCar}({Utility.GetPropertyName(() => new PrivatePassengerCar().Id)}),
                    {Utility.GetPropertyName(() => new PrivatePassengerCar().IsofixMounts)} BIT
                );
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
                    {Utility.GetPropertyName(() => new ProfessionalPassengerCar().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.PassengerCar}({Utility.GetPropertyName(() => new ProfessionalPassengerCar().Id)}),
                    {Utility.GetPropertyName(() => new ProfessionalPassengerCar().FireExtinguisher)} BIT,
                    {Utility.GetPropertyName(() => new ProfessionalPassengerCar().LoadCapacity)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new ProfessionalPassengerCar().RacingHarness)} BIT,
                    {Utility.GetPropertyName(() => new ProfessionalPassengerCar().RollCage)} BIT
                );
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
                    {Utility.GetPropertyName(() => new Bus().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.Vehicles}({Utility.GetPropertyName(() => new Bus().Id)}),
                    {Utility.GetPropertyName(() => new Bus().Height)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new Bus().Length)} DECIMAL(18, 2)
                );
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
                    {Utility.GetPropertyName(() => new Bus().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.HeavyVehicle}({Utility.GetPropertyName(() => new Bus().Id)}),
                    {Utility.GetPropertyName(() => new Bus().SeatingCapacity)} INT,
                    {Utility.GetPropertyName(() => new Bus().SleepingCapacity)} INT,
                    {Utility.GetPropertyName(() => new Bus().Toilet)} BIT
                );
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
                    {Utility.GetPropertyName(() => new Truck().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.HeavyVehicle}({Utility.GetPropertyName(() => new Truck().Id)}),
                    {Utility.GetPropertyName(() => new Truck().LoadCapacity)} DECIMAL(18, 2)
                );
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
                    {Utility.GetPropertyName(() => new AuctionModels.Auction().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.Base}({Utility.GetPropertyName(() => new AuctionModels.Auction().Id)}),
                    {Utility.GetPropertyName(() => new AuctionModels.Auction().AuctionStatus)} INT,
                    {Utility.GetPropertyName(() => new AuctionModels.Auction().EndDate)} DATETIME,
                    {Utility.GetPropertyName(() => new AuctionModels.Auction().MinimumAmount)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new AuctionModels.Auction().SellerId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Users}({Utility.GetPropertyName(() => new AuctionModels.Auction().Id)}),
                    {Utility.GetPropertyName(() => new AuctionModels.Auction().VehicleId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Vehicles}({Utility.GetPropertyName(() => new AuctionModels.Auction().Id)})
                );
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
                    {Utility.GetPropertyName(() => new AuctionBids().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.Base}({Utility.GetPropertyName(() => new AuctionBids().Id)}),
                    {Utility.GetPropertyName(() => new AuctionBids().BidAmount)} DECIMAL(18, 2),
                    {Utility.GetPropertyName(() => new AuctionBids().BidderId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Users}({Utility.GetPropertyName(() => new AuctionBids().Id)}),
                    {Utility.GetPropertyName(() => new AuctionBids().AuctionId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Auctions}({Utility.GetPropertyName(() => new AuctionBids().Id)})
                );
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
                    {Utility.GetPropertyName(() => new VehicleImage().Id)} INT PRIMARY KEY FOREIGN KEY REFERENCES {DatabaseTables.Base}({Utility.GetPropertyName(() => new VehicleImage().Id)}),
                    {Utility.GetPropertyName(() => new VehicleImage().Description)} NVARCHAR(255),
                    {Utility.GetPropertyName(() => new VehicleImage().Image)} VARCHAR(MAX),
                    {Utility.GetPropertyName(() => new VehicleImage().ImageHeight)} INT,
                    {Utility.GetPropertyName(() => new VehicleImage().ImageWidth)} INT,
                    {Utility.GetPropertyName(() => new VehicleImage().VehicleId)} INT FOREIGN KEY REFERENCES {DatabaseTables.Vehicles}({Utility.GetPropertyName(() => new VehicleImage().Id)})
                );
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