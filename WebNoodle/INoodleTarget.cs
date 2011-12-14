namespace WebNoodle
{
    public interface INoodleTarget
    {
        INode FetchReadonly();
        INode Fetch();

    }
}