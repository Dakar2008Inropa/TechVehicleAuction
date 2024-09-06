CREATE TABLE PrivatePersonalCar (
		Id INT PRIMARY KEY IDENTITY(1,1),
		IsofixMounts Bit,
		CarId NVARCHAR(100)
		FOREIGN KEY (CarId) REFERENCES PersonalCar(Id));
