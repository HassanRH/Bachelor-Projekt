
namespace WebShop.Services.Interfaces
{
    public interface IOrderService<T, R>
    {

        T GetOrder(int id);

        int insertOrder(T param);

        T ConvertModelToOrderTypeT(R model);

        void AddSubscriber(string email);

        string GenerateOrderPDF(T order);

        void UpdateOrderWithPDF(T order, string path);

        void SendOrderConfirmationMailByGmail(int orderno);
    }
}
