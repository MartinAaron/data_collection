using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using data.collection.business.Interface;
using data.collection.entity.Entity;
using data.collection.entity.Model;
using data.collection.util.ApiClient;
using data.collection.util.DI;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace data.collection.business.Implements
{
    public class OperatorBusiness : IOperatorBusiness, IScopedDependency
    {
        public OperatorBusiness(IHttpContextAccessor httpContextAccessor, ApiClient apiClient,
            IConfiguration configuration, IUserBusiness business)
        {
            _httpContextAccessor = httpContextAccessor;
            _apiClient = apiClient;
            _configuration = configuration;
            _business = business;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiClient _apiClient;
        private readonly IConfiguration _configuration;
        private readonly IUserBusiness _business;

        public async Task<LoginUser> GetOperatorInfo()
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString();
            var jwt = token.Replace("Bearer ", string.Empty);
            var resStr = await _apiClient.PostFormDatatoRemote(
                _configuration["AuthServer"], "/connect/userinfo",
                new List<KeyValuePair<string, string>>()
                {
                    new("access_token", jwt)
                });
            return JsonConvert.DeserializeObject<LoginUser>(resStr);
        }


    }
}