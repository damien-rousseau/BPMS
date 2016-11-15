using Domain.Entities;
using System;

namespace Contracts
{
    public interface IWorkflowRepository<T> where T: WorkflowObject
    {
        void SaveOrUpdateWorkflow(T workflow);

        T GetWorkflowByPersistenceId(Guid workflowPersistenceId);
    }
}
