namespace Domain.Entities
{
    public class VehicleModel : EntityBase
    {
        public VehicleModel()
        {
        }

        public string Name { get; set; }

        public virtual VehicleMake Make { get; set; }
    }
}
