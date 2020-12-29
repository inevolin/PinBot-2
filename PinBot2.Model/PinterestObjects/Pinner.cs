using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.PinterestObjects
{
    [Serializable]// [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public class Pinner : PinterestObject
    {
        protected Pinner()
        { }
        public Pinner(string id, string username, string baseUsername, string query, PinterestObjectResources res)
            : base(res)
        {
            Id = id;
            Username = username;
            BaseUsername = baseUsername;
            Resource = res;
            SearchQuery = query;
        }


 
        public string Username { get; set; }

 
        public string BaseUsername { get; set; }

 
        public string SearchQuery { get; set; }

 
        public int PinsCount { get; set; }

 
        public int FollowersCount { get; set; }

 
        public int FollowingCount { get; set; }

 
        public int BoardsCount { get; set; }

 
        public bool FollowedByMe { get; set; }
        
    }
}
