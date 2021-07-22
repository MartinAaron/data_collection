using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using data.collection.business.Interface;
using data.collection.entity.Entity;
using data.collection.entity.Model;
using data.collection.entity.Request;
using data.collection.repository.mongo.Repository;
using data.collection.util.ApiClient;
using data.collection.util.DI;
using data.collection.util.Extentions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace data.collection.business.Implements
{
    public class UserBusiness : IUserBusiness, ITransientDependency
    {
        public UserBusiness(UserRepository userRepository, ApiClient apiClient, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _apiClient = apiClient;
            _configuration = configuration;
        }

        private readonly UserRepository _userRepository;
        private readonly ApiClient _apiClient;
        private readonly IConfiguration _configuration;

        public async Task AddUserAsync(User request)
        {
            await _userRepository.Create(request);
        }

        public async Task<string> GetUserByFieldAsync(UserCommonRequest request)
        {
            var users = await _userRepository.GetListByField("account", request.Account);
            var user = users.FirstOrDefault();
            if (user == null)
                throw new Exception("输入账号有误");
            if (user.Password != request.Password.ToMD5String())
                throw new Exception("输入密码有误");
            return user.Id;
        }

        public async Task<string> GetUserLoginTokenAsync(UserCommonRequest request)
        {
            var token = await _apiClient.PostFormDatatoRemote(
                _configuration["AuthServer"], "/connect/token",
                new List<KeyValuePair<string, string>>()
                {
                    new("grant_type", "password"),
                    new("UserName", request.Account),
                    new("Password", request.Password),
                    new("client_id", "data_client"),
                    new("client_secret", "_123456")
                });
            return token;
        }
    }
}