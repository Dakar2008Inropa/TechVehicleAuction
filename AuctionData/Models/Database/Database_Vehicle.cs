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
            private static int CreateVehicle(VehicleModels.Vehicle vh, SqlConnection con, string discriminator)
            {
                int baseId = Base.InsertIntoBase(con);
                int vehicleId;

                StringBuilder vehicleQuery = new StringBuilder();
                vehicleQuery.Append($@"INSERT INTO {DatabaseTables.Vehicles} ");

                vehicleQuery.Append($@"({nameof(VehicleModels.Vehicle.Maker)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.Model)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.ModelYear)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.Mileage)},");
                vehicleQuery.Append($@"{nameof(VehicleModels.Vehicle.LicensePlate)},");
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
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.ModelYear)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.Mileage)},");
                vehicleQuery.Append($@"@{nameof(VehicleModels.Vehicle.LicensePlate)},");
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

                using (SqlCommand cmd = new SqlCommand(vehicleQuery.ToString(), con))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Maker)}", vh.Maker);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Model)}", vh.Model);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.ModelYear)}", vh.ModelYear);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Mileage)}", vh.Mileage);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.LicensePlate)}", vh.LicensePlate);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Towinghitch)}", vh.Towinghitch);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.LicenseType)}", (int)vh.LicenseType);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.EngineSize)}", vh.EngineSize);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.FuelType)}", (int)vh.FuelType);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.FuelEconomy)}", vh.FuelEconomy);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.FuelCapacity)}", vh.FuelCapacity);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.EnergyClass)}", (int)vh.EnergyClass);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Discriminator)}", discriminator);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.BaseId)}", baseId);

                    vehicleId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                if (vh.VehicleImages != null)
                {
                    foreach (VehicleImage VI in vh.VehicleImages)
                    {
                        VI.VehicleId = vehicleId;
                        VI.BaseId = Base.InsertIntoBase(con);
                    }
                    CreateVehicleImages(vh.VehicleImages, con);
                }

                return vehicleId;
            }

            #region Passenger Car

            private static int CreatePassengerCar(VehicleModels.PassengerCar pc, SqlConnection con, string discriminator)
            {
                int vehicleId = CreateVehicle(pc, con, discriminator);
                int passengerCarId;

                StringBuilder passengerCarQuery = new StringBuilder();
                passengerCarQuery.Append($@"INSERT INTO {DatabaseTables.PassengerCar} ");
                passengerCarQuery.Append($@"({nameof(VehicleModels.PassengerCar.VehicleId)},");
                passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.RequireCommercialLicense)},");
                passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.SeatCapacity)},");
                passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.TrunkHeight)},");
                passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.TrunkLength)},");
                passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.TrunkWidth)},");
                passengerCarQuery.Append($@"{nameof(VehicleModels.PassengerCar.TrunkDimensions)});");

                passengerCarQuery.Append($@" VALUES ");

                passengerCarQuery.Append($@"(@{nameof(VehicleModels.PassengerCar.VehicleId)},");
                passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.RequireCommercialLicense)},");
                passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.SeatCapacity)},");
                passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.TrunkHeight)},");
                passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.TrunkLength)},");
                passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.TrunkWidth)},");
                passengerCarQuery.Append($@"@{nameof(VehicleModels.PassengerCar.TrunkDimensions)});");

                using (SqlCommand cmd = new SqlCommand(passengerCarQuery.ToString(), con))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.VehicleId)}", vehicleId);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.RequireCommercialLicense)}", pc.RequireCommercialLicense);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.SeatCapacity)}", pc.SeatCapacity);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.TrunkHeight)}", pc.TrunkHeight);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.TrunkLength)}", pc.TrunkLength);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.TrunkWidth)}", pc.TrunkWidth);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PassengerCar.TrunkDimensions)}", pc.TrunkDimensions);

                    passengerCarId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                return passengerCarId;
            }

            public static bool CreatePrivatePassengerCar(VehicleModels.PrivatePassengerCar pc)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        int passengerCarId = CreatePassengerCar(pc, con, "PrivatePassengerCar");

                        StringBuilder privatePassengerCarQuery = new StringBuilder();
                        privatePassengerCarQuery.Append($@"INSERT INTO {DatabaseTables.PrivatePassengerCar} ");
                        privatePassengerCarQuery.Append($@"({nameof(VehicleModels.PrivatePassengerCar.PassengerCarId)},");
                        privatePassengerCarQuery.Append($@"({nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)});");

                        privatePassengerCarQuery.Append($@" VALUES ");

                        privatePassengerCarQuery.Append($@"(@{nameof(VehicleModels.PrivatePassengerCar.PassengerCarId)},");
                        privatePassengerCarQuery.Append($@"@{nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)});");

                        using (SqlCommand cmd = new SqlCommand(privatePassengerCarQuery.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PrivatePassengerCar.PassengerCarId)}", passengerCarId);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)}", pc.IsofixMounts);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }

            public static bool CreateProfessionalPassengerCar(VehicleModels.ProfessionalPassengerCar pc)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        int passengerCarId = CreatePassengerCar(pc, con, "ProfessionalPassengerCar");

                        StringBuilder professionalPassengerCarQuery = new StringBuilder();
                        professionalPassengerCarQuery.Append($@"INSERT INTO {DatabaseTables.ProfessionalPassengerCar} ");
                        professionalPassengerCarQuery.Append($@"({nameof(VehicleModels.ProfessionalPassengerCar.PassengerCarId)},");
                        professionalPassengerCarQuery.Append($@"({nameof(VehicleModels.ProfessionalPassengerCar.RollCage)},");
                        professionalPassengerCarQuery.Append($@"({nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)},");
                        professionalPassengerCarQuery.Append($@"({nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)},");
                        professionalPassengerCarQuery.Append($@"({nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)},");
                        professionalPassengerCarQuery.Append($@"({nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)});");

                        professionalPassengerCarQuery.Append($@" VALUES ");

                        professionalPassengerCarQuery.Append($@"(@{nameof(VehicleModels.ProfessionalPassengerCar.PassengerCarId)},");
                        professionalPassengerCarQuery.Append($@"@{nameof(VehicleModels.ProfessionalPassengerCar.RollCage)},");
                        professionalPassengerCarQuery.Append($@"@{nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)},");
                        professionalPassengerCarQuery.Append($@"@{nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)},");
                        professionalPassengerCarQuery.Append($@"@{nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)},");
                        professionalPassengerCarQuery.Append($@"@{nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)});");


                        using (SqlCommand cmd = new SqlCommand(professionalPassengerCarQuery.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.PassengerCarId)}", passengerCarId);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.RollCage)}", pc.RollCage);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)}", pc.FireExtinguisher);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)}", pc.RacingSeat);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)}", pc.RacingHarness);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)}", pc.LoadCapacity);

                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }

                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }

            #endregion

            #region Heavy Vehicle

            private static int CreateHeavyVehicle(VehicleModels.HeavyVehicle hv, SqlConnection con, string discriminator)
            {
                int vehicleId = CreateVehicle(hv, con, discriminator);
                int heavyVehicleId;

                StringBuilder heavyVehicleQuery = new StringBuilder();
                heavyVehicleQuery.Append($@"INSERT INTO {DatabaseTables.HeavyVehicle} ");
                heavyVehicleQuery.Append($@"({nameof(VehicleModels.HeavyVehicle.VehicleId)},");
                heavyVehicleQuery.Append($@"({nameof(VehicleModels.HeavyVehicle.Height)},");
                heavyVehicleQuery.Append($@"({nameof(VehicleModels.HeavyVehicle.Length)},");
                heavyVehicleQuery.Append($@"({nameof(VehicleModels.HeavyVehicle.Weight)});");

                heavyVehicleQuery.Append($@" VALUES ");

                heavyVehicleQuery.Append($@"(@{nameof(VehicleModels.HeavyVehicle.VehicleId)},");
                heavyVehicleQuery.Append($@"@{nameof(VehicleModels.HeavyVehicle.Height)},");
                heavyVehicleQuery.Append($@"@{nameof(VehicleModels.HeavyVehicle.Length)},");
                heavyVehicleQuery.Append($@"@{nameof(VehicleModels.HeavyVehicle.Weight)});");

                using (SqlCommand cmd = new SqlCommand(heavyVehicleQuery.ToString(), con))
                {
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.HeavyVehicle.VehicleId)}", vehicleId);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.HeavyVehicle.Height)}", hv.Height);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.HeavyVehicle.Length)}", hv.Length);
                    cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.HeavyVehicle.Weight)}", hv.Weight);

                    heavyVehicleId = Convert.ToInt32(cmd.ExecuteScalar());
                }
                return heavyVehicleId;
            }

            public static bool CreateBus(VehicleModels.Bus bus)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        int heavyVehicleId = CreateHeavyVehicle(bus, con, "Bus");

                        StringBuilder busQuery = new StringBuilder();
                        busQuery.Append($@"INSERT INTO {DatabaseTables.Bus} ");
                        busQuery.Append($@"({nameof(VehicleModels.Bus.HeavyVehicleId)},");
                        busQuery.Append($@"({nameof(VehicleModels.Bus.SeatingCapacity)},");
                        busQuery.Append($@"({nameof(VehicleModels.Bus.SleepingCapacity)},");
                        busQuery.Append($@"({nameof(VehicleModels.Bus.Toilet)});");

                        busQuery.Append($@" VALUES ");

                        busQuery.Append($@"(@{nameof(VehicleModels.Bus.HeavyVehicleId)},");
                        busQuery.Append($@"@{nameof(VehicleModels.Bus.SeatingCapacity)},");
                        busQuery.Append($@"@{nameof(VehicleModels.Bus.SleepingCapacity)},");
                        busQuery.Append($@"@{nameof(VehicleModels.Bus.Toilet)});");

                        using (SqlCommand cmd = new SqlCommand(busQuery.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Bus.HeavyVehicleId)}", heavyVehicleId);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Bus.SeatingCapacity)}", bus.SeatingCapacity);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Bus.SleepingCapacity)}", bus.SleepingCapacity);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Bus.Toilet)}", bus.Toilet);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }

            public static bool CreateTruck(VehicleModels.Truck truck)
            {
                try
                {
                    using (SqlConnection con = OpenNewConnection())
                    {
                        int heavyVehicleId = CreateHeavyVehicle(truck, con, "Truck");

                        StringBuilder truckQuery = new StringBuilder();
                        truckQuery.Append($@"INSERT INTO {DatabaseTables.Truck} ");
                        truckQuery.Append($@"({nameof(VehicleModels.Truck.HeavyVehicleId)},");
                        truckQuery.Append($@"({nameof(VehicleModels.Truck.LoadCapacity)});");

                        truckQuery.Append($@" VALUES ");

                        truckQuery.Append($@"(@{nameof(VehicleModels.Truck.HeavyVehicleId)},");
                        truckQuery.Append($@"@{nameof(VehicleModels.Truck.LoadCapacity)});");

                        using (SqlCommand cmd = new SqlCommand(truckQuery.ToString(), con))
                        {
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Truck.HeavyVehicleId)}", heavyVehicleId);
                            cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Truck.LoadCapacity)}", truck.LoadCapacity);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }

            #endregion

            private static void CreateVehicleImages(List<VehicleImage> vehicleImages, SqlConnection con)
            {
                StringBuilder vehicleImageQuery = new StringBuilder();
                vehicleImageQuery.Append($@"INSERT INTO {DatabaseTables.VehicleImages} ");
                vehicleImageQuery.Append($@"({nameof(VehicleImage.Image)},");
                vehicleImageQuery.Append($@"{nameof(VehicleImage.Description)},");
                vehicleImageQuery.Append($@"{nameof(VehicleImage.ImageWidth)},");
                vehicleImageQuery.Append($@"{nameof(VehicleImage.ImageHeight)},");
                vehicleImageQuery.Append($@"{nameof(VehicleImage.VehicleId)},");
                vehicleImageQuery.Append($@"{nameof(VehicleImage.BaseId)});");

                vehicleImageQuery.Append($@" VALUES ");

                vehicleImageQuery.Append($@"(@{nameof(VehicleImage.Image)},");
                vehicleImageQuery.Append($@"@{nameof(VehicleImage.Description)},");
                vehicleImageQuery.Append($@"@{nameof(VehicleImage.ImageWidth)},");
                vehicleImageQuery.Append($@"@{nameof(VehicleImage.ImageHeight)},");
                vehicleImageQuery.Append($@"@{nameof(VehicleImage.VehicleId)},");
                vehicleImageQuery.Append($@"@{nameof(VehicleImage.BaseId)});");

                foreach (VehicleImage VI in vehicleImages)
                {
                    using (SqlCommand cmd = new SqlCommand(vehicleImageQuery.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.Image)}", VI.Image);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.Description)}", VI.Description);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.ImageWidth)}", VI.ImageWidth);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.ImageHeight)}", VI.ImageHeight);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.VehicleId)}", VI.VehicleId);
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleImage.BaseId)}", VI.BaseId);

                        cmd.ExecuteNonQuery();
                    }
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

                    query.Append($@"ppc.{nameof(VehicleModels.PrivatePassengerCar.Id)},");
                    query.Append($@"ppc.{nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)},");

                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.Id)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.RollCage)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)},");
                    query.Append($@"prpc.{nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)},");

                    query.Append($@"hv.{nameof(VehicleModels.HeavyVehicle.Height)},");
                    query.Append($@"hv.{nameof(VehicleModels.HeavyVehicle.Weight)},");
                    query.Append($@"hv.{nameof(VehicleModels.HeavyVehicle.Length)},");

                    query.Append($@"bus.{nameof(VehicleModels.Bus.Id)},");
                    query.Append($@"bus.{nameof(VehicleModels.Bus.SeatingCapacity)},");
                    query.Append($@"bus.{nameof(VehicleModels.Bus.SleepingCapacity)},");
                    query.Append($@"bus.{nameof(VehicleModels.Bus.Toilet)},");

                    query.Append($@"truck.{nameof(VehicleModels.Truck.Id)},");
                    query.Append($@"truck.{nameof(VehicleModels.Truck.LoadCapacity)},");

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

                    query.Append($@"WHERE v.{nameof(VehicleModels.Vehicle.Id)} = @{nameof(VehicleModels.Vehicle.Id)}");

                    using (SqlCommand cmd = new SqlCommand(query.ToString(), con))
                    {
                        cmd.Parameters.AddWithValue($"@{nameof(VehicleModels.Vehicle.Id)}", vehicleId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string? discriminator = reader[nameof(VehicleModels.Vehicle.Discriminator)].ToString();

                                VehicleModels.Vehicle vehicle;

                                switch (discriminator)
                                {
                                    case "PrivatePassengerCar":
                                        vehicle = new PrivatePassengerCar
                                        {
                                            Id = Convert.ToInt32(reader[nameof(VehicleModels.PrivatePassengerCar.Id)]),
                                            IsofixMounts = Convert.ToBoolean(reader[nameof(VehicleModels.PrivatePassengerCar.IsofixMounts)])
                                        };
                                        break;
                                    case "ProfessionalPassengerCar":
                                        vehicle = new ProfessionalPassengerCar
                                        {
                                            Id = Convert.ToInt32(reader[nameof(VehicleModels.ProfessionalPassengerCar.Id)]),
                                            RollCage = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.RollCage)]),
                                            FireExtinguisher = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.FireExtinguisher)]),
                                            RacingSeat = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.RacingSeat)]),
                                            RacingHarness = Convert.ToBoolean(reader[nameof(VehicleModels.ProfessionalPassengerCar.RacingHarness)]),
                                            LoadCapacity = Convert.ToInt32(reader[nameof(VehicleModels.ProfessionalPassengerCar.LoadCapacity)])
                                        };
                                        break;
                                    case "Bus":
                                        vehicle = new Bus
                                        {
                                            Id = Convert.ToInt32(reader[nameof(VehicleModels.Bus.Id)]),
                                            SeatingCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Bus.SeatingCapacity)]),
                                            SleepingCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Bus.SleepingCapacity)]),
                                            Toilet = Convert.ToBoolean(reader[nameof(VehicleModels.Bus.Toilet)])
                                        };
                                        break;
                                    case "Truck":
                                        vehicle = new Truck
                                        {
                                            Id = Convert.ToInt32(reader[nameof(VehicleModels.Truck.Id)]),
                                            LoadCapacity = Convert.ToInt32(reader[nameof(VehicleModels.Truck.LoadCapacity)])
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
                                vehicle.Discriminator = discriminator;
                                vehicle.CreatedAt = Convert.ToDateTime(reader[nameof(Models.Base.CreatedAt)]);
                                vehicle.UpdatedAt = Convert.ToDateTime(reader[nameof(Models.Base.UpdatedAt)]);
                                vehicle.DeletedAt = Convert.ToDateTime(reader[nameof(Models.Base.DeletedAt)]);
                                vehicle.Status = (BaseStatus)Convert.ToInt32(reader[nameof(Models.Base.Status)]);

                                return vehicle;
                            }
                        }
                    }
                }
                return null;
            }
            #endregion
        }
    }
}