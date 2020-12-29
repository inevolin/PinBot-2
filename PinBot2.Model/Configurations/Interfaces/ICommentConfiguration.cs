using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.Configurations
{
    // [ProtoBuf.ProtoContract, ProtoBuf.ProtoInclude(101, typeof(CommentConfiguration))]
    public interface ICommentConfiguration : IConfiguration
    {
        IList<string> Comments { get; set; }
    }
}
