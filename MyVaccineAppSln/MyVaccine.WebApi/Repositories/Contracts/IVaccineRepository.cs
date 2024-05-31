using MyVaccine.WebApi.Models;

namespace MyVaccine.WebApi.Repositories.Contracts;

public interface IVaccineRepository<T> : IBaseRepository<Vaccine>
{
    Task<IEnumerable<T>> GetAll(Func<IQueryable<T>, IQueryable<T>> include = null);
    Task<T> GetById(int id, Func<IQueryable<T>, IQueryable<T>> include = null);
    Task<List<T>> GetAllByIds(IEnumerable<int> ids);
}
