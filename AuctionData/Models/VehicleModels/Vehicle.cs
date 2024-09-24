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

        public string GetEnergyClass()
        {
            if (FuelType == FuelType.Electric)
            {
                return "A";
            }

            var dieselPre2010 = new List<(double min, double max, string energyClass)>
            {
                (23, double.MaxValue, "A"),
                (18, 23, "B"),
                (13, 18, "C"),
                (double.MinValue, 13, "D")
            };

            var gasolinePre2010 = new List<(double min, double max, string energyClass)>
            {
                (18, double.MaxValue, "A"),
                (14, 18, "B"),
                (10, 14, "C"),
                (double.MinValue, 10, "D")
            };

            var dieselPost2010 = new List<(double min, double max, string energyClass)>
            {
                (25, double.MaxValue, "A"),
                (20, 25, "B"),
                (15, 20, "C"),
                (double.MinValue, 15, "D")
            };

            var gasolinePost2010 = new List<(double min, double max, string energyClass)>
            {
                (20, double.MaxValue, "A"),
                (16, 20, "B"),
                (12, 16, "C"),
                (double.MinValue, 12, "D")
            };

            var energyClassLookup = new Dictionary<(FuelType fuelType, bool isPost2010), List<(double min, double max, string energyClass)>>
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
                return energyClass.energyClass ?? "Unknown";
            }

            return "Unknown";
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