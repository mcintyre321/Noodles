using System;

namespace Noodles.Example.Domain
{
    public class AuthService
    {
        public static Action<string> SetAuthToken { get; set; }
        public static Func<bool> RequestHasAuthToken { get; set; }
        public static Action ClearAuthToken { get; set; }
        public static Func<string> GetAuthTokenKey { get; set; }
    }
}