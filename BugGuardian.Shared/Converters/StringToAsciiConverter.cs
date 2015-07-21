namespace DBTek.BugGuardian.Converters
{
    internal class StringToAsciiConverter
    {
        /// <summary>
        /// Convert a string to it's ascii byte array equivalent
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] StringToAscii(string str)
        {
            byte[] rtn = new byte[str.Length];
            for (int i = 0; i < str.Length; ++i)
            {
                char ch = str[i];
                if (ch <= 0x7f)
                    rtn[i] = (byte)ch;
                else
                    rtn[i] = (byte)'?';
            }
            return rtn;
        }
    }
}
