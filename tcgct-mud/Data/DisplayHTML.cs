namespace tcgct_mud.Data
{
    public class DisplayHTML<T>
    {
        public DisplayHTML(T displayObject)
        {
            Object = displayObject;
            HTML = new HTMLHelperProps();
        }
        public T Object { get; set; }
        public HTMLHelperProps HTML { get; set; }
    }
}
