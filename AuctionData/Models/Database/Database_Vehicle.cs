using AuctionData.Models.VehicleModels;
using Microsoft.Data.SqlClient;
using System.Text;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static class Vehicle
        {
            #region Create

            public static bool CreateVehicleAndAuction(VehicleModels.Vehicle vehicle, AuctionModels.Auction auction, int userId)
            {
                using (SqlConnection con = OpenNewConnection())
                {
                    using (SqlTransaction trans = con.BeginTransaction())
                    {
                        try
                        {
                            int vehicleId = CreateVehicle(con, trans, vehicle);
                            Auction.CreateAuction(con, trans, auction, userId, vehicleId);

                            trans.Commit();
                            log.Info("Auction created");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            log.Error("Something went wrong when trying to create an auction in database", ex);
                            trans.Rollback();
                            return false;
                        }
                    }
                }
            }

            private static int CreateVehicle(SqlConnection con, SqlTransaction trans, VehicleModels.Vehicle vehicle)
            {
                StringBuilder vehicleQuery = new StringBuilder();
                vehicleQuery.Append($@"INSERT INTO {DatabaseTables.Vehicles} ");
                vehicleQuery.Append($@"({nameof(VehicleModels.Vehicle.Maker)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.Model)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.Mileage)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.LicensePlate)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.ModelYear)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.Towinghitch)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.LicenseType)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.EngineSize)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.FuelType)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.FuelEconomy)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.FuelCapacity)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.EnergyClass)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.Discriminator)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.BaseId)})");

                vehicleQuery.Append($@" VALUES ");

                vehicleQuery.Append($@"(@{nameof(VehicleModels.Vehicle.Maker)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.Model)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.Mileage)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.LicensePlate)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.ModelYear)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.Towinghitch)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.LicenseType)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.EngineSize)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.FuelType)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.FuelEconomy)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.FuelCapacity)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.EnergyClass)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.Discriminator)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.BaseId)});");
                vehicleQuery.Append($@"SELECT SCOPE_IDENTITY();");

                int baseId = Base.InsertIntoBase(con, trans);

                using (SqlCommand cmd = new SqlCommand(vehicleQuery.ToString(), con, trans))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Maker)}", vehicle.Maker);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Model)}", vehicle.Model);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Mileage)}", vehicle.Mileage);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.LicensePlate)}", vehicle.LicensePlate);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.ModelYear)}", vehicle.ModelYear);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Towinghitch)}", vehicle.Towinghitch);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.LicenseType)}", (int)vehicle.LicenseType);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.EngineSize)}", vehicle.EngineSize);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.FuelType)}", (int)vehicle.FuelType);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.FuelEconomy)}", vehicle.FuelEconomy);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.FuelCapacity)}", vehicle.FuelCapacity);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.EnergyClass)}", (int)vehicle.EnergyClass);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Discriminator)}", vehicle.Discriminator);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.BaseId)}", baseId);

                    vehicle.Id = Convert.ToInt32(cmd.ExecuteScalar());
                }

                switch (vehicle.Discriminator)
                {
                    case "PrivatePassengerCar":
                    case "ProfessionalPassengerCar":
                        CreatePassengerCar(vehicle, con, trans);
                        break;
                    case "Bus":
                    case "Truck":
                        CreateHeavyVehicle(vehicle, con, trans);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown vehicle type");
                }

                CreateVehicleImages(vehicle, con, trans);

                return vehicle.Id;
            }

            private static void CreatePassengerCar(VehicleModels.Vehicle vehicle, SqlConnection con, SqlTransaction trans)
            {
                try
                {
                    StringBuilder passengerCarQuery = new StringBuilder();
                    passengerCarQuery.Append($@"INSERT INTO {DatabaseTables.PassengerCar} ");
                    passengerCarQuery.Append($@"({nameof(VehicleModels.PassengerCar.VehicleId)},");
                    passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.TrunkWidth)},");
                    passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.TrunkHeight)},");
                    passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.TrunkLength)},");
                    passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.SeatCapacity)},");
                    passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.RequireCommercialLicense)},");
                    passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.TrunkDimensions)})");

                    passengerCarQuery.Append($@" VALUES ");

                    passengerCarQuery.Append($@"(@{nameof(VehicleModels.PassengerCar.VehicleId)},");
                    passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.TrunkWidth)},");
                    passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.TrunkHeight)},");
                    passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.TrunkLength)},");
                    passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.SeatCapacity)},");
                    passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.RequireCommercialLicense)},");
                    passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.TrunkDimensions)});");
                    passengerCarQuery.Append($@"SELECT SCOPE_IDENTITY();");

                    int passengerCarId;

                    using (SqlCommand cmd = new SqlCommand(passengerCarQuery.ToString(), con, trans))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.VehicleId)}", vehicle.Id);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.TrunkWidth)}", ((PassengerCar)vehicle).TrunkWidth);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.TrunkHeight)}", ((PassengerCar)vehicle).TrunkHeight);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.TrunkLength)}", ((PassengerCar)vehicle).TrunkLength);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.SeatCapacity)}", ((PassengerCar)vehicle).SeatCapacity);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.RequireCommercialLicense)}", ((PassengerCar)vehicle).RequireCommercialLicense);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.TrunkDimensions)}", ((PassengerCar)vehicle).TrunkDimensions);

                        passengerCarId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (vehicle is PrivatePassengerCar car)
                    {
                        StringBuilder ppcQuery = new StringBuilder();
                        ppcQuery.Append($@"INSERT INTO {DatabaseTables.PrivatePassengerCar} ");
                        ppcQuery.Append($@"({nameof(VehicleModels.PrivatePassengerCar.PassengerCarId)},");
                        ppcQuery.Append($@"{nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)})");

                        ppcQuery.Append($@" VALUES ");

                        ppcQuery.Append($@"(@{nameof(VehicleModels.PrivatePassengerCar.PassengerCarId)},");
                        ppcQuery.Append($@"@{nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)});");

                        using (SqlCommand cmd = new SqlCommand(ppcQuery.ToString(), con, trans))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PrivatePassengerCar.PassengerCarId)}", passengerCarId);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)}", car.IsofixMounts);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (vehicle is ProfessionalPassengerCar pcar)
                    {
                        StringBuilder prpcQuery = new StringBuilder();
                        prpcQuery.Append($@"INSERT INTO {DatabaseTables.ProfessionalPassengerCar} ");
                        prpcQuery.Append($@"({nameof(VehicleModels.ProfessionalPassengerCar.PassengerCarId)},");
                        prpcQuery.Append($@"{nameof(VehicleModels.ProfessionalPassengerCar.RollCage)},");
                        prpcQuery.Append($@"{nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)},");
                        prpcQuery.Append($@"{nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)},");
                        prpcQuery.Append($@"{nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)},");
                        prpcQuery.Append($@"{nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)})");

                        prpcQuery.Append($@" VALUES ");

                        prpcQuery.Append($@"(@{nameof(VehicleModels.ProfessionalPassengerCar.PassengerCarId)},");
                        prpcQuery.Append($@"@{nameof(VehicleModels.ProfessionalPassengerCar.RollCage)},");
                        prpcQuery.Append($@"@{nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)},");
                        prpcQuery.Append($@"@{nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)},");
                        prpcQuery.Append($@"@{nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)},");
                        prpcQuery.Append($@"@{nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)});");

                        using (SqlCommand cmd = new SqlCommand(prpcQuery.ToString(), con, trans))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.PassengerCarId)}", passengerCarId);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.RollCage)}", pcar.RollCage);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)}", pcar.FireExtinguisher);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)}", pcar.RacingSeat);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)}", pcar.RacingHarness);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)}", pcar.LoadCapacity);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }

            private static void CreateHeavyVehicle(VehicleModels.Vehicle vehicle, SqlConnection con, SqlTransaction trans)
            {
                try
                {
                    StringBuilder heavyVehicleQuery = new StringBuilder();
                    heavyVehicleQuery.Append($@"INSERT INTO {DatabaseTables.HeavyVehicle} ");
                    heavyVehicleQuery.Append($@"({nameof(VehicleModels.HeavyVehicle.VehicleId)},");
                    heavyVehicleQuery.Append($@"{nameof(VehicleModels.HeavyVehicle.Height)},");
                    heavyVehicleQuery.Append($@"{nameof(VehicleModels.HeavyVehicle.Weight)},");
                    heavyVehicleQuery.Append($@"{nameof(VehicleModels.HeavyVehicle.Length)})");

                    heavyVehicleQuery.Append($@" VALUES ");

                    heavyVehicleQuery.Append($@"(@{nameof(VehicleModels.HeavyVehicle.VehicleId)},");
                    heavyVehicleQuery.Append($@"@{nameof(VehicleModels.HeavyVehicle.Height)},");
                    heavyVehicleQuery.Append($@"@{nameof(VehicleModels.HeavyVehicle.Weight)},");
                    heavyVehicleQuery.Append($@"@{nameof(VehicleModels.HeavyVehicle.Length)});");
                    heavyVehicleQuery.Append($@"SELECT SCOPE_IDENTITY();");

                    int heavyVehicleId;

                    using (SqlCommand cmd = new SqlCommand(heavyVehicleQuery.ToString(), con, trans))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.HeavyVehicle.VehicleId)}", vehicle.Id);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.HeavyVehicle.Height)}", ((HeavyVehicle)vehicle).Height);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.HeavyVehicle.Weight)}", ((HeavyVehicle)vehicle).Weight);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.HeavyVehicle.Length)}", ((HeavyVehicle)vehicle).Length);

                        heavyVehicleId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (vehicle is Bus bus)
                    {
                        StringBuilder busQuery = new StringBuilder();
                        busQuery.Append($@"INSERT INTO {DatabaseTables.Bus} ");
                        busQuery.Append($@"({nameof(VehicleModels.Bus.HeavyVehicleId)},");
                        busQuery.Append($@"{nameof(VehicleModels.Bus.SeatingCapacity)},");
                        busQuery.Append($@"{nameof(VehicleModels.Bus.SleepingCapacity)},");
                        busQuery.Append($@"{nameof(VehicleModels.Bus.Toilet)})");

                        busQuery.Append($@" VALUES ");

                        busQuery.Append($@"(@{nameof(VehicleModels.Bus.HeavyVehicleId)},");
                        busQuery.Append($@"@{nameof(VehicleModels.Bus.SeatingCapacity)},");
                        busQuery.Append($@"@{nameof(VehicleModels.Bus.SleepingCapacity)},");
                        busQuery.Append($@"@{nameof(VehicleModels.Bus.Toilet)});");

                        using (SqlCommand cmd = new SqlCommand(busQuery.ToString(), con, trans))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Bus.HeavyVehicleId)}", heavyVehicleId);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Bus.SeatingCapacity)}", bus.SeatingCapacity);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Bus.SleepingCapacity)}", bus.SleepingCapacity);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Bus.Toilet)}", bus.Toilet);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    else if (vehicle is Truck truck)
                    {
                        StringBuilder truckQuery = new StringBuilder();
                        truckQuery.Append($@"INSERT INTO {DatabaseTables.Truck} ");
                        truckQuery.Append($@"({nameof(VehicleModels.Truck.HeavyVehicleId)},");
                        truckQuery.Append($@"{nameof(VehicleModels.Truck.LoadCapacity)})");

                        truckQuery.Append($@" VALUES ");

                        truckQuery.Append($@"(@{nameof(VehicleModels.Truck.HeavyVehicleId)},");
                        truckQuery.Append($@"@{nameof(VehicleModels.Truck.LoadCapacity)});");

                        using (SqlCommand cmd = new SqlCommand(truckQuery.ToString(), con, trans))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Truck.HeavyVehicleId)}", heavyVehicleId);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Truck.LoadCapacity)}", truck.LoadCapacity);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex);
                    throw;
                }
            }

            private static void CreateVehicleImages(VehicleModels.Vehicle vehicle, SqlConnection con, SqlTransaction trans)
            {
                try
                {
                    StringBuilder vehicleImageQuery = new StringBuilder();
                    vehicleImageQuery.Append($@"INSERT INTO {DatabaseTables.VehicleImages} ");
                    vehicleImageQuery.Append($@"({nameof(VehicleImage.Image)},");
                    vehicleImageQuery.Append($@"{nameof(VehicleImage.Description)},");
                    vehicleImageQuery.Append($@"{nameof(VehicleImage.ImageWidth)},");
                    vehicleImageQuery.Append($@"{nameof(VehicleImage.ImageHeight)},");
                    vehicleImageQuery.Append($@"{nameof(VehicleImage.VehicleId)},");
                    vehicleImageQuery.Append($@"{nameof(VehicleImage.BaseId)})");

                    vehicleImageQuery.Append($@" VALUES ");

                    vehicleImageQuery.Append($@"(@{nameof(VehicleImage.Image)},");
                    vehicleImageQuery.Append($@"@{nameof(VehicleImage.Description)},");
                    vehicleImageQuery.Append($@"@{nameof(VehicleImage.ImageWidth)},");
                    vehicleImageQuery.Append($@"@{nameof(VehicleImage.ImageHeight)},");
                    vehicleImageQuery.Append($@"@{nameof(VehicleImage.VehicleId)},");
                    vehicleImageQuery.Append($@"@{nameof(VehicleImage.BaseId)});");

                    if (vehicle.VehicleImages != null && vehicle.VehicleImages.Any())
                    {
                        foreach (VehicleImage image in vehicle.VehicleImages)
                        {
                            int baseId = Base.InsertIntoBase(con, trans);

                            using (SqlCommand cmd = new SqlCommand(vehicleImageQuery.ToString(), con, trans))
                            {
                                cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.Image)}", image.Image);
                                cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.Description)}", string.IsNullOrEmpty(image.Description) ? "" : image.Description);
                                cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.ImageWidth)}", image.ImageWidth);
                                cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.ImageHeight)}", image.ImageHeight);
                                cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.VehicleId)}", vehicle.Id);
                                cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.BaseId)}", baseId);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Could not add image to database", ex);
                }
            }
            #endregion

            #region Read
            public static VehicleModels.Vehicle GetVehicle(int vehicleId)
            {
                using (SqlConnection con = OpenNewConnection())
                {
                    StringBuilder query = new StringBuilder();
                    query.Append($@"SELECT ");

                    query.Append($@"v.{nameof(VehicleModels.Vehicle.Maker)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.Model)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.Mileage)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.LicensePlate)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.ModelYear)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.Towinghitch)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.LicenseType)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.EngineSize)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.FuelType)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.FuelEconomy)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.FuelCapacity)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.EnergyClass)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.Discriminator)},");

                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.TrunkWidth)},");
                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.TrunkHeight)},");
                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.TrunkLength)},");
                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.SeatCapacity)},");
                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.RequireCommercialLicense)},");
                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.TrunkDimensions)},");

                    query.Append($@"ppc.{nameof(VehicleModels.PrivatePassengerCar.Id)} AS PrivatePassengerCarId,");
                    query.Append($@"ppc.{nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)},");

                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.Id)} AS ProfessionalPassengerCarId,");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.RollCage)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)},");

                    query.Append($@"hv.{nameof(VehicleModels.HeavyVehicle.Height)},");
                    query.Append($@"hv.{nameof(VehicleModels.HeavyVehicle.Weight)},");
                    query.Append($@"hv.{nameof(VehicleModels.HeavyVehicle.Length)},");

                    query.Append($@"bus.{nameof(VehicleModels.Bus.Id)} AS BusId,");
                    query.Append($@"bus.{nameof(VehicleModels.Bus.SeatingCapacity)},");
                    query.Append($@"bus.{nameof(VehicleModels.Bus.SleepingCapacity)},");
                    query.Append($@"bus.{nameof(VehicleModels.Bus.Toilet)},");

                    query.Append($@"truck.{nameof(VehicleModels.Truck.Id)} AS TruckId,");
                    query.Append($@"truck.{nameof(VehicleModels.Truck.LoadCapacity)},");

                    query.Append($@"vehicleImage.{nameof(VehicleImage.Id)} AS VehicleImageId,");
                    query.Append($@"vehicleImage.{nameof(VehicleImage.Image)},");
                    query.Append($@"vehicleImage.{nameof(VehicleImage.Description)},");
                    query.Append($@"vehicleImage.{nameof(VehicleImage.ImageWidth)},");
                    query.Append($@"vehicleImage.{nameof(VehicleImage.ImageHeight)},");

                    query.Append($@"b.{nameof(Models.Base.CreatedAt)},");
                    query.Append($@"b.{nameof(Models.Base.UpdatedAt)},");
                    query.Append($@"b.{nameof(Models.Base.DeletedAt)},");
                    query.Append($@"b.{nameof(Models.Base.Status)}");

                    query.Append($@" FROM ");

                    query.Append($@"{DatabaseTables.Vehicles} v ");
                    query.Append($@"LEFT JOIN {DatabaseTables.PassengerCar} pc ON v.{nameof(VehicleModels.Vehicle.Id)} = pc.{nameof(VehicleModels.PassengerCar.VehicleId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.PrivatePassengerCar} ppc ON pc.{nameof(VehicleModels.PassengerCar.Id)} = ppc.{nameof(VehicleModels.PrivatePassengerCar.PassengerCarId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.ProfessionalPassengerCar} prpc ON pc.{nameof(VehicleModels.PassengerCar.Id)} = prpc.{nameof(VehicleModels.ProfessionalPassengerCar.PassengerCarId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.HeavyVehicle} hv ON v.{nameof(VehicleModels.Vehicle.Id)} = hv.{nameof(VehicleModels.HeavyVehicle.VehicleId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.Bus} bus ON hv.{nameof(VehicleModels.HeavyVehicle.Id)} = bus.{nameof(VehicleModels.Bus.HeavyVehicleId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.Truck} truck ON hv.{nameof(VehicleModels.HeavyVehicle.Id)} = truck.{nameof(VehicleModels.Truck.HeavyVehicleId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.Base} b ON v.{nameof(VehicleModels.Vehicle.BaseId)} = b.{nameof(Models.Base.Id)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.VehicleImages} vehicleImage ON v.{nameof(VehicleModels.Vehicle.Id)} = vehicleImage.{nameof(VehicleImage.VehicleId)} ");

                    query.Append($@"WHERE v.{nameof(VehicleModels.Vehicle.Id)} = @{nameof(VehicleModels.Vehicle.Id)}");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Id)}", vehicleId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            VehicleModels.Vehicle vehicle = null;
                            List<VehicleImage> vehicleImages = new List<VehicleImage>();

                            while (reader.Read())
                            {
                                if (vehicle == null)
                                {
                                    string? discriminator = reader[nameof(VehicleModels.Vehicle.Discriminator)].ToString();

                                    switch (discriminator)
                                    {
                                        case "PrivatePassengerCar":
                                            vehicle = new PrivatePassengerCar
                                            {
                                                Id = Convert.ToInt32(reader["PrivatePassengerCarId"]),
                                                TrunkHeight = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkHeight)]),
                                                TrunkWidth = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkWidth)]),
                                                TrunkLength = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkLength)]),
                                                SeatCapacity = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.SeatCapacity)]),
                                                RequireCommercialLicense = Convert.ToBoolean(reader[nameof(VehicleModels.PassengerCar.RequireCommercialLicense)]),
                                                TrunkDimensions = Convert.ToDouble(reader[nameof(VehicleModels.PassengerCar.TrunkDimensions)]),
                                                IsofixMounts = Convert.ToBoolean(reader[nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)])
                                            };
                                            break;
                                        case "ProfessionalPassengerCar":
                                            vehicle = new ProfessionalPassengerCar
                                            {
                                                Id = Convert.ToInt32(reader["ProfessionalPassengerCarId"]),
                                                RollCage = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.RollCage)]),
                                                FireExtinguisher = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)]),
                                                RacingSeat = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)]),
                                                RacingHarness = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)]),
                                                LoadCapacity = Convert.ToInt32(reader[nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)]),
                                                TrunkHeight = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkHeight)]),
                                                TrunkWidth = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkWidth)]),
                                                TrunkLength = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkLength)]),
                                                SeatCapacity = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.SeatCapacity)]),
                                                RequireCommercialLicense = Convert.ToBoolean(reader[nameof(VehicleModels.PassengerCar.RequireCommercialLicense)]),
                                                TrunkDimensions = Convert.ToDouble(reader[nameof(VehicleModels.PassengerCar.TrunkDimensions)]),
                                            };
                                            break;
                                        case "Bus":
                                            vehicle = new Bus
                                            {
                                                Id = Convert.ToInt32(reader["BusId"]),
                                                SeatingCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Bus.SeatingCapacity)]),
                                                SleepingCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Bus.SleepingCapacity)]),
                                                Toilet = Convert.ToBoolean(reader[nameof(VehicleModels.Bus.Toilet)]),
                                                Height = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Height)]),
                                                Weight = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Weight)]),
                                                Length = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Length)])
                                            };
                                            break;
                                        case "Truck":
                                            vehicle = new Truck
                                            {
                                                Id = Convert.ToInt32(reader["TruckId"]),
                                                LoadCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Truck.LoadCapacity)]),
                                                Height = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Height)]),
                                                Weight = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Weight)]),
                                                Length = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Length)])
                                            };
                                            break;
                                        default:
                                            throw new InvalidOperationException("Unknown vehicle type");
                                    }

                                    vehicle.Maker = reader[nameof(VehicleModels.Vehicle.Maker)].ToString();
                                    vehicle.Model = reader[nameof(VehicleModels.Vehicle.Model)].ToString();
                                    vehicle.Mileage = Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.Mileage)]);
                                    vehicle.LicensePlate = reader[nameof(VehicleModels.Vehicle.LicensePlate)].ToString();
                                    vehicle.ModelYear = Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.ModelYear)]);
                                    vehicle.Towinghitch = Convert.ToBoolean(reader[nameof(VehicleModels.Vehicle.Towinghitch)]);
                                    vehicle.LicenseType = (LicenseType)Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.LicenseType)]);
                                    vehicle.EngineSize = Convert.ToDouble(reader[nameof(VehicleModels.Vehicle.EngineSize)]);
                                    vehicle.FuelType = (FuelType)Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.FuelType)]);
                                    vehicle.FuelEconomy = Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.FuelEconomy)]);
                                    vehicle.FuelCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.FuelCapacity)]);
                                    vehicle.EnergyClass = (EnergyClass)Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.EnergyClass)]);
                                    vehicle.CreatedAt = Convert.ToDateTime(reader[nameof(Models.Base.CreatedAt)]);
                                    vehicle.UpdatedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.UpdatedAt))) ? null : Convert.ToDateTime(reader[nameof(Models.Base.UpdatedAt)]);
                                    vehicle.DeletedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.DeletedAt))) ? null : Convert.ToDateTime(reader[nameof(Models.Base.DeletedAt)]);
                                    vehicle.Status = (BaseStatus)Convert.ToInt32(reader[nameof(Models.Base.Status)]);
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("VehicleImageId")))
                                {
                                    vehicleImages.Add(new VehicleImage
                                    {
                                        Id = Convert.ToInt32(reader["VehicleImageId"]),
                                        Image = reader["Image"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        ImageWidth = Convert.ToInt32(reader["ImageWidth"]),
                                        ImageHeight = Convert.ToInt32(reader["ImageHeight"]),
                                        VehicleId = vehicleId
                                    });
                                }
                            }

                            if (vehicle != null)
                            {
                                vehicle.VehicleImages = vehicleImages;
                                return vehicle;
                            }
                        }
                    }
                }
                return null;
            }

            public static VehicleModels.Vehicle GetVehicle(int vehicleId, SqlConnection con)
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    query.Append($@"SELECT ");

                    query.Append($@"v.{nameof(VehicleModels.Vehicle.Maker)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.Model)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.Mileage)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.LicensePlate)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.ModelYear)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.Towinghitch)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.LicenseType)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.EngineSize)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.FuelType)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.FuelEconomy)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.FuelCapacity)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.EnergyClass)},");
                    query.Append($@"v.{nameof(VehicleModels.Vehicle.Discriminator)},");

                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.TrunkWidth)},");
                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.TrunkHeight)},");
                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.TrunkLength)},");
                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.SeatCapacity)},");
                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.RequireCommercialLicense)},");
                    query.Append($@"pc.{nameof(VehicleModels.PassengerCar.TrunkDimensions)},");

                    query.Append($@"ppc.{nameof(VehicleModels.PrivatePassengerCar.Id)} AS PrivatePassengerCarId,");
                    query.Append($@"ppc.{nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)},");

                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.Id)} AS ProfessionalPassengerCarId,");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.RollCage)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)},");

                    query.Append($@"hv.{nameof(VehicleModels.HeavyVehicle.Height)},");
                    query.Append($@"hv.{nameof(VehicleModels.HeavyVehicle.Weight)},");
                    query.Append($@"hv.{nameof(VehicleModels.HeavyVehicle.Length)},");

                    query.Append($@"bus.{nameof(VehicleModels.Bus.Id)} AS BusId,");
                    query.Append($@"bus.{nameof(VehicleModels.Bus.SeatingCapacity)},");
                    query.Append($@"bus.{nameof(VehicleModels.Bus.SleepingCapacity)},");
                    query.Append($@"bus.{nameof(VehicleModels.Bus.Toilet)},");

                    query.Append($@"truck.{nameof(VehicleModels.Truck.Id)} AS TruckId,");
                    query.Append($@"truck.{nameof(VehicleModels.Truck.LoadCapacity)},");

                    query.Append($@"vehicleImage.{nameof(VehicleImage.Id)} AS VehicleImageId,");
                    query.Append($@"vehicleImage.{nameof(VehicleImage.Image)},");
                    query.Append($@"vehicleImage.{nameof(VehicleImage.Description)},");
                    query.Append($@"vehicleImage.{nameof(VehicleImage.ImageWidth)},");
                    query.Append($@"vehicleImage.{nameof(VehicleImage.ImageHeight)},");

                    query.Append($@"b.{nameof(Models.Base.CreatedAt)},");
                    query.Append($@"b.{nameof(Models.Base.UpdatedAt)},");
                    query.Append($@"b.{nameof(Models.Base.DeletedAt)},");
                    query.Append($@"b.{nameof(Models.Base.Status)}");

                    query.Append($@" FROM ");

                    query.Append($@"{DatabaseTables.Vehicles} v ");
                    query.Append($@"LEFT JOIN {DatabaseTables.PassengerCar} pc ON v.{nameof(VehicleModels.Vehicle.Id)} = pc.{nameof(VehicleModels.PassengerCar.VehicleId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.PrivatePassengerCar} ppc ON pc.{nameof(VehicleModels.PassengerCar.Id)} = ppc.{nameof(VehicleModels.PrivatePassengerCar.PassengerCarId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.ProfessionalPassengerCar} prpc ON pc.{nameof(VehicleModels.PassengerCar.Id)} = prpc.{nameof(VehicleModels.ProfessionalPassengerCar.PassengerCarId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.HeavyVehicle} hv ON v.{nameof(VehicleModels.Vehicle.Id)} = hv.{nameof(VehicleModels.HeavyVehicle.VehicleId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.Bus} bus ON hv.{nameof(VehicleModels.HeavyVehicle.Id)} = bus.{nameof(VehicleModels.Bus.HeavyVehicleId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.Truck} truck ON hv.{nameof(VehicleModels.HeavyVehicle.Id)} = truck.{nameof(VehicleModels.Truck.HeavyVehicleId)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.Base} b ON v.{nameof(VehicleModels.Vehicle.BaseId)} = b.{nameof(Models.Base.Id)} ");
                    query.Append($@"LEFT JOIN {DatabaseTables.VehicleImages} vehicleImage ON v.{nameof(VehicleModels.Vehicle.Id)} = vehicleImage.{nameof(VehicleImage.VehicleId)} ");

                    query.Append($@"WHERE v.{nameof(VehicleModels.Vehicle.Id)} = @{nameof(VehicleModels.Vehicle.Id)}");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Id)}", vehicleId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            VehicleModels.Vehicle vehicle = null;
                            List<VehicleImage> vehicleImages = new List<VehicleImage>();

                            while (reader.Read())
                            {
                                if (vehicle == null)
                                {
                                    string? discriminator = reader[nameof(VehicleModels.Vehicle.Discriminator)].ToString();

                                    switch (discriminator)
                                    {
                                        case "PrivatePassengerCar":
                                            vehicle = new PrivatePassengerCar
                                            {
                                                Id = Convert.ToInt32(reader["PrivatePassengerCarId"]),
                                                TrunkHeight = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkHeight)]),
                                                TrunkWidth = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkWidth)]),
                                                TrunkLength = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkLength)]),
                                                SeatCapacity = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.SeatCapacity)]),
                                                RequireCommercialLicense = Convert.ToBoolean(reader[nameof(VehicleModels.PassengerCar.RequireCommercialLicense)]),
                                                TrunkDimensions = Convert.ToDouble(reader[nameof(VehicleModels.PassengerCar.TrunkDimensions)]),
                                                IsofixMounts = Convert.ToBoolean(reader[nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)])
                                            };
                                            break;
                                        case "ProfessionalPassengerCar":
                                            vehicle = new ProfessionalPassengerCar
                                            {
                                                Id = Convert.ToInt32(reader["ProfessionalPassengerCarId"]),
                                                RollCage = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.RollCage)]),
                                                FireExtinguisher = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)]),
                                                RacingSeat = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)]),
                                                RacingHarness = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)]),
                                                LoadCapacity = Convert.ToInt32(reader[nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)]),
                                                TrunkHeight = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkHeight)]),
                                                TrunkWidth = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkWidth)]),
                                                TrunkLength = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.TrunkLength)]),
                                                SeatCapacity = Convert.ToInt32(reader[nameof(VehicleModels.PassengerCar.SeatCapacity)]),
                                                RequireCommercialLicense = Convert.ToBoolean(reader[nameof(VehicleModels.PassengerCar.RequireCommercialLicense)]),
                                                TrunkDimensions = Convert.ToDouble(reader[nameof(VehicleModels.PassengerCar.TrunkDimensions)]),
                                            };
                                            break;
                                        case "Bus":
                                            vehicle = new Bus
                                            {
                                                Id = Convert.ToInt32(reader["BusId"]),
                                                SeatingCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Bus.SeatingCapacity)]),
                                                SleepingCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Bus.SleepingCapacity)]),
                                                Toilet = Convert.ToBoolean(reader[nameof(VehicleModels.Bus.Toilet)]),
                                                Height = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Height)]),
                                                Weight = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Weight)]),
                                                Length = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Length)])
                                            };
                                            break;
                                        case "Truck":
                                            vehicle = new Truck
                                            {
                                                Id = Convert.ToInt32(reader["TruckId"]),
                                                LoadCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Truck.LoadCapacity)]),
                                                Height = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Height)]),
                                                Weight = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Weight)]),
                                                Length = Convert.ToInt32(reader[nameof(VehicleModels.HeavyVehicle.Length)])
                                            };
                                            break;
                                        default:
                                            throw new InvalidOperationException("Unknown vehicle type");
                                    }

                                    vehicle.Maker = reader[nameof(VehicleModels.Vehicle.Maker)].ToString();
                                    vehicle.Model = reader[nameof(VehicleModels.Vehicle.Model)].ToString();
                                    vehicle.Mileage = Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.Mileage)]);
                                    vehicle.LicensePlate = reader[nameof(VehicleModels.Vehicle.LicensePlate)].ToString();
                                    vehicle.ModelYear = Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.ModelYear)]);
                                    vehicle.Towinghitch = Convert.ToBoolean(reader[nameof(VehicleModels.Vehicle.Towinghitch)]);
                                    vehicle.LicenseType = (LicenseType)Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.LicenseType)]);
                                    vehicle.EngineSize = Convert.ToDouble(reader[nameof(VehicleModels.Vehicle.EngineSize)]);
                                    vehicle.FuelType = (FuelType)Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.FuelType)]);
                                    vehicle.FuelEconomy = Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.FuelEconomy)]);
                                    vehicle.FuelCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.FuelCapacity)]);
                                    vehicle.EnergyClass = (EnergyClass)Convert.ToInt32(reader[nameof(VehicleModels.Vehicle.EnergyClass)]);
                                    vehicle.CreatedAt = Convert.ToDateTime(reader[nameof(Models.Base.CreatedAt)]);
                                    vehicle.UpdatedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.UpdatedAt))) ? null : Convert.ToDateTime(reader[nameof(Models.Base.UpdatedAt)]);
                                    vehicle.DeletedAt = reader.IsDBNull(reader.GetOrdinal(nameof(Models.Base.DeletedAt))) ? null : Convert.ToDateTime(reader[nameof(Models.Base.DeletedAt)]);
                                    vehicle.Status = (BaseStatus)Convert.ToInt32(reader[nameof(Models.Base.Status)]);
                                }

                                if (!reader.IsDBNull(reader.GetOrdinal("VehicleImageId")))
                                {
                                    vehicleImages.Add(new VehicleImage
                                    {
                                        Id = Convert.ToInt32(reader["VehicleImageId"]),
                                        Image = reader["Image"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        ImageWidth = Convert.ToInt32(reader["ImageWidth"]),
                                        ImageHeight = Convert.ToInt32(reader["ImageHeight"]),
                                        VehicleId = vehicleId
                                    });
                                }
                            }

                            if (vehicle != null)
                            {
                                vehicle.VehicleImages = vehicleImages;
                                return vehicle;
                            }
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    log.Error("Could not retrieve vehicle", ex);
                    return null;
                }
            }
            #endregion
        }
    }
}