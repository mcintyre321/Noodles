using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using FormFactory.Attributes;
using Noodles.AspMvc.RequestHandling.Transforms;
using Noodles.AspMvc.UiAttributes;

namespace Noodles.Example.Domain
{
    public class AuthBehaviour
    {
        public AuthBehaviour()
        {
        }

        [Show][Dropdown]
        [Description("You can enter any email/password combo - this is just an example!")]
        public RedirectResult SignIn([Required] [Placeholder("Enter any email address")]string email, [DataType(DataType.Password)][Required]  string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new UserException("Please enter any username or password");
            }
            if(password == "error") throw new UserException("An error occurred");
            //check username and password here
            AuthService.SetAuthToken(email);
            return new RedirectResult(HttpContext.Current.Request["ReturnUrl"] ?? "../");
        }
        
        public string SignIn_email_default()
        {
            return HttpContext.Current.Request["email"] ?? "demo@email.com";
        }

        public bool? AllowSignIn()
        {
            return !AuthService.RequestHasAuthToken();
        }

        [Show()][AutoSubmit]
        public RedirectResult SignOut()
        {
            AuthService.ClearAuthToken();
            return new RedirectResult(HttpContext.Current.Request["ReturnUrl"] ?? "../");
        }

        public bool? Allow(MethodInfo methodInfo)
        {
            return AuthService.RequestHasAuthToken() ? null as bool? : false;
        }
    }
}