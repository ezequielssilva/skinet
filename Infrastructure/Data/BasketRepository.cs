using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data;

public class BasketRepository : IBasketRepository
{
    public Task<bool> DeleteBasketAsync(string basketId)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerBasket> GetBasketAsync(string basketId)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        throw new NotImplementedException();
    }
}