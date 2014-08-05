using System.Collections.Generic;
using System.Threading.Tasks;
using UsedParts.Domain;

namespace UsedParts.Services
{
    public interface IClient
    {
        #region user management 

        bool IsAuthorized { get; }
        AccountInfo Profile { get; }

        Task<string> Login(string email, string password);
        Task<string> Register(AccountInfo info);

        Task Logout();
        Task<AccountInfo> GetStatus();

        Task RegisterDevice(string url);

        #endregion

        #region request-offer

        Task<OrdersResult> GetRequestsByManufacturer(int manufacturerId, int page = 0);
        Task<OrdersResult> GetRequestsBySeller(int sellerId, int page = 0);
        Task<Order> GetRequest(int id);
        Task<IEnumerable<Manufacturer>> GetManufacturers();
        Task<IEnumerable<Offer>> GetRequestOffers(long requestId);
        Task<long> MakeOfferForRequest(Offer offer);
        Task<int> GetUnreadRequestCountByManufacturer(int manufacturerId, int lastId);

        #endregion

        #region other

        void SetLang(string code);

        #endregion


    }
}
