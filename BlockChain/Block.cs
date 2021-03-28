using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain
{
    [Serializable]
    class Block
    {
        public long Index { get; set; }  //Index
        public string Data { get; set; } //Podatek
        public DateTime TimeStamp { get; set;  } //Časovna značka
        public string Hash { get; set; } //Zgoščena vrednost
        public string PreviousHash { get; set; } //Zgoščena vrednost prejšnjega bloka
        public int Nonce { get; set; } //Žeton
        public int Difficulty { get; set; } //Težavnost izračuna hasha (Koliko "0" ima hash na začetku
    }
}
