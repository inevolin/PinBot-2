using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.PinterestObjects
{
    [Serializable]// [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public class Comment : PinterestObject
    {
        protected Comment()
        { }

        public Comment(string id, string username, string comment, PinterestObjectResources res)
            : base(res)
        {
            this.Id = id;
            Username = username;
            Text = comment;
        }

 
        public string Username { get; set; }

 
        public string Text { get; set; }
    }
}
