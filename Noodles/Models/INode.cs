namespace Noodles.Models
{
    public interface INode
    {
        INode GetChild(string fragment);
        string Fragment { get; }
        string DisplayName { get; }
        string Url { get; }
        INode Parent { get; }
        string UiHint { get; }
    }
}