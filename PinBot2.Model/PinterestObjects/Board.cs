using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.PinterestObjects
{
    [Serializable]// [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public class Board : PinterestObject, IEquatable<Board>
    {
        public Board()
        { }

        public Board(string boardId, string userid, string username, string query, PinterestObjectResources res) : base (res)
        {
            UserId = userid;
            Id = boardId;
            Resource = res;
            Username = username;
            SearchQuery = query;
        }

 
        public string UserId { get; set; }

 
        public string SearchQuery { get; set; }

 
        public string Username { get; set; }

 
        public string BaseUsername { get; set; }

 
        public string Url { get; set; }

 
        public string Boardname { get; set; }

 
        public int PinsCount { get; set; }

 
        public int FollowersCount { get; set; }

 
        public bool FollowedByMe { get; set; }


        public override int GetHashCode()
        {
            if (Id == null) Id = "0";
            return Id.GetHashCode();
        }
        public bool Equals(Board other)
        {
            return this.Id.Equals(other.Id);
        }

    }
}
