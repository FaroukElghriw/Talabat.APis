using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications.EmpSpec;

namespace Talabat.APis.Controllers
{
 
    public class EmployeesController :BaseApiController
    {
        private readonly IGenericRepository<Employee> _employeeRepo;

        public EmployeesController(IGenericRepository<Employee> employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Employee>>> GetEmployees()
        {
            var spec = new EmployeeDepartmentSpecification();
            var Employees = await _employeeRepo.GetALlWithSpecAsync(spec);

            return Ok(Employees);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmpById(int id)
        {
            var sepc = new EmployeeDepartmentSpecification(id);
            var emp= await _employeeRepo.GetEntityNWithSpecAsync(sepc);
            return Ok(emp);
        }
    }
}
