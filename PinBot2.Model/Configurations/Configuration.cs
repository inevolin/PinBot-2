using PinBot2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PinBot2.Model.PinterestObjects;
using Newtonsoft.Json;

namespace PinBot2.Model.Configurations
{
    [Serializable]
    public class Configuration : IConfiguration
    {
        protected Configuration()
        { }

        [JsonIgnore]
        public IDictionary<string, PinterestObjectResources> Queries { get; set; }
        public List<KeyValuePair<String, PinterestObjectResources>> Queries0
        {
            get
            {
                var ls = new List<KeyValuePair<String, PinterestObjectResources>>();
                if (Queries == null) return null;
                foreach (var kv in Queries)
                {
                    ls.Add(new KeyValuePair<string, PinterestObjectResources>(kv.Key, kv.Value));
                }
                return ls;
            }
            set {
                foreach (var kv in value) {
                    if (Queries == null)
                        Queries = new Dictionary<string, PinterestObjectResources>();
                    Queries.Add(kv);
                }
            }
        }

        public bool Enabled { get; set; }
        public bool Autopilot { get; set; }
        public Range<int> Timeout { get; set; }
        public Range<DateTime> AutoStart { get; set; }
        public Range<int> CurrentCount { get; set; }
    }
}
