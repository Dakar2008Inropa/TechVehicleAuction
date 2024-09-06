CREATE TABLE Truck (
		Id Int PRIMARY KEY IDENTITY(1,1),
		LoadCapacity Int,
		VehicleId NVARCHAR(100)
		FOREIGN KEY (VehicleId) REFERENCES Vehicle(Id));