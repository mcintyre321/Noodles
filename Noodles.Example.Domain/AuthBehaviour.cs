using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Noodles.Example.Domain
{
    public class AuthBehaviour
    {
        public AuthBehaviour()
        {
        }

        [Show(UiHint = "TopBar.RightItems")]
        public void SignIn([Required] string email, [DataType(DataType.Password)][Required]  string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new UserException("");
            }
            //check username and password here
            AuthService.SetAuthToken(email);
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