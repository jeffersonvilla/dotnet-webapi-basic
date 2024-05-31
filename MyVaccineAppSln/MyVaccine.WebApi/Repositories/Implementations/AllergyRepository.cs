using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;

namespace MyVaccine.WebApi.Repositories.Implementations;

public class AllergyRepository : BaseRepository<Allergy>, IAllergyRepository<Allergy>
{
    private readonly MyVaccineAppDbContext _context;

    public AllergyRepository(MyVaccineAppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Allergy>> GetAll(Func<IQueryable<Allergy>, IQueryable<Allergy>> include = null)
    {
        IQueryable<Allergy> query = _context.Set<Allergy>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.ToListAsync();
    }

    public async Task<Allergy> GetById(int id, Func<IQueryable<Allergy>, IQueryable<Allergy>> include = null)
    {
        IQueryable<Allergy> query = _context.Set<Allergy>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(v => v.AllergyId == id);
    }

    public async Task<List<Allergy>> GetAllByIds(IEnumerable<int> ids)
    {
        return await _context.Set<Allergy>()
            .Where(v => ids.Contains(v.AllergyId))
            .ToListAsync();
    }

}