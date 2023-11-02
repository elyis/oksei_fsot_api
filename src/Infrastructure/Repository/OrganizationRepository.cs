using Microsoft.EntityFrameworkCore;
using oksei_fsot_api.src.Domain.Entities.Request;
using oksei_fsot_api.src.Domain.IRepository;
using oksei_fsot_api.src.Domain.Models;
using oksei_fsot_api.src.Infrastructure.Data;

namespace oksei_fsot_api.src.Infrastructure.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AppDbContext _context;

        public OrganizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrganizationModel?> AddAsync(CreateOrganizationBody body)
        {
            var organization = await GetAsync(body.Email);
            if (organization != null)
                return null;

            organization = new OrganizationModel
            {
                Name = body.Name,
                Email = body.Email,
            };

            var result = await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }


        public async Task<OrganizationModel?> GetAsync(Guid id)
            => await _context.Organizations
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<OrganizationModel?> GetAsync(string email)
            => await _context.Organizations
                .FirstOrDefaultAsync(e => e.Email == email);

    }
}