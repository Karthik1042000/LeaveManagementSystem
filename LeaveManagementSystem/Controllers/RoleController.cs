using AutoMapper;
using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveManagementSystem.Models;

namespace LeaveManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleRepository roleRepository;
        private readonly IReadRepository<Role> repository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository, IReadRepository<Role> repository, 
            IMapper mapper)
        {
            this.roleRepository = roleRepository;
            this.repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> RoleManagement()
        {
            var roles = await repository.ListAsync();
            var roleModels = _mapper.Map<List<RoleModel>>(roles);
            return View(roleModels);
        }

        public async Task<IActionResult> RoleForm(int id)
        {
            var model = id == 0 ? new RoleModel() : _mapper.Map<RoleModel>(await repository.GetByIdAsync(id));
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await roleRepository.DeleteAsync(id);
            return RedirectToAction("RoleManagement");
        }

        [HttpPost]
        public async Task<IActionResult> SaveRole([FromBody] RoleModel model)
        {
            var role = _mapper.Map<Role>(model);
            role = role.Id == 0 ? await roleRepository.CreateAsync(role) :
                await roleRepository.UpdateAsync(role);
            return Json(role);
        }
    }
}
