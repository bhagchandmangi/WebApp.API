using DataAccessLayer;

namespace WebApp.API.Repository
{
    public interface IEmployeeRepository 
    {
        Task<IEnumerable<Employee>> Search(string name);
        Task<IEnumerable<Employee>> GetEmplyees();
        Task<Employee> GetEmployee(int id);
        Task<Employee> AddEmployee(Employee employee);
        Task<Employee> UpdateEmployee(Employee employee);
        Task<Employee> DeleteEmployee(int id); 

    }
}
