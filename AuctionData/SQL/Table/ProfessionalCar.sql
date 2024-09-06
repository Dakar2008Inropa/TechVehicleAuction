CREATE TABLE ProfessionalCar (
		Id INT PRIMARY KEY IDENTITY(1,1),
		RollCage Bit,
		FireExtinguisher Bit,
		RacingSeat Bit,
		RachingHarness Bit,
		LoadCapacity Int,
		CarId NVARCHAR(100)
		FOREIGN KEY (CarId) REFERENCES PersonalCar(Id));