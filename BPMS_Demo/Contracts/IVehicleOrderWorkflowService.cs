using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Contracts
{
    public interface IVehicleOrderWorkflowService
    {
        void SaveManagerRemark(Guid workflowObjectPersistenceId, string managerRemark);

        void SaveVehicleSelection(Guid workflowObjectPersistenceId, int makeId, int modelId);

        void SaveFleetManagerRemark(Guid workflowObjectPersistenceId, string fleetManagerRemark);
    }
}
