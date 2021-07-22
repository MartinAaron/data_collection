using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using data.collection.business.Interface;
using data.collection.entity.Model;
using data.collection.entity.Request;
using data.collection.util.IdWorker;
using InfluxData.Net.InfluxDb.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User = data.collection.entity.Entity.User;

namespace data.collection.api.Controllers.UserManage
{
    /// <summary>
    /// 测试用户
    /// </summary>
    [Route("api/user")]
    public class UserController : BaseController
    {
        public UserController(SnowWorker snowWorker, IUserBusiness userBusiness, IOperatorBusiness @operator) :
            base(snowWorker)
        {
            _userBusiness = userBusiness;
            _operator = @operator;
        }

        private readonly IUserBusiness _userBusiness;
        private readonly IOperatorBusiness _operator;

        [Route("put")]
        [HttpPut]
        public async Task Add([FromBody] User request)
        {
            InitEntity(request);
            await _userBusiness.AddUserAsync(request);
        }


        /// <summary>
        /// user login
        /// </summary>
        /// <returns></returns>
        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<string> UserLogin([FromBody] UserCommonRequest request)
        {
            return await _userBusiness.GetUserLoginTokenAsync(request);
        }

        [Route("info")]
        [HttpGet]
        public async Task<LoginUser> GetLoginUser()
        {
            return await _operator.GetOperatorInfo();
        }
    }
}