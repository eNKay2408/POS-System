using System.Collections.Generic;
using System.Threading.Tasks;
using POSSystem.Models;

namespace POSSystem.Repositories
{
    public interface IEmployeeRepository
    {
        Task SaveEmployee(Employee employee);

        Task<Employee> GetEmployeeByEmail(string email);

        Task SaveEmployeeFromGoogle(Employee employee);

        Task<List<Employee>> GetAllEmployees();

        Task DeleteEmployee(int id);

        Task UpdateEmployee(Employee employee);
    }
}
