using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Domain.Entities;

namespace DAL.Base
{
    public class WfCustomDatabaseContextInitializer : DropCreateDatabaseIfModelChanges<WfCustomDatabaseContext>
    {
        protected override void Seed(WfCustomDatabaseContext context)
        {
            var companies = new List<Company>
            {
                new Company
                {
                    Id = 1,
                    Name = "Avanade"
                }
            };

            var manager = new Employee
            {
                Id = 1,
                FirstName = "Manager1",
                LastName = "Manager1",
                EmailAddress = "me@me.com",
                Company = companies.First(),
            };

            var listOfEmployees = new List<Employee>
            {
                manager,
                new Employee
                {
                    Id = 1,
                    FirstName = "Employee1",
                    LastName = "Employee1",
                    EmailAddress = "me@me.com",
                    Company = companies.First(),
                    Manager = manager
                },
                new Employee
                {
                    Id = 2,
                    FirstName = "Employee2",
                    LastName = "Employee2",
                    EmailAddress = "me@me.com",
                    Company = companies.First(),
                    Manager = manager
                },
            };

            var listofMakes = new List<VehicleMake>
            {
                new VehicleMake
                {
                    Id = 1,
                    Name = "Audi",
                },

                new VehicleMake
                {
                    Id = 2,
                    Name = "BMW",
                }
            };

            var audiModels = new List<VehicleModel>
            {
                new VehicleModel
                {
                    Id = 1,
                    Name = "A1",
                    Make = listofMakes.First()
                },
                new VehicleModel
                {
                    Id = 2,
                    Name = "A3",
                    Make = listofMakes.First()
                }
            };

            var bmwModels = new List<VehicleModel>
            {
                new VehicleModel
                {
                    Id = 3,
                    Name = "1",
                    Make = listofMakes.Last()
                },
                new VehicleModel
                {
                    Id = 4,
                    Name = "3",
                    Make = listofMakes.Last()
                }
            };

            companies.ForEach(entity => context.Company.Add(entity));
            listOfEmployees.ForEach(entity => context.Employee.Add(entity));
            audiModels.ForEach(entity => context.VehicleModel.Add(entity));
            bmwModels.ForEach(entity => context.VehicleModel.Add(entity));
            listofMakes.ForEach(entity => context.VehicleMake.Add(entity));
        }
    }
}