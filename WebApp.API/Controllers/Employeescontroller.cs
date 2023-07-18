using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using WebApp.API.Repository;

namespace WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Employeescontroller : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;
        public Employeescontroller(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetEmplyees());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var res = await _employeeRepository.GetEmployee(id);
                if (res == null)
                {
                    return NotFound();
                }
                return res;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            try
            {
                if (employee == null)
                {
                    return BadRequest();
                }
                var CreatedEmployee = await _employeeRepository.AddEmployee(employee);  
                return CreatedAtAction(nameof(GetEmployee),new {id = CreatedEmployee.Id},CreatedEmployee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (id != employee.Id)
                {
                    return BadRequest("Id Mismatch");
                }
                var employeeUpdate = await _employeeRepository.GetEmployee(id);
                if(employeeUpdate== null)
                {
                    return NotFound($"Employee Id={id} not found");
                }
               return await _employeeRepository.UpdateEmployee(employee);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            try
            {
                
                var employeeDelete = await _employeeRepository.GetEmployee(id);
                if (employeeDelete == null)
                {
                    return NotFound($"Employee Id={id} not found");
                }
                return await _employeeRepository.DeleteEmployee(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name)
        {
            try
            {
                var res = await _employeeRepository.Search(name);
               if (res.Any())
                {
                    return Ok(res); 
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }

}
