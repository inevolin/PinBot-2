using PinBot2.Common;
using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.Configurations
{
    // [ProtoBuf.ProtoContract, ProtoBuf.ProtoInclude(104, typeof(FollowConfiguration))]
    public interface IFollowConfiguration : IConfiguration
    {
        bool UsersCriteria { get; set; }
        bool BoardsCriteria { get; set; }
        bool? FollowUsers { get; set; }
        bool? FollowBoards { get; set; }

        Range<int> BoardFollowers { get; set; }
        Range<int> BoardPins { get; set; }

        Range<int> UserFollowers { get; set; }
        Range<int> UserPins { get; set; }
        Range<int> UserFollowing { get; set; }
        Range<int> UserBoards { get; set; }
    }
}
