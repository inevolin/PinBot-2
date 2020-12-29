using PinBot2.Common;
using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.Configurations
{
    [Serializable]
    // [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public class FollowConfiguration : Configuration, IFollowConfiguration
    {
        public FollowConfiguration()
        { }

 
        public bool UsersCriteria { get; set; }
 
        public bool BoardsCriteria { get; set; }
        public bool? FollowUsers { get; set; }
        public bool? FollowBoards { get; set; }

 
        public Range<int> BoardFollowers { get; set; }
 
        public Range<int> BoardPins { get; set; }

 
        public Range<int> UserFollowers { get; set; }
 
        public Range<int> UserFollowing { get; set; }
 
        public Range<int> UserBoards { get; set; }
 
        public Range<int> UserPins { get; set; }

    }
}
