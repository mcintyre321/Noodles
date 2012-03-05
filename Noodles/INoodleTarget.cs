namespace Noodles
{
    public interface INoodleTarget
    {
        IHasName FetchReadonly();
        IHasName Fetch();

    }
}