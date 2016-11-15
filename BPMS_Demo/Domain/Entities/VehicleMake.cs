using System.Collections.Generic;

namespace Domain.Entities
{
    public class VehicleMake : EntityBase
    {
        public string Name { get; set; }

        public virtual ICollection<VehicleModel> Models { get; set; }
    }
}
