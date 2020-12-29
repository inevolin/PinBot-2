using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.Configurations
{
    public class ConcreteConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType) { return true; }

        public override object ReadJson(JsonReader reader,
         Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<T>(reader);
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }

    [Serializable]// [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public class Campaign : ICampaign
    {

 
        public int ID { get; set; }
 
        public int AccountId { get; set; }
 
        public string CampaignName { get; set; }

        [JsonConverter(typeof(ConcreteConverter<ConfigurationContainer>))]    
        public IConfigurationContainer ConfigurationContainer { get; set; }

        public Campaign()
        { }
        public Campaign(Account account)
        {
            AccountId = account.Id;
            ConfigurationContainer = new ConfigurationContainer();

            ///////////
            ConfigurationContainer.LikeConfiguration = new LikeConfiguration();
            ConfigurationContainer.FollowConfiguration = new FollowConfiguration();
            ConfigurationContainer.UnfollowConfiguration = new UnfollowConfiguration();
            ConfigurationContainer.RepinConfiguration = new RepinConfiguration();
            ConfigurationContainer.InviteConfiguration = new InviteConfiguration();
            ConfigurationContainer.PinConfiguration = new PinConfiguration();
            ConfigurationContainer.CommentConfiguration = new CommentConfiguration();
            //...
        }
    }
}
