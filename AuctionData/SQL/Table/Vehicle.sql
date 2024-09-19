CREATE TABLE Vehicle ( 
		Id INT PRIMARY KEY IDENTITY (1,1),
		Maker VARCHAR(25),
		Model VARCHAR(25),
		Odometer Int,
		LicensePlate VARCHAR(15),
		ModelYear Int,
		Towinghitch Bit,
		LicenseType Int NOT NULL DEFAULT 1,
		EngineSize Decimal,
		FuelEconomy Int,
		FuelCapacity Int,
		EnergyClass Int NOT NULL DEFAULT 1,
		FOREIGN KEY (LicenseType) references LicenseType (Id),
		FOREIGN KEY (EnergyClass) references EnergyClass (Id));

