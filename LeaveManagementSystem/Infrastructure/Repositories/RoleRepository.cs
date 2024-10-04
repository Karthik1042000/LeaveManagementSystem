using LeaveManagementSystem.Domain;
using LeaveManagementSystem.Infrastructure.Common;
using LeaveManagementSystem.Infrastructure.Interfaces.GenericInterfaces;

namespace LeaveManagementSystem.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IWriteRepository<Role> writeRepository;
        private readonly IRepository<Role> repository;
        public RoleRepository(IWriteRepository<Role> writeRepository, IRepository<Role> repository)
        {
            this.writeRepository = writeRepository;
            this.repository = repository;
        }

        public async Task<Role> CreateAsync(Role role)
        {
            if (await this.repository.ExistsAsync(x => x.Name == role.Name))
            {
                throw new Exception($"The Role {role.Name} already exists.");
            }

            role.State = Role.Types.State.Active;
            return await writeRepository.CreateAsync(role);
        }

        public async Task<Role> UpdateAsync(Role role)
        {
            var existRole = await repository.GetByIdAsync(role.Id);
            if (existRole == null)
            {
                throw new Exception($"The RoleID {role.Id} does not exist");
            }

            if (await this.repository.ExistsAsync(x => x.Name == role.Name && x.Id != existRole.Id))
            {
                throw new Exception($"The Role {role.Name} already exists");
            }

            bool isUpdated = false;
            existRole.Name = PropertyUpdater.UpdateIfChanged(existRole.Name, role.Name, ref isUpdated);
            existRole.State = PropertyUpdater.UpdateIfChanged(existRole.State, role.State, ref isUpdated);

            if (isUpdated)
            {
                existRole = await writeRepository.UpdateAsync(existRole);
            }
            return existRole;
        }

        public async Task<Role> DeleteAsync(int id)
        {
            var role = await repository.GetByIdAsync(id);
            if (role == null)
            {
                throw new Exception($"Role with Id: {id} was not found");
            }
            return await writeRepository.DeleteAsync(role);
        }
    }
}
