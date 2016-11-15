using Domain.Entities;
using System;

namespace OrderVehicle.Models
{
    public class SelectedCarViewModel
    {
        public Employee Employee { get; set; }

        public string VehicleMake { get; set; }

        public string VehicleModel { get; set; }

        public Guid WorkflowPersistenceId { get; set; }

        public int WorkflowId { get; set; }

        public string FleetManagerRemark { get; set; }
    }
}