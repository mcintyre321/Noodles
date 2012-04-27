namespace Noodles
{
    public interface IHasParent<out T>
    {
        T Parent { get; }
    }
}