using System;
using System.Activities;
using System.Collections.Generic;
using Domain.Entities;

namespace Contracts
{
    public interface IVehicleOrderService<T>
        where T : Activity, new()
    {
        void StartWorkflow(IDictionary<string, object> dictionary, VehicleOrderWorkflowObject workflowObject);

        string GetBookmark(Guid id);

        void GoToNextStep(Guid id, object decision);

        VehicleOrderWorkflowObject GetWorkflowObjectByWorkflowPersistenceId(Guid workflowPersistenceId);

        void SaveManagerRemark(Guid workflowObjectPersistenceId, string managerRemark);

        void SaveVehicleSelection(Guid workflowObjectPersistenceId, int makeId, int modelId);

        void SaveFleetManagerRemark(Guid workflowObjectPersistenceId, string fleetManagerRemark);
    }
}
