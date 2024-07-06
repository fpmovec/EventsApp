using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Web.ViewModels
{
    public class UserBrief : IEquatable<UserBrief>
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateOnly Birthday { get; set; }

        public bool Equals(UserBrief? other)
        {
            return Id == other.Id &&
                FullName == other.FullName &&
                Email == other.Email &&
                Phone == other.Phone &&
                Birthday == other.Birthday;
        }
    }
}
