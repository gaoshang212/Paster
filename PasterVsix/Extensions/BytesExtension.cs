using System.Text;

namespace PasterVsix.Extensions
{
    public static class BytesExtension
    {
        public static string ToArrayString(this byte[] p_bytes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var b in p_bytes)
            {
                if (sb.Length > 0)
                {
                    sb.Append(", ");
                }

                sb.AppendFormat("0x{0}", b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}