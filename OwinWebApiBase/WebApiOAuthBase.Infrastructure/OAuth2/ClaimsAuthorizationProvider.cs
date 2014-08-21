using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiOAuthBase.Infrastructure.Models;

namespace WebApiOAuthBase.Infrastructure.OAuth2
{
    public class ClaimsAuthorizationProvider : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            // liste des claims de l'utilisateur (chaine construite lors de l'obtention du Token
            // dans GrantResourceOwnerCredentials de la classe ApplicationOAuthProvider
            List<Claim> t = context.Principal.Claims.ToList();
            // Authorisation demanée par le backend indique dans l'attribut de la methode
            string AskedActionAutorization = context.Action[0].Value;

            // Check if superAdmin
            if (t.Count(p => p.Type == "Auth" && p.Value == AuthorizationCode.SuperAdmin.ToString()) != 0)
            {
                return true;
            }

            // Verifier si l'utilisateur a l'autorisation demandée par le Backend via l'attribut
            if (t.Count(p => p.Type == "Auth" && p.Value == AskedActionAutorization) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }

            //  Exemple de chaine de claim recue
            // Type           Value
            //------------------
            // UserID          5
            // TenantID        1     (Autorisation en fonction du tenant encore a implementée mais le filtre par tenant si le tenantn'est pas 0 peut etre fait
            // CreateUser      Web  ( Le client n'aura dans son token que les claim de l'application qu'il utilise actuelement (soit WPF soit WEb,...) 
            // Delete User     Web  
            // Delete User     WPF 




        }
    }
}
