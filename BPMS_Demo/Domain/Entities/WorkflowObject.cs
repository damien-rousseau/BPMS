using System;

namespace Domain.Entities
{
    public class WorkflowObject : EntityBase
    {
        public Guid PersistedWorkflowId { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
