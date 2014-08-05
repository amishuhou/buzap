using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Hammock;
using Hammock.Web;
using Newtonsoft.Json;
using UsedParts.Common;
using UsedParts.Domain;
using UsedParts.Services.Impl.Dto;

namespace UsedParts.Services.Impl
{
    public class Client : IClient
    {
        private readonly ISettings _settings;

        private const string BaseUrl = "http://api.buzap.by";
        private const string Platform = "wp";

        private const string AuthTokenKey = "auth_token";
        private const string ProfileKey  = "profile";

        private string _lang = "ru";

        public Client(ISettings settings)
        {
            _settings = settings;
        }

        public bool IsAuthorized 
        {
            get { return !string.IsNullOrEmpty(_settings.GetValue<string>(AuthTokenKey)); }
        }

        public AccountInfo Profile 
        {
            get { return _settings.GetValue<AccountInfo>(ProfileKey); }
        }

        public async Task<string> Login(string email, string password)
        {
            var ps = new Dictionary<string, string>();
            ps["ln"] = email;
            ps["pwd"] = password;
            var result = await Get<AuthResult>("login", ps, false);
            _settings.SetValue(AuthTokenKey, result.Token);
            await GetStatus();
            return result.UserId;
        }

        public async Task<string> Register(AccountInfo info)
        {
            var ps = new Dictionary<string, string>();
            ps["nln"] = info.Email;
            ps["npwd"] = info.Password;
            ps["acc_type"] = info.OrganizationType.ToString(CultureInfo.InvariantCulture);
            ps["acc_name"] = info.Name;
            ps["acc_phone"] = info.Phone;
            var result = await Get<AuthResult>("registration", ps, false);
            _settings.SetValue(AuthTokenKey, result.Token);
            await GetStatus();
            return result.UserId;
        }

        public async Task Logout()
        {
            await Get<AuthResult>("logout");
            ResetProfileData();
        }

        private void ResetProfileData()
        {
            _settings.ResetValue(AuthTokenKey);
            _settings.ResetValue(ProfileKey);
        }

        public async Task<AccountInfo> GetStatus()
        {
            var profile = await Get<AccountInfo>("usr/status");
            _settings.SetValue(ProfileKey, profile);
            return profile;
        }

        public async Task RegisterDevice(string url)
        {
            var ps = new Dictionary<string, string>();
            ps["device"] = url;
            await Get<object>("push/wp", ps);
        }

        public async Task<OrdersResult> GetRequestsByManufacturer(int manufacturerId, int page = 0)
        {
            return await Get<OrdersResult>(string.Format("requests/{0}/{1}", 
                manufacturerId != 0 ? manufacturerId.ToString(CultureInfo.InvariantCulture) : "all", page));
        }

        public async Task<OrdersResult> GetRequestsBySeller(int sellerId, int page = 0)
        {
            return await Get<OrdersResult>(string.Format("requestsbyseller/{0}/{1}", sellerId, page));
        }

        public async Task<Order> GetRequest(int id)
        {
            return await Get<Order>(string.Format("request/{0}", id));
        }

        public async Task<IEnumerable<Manufacturer>> GetManufacturers()
        {
            return await Get<IEnumerable<Manufacturer>>("mfa");
        }

        public async Task<IEnumerable<Offer>> GetRequestOffers(long requestId)
        {
            var result = await Get<OffersResult>(string.Format("offers/{0}", requestId));
            return result.Items;
        }

        public async Task<long> MakeOfferForRequest(Offer offer)
        {
            var ps = new Dictionary<string, string>();
            ps["reqId"] = offer.OrderId.ToString(CultureInfo.InvariantCulture);
            ps["cond"] = offer.Condition.ToString(CultureInfo.InvariantCulture);
            ps["gara"] = offer.Warranty.ToString(CultureInfo.InvariantCulture);
            ps["deli"] = offer.Delivery.ToString(CultureInfo.InvariantCulture);
            ps["ava"] = offer.Available.ToString(CultureInfo.InvariantCulture);
            ps["price"] = offer.Price.ToString(CultureInfo.InvariantCulture);

            var result = await Post<OfferResult>("makeoffer", ps, true, offer.Images);
            return result.OfferId;
        }

        public async Task<int> GetUnreadRequestCountByManufacturer(int manufacturerId, int lastId)
        {
            var countStr = await Get<string>(string.Format("requestsafter/{0}/{1}", 
                manufacturerId, lastId));
            return Convert.ToInt32(countStr);
        }

        public void SetLang(string code)
        {
            _lang = code;
        }

        #region helpers

        private async Task<T> Post<T>(string methodUrl, IEnumerable<KeyValuePair<string, 
            string>> ps = null, 
            bool isAuth = true,
            IEnumerable<string> files = null)
            where T : class
        {
            return await SendRequest<T>((request, requestParams) =>
            {
                request.Method = WebMethod.Post;
                if (files != null)
                {
                    foreach (var parameter in requestParams)
                        request.AddField(parameter.Name, parameter.Value);
                    var i = 1;
                    foreach (var file in files)
                    {
                        var name = Path.GetFileName(file);
                        request.AddFile("img" + i, name, file);
                        i++;
                    }
                }
                else
                {
                    foreach (var parameter in requestParams)
                        request.AddParameter(parameter.Name, parameter.Value);
                }
            },
            methodUrl, ps, isAuth);
        }

        private async Task<T> Get<T>(string methodUrl, IEnumerable<KeyValuePair<string, string>> ps = null,
                                     bool isAuth = true)
            where T : class
        {
            return await SendRequest<T>((request, requestParams) =>
            {
                foreach (var parameter in requestParams)
                    request.AddParameter(parameter.Name, parameter.Value);
            },
            methodUrl, ps, isAuth);
        }

        private async Task<T> SendRequest<T>(Action<RestRequest, WebParameterCollection> prepareRequestHandler,
                                               string methodUrl, IEnumerable<KeyValuePair<string, string>> ps = null,
                                               bool isAuth = false) where T : class
        {
            var restClient = new RestClient();
            var requestParams = new WebParameterCollection();

            var authToken = _settings.GetValue<string>(AuthTokenKey);
            if (string.IsNullOrEmpty(authToken))
                authToken = "anonymous";
            var url = isAuth ? string.Format("{0}/{1}/{2}/{3}/{4}", BaseUrl, _lang, Platform, authToken, methodUrl)
                : string.Format("{0}/{1}/{2}/{3}", BaseUrl, _lang, Platform, methodUrl);

            restClient.Authority = url.Replace("/" + methodUrl, string.Empty);
            if (ps != null)
            {
                requestParams.AddRange(
                    ps.Select(
                        pair =>
                        new WebPair(pair.Key,
                                    pair.Value != null
                                        ? pair.Value.ToString(CultureInfo.InvariantCulture)
                                        : string.Empty)));
            }


            var r = new Random();
            requestParams.Add("r", r.Next(100000, 999999).ToString(CultureInfo.InvariantCulture));

            var queryString = string.Join("&", requestParams.Select(p => string.Format("{0}={1}", p.Name, Uri.EscapeDataString(p.Value))));
            var request = new RestRequest
            {
                Encoding = Encoding.UTF8,
                Path = methodUrl
            };

            Debug.WriteLine("\n\nrequest: {0}/{1}?{2}", restClient.Authority, request.Path, queryString);

            prepareRequestHandler(request, requestParams);

            var tcs = new TaskCompletionSource<string>();
            var restCallback = new RestCallback<string>((restRequest, resp, state) =>
            {
                if (resp != null && resp.InnerException != null)
                    tcs.TrySetException(resp.InnerException);
                else if (resp != null)
                    tcs.TrySetResult(resp.Content);
                else
                    tcs.SetCanceled();
            });

            try
            {

                restClient.BeginRequest(request, restCallback);

                var result = await tcs.Task;

                Debug.WriteLine("Response: {0}", result);
                var response = JsonConvert.DeserializeObject<Response<T>>(result);
                if (response.ErrorCode > 0)
                {
                    if (response.ErrorCode == (int)ErrorCode.NotAuthorized)
                        ResetProfileData();
                    throw new ServiceException(response.ErrorDescription);
                }

                //Debug.Assert(!Deployment.Current.Dispatcher.CheckAccess());

                return response.Data;
            }
            catch (WebException webEx)
            {
                Debug.WriteLine("Web error: {0}", webEx);
                throw new ServiceException(string.Empty);
            }

        }

        #endregion
    }

    internal enum ErrorCode
    {
        NotAuthorized = 19
    }

}
