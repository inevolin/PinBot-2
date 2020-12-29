using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.PinterestObjects
{
    [Serializable]
    public class ExternalPin : PinterestObject
    {
        protected ExternalPin()
        { }

        public ExternalPin(string description, string imageFound, List<string> tags, PinterestObjectResources res)
            : base(res)
        {
            this.Description = description;
            this.Id = this.ImageFound = imageFound;
            this.Tags = tags;
        }


 
        public string Description { get; set; }

 
        public string ImageFound { get; set; }
        public string ImageUploaded { get; set; }

 
        public List<string> Tags { get; set; }

 
        public string Link { get; set; }


    }
}
