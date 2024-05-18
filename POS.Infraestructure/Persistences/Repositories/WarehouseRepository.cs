using POS.Domain.Entities;
using POS.Infraestructure.Persistences.Contexts;
using POS.Infraestructure.Persistences.Interfaces;

namespace POS.Infraestructure.Persistences.Repositories
{
    public class WarehouseRepository : GenericRepository<Warehouse>, IWarehouseRepository
    {
        private readonly POSContext _context;

        public WarehouseRepository(POSContext context): base(context)
        {
            _context = context;
        }
    }
}
