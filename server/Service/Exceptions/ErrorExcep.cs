namespace API.Exceptions
{
    public class ErrorExcep : Exception
    {
        public string Source { get; set; }
        public string Description { get; set; }

        public ErrorExcep(string source, string description)
            : base(description)
        {
            Source = source;
            Description = description;
        }
    }
}