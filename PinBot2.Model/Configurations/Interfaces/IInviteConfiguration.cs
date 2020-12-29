using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PinBot2.Model.Configurations
{
    // [ProtoBuf.ProtoContract, ProtoBuf.ProtoInclude(105, typeof(InviteConfiguration))]
    public interface IInviteConfiguration : IConfiguration
    {

        [JsonIgnore]
        IDictionary<Board, bool> SelectedBoards { get; set; }
    }
}
