namespace AuthService.Application.Helpers
{
    public class DefaultDateTimeService : IDateTimeService
    {
        public DateTime DateTime => DateTime.UtcNow;
    }
}
