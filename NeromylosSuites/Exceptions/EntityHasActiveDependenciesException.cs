namespace NeromylosSuites.Exceptions
{
    public class EntityHasActiveDependenciesException : AppException
    {
        private static readonly string DEFAULT_CODE = "Has Active Dependencies";

        public EntityHasActiveDependenciesException(string code, string message)
            : base(code + DEFAULT_CODE, message)
        {
        }
    }
}