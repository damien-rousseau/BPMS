using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace OrderVehicle.Models
{
    public class EmployeeSelectCarViewModel
    {
        public EmployeeSelectCarViewModel()
        {
            ListOfMakes = new List<SelectListItem>();
            ListOfModels = new List<SelectListItem>();
        }

        public IEnumerable<SelectListItem> ListOfMakes { get; set; }

        public string VehicleMake { get; set; }

        public IEnumerable<SelectListItem> ListOfModels { get; set; }

        public string VehicleModel { get; set; }

        public Guid WorkflowPersistenceId { get; set; }

        public int WorkflowId { get; set; }
    }
}