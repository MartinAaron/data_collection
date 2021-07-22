using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace data.collection.identity
{
    public class Conf
    {
        /// <summary>
        /// 定义API范围
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api", "data_collection")
            };

        /// <summary>
        /// 定义API资源
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("api", "data_collection") {Scopes = {"api"}}
            };
        }


        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //未添加导致scope错误
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "ak1425687_zjpt",
                    AccessTokenLifetime = 36000,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("_dse_dce_btu2157".Sha256()),
                    },
                    AllowedScopes = {"api"}
                },
                new Client
                {
                    ClientId = "ro.admin",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AccessTokenLifetime = 36000,
                    ClientSecrets =
                    {
                        new Secret("_123456".Sha256())
                    },
                    AllowedScopes = {"api"}
                },
                new Client()
                {
                    ClientId = "data_client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("_123456".Sha256())
                    },
                    AllowedScopes =
                    {
                        "api", IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                }
            };
        }
    }
}