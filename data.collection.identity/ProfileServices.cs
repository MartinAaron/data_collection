using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using data.collection.entity.Entity;
using data.collection.repository.mongo.Repository;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace data.collection.identity
{
    public class ProfileServices : IProfileService
    {
        public ProfileServices(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private readonly UserRepository _userRepository;

        public async Task<List<Claim>> GetClaimsFromUserAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Id, user.Id.ToString()),
                new Claim(JwtClaimTypes.NickName, user.RealName),
                new Claim(type: JwtClaimTypes.Role, user.RoleId ?? ""),
                new Claim(type: JwtClaimTypes.Profile, user.DepartmentId ?? "")
            };
            await Task.CompletedTask;
            return claims;
        }

        /// <summary>
        /// http://localhost:5002/connect/userinfo
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userAccount = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userRepository.GetListByField("account", userAccount);
            context.IssuedClaims = await GetClaimsFromUserAsync(user.First());
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userAccount = context.Subject.Claims.FirstOrDefault(c => c.Type == "sub").Value;
            var user = await _userRepository.GetListByField("account", userAccount);
            context.IsActive = user.Any();
        }
    }
}