namespace Noodles.AspMvc.UiAttributes
{
    public class ShowAsListWithHeader : ShowCollectionAttribute
    {
        public ShowAsListWithHeader()
        {
            UiHint = "ListWithHeader";
        }

        public string ItemsPropertyName { get; set; }
    }
}