using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiceGame
{
    class SecretNumber
    {
        public int Number { get; set; }
        public string Key { get;}
        public string HMAC { get;}

        public SecretNumber(int number) 
        {
            this.Number = number;
            this.Key = Crypto.GetKey();
            this.HMAC = Crypto.GetHMAC(Number.ToString(), Key);
        }
    }
}
