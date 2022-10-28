using System.Text;

namespace Hermes.Util
{
    public static class ConvertStringToUtf8
    {
        public static string ConvertToUtf8(string? text)
        {
            byte[]? utfBytes = Encoding.UTF8.GetBytes(text);
            return Encoding.UTF8.GetString(utfBytes);
        }
    }
}
