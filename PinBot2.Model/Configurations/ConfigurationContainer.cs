using Newtonsoft.Json;
using PinBot2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.Configurations
{
    [Serializable]
    // [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public class ConfigurationContainer : IConfigurationContainer
    {
        public ConfigurationContainer()
        { }


        [JsonConverter(typeof(ConcreteConverter<LikeConfiguration>))]    
        public ILikeConfiguration LikeConfiguration { get; set; }

        [JsonConverter(typeof(ConcreteConverter<PinConfiguration>))]    
        public IPinConfiguration PinConfiguration { get; set; }

        [JsonConverter(typeof(ConcreteConverter<RepinConfiguration>))]    
        public IRepinConfiguration RepinConfiguration { get; set; }

        [JsonConverter(typeof(ConcreteConverter<FollowConfiguration>))]    
        public IFollowConfiguration FollowConfiguration { get; set; }

        [JsonConverter(typeof(ConcreteConverter<UnfollowConfiguration>))]    
        public IUnfollowConfiguration UnfollowConfiguration { get; set; }

        [JsonConverter(typeof(ConcreteConverter<CommentConfiguration>))]    
        public ICommentConfiguration CommentConfiguration { get; set; }

        [JsonConverter(typeof(ConcreteConverter<InviteConfiguration>))]    
        public IInviteConfiguration InviteConfiguration { get; set; }
        //...

 
        private IList<IConfiguration> configurations = new List<IConfiguration>();


        public IList<IConfiguration> EnabledConfigurations()
        {
       

                if (LikeConfiguration != null && LikeConfiguration.Enabled && !configurations.Contains(LikeConfiguration))
                    configurations.Add(LikeConfiguration);
                if (PinConfiguration != null && PinConfiguration.Enabled && !configurations.Contains(PinConfiguration))
                    configurations.Add(PinConfiguration);
                if (RepinConfiguration != null && RepinConfiguration.Enabled && !configurations.Contains(RepinConfiguration))
                    configurations.Add(RepinConfiguration);
                if (FollowConfiguration != null && FollowConfiguration.Enabled && !configurations.Contains(FollowConfiguration))
                    configurations.Add(FollowConfiguration);
                if (UnfollowConfiguration != null && UnfollowConfiguration.Enabled && !configurations.Contains(UnfollowConfiguration))
                    configurations.Add(UnfollowConfiguration);
                if (CommentConfiguration != null && CommentConfiguration.Enabled && !configurations.Contains(CommentConfiguration))
                    configurations.Add(CommentConfiguration);
                if (InviteConfiguration != null && InviteConfiguration.Enabled && !configurations.Contains(InviteConfiguration))
                    configurations.Add(InviteConfiguration);
                //...


                return configurations;
        }
        public void ClearConfigurations()
        { configurations.Clear(); }
    }
}
