namespace Wxt.OnlineSuperMarket.Data.Repositories
{
    using Wxt.OnlineSuperMarket.Data.Entities;

    public interface ICustomerRepository
    {
        Customer AddCustomer(Customer customer);

        int Login(string userName, string password);

        void DeleteCustomer(int customerId);

        void AddToCart(int customerId, int productId, int count);

        int RemoveFromCart(int customerId, int productId, int count);

        Receipt CheckOut(int customerId);
    }
}
