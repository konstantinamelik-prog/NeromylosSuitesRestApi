namespace NeromylosSuites.Exceptions
{
    public class EntityNotAuthorizedException : AppException
    {
        private static readonly string DEFAULT_CODE = "Not Authorized";

        public EntityNotAuthorizedException(string code, string message)
            : base(code + DEFAULT_CODE, message)
        {
        }
    }
}
