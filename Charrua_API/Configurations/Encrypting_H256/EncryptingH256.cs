using System.Security.Cryptography;
using System.Text;

namespace Charrua_API.Configurations.Encrypting_H256
{
    public class EncryptingH256
    {
        private static EncryptingH256 instancia;

        public static EncryptingH256 getInstance()
        {
            if (instancia == null)
            {
                instancia= new EncryptingH256();
            }
            return instancia;
        }


        public string ConvertirSHA256(string texto)
        {
            string hash = string.Empty;

            SHA256 sha256 = SHA256.Create();

            byte[] hashValue = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));

            foreach(byte b in hashValue)
            {
                hash += $"{b:X2}";
            }
            return hash;
        }

        public string GenerarToken()
        {
            string token = Guid.NewGuid().ToString("N");
            return token;
        }

    }
}
