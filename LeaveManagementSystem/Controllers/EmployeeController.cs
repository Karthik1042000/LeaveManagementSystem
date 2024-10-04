using AutoMapper;
using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Infrastructure.Interfaces;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;
using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagementSystem.Controllers
{
    
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository employeeRepository;
        private readonly IReadRepository<Employee> repository;
        private readonly IReadRepository<Role> roleRepository;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeRepository employeeRepository, IReadRepository<Employee> repository, 
            IReadRepository<Role> roleRepository, IMapper mapper)
        {
            this.employeeRepository = employeeRepository;
            this.repository = repository;
            this.roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string id)
        {
            var employeeProfile = _mapper.Map<ProfileModel>(await employeeRepository.GetByIdAsync(id));
            return View(employeeProfile);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProfile([FromBody] ProfileModel employeeProfile)
        {
            var employee = _mapper.Map<Employee>(employeeProfile);
            employee = await this.employeeRepository.UpdateAsync(employee);
            return Json(employee);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EmployeeForm(string id)
        {
            var roleList = await roleRepository.ListAsync();
            var model = id == null ? new EmployeeModel() : _mapper.Map<EmployeeModel>(await employeeRepository.GetByIdAsync(id));
            model.Roles = roleList.ToList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<IActionResult> EmployeeManagement()
        {
            var employees = await repository.ListAsync();
            var modelEmployees = _mapper.Map<List<EmployeeModel>>(employees);
            return View(modelEmployees);
        }
        [HttpPost]
        public async Task<IActionResult> SaveEmployee([FromBody] EmployeeModel model)
        {
            var employee = _mapper.Map<Employee>(model);
            employee = string.IsNullOrEmpty(employee.Id) ? await employeeRepository.CreateAsync(employee) :
                await employeeRepository.UpdateAsync(employee);
            return Json(employee);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            await employeeRepository.DeleteAsync(id);
            return RedirectToAction("EmployeeManagement");
        }
    }
}
