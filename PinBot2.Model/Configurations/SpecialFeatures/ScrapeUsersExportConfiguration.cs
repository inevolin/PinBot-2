using PinBot2.Common;
using PinBot2.Model.PinterestObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Model.Configurations.SpecialFeatures
{
    [Serializable]
    public class ScrapeUsersExportConfiguration : Configuration
    {
        public ScrapeUsersExportConfiguration()
        { }

        public bool AppendToFile { get; set; }
        public bool UsersCriteria { get; set; }
        public bool DoTimeout { get; set; }
        public bool DoLimitScrape { get; set; }

        public Range<int> UserFollowers { get; set; }
        public Range<int> UserFollowing { get; set; }
        public Range<int> UserBoards { get; set; }
        public Range<int> UserPins { get; set; }

        public bool? HasCustomPic { get; set; }
        public bool? HasFb { get; set; }
        public bool? HasTw { get; set; }
        public bool? HasWebsite { get; set; }
        public bool? HasAboutText { get; set; }
        public bool? HasLocation { get; set; }

    }
}
