﻿using PinBot2.Common;
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
    public class CommentConfiguration : Configuration, ICommentConfiguration
    {
        public CommentConfiguration()
        { }

 
        public IList<string> Comments { get; set; }
    }
}
