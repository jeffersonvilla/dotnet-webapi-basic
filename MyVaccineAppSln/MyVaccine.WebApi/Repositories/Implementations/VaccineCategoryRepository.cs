using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;

namespace MyVaccine.WebApi.Repositories.Implementations;

public class VaccineCategoryRepository : BaseRepository<VaccineCategory>
    , IVaccineCategoryRepository<VaccineCategory>
{
    private readonly MyVaccineAppDbContext _context;

    public VaccineCategoryRepository(MyVaccineAppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VaccineCategory>> GetAll(Func<IQueryable<VaccineCategory>
        , IQueryable<VaccineCategory>> include = null)
    {
        IQueryable<VaccineCategory> query = _context.Set<VaccineCategory>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.ToListAsync();
    }

    public async Task<VaccineCategory> GetById(int id, Func<IQueryable<VaccineCategory>
        , IQueryable<VaccineCategory>> include = null)
    {
        IQueryable<VaccineCategory> query = _context.Set<VaccineCategory>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(v => v.VaccineCategoryId == id);
    }

    public async Task<List<VaccineCategory>> GetAllByIds(IEnumerable<int> ids)
    {
        return await _context.Set<VaccineCategory>()
            .Where(v => ids.Contains(v.VaccineCategoryId))
            .ToListAsync();
    }

}
