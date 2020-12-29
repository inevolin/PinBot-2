using PinBot2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PinBot2.Model.PinterestObjects;
using Newtonsoft.Json;

namespace PinBot2.Model.Configurations
{
    // [ProtoBuf.ProtoContract, ProtoBuf.ProtoInclude(102, typeof(Configuration))]
    public interface IConfiguration
    {
        [JsonIgnore]
        IDictionary<string, PinterestObjectResources> Queries { get; set; }

        bool Enabled { get; set; }
        bool Autopilot { get; set; }
        Range<int> Timeout { get; set; }
        Range<DateTime> AutoStart { get; set; }
        Range<int> CurrentCount { get; set; }
    }
}
