using System.Threading.Tasks;
using POSSystem.Models;

namespace POSSystem.Repository
{
    public interface IEmployeeRepository
    {
        Task SaveEmployee(Employee employee);
        Task<Employee> GetEmployeeByEmail(string email);
        Task SaveEmployeeFromGoogle(Employee employee);
    }
}
