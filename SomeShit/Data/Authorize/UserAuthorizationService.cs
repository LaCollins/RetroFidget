using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Security.Principal;

namespace RetroFidget.Data.Authorize
{
    public class UserAuthorizationService : IClaimsTransformation
    {

        public UserInfo userInfo;

        private ClaimsPrincipal CustomClaimsPrincipal;

        //TransformAsync Runs everytime the Middleware Calls AuthenticateAsync, As a Scoped Service this will always be ran on the same instance of UserAuthorizationService
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            //Creates UserInfo Object on the first TransformAsync call and ignores on subsequent calls
            if (userInfo == null)
                userInfo = new UserInfo((principal.Identity as WindowsIdentity).Owner.Value); //Initializes UserInfo Object With SID pulled from WindowsIdentity.Owner.Value

            //Establishes CustomClaimsPrincipal on first Call and configures a new Claims Identity with Policies assigned based on users AD Group Membership
            if (CustomClaimsPrincipal == null)
            {
                CustomClaimsPrincipal = principal;
                var claimsIdentity = new ClaimsIdentity();

                //Loop through AD Group list and applies policies to user on matches in switch statement
                foreach (var group in userInfo.ADGroups)
                {
                    switch (group)
                    {
                        case "CAMP NEC Users":
                            claimsIdentity.AddClaim(new Claim("ExampleClaim", "Test"));
                            break;
                            //Use the Template below to assign AD Groups to the Policies Outlined in Startup.CS
                            //case "<Insert AD Group Name Here>":
                            //    claimsIdentity.AddClaim(new Claim(Claim Type, Claim Value));
                            //    break;
                    }
                }
                CustomClaimsPrincipal.AddIdentity(claimsIdentity);
            }

            return Task.FromResult(CustomClaimsPrincipal);
        }
    }
}
