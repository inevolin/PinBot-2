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
    public class InviteConfiguration : Configuration, IInviteConfiguration
    {
        public InviteConfiguration()
        { }

        [JsonIgnore]
        public IDictionary<Board, bool> SelectedBoards { get; set; }
        public List<KeyValuePair<Board, bool>> SelectedBoards0
        {
            get
            {
                var ls = new List<KeyValuePair<Board, bool>>();
                if (SelectedBoards == null) return null;
                foreach (var kv in SelectedBoards)
                {
                    ls.Add(new KeyValuePair<Board, bool>(kv.Key, kv.Value));
                }
                return ls;
            }
            set
            {
                foreach (var kv in value)
                {
                    if (SelectedBoards == null)
                        SelectedBoards = new Dictionary<Board, bool>();
                    SelectedBoards.Add(kv);
                }
            }
        }
    }
}
