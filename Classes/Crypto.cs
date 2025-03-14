using System;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace DiceGame
{
    class Crypto
    {
        public static string GetKey()
        {
            Random rng = new Random();
            return GetHash(rng.Next(-2147483648, 2147483647).ToString());
        }

        private static string GetHash(string input)
        {
            byte[] data = Encoding.UTF8.GetBytes(input);
            Sha3Digest sha3 = new Sha3Digest(256);
            sha3.BlockUpdate(data, 0, data.Length);
            byte[] hash = new byte[sha3.GetDigestSize()];
            sha3.DoFinal(hash, 0);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public static string GetHMAC(string message, string key)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            HMac hmac = new HMac(new Sha3Digest(256));
            hmac.Init(new KeyParameter(keyBytes));
            hmac.BlockUpdate(messageBytes, 0, messageBytes.Length);
            byte[] hmacResult = new byte[hmac.GetMacSize()];
            hmac.DoFinal(hmacResult, 0);
            return BitConverter.ToString(hmacResult).Replace("-", "").ToLower();
        }
    }
}
