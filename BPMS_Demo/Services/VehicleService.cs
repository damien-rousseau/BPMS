using System.Collections.Generic;
using Contracts;
using DAL;
using Domain.Entities;

namespace Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IRepository<VehicleMake> _makeRepository;
        private readonly IRepository<VehicleModel> _modelRepository;

        public VehicleService(IRepository<VehicleMake> makeRepository, IRepository<VehicleModel> modelRepository)
        {
            _makeRepository = makeRepository;
            _modelRepository = modelRepository;
        }

        public IEnumerable<VehicleMake> GetAllMakes()
        {
            return _makeRepository.GetAll();
        }

        public IEnumerable<VehicleModel> GetAllModels(int makeId)
        {
            return _modelRepository.Where(x => x.Make.Id == makeId);
        }

        public VehicleMake GetMakeById(int id)
        {
            return GetById(id, _makeRepository);
        }

        public VehicleModel GetModelById(int id)
        {
            return GetById(id, _modelRepository);
        }

        private T GetById<T>(int id, IRepository<T> repository) where T : EntityBase
        {
            return repository.FirstOrDefault(x => x.Id == id);
        }
    }
}