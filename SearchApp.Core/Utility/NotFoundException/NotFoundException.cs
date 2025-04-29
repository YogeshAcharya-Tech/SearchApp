namespace SearchApp.Domain.Utility.NotFoundException
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
