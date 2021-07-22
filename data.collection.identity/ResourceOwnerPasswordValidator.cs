using System;
using System.Linq;
using System.Threading.Tasks;
using data.collection.repository.mongo.Repository;
using data.collection.util.Extentions;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace data.collection.identity
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public ResourceOwnerPasswordValidator(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        private readonly UserRepository _userRepository;

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userRepository.GetListByField("account", context.UserName);
            if (user.Count == 0)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                    "账号输入错误");
            }
            else if (user.First().Password != context.Password.ToMD5String())
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant,
                        "密码错误")
                    ;
            }
            else
            {
                context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: OidcConstants.AuthenticationMethods.Password);
            }
        }
    }
}