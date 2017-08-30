using System;

namespace Tesco.ClubcardProducts.MCA.API.Common.Entities
{
    public class KVPair
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public KVPair()
        {
        }
        
        public KVPair( int id, string name)
        {
            ID = id;
            Name = name;
        }
    }
}
