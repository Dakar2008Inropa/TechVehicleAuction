using AuctionData.Models.VehicleModels;
using Microsoft.Data.SqlClient;
using System.Text;

namespace AuctionData.Models.Database
{
    public partial class Database
    {
        public static class Vehicle
        {
            public static int CreateVehicle(VehicleModels.Vehicle vh, string discriminator)
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString(Instance.Settings!)))
                {
                    con.Open();

                    int baseId = InsertIntoBase(con);
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
                            VI.BaseId = baseId;
                        }
                        CreateVehicleImages(vh.VehicleImages, con);
                    }

                    con.Close();

                    return vehicleId;
                }
            }

            public static int CreatePassengerCar(VehicleModels.PassengerCar pc, string discriminator)
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString(Instance.Settings!)))
                {
                    con.Open();
                    int vehicleId = CreateVehicle(pc, discriminator);
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
                    con.Close();
                    return passengerCarId;
                }
            }

            public static bool CreatePrivatePassengerCar(VehicleModels.PrivatePassengerCar pc)
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(GetConnectionString(Instance.Settings!)))
                    {
                        con.Open();
                        int passengerCarId = CreatePassengerCar(pc, "PrivatePassengerCar");

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
                        con.Close();
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
                    using (SqlConnection con = new SqlConnection(GetConnectionString(Instance.Settings!)))
                    {
                        con.Open();
                        int passengerCarId = CreatePassengerCar(pc, "ProfessionalPassengerCar");

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

            public static void CreateVehicleImages(List<VehicleImage> vehicleImages, SqlConnection con)
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
        }
    }
}