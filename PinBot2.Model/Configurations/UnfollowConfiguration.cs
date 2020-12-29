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
    public class UnfollowConfiguration : Configuration, IUnfollowConfiguration
    {
        public UnfollowConfiguration()
        { }

 
        public bool UsersCriteria { get; set; }
 
        public bool BoardsCriteria { get; set; }
 
        public bool UnfollowUsers { get; set; }
 
        public bool UnfollowBoards { get; set; }
 
        public bool UnfollowNonFollowers { get; set; }
 
        public Range<int> BoardFollowers { get; set; }
 
        public Range<int> BoardPins { get; set; }
 
        public Range<int> UserFollowers { get; set; }
 
        public Range<int> UserPins { get; set; }
        // [ProtoBuf.ProtoMember(10)]
        public Range<int> UserFollowing { get; set; }
        // [ProtoBuf.ProtoMember(11)]
        public Range<int> UserBoards { get; set; }
             
    }
}
