namespace Noodles.Example.Domain
{
    public class DummyData
    {
        public static void SeedApplication(Application application)
        {
            application.Membership.Users.Add(new User()
            {
                DisplayName = "Mr Example",
                Email = "example@email.com",
                Password = "password"
            });
            application.Organisations.Items.Add(new Organisation("Your projects")
            {
                Projects =
                {
                    new Project("Wedding planning"),
                }
            });
            application.Organisations.Items.Add(new Organisation("ACME Corps Projects")
            {
                Projects =
                {
                    new Project("North Korea Marketing Campaign"),
                    new Project("South Korea Marketing Campaign"),

                }
            });
        }
    }
}