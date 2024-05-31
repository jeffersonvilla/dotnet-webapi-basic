using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;
using MyVaccine.WebApi.Repositories.Implementations;

namespace MyFamilyGroup.WebApi.Repositories.Implementations;

public class FamilyGroupRepository : BaseRepository<FamilyGroup>, IFamilyGroupRepository<FamilyGroup>
{
    private readonly MyVaccineAppDbContext _context;

    public FamilyGroupRepository(MyVaccineAppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FamilyGroup>> GetAll(Func<IQueryable<FamilyGroup>, IQueryable<FamilyGroup>> include = null)
    {
        IQueryable<FamilyGroup> query = _context.Set<FamilyGroup>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.ToListAsync();
    }

    public async Task<FamilyGroup> GetById(int id, Func<IQueryable<FamilyGroup>, IQueryable<FamilyGroup>> include = null)
    {
        IQueryable<FamilyGroup> query = _context.Set<FamilyGroup>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(v => v.FamilyGroupId == id);
    }

    public async Task<List<FamilyGroup>> GetAllByIds(IEnumerable<int> ids)
    {
        return await _context.Set<FamilyGroup>()
            .Where(v => ids.Contains(v.FamilyGroupId))
            .ToListAsync();
    }

}