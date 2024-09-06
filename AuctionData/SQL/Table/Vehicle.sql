CREATE TABLE Vehicle ( 
		Id VARCHAR(100) PRIMARY KEY,
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
		constraint fk_LicenseType FOREIGN KEY (LicenseType) references LicenseType (Id),
		constraint fk_EnergyClass FOREIGN KEY (EnergyClass) references EnergyClass (Id));

