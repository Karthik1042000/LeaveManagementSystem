using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;
using LeaveManagementSystem.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LeaveManagementSystem.Models;
using AutoMapper;

namespace LeaveManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AnnualLeaveRecordController : Controller
    {
        private readonly IAnnualLeaveRecordRepository annualLeaveRecordRepository;
        private readonly IReadRepository<AnnualLeaveRecord> repository;
        private readonly IReadRepository<Role> roleRepository;
        private readonly IMapper _mapper;

        public AnnualLeaveRecordController(IAnnualLeaveRecordRepository annualLeaveRecordRepository,
            IReadRepository<AnnualLeaveRecord> repository,
            IReadRepository<Role> roleRepository, IMapper mapper)
        {
            this.annualLeaveRecordRepository = annualLeaveRecordRepository;
            this.repository = repository;
            this.roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> AnnualRecordManagement()
        {
            var annualRecords = await repository.ListAsync();
            var annualRecordModels = _mapper.Map<List<AnnualLeaveRecordModel>>(annualRecords);
            return View(annualRecordModels);
        }

        public async Task<IActionResult> AnnualLeaveRecordForm(int id)
        {
            var roleList = await roleRepository.ListAsync();
            var model = id == 0 ? new AnnualLeaveRecordModel() : _mapper.Map<AnnualLeaveRecordModel>(await repository.GetByIdAsync(id));
            model.Roles = roleList.ToList();
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await annualLeaveRecordRepository.DeleteAsync(id);
            return RedirectToAction("AnnualRecordManagement");
        }

        [HttpPost]
        public async Task<IActionResult> SaveAnnualLeaveRecord([FromBody] AnnualLeaveRecordModel model)
        {
            var annualRecord = _mapper.Map<AnnualLeaveRecord>(model);
            annualRecord = annualRecord.Id == 0 ? await annualLeaveRecordRepository.CreateAsync(annualRecord) :
                await annualLeaveRecordRepository.UpdateAsync(annualRecord);
            return Json(annualRecord);
        }
    }
}
