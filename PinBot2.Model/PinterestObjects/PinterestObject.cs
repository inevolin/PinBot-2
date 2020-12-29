using PinBot2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.PinterestObjects
{
    [Serializable]
    public enum PinterestObjectResources { SearchResource, BoardFeedResource, UserPinsResource, IndividualPin, UserResource, UserFollowersResource, UserFollowingResource, BoardFollowingResource, BoardFollowersResource, ProfileBoardsResource, UserHomefeedResource, External, PinCommentListResource, IndividualUser }

    [Serializable]
    // [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public abstract class PinterestObject
    {
        protected PinterestObject()
        { }
        public PinterestObject(PinterestObjectResources res)
        {
            this.Resource = res;
        }

 
        public string Id { get; set; }

 
        private PinterestObjectResources resource;
        public PinterestObjectResources Resource { get { return resource; } set { resource = value; } }


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            PinterestObject p = obj as PinterestObject;
            if ((System.Object)p == null)
            {
                return false;
            }

            return (Id == p.Id);
        }
        public bool Equals(PinterestObject p)
        {
            if ((object)p == null)
            {
                return false;
            }

            return (Id == p.Id);
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
