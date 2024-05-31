using System.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyVaccine.WebApi.Dtos;
using MyVaccine.WebApi.Models;
using MyVaccine.WebApi.Repositories.Contracts;

namespace MyVaccine.WebApi.Repositories.Implementations;

public class UserRepository : BaseRepository<User>,IUserRepository
{
    private readonly MyVaccineAppDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    public UserRepository(MyVaccineAppDbContext context, UserManager<IdentityUser> userManager):base(context)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IdentityResult> AddUser(RegisterRequetDto request)
    {
        var response = new IdentityResult();
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            var user = new ApplicationUser
            {
                UserName = request.Username.ToLower(),
                Email = request.Username
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            response = result;

            if (!result.Succeeded)
            {
                return response;
            }

            var roleResult = await _userManager.AddToRoleAsync(user, "user");
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return roleResult;
            }
            

            var newUser = new User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                AspNetUserId = user.Id
            };

            var res = await _context.Users.AddAsync(newUser);

            await _context.SaveChangesAsync();
            scope.Complete();
        }



        //var user = new ApplicationUser
        //{
        //    UserName = request.Email.ToLower(),
        //    Email = request.Email,

        //};

        //var result = await _userManager.CreateAsync(user, model.Password);
        return response;
    }

    public async Task<User> GetById(int id, Func<IQueryable<User>, IQueryable<User>> include = null)
    {
        IQueryable<User> query = _context.Set<User>();

        if (include != null)
        {
            query = include(query);
        }

        return await query.FirstOrDefaultAsync(v => v.UserId == id);
    }

    public async Task<List<User>> GetAllByIds(IEnumerable<int> ids)
    {
        return await _context.Set<User>()
            .Where(v => ids.Contains(v.UserId))
            .ToListAsync();
    }
}
