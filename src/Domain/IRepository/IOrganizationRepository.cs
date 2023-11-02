using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.Models;

namespace oksei_fsot_api.src.Domain.IRepository
{
    public interface IOrganizationRepository
    {
        Task<OrganizationModel?> GetAsync(Guid id);
        Task<OrganizationModel?> AddAsync(CreateOrganizationBody body);
    }
}