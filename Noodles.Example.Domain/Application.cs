using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Noodles.Example.Domain.Tasks;

namespace Noodles.Example.Domain
{
    [DisplayName("Project Organiser")]
    public class Application
    {
        [Link]
        public ToDoLists ToDoLists { get; private set; }
        
        [Link]
        public Discussions.DiscussionsManager DiscussionsManager { get; private set; }

        public Application()
        {
            ToDoLists = new ToDoLists();
            DiscussionsManager = new Discussions.DiscussionsManager();
            Settings = new Settings();
            Membership = new Membership();
            WelcomeMessage = "See https://github.com/mcintyre321/Noodles/blob/master/Noodles.Example.Domain/Application.cs";
        }

        public string WelcomeMessage { [Show] get; set; }

        [Link]
        public Membership Membership { get; set; }

        [Link]
        public Settings Settings { get; set; }

        [Show(UiHint = "TopBar.RightItems")]
        public void SignIn(string email, [DataType(DataType.Password)] string password)
        {
            //check username and password here
            AuthService.SetAuthToken(email);
        }

        public bool? AllowSignIn()
        {
            return AuthService.RequestHasAuthToken() ? false : null as bool?;
        }

        [Show(UiHint = "TopBar.RightItems")]
        public void SignOut()
        {
            AuthService.ClearAuthToken();
        }

        public bool? AllowSignOut()
        {
            return AuthService.RequestHasAuthToken() ? null as bool? : false;
        }

        [Show][HttpGet]
        public RedirectResult Google()
        {
            return new RedirectResult("http://google.com");
        }
    }
}