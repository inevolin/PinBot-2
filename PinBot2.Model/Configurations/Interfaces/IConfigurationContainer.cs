using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.Configurations
{
    // [ProtoBuf.ProtoContract, ProtoBuf.ProtoInclude(103, typeof(ConfigurationContainer))]
    public interface IConfigurationContainer
    {

        ILikeConfiguration LikeConfiguration { get; set; }
        IPinConfiguration PinConfiguration { get; set; }
        IRepinConfiguration RepinConfiguration { get; set; }
        IFollowConfiguration FollowConfiguration { get; set; }
        IUnfollowConfiguration UnfollowConfiguration { get; set; }        
        ICommentConfiguration CommentConfiguration { get; set; }
        IInviteConfiguration InviteConfiguration { get; set; }

        IList<IConfiguration> EnabledConfigurations();
        void ClearConfigurations();
    }
}
