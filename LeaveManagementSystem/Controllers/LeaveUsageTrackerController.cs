using AutoMapper;
using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;
using LeaveManagementSystem.Infrastructure.Interfaces;
using LeaveManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class LeaveUsageTrackerController : Controller
    {
        private readonly ILeaveUsageTrackerRepository leaveUsageTrackerRepository;
        private readonly IReadRepository<LeaveUsageTracker> repository;
        private readonly IMapper _mapper;
        private readonly ILeaveService leaveUsageService;

        public LeaveUsageTrackerController(ILeaveUsageTrackerRepository leaveTrackerRepository, 
            IReadRepository<LeaveUsageTracker> repository, IMapper mapper, ILeaveService leaveService)
        {
            this.leaveUsageTrackerRepository = leaveTrackerRepository;
            this.repository = repository;
            _mapper = mapper;
            this.leaveUsageService = leaveService;
        }

        [HttpGet]
        public async Task<IActionResult> LeaveUsageTrackerManagement()
        {
            List<LeaveUsageTrackerModel> usageTrackers;
            
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if(userRole == "Admin")
            {
                ViewBag.Data = true;
                usageTrackers = await leaveUsageService.GetLeaveUsageTrackerListAsync();
            }
            else
            {
                var userId = User.FindFirst(ClaimTypes.Name)?.Value;
                usageTrackers = await leaveUsageService.GetLeaveUsageTrackerByEmployeeIdAsync(userId!);
                ViewBag.Data = false;
            }
            
            return View(usageTrackers);
        }

        public async Task<IActionResult> LeaveUsageTrackerForm(int id, bool isViewOnly = false)
        {
            var model = await leaveUsageService.GetLeaveUsageTrackerByIdAsync(id);

            ViewBag.IsViewOnly = isViewOnly;

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await leaveUsageTrackerRepository.DeleteAsync(id);
            return RedirectToAction("LeaveUsageTrackerManagement");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> SaveLeaveUsageTracker([FromBody] LeaveUsageTrackerModel model)
        {
            var leaveTracker = _mapper.Map<LeaveUsageTracker>(model);
            leaveTracker = await leaveUsageTrackerRepository.UpdateAsync(leaveTracker);
            return Json(leaveTracker);
        }
    }
}
