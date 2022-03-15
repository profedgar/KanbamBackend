
using backend.Api.Repository;
using backend.Context;
using backend.Interfaces;
using backend.Models;

namespace backend.Repository
{
    public class CardsRepository : Repository<Cards>, ICardsRepository
    {
        public CardsRepository(MeuDbContext context) : base(context) { }

    }
}
