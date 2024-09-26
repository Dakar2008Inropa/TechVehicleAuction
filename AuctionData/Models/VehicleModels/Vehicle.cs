namespace AuctionData.Models.VehicleModels
{
    public abstract class Vehicle : Base
    {
        private double _engineSize;

        public string? Maker { get; set; }
        public string? Model { get; set; }
        public int Mileage { get; set; }
        public string? LicensePlate { get; set; }
        public int ModelYear { get; set; }
        public bool Towinghitch { get; set; }
        public LicenseType LicenseType { get; set; }
        public double EngineSize
        {
            get => _engineSize;
            set
            {
                _engineSize = value;
                GetEngineSize();
            }
        }
        public FuelType FuelType { get; set; }
        public int FuelEconomy { get; set; }
        public int FuelCapacity { get; set; }
        public EnergyClass EnergyClass { get; set; }
        public string? Discriminator { get; set; }
        public List<VehicleImage>? VehicleImages { get; set; }
        public int BaseId { get; set; }



        public override string ToString()
        {
            return $"{Maker} {Model} ({ModelYear})";
        }

        public int GetEnergyClass()
        {
            if (FuelType == FuelType.Electric)
            {
                return 0;
            }

            var dieselPre2010 = new List<(double min, double max, int energyClass)>
            {
                (23, double.MaxValue, 0),
                (18, 23, 1),
                (13, 18, 2),
                (double.MinValue, 13, 3)
            };

            var gasolinePre2010 = new List<(double min, double max, int energyClass)>
            {
                (18, double.MaxValue, 0),
                (14, 18, 1),
                (10, 14, 2),
                (double.MinValue, 10, 3)
            };

            var dieselPost2010 = new List<(double min, double max, int energyClass)>
            {
                (25, double.MaxValue, 0),
                (20, 25, 1),
                (15, 20, 2),
                (double.MinValue, 15, 3)
            };

            var gasolinePost2010 = new List<(double min, double max, int energyClass)>
            {
                (20, double.MaxValue, 0),
                (16, 20, 1),
                (12, 16, 2),
                (double.MinValue, 12, 3)
            };

            var energyClassLookup = new Dictionary<(FuelType fuelType, bool isPost2010), List<(double min, double max, int energyClass)>>
            {
                { (FuelType.Diesel, false), dieselPre2010 },
                { (FuelType.Gasoline, false), gasolinePre2010 },
                { (FuelType.Diesel, true), dieselPost2010 },
                { (FuelType.Gasoline, true), gasolinePost2010 },
                { (FuelType.Hybrid, false), gasolinePre2010 },
                { (FuelType.Hybrid, true), gasolinePost2010 }
            };

            bool isPost2010 = ModelYear >= 2010;

            if (energyClassLookup.TryGetValue((FuelType, isPost2010), out var ranges))
            {
                var energyClass = ranges.Find(range => FuelEconomy >= range.min && FuelEconomy < range.max);
                return energyClass.energyClass;
            }

            return 0;
        }

        public static LicenseType GetLicenseType()
        {
            return LicenseType.B;
        }

        public double GetEngineSize()
        {
            return EngineSize;
        }
    }
}