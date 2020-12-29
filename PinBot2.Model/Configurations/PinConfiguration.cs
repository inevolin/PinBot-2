using PinBot2.Common;
using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PinBot2.Model.Configurations
{
    [Serializable]
    // [ProtoBuf.ProtoContract(SkipConstructor = true)]
    public class PinConfiguration : Configuration, IPinConfiguration
    {
        public PinConfiguration()
        { }

        [JsonIgnore]
        public IDictionary<PinterestObject, Board> Queue { get; set; }
        public List<KeyValuePair<PinterestObject, Board>> Queue0
        {
            get
            {
                var ls = new List<KeyValuePair<PinterestObject, Board>>();
                if (Queue == null) return null;
                foreach (var kv in Queue)
                {
                    ls.Add(new KeyValuePair<PinterestObject, Board>(kv.Key, kv.Value));
                }
                return ls;
            }
            set
            {
                foreach (var kv in value)
                {
                    if (Queue == null)
                        Queue = new Dictionary<PinterestObject, Board>();
                    Queue.Add(kv);
                }
            }
        }

        [JsonIgnore]
        public IDictionary<Board, IDictionary<string, PinterestObjectResources>> AllQueries { get; set; }
        public List<KeyValuePair<Board, List<KeyValuePair<string, PinterestObjectResources>>>> AllQueries0
        {
            get
            {
                var ls = new List<KeyValuePair<Board, List<KeyValuePair<string, PinterestObjectResources>>>>();
                if (AllQueries == null) return null;
                foreach (var kv in AllQueries)
                {
                    var kv0 = new KeyValuePair<Board, List<KeyValuePair<string, PinterestObjectResources>>>(kv.Key, new List<KeyValuePair<string, PinterestObjectResources>>());
                    if (kv.Value == null) continue;
                    foreach (var kv00 in kv.Value)
                        kv0.Value.Add(kv00);
                    ls.Add(kv0);
                }
                return ls;
            }
            set
            {
                foreach (var kv in value)
                {
                    if (AllQueries == null)
                        AllQueries = new Dictionary<Board, IDictionary<string, PinterestObjectResources>>();

                    var val = new Dictionary<string, PinterestObjectResources>();
                    if (kv.Value == null) continue;
                    foreach (var kv0 in kv.Value)
                        val.Add(kv0.Key,kv0.Value);

                    AllQueries.Add(kv.Key, val);
                }
            }
        }

        public Range<int> Scrape { get; set; }
        
        public IList<string> SourceUrls { get; set; }
        public int SourceUrlRate { get; set; }

        public int DescUrlRate { get; set; }
        public IList<string> DescUrls { get; set; }
    }
}
