namespace WebNoodle
{
    public interface INoodleTarget
    {
        IHasName FetchReadonly();
        IHasName Fetch();

    }
}