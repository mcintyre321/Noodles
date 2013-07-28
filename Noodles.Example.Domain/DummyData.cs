using Noodles.Example.Domain.Discussions;
using Noodles.Example.Domain.Tasks;

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
            var project = new Project("Wedding planning");
            application.Organisations.Items.Add(new Organisation("Your projects")
            {
                Projects =
                {
                    project
                }
            });
            project.DiscussionsManager.NewDiscussion(new Discussion() {Title = "Where should we get married?"},
                                                     new Message() {Text = "I'd like to do it at the zoo"});
            var toDoList = new ToDoList() {ListName = "Things to buy"};
            project.ToDoLists.AddList(toDoList);
            toDoList.AddTask(new Task() { Title = "Wedding Dress" });
            toDoList.AddTask(new Task() { Title = "Wedding Rings" });

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