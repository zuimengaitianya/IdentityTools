using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Test;
using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityTools
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                //new IdentityResources.Profile(),
                //new IdentityResources.
                //new IdentityResource("api1",new List<string>(){ "api1"})
                //new IdentityResource("roles","roles",new List<string>{ "role"})
             };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                //new ApiResource ("api1","My API")
                //new ApiResource ("api1","ToolsApi")
                //{
                //    UserClaims =new List<string>
                //    { 
                //        JwtClaimTypes.Audience,
                //        "role"
                //    }
                //}
                new ApiResource("api1")
                {
                    UserClaims =
                    {
                        JwtClaimTypes.Audience
                    },
                    Scopes = new List<string>
                    {
                        "api1"
                    },
                }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api1")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                
                new Client
                {
                    ClientId ="ToolsApiClient",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowOfflineAccess = true,
                    AccessTokenLifetime = 3600 * 6, //6小时
                    SlidingRefreshTokenLifetime = 1296000, //15天
                    AccessTokenType = AccessTokenType.Jwt,
                    //RequirePkce = false,
                    //RequireClientSecret = false,
                    //RedirectUris = { "http://localhost:5003/callback.html"},
                    //PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                    //AllowedCorsOrigins =     { "http://localhost:5003" },
                    RequireConsent = false,
                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes=
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        //IdentityServerConstants.StandardScopes.Profile,
                        //IdentityServerConstants.StandardScopes.OfflineAccess,
                        "api1"
                    },
                    //Claims = new List<ClientClaim>
                    //{
                    //    new ClientClaim(JwtClaimTypes.Role,"normal")
                    //},
                    //ClientClaimsPrefix = "" //
                }

            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>()
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "123456",
                    Claims = new Claim[]
                            {
                                new Claim("UserId",1.ToString()),
                                new Claim(JwtClaimTypes.Name,"alice"),
                                new Claim(JwtClaimTypes.GivenName,"alice"),
                                new Claim(JwtClaimTypes.FamilyName,"globetools"),
                                new Claim(JwtClaimTypes.Email,"565009871@qq.com"),
                                new Claim(JwtClaimTypes.Role,"normal")
                            }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "alone",
                    Password = "123456",
                    Claims = new Claim[]
                            {
                                new Claim("UserId",1.ToString()),
                                new Claim(JwtClaimTypes.Name,"anlong"),
                                new Claim(JwtClaimTypes.GivenName,"alone"),
                                new Claim(JwtClaimTypes.FamilyName,"globetools"),
                                new Claim(JwtClaimTypes.Email,"565009871@qq.com"),
                                new Claim(JwtClaimTypes.Role,"admin")
                            }
                }
            };
        }
    }

    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly TestUserStore _userService;
        public ResourceOwnerPasswordValidator(TestUserStore userService)
        {
            _userService = userService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (_userService.ValidateCredentials(context.UserName, context.Password))
            {


                TestUser user = _userService.FindByUsername(context.UserName);

                var useRoleClaims = new List<Claim>()
                {
                    new Claim(JwtClaimTypes.Role, user.Claims.FirstOrDefault(m => m.Type == JwtClaimTypes.Role).Value)
                };

                context.Result = new GrantValidationResult(
                    subject: user.SubjectId,
                    authenticationMethod: OidcConstants.AuthenticationMethods.Password,
                    claims: useRoleClaims
                    );
            }
            else
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
        }

    }

    public class ProfileService : IProfileService
    {
        private readonly TestUserStore _userService;

        //UserManager<ApplicationUser>
        public ProfileService(TestUserStore userService)
        {
            _userService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                var claims = context.Subject.Claims.ToList();

                //set issued claims to return
                context.IssuedClaims = claims.ToList();

                var user = _userService.FindBySubjectId(context.Subject.GetSubjectId());
                if (user != null)
                {
                    context.AddRequestedClaims(user.Claims);
                    //context.IssuedClaims.AddRange(user.Claims);
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
        }
    }

}
