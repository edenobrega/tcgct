namespace tcgct.Data.Interface
{
    public interface ITableItem
    {
        string Display { get; set; }
        string Visibility { get; set; }
        bool Filtered { get; set; }
    }
}
