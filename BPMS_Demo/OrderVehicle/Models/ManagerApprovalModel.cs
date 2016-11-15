using System;
using Domain.Entities;

namespace OrderVehicle.Models
{
    public class ManagerApprovalModel
    {
        public int WorkflowId { get; set; }

        public Guid WorkflowPersistenceId { get; set; }

        public Employee Employee { get; set; }

        public string ManagerRemark { get; set; }
    }
}