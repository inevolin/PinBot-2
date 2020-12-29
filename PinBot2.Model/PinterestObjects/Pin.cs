using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.PinterestObjects
{
    [Serializable]// [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public class Pin : PinterestObject
    {
        protected Pin()
        { }

        public Pin(string id, string username, string query, string boardId, string boardName, string image, PinterestObjectResources res)
            : base(res)
        {
            this.Id = id;
            Resource = res;
            Username = username;
            SearchQuery = query;
            BoardId = boardId;
            Boardname = boardName;
            Image = image;
        }


 
        public string Image { get; set; }

 
        public string Username { get; set; }

 
        public string SearchQuery { get; set; }

 
        public string BoardId { get; set; }

 
        public string Boardname { get; set; }

 
        public string Description { get; set; }

 
        public string Link { get; set; }

 
        public bool LikedByMe { get; set; }

    }
}
