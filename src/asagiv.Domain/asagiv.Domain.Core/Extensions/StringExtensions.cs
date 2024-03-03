namespace asagiv.Domain.Core.Extensions
{
    public static class StringExtensions
    {
        public static bool IsCarriageReturn(this string input)
        {
            return input == "\r" ||
                input == "\n" ||
                input == "\r\n";
        }
    }
}