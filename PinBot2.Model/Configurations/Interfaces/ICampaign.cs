using PinBot2.Model.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.Configurations
{
    // [ProtoBuf.ProtoContract, ProtoBuf.ProtoInclude(100, typeof(Campaign))]
    public interface ICampaign
    {
        int ID { get; set; }
        int AccountId { get; set; }
        string CampaignName { get; set; }
        IConfigurationContainer ConfigurationContainer { get; set; }
    }
}
