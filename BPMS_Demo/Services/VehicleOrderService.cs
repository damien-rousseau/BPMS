using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Contracts;
using DAL;
using Domain.Entities;

namespace Services
{
    public class VehicleOrderService<T> : IVehicleOrderService<T>
        where T: Activity, new()
    {
        private readonly AutoResetEvent _instanceUnloaded = new AutoResetEvent(false);

        private readonly IRepository<VehicleOrderWorkflowObject> _repository;
        private readonly IConfigurationService _configurationService;
        private readonly IVehicleService _vehicleService;

        public VehicleOrderService(IConfigurationService configurationService, IRepository<VehicleOrderWorkflowObject> repository, IVehicleService vehicleService)
        {
            if (configurationService == null) throw new ArgumentNullException(nameof(configurationService));
            if (repository == null) throw new ArgumentNullException(nameof(repository));
            if (vehicleService == null) throw new ArgumentNullException(nameof(vehicleService));

            _configurationService = configurationService;
            _repository = repository;
            _vehicleService = vehicleService;
        }

        private WorkflowApplication CreateWorkflow()
        {
            return CreateWorkflow(new Dictionary<string, object>());
        }

        private WorkflowApplication CreateWorkflow(IDictionary<string, object> dictionary)
        {
            var workflowApplication = dictionary.Any() ? new WorkflowApplication(new T(), dictionary) : new WorkflowApplication(new T());

            workflowApplication.InstanceStore = new SqlWorkflowInstanceStore(_configurationService.InstanceStore);

            workflowApplication.PersistableIdle = (e) => PersistableIdleAction.Persist;

            workflowApplication.Completed = (e) =>
            {
                _instanceUnloaded.Set();
            };
            workflowApplication.Idle = (e) =>
            {
                _instanceUnloaded.Set();
            };

            workflowApplication.Unloaded = (e) =>
            {
            };

            return workflowApplication;
        }

        public void StartWorkflow(IDictionary<string, object> dictionary, VehicleOrderWorkflowObject workflowObject)
        {
            var workflowApplication = CreateWorkflow(dictionary);
            workflowApplication.Run();
            _instanceUnloaded.WaitOne();
            workflowApplication.Unload();

            workflowObject.PersistedWorkflowId = workflowApplication.Id;

            _repository.SaveOrUpdate(workflowObject);
        }

        public string GetBookmark(Guid id)
        {
            var workflowApplication = CreateWorkflow();
            workflowApplication.Load(id);

            var bookmarkName = string.Empty;
            var bookmarks = workflowApplication.GetBookmarks();

            if (bookmarks != null && bookmarks.Count > 0)
            {
                bookmarkName = bookmarks[0].BookmarkName;
            }

            workflowApplication.Unload();

            return bookmarkName;
        }

        public void GoToNextStep(Guid id, object decision)
        {
            var workflowApplication = CreateWorkflow();
            workflowApplication.Load(id);

            var bookmarkName = workflowApplication.GetBookmarks()[0].BookmarkName;
            workflowApplication.ResumeBookmark(bookmarkName, decision);
            _instanceUnloaded.WaitOne();
            workflowApplication.Unload();
        }

        public VehicleOrderWorkflowObject GetWorkflowObjectByWorkflowPersistenceId(Guid id)
        {
            return _repository.FirstOrDefault(x => x.PersistedWorkflowId == id);
        }

        public void SaveManagerRemark(Guid workflowObjectPersistenceId, string managerRemark)
        {
            var wf = _repository.FirstOrDefault(x => x.PersistedWorkflowId == workflowObjectPersistenceId);
            wf.ManagerRemark = managerRemark;
            wf.ManagerApproval = DateTime.Now;

            _repository.SaveOrUpdate(wf);
        }

        public void SaveVehicleSelection(Guid workflowObjectPersistenceId, int makeId, int modelId)
        {
            var make = _vehicleService.GetMakeById(makeId);
            var model = _vehicleService.GetModelById(modelId);

            var wf = _repository.FirstOrDefault(x => x.PersistedWorkflowId == workflowObjectPersistenceId);
            wf.VehicleMake = make != null ? make.Name : string.Empty;
            wf.VehicleModel = model != null ? model.Name : string.Empty;

            _repository.SaveOrUpdate(wf);
        }

        public void SaveFleetManagerRemark(Guid workflowObjectPersistenceId, string fleetManagerRemark)
        {
            var wf = _repository.FirstOrDefault(x => x.PersistedWorkflowId == workflowObjectPersistenceId);
            wf.FleetManagerRemark = fleetManagerRemark;
            _repository.SaveOrUpdate(wf);
        }
    }
}