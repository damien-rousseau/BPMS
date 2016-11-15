using Domain.Entities;
using System.Collections.Generic;

namespace Contracts
{
    public interface IEmployeeService
    {
        IEnumerable<Employee> GetAllEmployee();

        Employee GetById(int id);
    }
}
