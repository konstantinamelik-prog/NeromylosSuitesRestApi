namespace NeromylosSuites.Exceptions
{
    public class EntityNotFoundException : AppException
    {
        private static readonly string DEFAULT_CODE = "Not Found";

        public EntityNotFoundException(string code, string message) 
            : base(code + DEFAULT_CODE, message)
        {
        }
    }
}
