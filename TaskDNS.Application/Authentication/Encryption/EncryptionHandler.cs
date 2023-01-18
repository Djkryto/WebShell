using System.Text;

namespace TaskDNS.Application.Authentication.Encryption
{
    /// <summary>
    /// Класс кодирования.
    /// </summary>
    public static class EncryptionHandler
    {
        /// <summary>
        /// Шифрование пароля.
        /// </summary>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        public static string EncodingPassword(string password)
        {
            var byteString = Encoding.UTF32.GetBytes(password);
            var changesByteString = EncodingByteString(byteString);
            string dilutedHash = Convert.ToBase64String(changesByteString);

            return ChangeStartEndString(dilutedHash);
        }

        /// <summary>
        /// Расшифровка пароля.
        /// </summary>
        /// <param name="password">Пароль.</param>
        /// <returns></returns>
        public static string DecodingPassword(string dilutedHash)
        {
            var changesByteString = Convert.FromBase64String(dilutedHash);
            var byteString = DecodingByteString(changesByteString);
            string password = Encoding.UTF32.GetString(byteString);

            return ChangeStartEndString(password);
        }

        private static string ChangeStartEndString(string password)
        {
            char end = password[^1];
            char start = password[0];

            string middle = password.Remove(0, 1).Remove(password.Length - 1);

            return end + middle + start;
        }

        private static byte[] EncodingByteString(byte[] password)
        {
            for (byte i = 0; i < password.Length; i++)
                if (i % 3 == 0)
                    password[i] = (byte)(password[i] + 3);

            return password;
        }

        private static byte[] DecodingByteString(byte[] password)
        {
            for (byte i = 0; i < password.Length; i++)
                if (i % 3 == 0)
                    password[i] = (byte)(password[i] - 3);

            return password;
        }
    }
}
