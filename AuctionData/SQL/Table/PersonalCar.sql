CREATE TABLE PersonalCar (
		Id Int PRIMARY KEY IDENTITY(1,1),
		TrunkWidth Decimal,
		TrunkHeight Decimal,
		TrunkLength Decimal,
		SeatCapacity Int,
		RequireCommercialLicense Bit,
		TrunkDimensions Decimal,
		VehicleId NVARCHAR(100)
		FOREIGN KEY (VehicleId) REFERENCES Vehicle(Id));