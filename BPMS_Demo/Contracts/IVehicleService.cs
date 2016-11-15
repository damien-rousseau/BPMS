using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IVehicleService
    {
        IEnumerable<VehicleMake> GetAllMakes();

        VehicleMake GetMakeById(int id);

        IEnumerable<VehicleModel> GetAllModels(int makeId);

        VehicleModel GetModelById(int id);
    }
}
