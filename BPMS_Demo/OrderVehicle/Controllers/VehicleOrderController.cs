using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ActivityLibrary;
using Contracts;
using Domain.Entities;
using OrderVehicle.Models;

namespace OrderVehicle.Controllers
{
    public class VehicleOrderController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IVehicleOrderService<VehicleOrderWf> _vehicleOrderService;
        private readonly IVehicleService _vehicleService;

        public VehicleOrderController(IEmployeeService employeeService, IVehicleOrderService<VehicleOrderWf> vehicleOrderService, IVehicleService vehicleService)
        {
            if (employeeService == null) throw new ArgumentNullException(nameof(employeeService));
            if (vehicleOrderService == null) throw new ArgumentNullException(nameof(vehicleOrderService));
            if (vehicleService == null) throw new ArgumentNullException(nameof(vehicleService));

            _employeeService = employeeService;
            _vehicleOrderService = vehicleOrderService;
            _vehicleService = vehicleService;
        }

        // GET: VehicleOrder
        public ActionResult Index()
        {
            return RedirectToAction("StartWorkflow");
        }

        public ActionResult StartWorkflow()
        {
            var employees = _employeeService.GetAllEmployee();
            var viewModel = employees.Where(e => !e.FirstName.Contains("Manager")).Select(employee => new SelectListItem { Value = employee.Id.ToString(), Text = string.Join(" ", employee.FirstName, employee.LastName) }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult StartWorkflow(int employee)
        {
            var employeeObject = _employeeService.GetById(employee);

            var vehicleOrderWf = new VehicleOrderWorkflowObject
            {
                Employee = employeeObject,
                CreatedAt = DateTime.Now,
            };

            var dictionary = new Dictionary<string, object>
            {
                {"Employee", employeeObject},
                {"Link", Url.Action("HumanStep", "VehicleOrder", null, Request.Url.Scheme)}
            };

            // Start the workflow
            _vehicleOrderService.StartWorkflow(dictionary, vehicleOrderWf);

            return RedirectToAction("StartWorkflow");
        }

        public ActionResult HumanStep(Guid workflowId)
        {
            var stepName = _vehicleOrderService.GetBookmark(workflowId);
            return RedirectToAction(stepName, new { id = workflowId });
        }

        [HttpPost]
        public ActionResult Resume(object decision, Guid workflowId)
        {
            _vehicleOrderService.GoToNextStep(workflowId, decision);
            return View("Resume");
        }

        public ActionResult ManagerApproval(Guid id)
        {
            var workflowObject = _vehicleOrderService.GetWorkflowObjectByWorkflowPersistenceId(id);

            var viewModel = new ManagerApprovalModel
            {
                WorkflowPersistenceId = workflowObject.PersistedWorkflowId,
                Employee = workflowObject.Employee,
                ManagerRemark = workflowObject.ManagerRemark
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ManagerApproval(string submitButton, ManagerApprovalModel viewModel)
        {
            _vehicleOrderService.SaveManagerRemark(viewModel.WorkflowPersistenceId, viewModel.ManagerRemark);

            var decision = false;
            if (submitButton.Equals("Accept", StringComparison.InvariantCultureIgnoreCase))
            {
                decision = true;
            }

            return Resume(decision, viewModel.WorkflowPersistenceId);
        }

        public ActionResult EmployeeSelectCar(Guid id)
        {
            var workflowObject = _vehicleOrderService.GetWorkflowObjectByWorkflowPersistenceId(id);

            var listOfMakes = _vehicleService.GetAllMakes();

            var viewModel = new EmployeeSelectCarViewModel
            {
                WorkflowPersistenceId = workflowObject.PersistedWorkflowId,
                ListOfMakes = listOfMakes.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                }),
                ListOfModels = listOfMakes.First().Models.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                })
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EmployeeSelectCar(EmployeeSelectCarViewModel viewModel)
        {
            _vehicleOrderService.SaveVehicleSelection(viewModel.WorkflowPersistenceId, Convert.ToInt32(viewModel.VehicleMake), Convert.ToInt32(viewModel.VehicleModel));
            return Resume(null, viewModel.WorkflowPersistenceId);
        }

        public ActionResult GetModels(string makeId)
        {
            var models = _vehicleService.GetAllModels(Convert.ToInt32(makeId));
            var viewModel = new Dictionary<string, string>();
            foreach (var m in models)
            {
                viewModel.Add(m.Id.ToString(), m.Name);
            }
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApprovalByFleetManager(Guid id)
        {
            var workflowObject = _vehicleOrderService.GetWorkflowObjectByWorkflowPersistenceId(id);

            var viewModel = new SelectedCarViewModel
            {
                Employee = workflowObject.Employee,
                WorkflowPersistenceId = workflowObject.PersistedWorkflowId,
                VehicleModel = workflowObject.VehicleModel,
                VehicleMake = workflowObject.VehicleMake,
                FleetManagerRemark = workflowObject.FleetManagerRemark
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult ApprovalByFleetManager(string submitButton, SelectedCarViewModel viewModel)
        {
            _vehicleOrderService.SaveFleetManagerRemark(viewModel.WorkflowPersistenceId, viewModel.FleetManagerRemark);

            var decision = false;
            if (submitButton.Equals("Accept", StringComparison.InvariantCultureIgnoreCase))
            {
                decision = true;
            }

            return Resume(decision, viewModel.WorkflowPersistenceId);
        }
    }
}