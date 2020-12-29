using PinBot2.Common;
using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PinBot2.Model.Configurations
{
    // [ProtoBuf.ProtoContract, ProtoBuf.ProtoInclude(107, typeof(PinConfiguration)), ProtoBuf.ProtoInclude(108, typeof(RepinConfiguration))]
    public interface IPinRepinConfiguration : IConfiguration
    {

        [JsonIgnore]
        IDictionary<PinterestObject, Board> Queue { get; set; }
        [JsonIgnore]
        IDictionary<Board, IDictionary<string, PinterestObjectResources>> AllQueries { get; set; }
        Range<int> Scrape { get; set; }
        int DescUrlRate { get; set; }
        IList<string> DescUrls { get; set; }   
    }

    // [ProtoBuf.ProtoContract, ProtoBuf.ProtoInclude(109, typeof(RepinConfiguration))]
    public interface IRepinConfiguration : IPinRepinConfiguration
    {
        
    }

    // [ProtoBuf.ProtoContract, ProtoBuf.ProtoInclude(110, typeof(PinConfiguration))]
    public interface IPinConfiguration : IPinRepinConfiguration
    {
        IList<string> SourceUrls { get; set; }
        int SourceUrlRate { get; set; }
    }
}
