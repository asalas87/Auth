using System.Text.RegularExpressions;

namespace Domain.ValueObjects
{
    public partial record Email
    {
        private const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
        private Email(string value) => Value = value;
        public static Email? Create(string value)
        {
            if(string.IsNullOrEmpty(value) || !EmailRegex().IsMatch(value)) return null;
            return new Email(value);
        }
        public string Value { get; init; }
        [GeneratedRegex(Pattern)]
        private static partial Regex EmailRegex();
    }
}
