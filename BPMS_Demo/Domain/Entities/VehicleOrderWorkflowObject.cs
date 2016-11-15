using System;

namespace Domain.Entities
{
    public class VehicleOrderWorkflowObject : WorkflowObject
    {
        public string ManagerRemark { get; set; }

        public DateTime? ManagerApproval { get; set; }

        public string VehicleMake { get; set; }

        public string VehicleModel { get; set; }

        public string FleetManagerRemark { get; set; }
    }
}
