using System.Data.Entity;
using Domain;
using Domain.Entities;

namespace DAL.Base
{
    public class WfCustomDatabaseContext : DbContext
    {
        public WfCustomDatabaseContext() : base("WFCustomDatabase")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var employee = modelBuilder.Entity<Employee>();

            employee.ToTable("Employee");
            employee.HasKey(x => x.Id);
            employee.Property(x => x.FirstName);
            employee.Property(x => x.LastName);
            employee.Property(x => x.EmailAddress);
            employee.HasRequired(x => x.Company);

            var company = modelBuilder.Entity<Company>();
            company.ToTable("Company");
            company.HasKey(x => x.Id);
            company.Property(x => x.Name);

            var vehicleOrderWorkflow = modelBuilder.Entity<VehicleOrderWorkflowObject>();
            vehicleOrderWorkflow.ToTable("VehicleOrderWorkflow");
            vehicleOrderWorkflow.HasKey(x => x.Id);
            vehicleOrderWorkflow.Property(x => x.PersistedWorkflowId);
            vehicleOrderWorkflow.Property(x => x.CreatedAt);
            vehicleOrderWorkflow.HasRequired(x => x.Employee);
            vehicleOrderWorkflow.Property(x => x.ManagerRemark);
            vehicleOrderWorkflow.Property(x => x.ManagerApproval);
            vehicleOrderWorkflow.Property(x => x.VehicleMake);
            vehicleOrderWorkflow.Property(x => x.VehicleModel);
            vehicleOrderWorkflow.Property(x => x.FleetManagerRemark);

            var vehicleModels = modelBuilder.Entity<VehicleModel>();
            vehicleModels.ToTable("VehicleModel");
            vehicleModels.HasKey(x => x.Id);
            vehicleModels.Property(x => x.Name);
            vehicleModels.HasRequired(x => x.Make);

            var vehicleMake = modelBuilder.Entity<VehicleMake>();
            vehicleMake.ToTable("VehicleMake");
            vehicleMake.HasKey(x => x.Id);
            vehicleMake.Property(x => x.Name);
            vehicleMake.HasMany(x => x.Models);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Employee> Employee { get; set; }

        public DbSet<Company> Company { get; set; }

        public DbSet<VehicleMake> VehicleMake { get; set; }

        public DbSet<VehicleModel> VehicleModel { get; set; }
    }
}