using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;
using LeaveManagementSystem.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveManagementSystem.Models;
using AutoMapper;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace LeaveManagementSystem.Controllers
{
    [Authorize]
    public class LeaveApplicationController : Controller
    {
        private readonly ILeaveApplicationRepository leaveApplicationRepository;
        private readonly IReadRepository<LeaveApplication> repository;
        private readonly IMapper _mapper;

        public LeaveApplicationController(ILeaveApplicationRepository leaveApplicationRepository, 
            IReadRepository<LeaveApplication> repository, IMapper mapper)
        {
            this.leaveApplicationRepository = leaveApplicationRepository;
            this.repository = repository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> ApplicationManagement()
        {
            var applications = await leaveApplicationRepository.GetPendingLeaveApplicationsAsync();
            var applicationModels = _mapper.Map<List<LeaveApplicationModel>>(applications);
            return View(applicationModels);
        }

        public IActionResult LeaveApplicationForm()
        {
            return View(new LeaveApplicationModel());
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await leaveApplicationRepository.DeleteAsync(id);
            return Json(application);
        }

        [HttpPost]
        public async Task<IActionResult> SaveLeaveApplication([FromBody] LeaveApplicationModel model)
        {
            var application = _mapper.Map<LeaveApplication>(model);
            application = application.Id == 0 ? await leaveApplicationRepository.CreateAsync(application) :
                await leaveApplicationRepository.UpdateAsync(application);
            return Json(application);
        }

        [HttpGet]
        public async Task<IActionResult> ApplicationHistory(string id)
        {
            var applications = await leaveApplicationRepository.GetLeaveApplicationsByEmployeeIdAsync(id);
            var applicationModels = _mapper.Map<List<LeaveApplicationModel>>(applications);
            return View(applicationModels);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LeaveApplicationApprove(int id,string approverId)
        {
            var application = await leaveApplicationRepository.ApproveAsync(id, approverId);
            return Json(application);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LeaveApplicationReject(int id, string approverId)
        {
            var application = await leaveApplicationRepository.RejectAsync(id, approverId);
            return Json(application);
        }
    }
}
