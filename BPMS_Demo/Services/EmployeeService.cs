using System.Collections.Generic;
using Contracts;
using DAL;
using Domain.Entities;

namespace Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IRepository<Employee> _repository;

        public EmployeeService(IRepository<Employee> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Employee> GetAllEmployee()
        {
            return _repository.GetAll();
        }

        public Employee GetById(int id)
        {
            return _repository.FirstOrDefault(x => x.Id == id);
        }
    }
}
