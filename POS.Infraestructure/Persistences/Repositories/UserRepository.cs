using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infraestructure.Persistences.Contexts;
using POS.Infraestructure.Persistences.Interfaces;

namespace POS.Infraestructure.Persistences.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly POSContext _context;
        public UserRepository(POSContext context): base(context) 
        {
            _context = context;
        }
        public async Task<User> AccountByEmail(string email)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email!.Equals(email));
            return user!;
        }
    }
}
