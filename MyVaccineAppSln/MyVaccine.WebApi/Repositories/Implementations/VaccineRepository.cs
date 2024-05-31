using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;

namespace MyVaccine.WebApi.Repositories.Implementations;

public class VaccineRepository : BaseRepository<Vaccine>, IVaccineRepository<Vaccine>
{
    private readonly MyVaccineAppDbContext _context;

    public VaccineRepository(MyVaccineAppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Vaccine>> GetAll(Func<IQueryable<Vaccine>, IQueryable<Vaccine>> include = null)
    {
        IQueryable<Vaccine> query = _context.Set<Vaccine>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.ToListAsync();
    }

    public async Task<Vaccine> GetById(int id, Func<IQueryable<Vaccine>, IQueryable<Vaccine>> include = null)
    {
        IQueryable<Vaccine> query = _context.Set<Vaccine>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(v => v.VaccineId == id);
    }

    public async Task<List<Vaccine>> GetAllByIds(IEnumerable<int> ids)
    {
        return await _context.Set<Vaccine>()
            .Where(v => ids.Contains(v.VaccineId))
            .ToListAsync();
    }

}
