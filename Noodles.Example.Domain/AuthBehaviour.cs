using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web;
using FormFactory.Attributes;

namespace Noodles.Example.Domain
{
    public class AuthBehaviour
    {
        public AuthBehaviour()
        {
        }

        [Show(UiHint = "TopBar.RightItems")]
        public void SignIn([Required] [Placeholder("Enter any email address")]string email, [DataType(DataType.Password)][Required]  string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new UserException("Please enter any username or password");
            }
            //check username and password here
            AuthService.SetAuthToken(email);
        }
        
        public string SignIn_email_default()
        {
            return HttpContext.Current.Request["email"] ?? "demo@email.com";
        }

        public bool? AllowSignIn()
        {
            return !AuthService.RequestHasAuthToken();
        }

        [Show(UiHint = "TopBar.RightItems"), AutoSubmit]
        public void SignOut()
        {
            AuthService.ClearAuthToken();
        }

        public bool? Allow(MethodInfo methodInfo)
        {
            return AuthService.RequestHasAuthToken() ? null as bool? : false;
        }
    }
}