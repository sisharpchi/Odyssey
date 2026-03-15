using AuthService.Core.Entities.Common;

namespace AuthService.Core.Entities
{
    public class EmailOtpCodeMessage : IEntity<long>
    {
        public long Id { get; set; }
        public string Email { get; set; } = default!;
        public string Code { get; set; } = default!;
        public DateTime SendedAt { get; set; }
        public DateTime? UsedAt { get; set; }
    }
}
