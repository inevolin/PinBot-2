using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Common
{
    [Serializable]
    // [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public class Range<T>
    {
        public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }
        private Range()
        { }

        // [ProtoBuf.ProtoMember(1)]
        public T Min { get; set; }

        // [ProtoBuf.ProtoMember(2)]
        public T Max { get; set; }

    } 
    
}
