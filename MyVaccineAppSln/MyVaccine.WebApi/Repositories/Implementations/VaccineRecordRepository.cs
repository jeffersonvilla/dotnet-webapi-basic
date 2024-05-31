using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;

namespace MyVaccine.WebApi.Repositories.Implementations;

public class VaccineRecordRepository : BaseRepository<VaccineRecord>, IVaccineRecordRepository<VaccineRecord>
{
    private readonly MyVaccineAppDbContext _context;

    public VaccineRecordRepository(MyVaccineAppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VaccineRecord>> GetAll(Func<IQueryable<VaccineRecord>, IQueryable<VaccineRecord>> include = null)
    {
        IQueryable<VaccineRecord> query = _context.Set<VaccineRecord>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.ToListAsync();
    }

    public async Task<VaccineRecord> GetById(int id, Func<IQueryable<VaccineRecord>, IQueryable<VaccineRecord>> include = null)
    {
        IQueryable<VaccineRecord> query = _context.Set<VaccineRecord>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(v => v.VaccineRecordId == id);
    }

    public async Task<List<VaccineRecord>> GetAllByIds(IEnumerable<int> ids)
    {
        return await _context.Set<VaccineRecord>()
            .Where(v => ids.Contains(v.VaccineRecordId))
            .ToListAsync();
    }

}