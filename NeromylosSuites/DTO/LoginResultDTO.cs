namespace NeromylosSuites.DTO
{
    public record LoginResultDTO
    {
        public UserReadOnlyDTO User { get; init; } = null!;
        public string Token { get; init; } = string.Empty;
    }
}
