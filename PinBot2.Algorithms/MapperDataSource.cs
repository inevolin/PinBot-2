using PinBot2.Common;
using PinBot2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Algorithms
{
    public class MapperDataSource
    {
        public IAccount Account { get; set; } //data of row

        public string Username { get { return Account.Username; } }
        public string Email { get { return Account.Email; } }
        public string Status { get { return Account.getStatus; } }

        ////////
        public Algo LikeAlgo { get; set; } //data of cell
        public string LikeStatus { get { return LikeAlgo.getStatus + LikeCurrentCount; } }
        public string LikeCurrentCount { get { return LikeAlgo.CurrentCount == null ? "" : " (" + LikeAlgo.CurrentCount.Min + "/" + LikeAlgo.CurrentCount.Max + ")"; } }

        public Algo FollowAlgo { get; set; } //data of cell
        public string FollowStatus { get { return FollowAlgo.getStatus + FollowCurrentCount; } }
        public string FollowCurrentCount { get { return FollowAlgo.CurrentCount == null ? "" : " (" + FollowAlgo.CurrentCount.Min + "/" + FollowAlgo.CurrentCount.Max + ")"; } }

        public Algo UnfollowAlgo { get; set; } //data of cell
        public string UnfollowStatus { get { return UnfollowAlgo.getStatus + UnfollowCurrentCount; } }
        public string UnfollowCurrentCount { get { return UnfollowAlgo.CurrentCount == null ? "" : " (" + UnfollowAlgo.CurrentCount.Min + "/" + UnfollowAlgo.CurrentCount.Max + ")"; } }

        public Algo RepinAlgo { get; set; } //data of cell
        public string RepinStatus { get { return RepinAlgo.getStatus + RepinCurrentCount; } }
        public string RepinCurrentCount { get { return RepinAlgo.CurrentCount == null ? "" : " (" + RepinAlgo.CurrentCount.Min + "/" + RepinAlgo.CurrentCount.Max + ")"; } }

        public Algo InviteAlgo { get; set; } //data of cell
        public string InviteStatus { get { return InviteAlgo.getStatus + InviteCurrentCount; } }
        public string InviteCurrentCount { get { return InviteAlgo.CurrentCount == null ? "" : " (" + InviteAlgo.CurrentCount.Min + "/" + InviteAlgo.CurrentCount.Max + ")"; } }

        public Algo PinAlgo { get; set; } //data of cell
        public string PinStatus { get { return PinAlgo.getStatus + PinCurrentCount; } }
        public string PinCurrentCount { get { return PinAlgo.CurrentCount == null ? "" : " (" + PinAlgo.CurrentCount.Min + "/" + PinAlgo.CurrentCount.Max + ")"; } }

        public Algo CommentAlgo { get; set; } //data of cell
        public string CommentStatus { get { return CommentAlgo.getStatus + CommentCurrentCount; } }
        public string CommentCurrentCount { get { return CommentAlgo.CurrentCount == null ? "" : " (" + CommentAlgo.CurrentCount.Min + "/" + CommentAlgo.CurrentCount.Max + ")"; } }
        //...

        public static IList<MapperDataSource> DATASOURCE;
        private static IList<IAccount> accounts;
        public static /*IEnumerable<MapperDataSource>*/ void DataSource(IList<IAccount> accounts = null)
        {
            try
            {
                if (DATASOURCE == null)
                    DATASOURCE = new List<MapperDataSource>();
                if (accounts != null)
                    MapperDataSource.accounts = accounts;
                else
                    accounts = MapperDataSource.accounts;

                IList<MapperDataSource> list = new List<MapperDataSource>();
                foreach (IAccount a in accounts)
                {
                    MapperDataSource mds = new MapperDataSource();
                    mds.Account = a;
                    IList<Algo> algos = Mapper.mapping[a];

                    ////////
                    mds.LikeAlgo = algos.First(x => x.GetType() == typeof(LikeAlgo));
                    mds.FollowAlgo = algos.First(x => x.GetType() == typeof(FollowAlgo));
                    mds.UnfollowAlgo = algos.First(x => x.GetType() == typeof(UnfollowAlgo));
                    mds.RepinAlgo = algos.First(x => x.GetType() == typeof(RepinAlgo));
                    mds.InviteAlgo = algos.First(x => x.GetType() == typeof(InviteAlgo));
                    mds.PinAlgo = algos.First(x => x.GetType() == typeof(PinAlgo));
                    mds.CommentAlgo = algos.First(x => x.GetType() == typeof(CommentAlgo));
                    //...

                    list.Add(mds);
                }
                DATASOURCE = list;
                //return new List<MapperDataSource>(list);
            }
            catch (Exception ex)
            {
                string msg = "Error MDS84." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("auto", "MapperDataSrc", msg);
            }
        }

        public static void UpdateDataSource()
        {
            try
            {
                for (int i = 0; i < DATASOURCE.Count; i++)
                {
                    var v = DATASOURCE[i];
                    IList<Algo> algos = Mapper.mapping[v.Account];
                    v.CommentAlgo = algos.First(x => x.GetType() == typeof(CommentAlgo));
                    v.LikeAlgo = algos.First(x => x.GetType() == typeof(LikeAlgo));
                    v.FollowAlgo = algos.First(x => x.GetType() == typeof(FollowAlgo));
                    v.UnfollowAlgo = algos.First(x => x.GetType() == typeof(UnfollowAlgo));
                    v.PinAlgo = algos.First(x => x.GetType() == typeof(PinAlgo));
                    v.InviteAlgo = algos.First(x => x.GetType() == typeof(InviteAlgo));
                    v.RepinAlgo = algos.First(x => x.GetType() == typeof(RepinAlgo));
                }
            }
            catch (Exception ex)
            {
                string msg = "Error MDS108." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log("auto", "MapperDataSrc", msg);
            }
        }


    }
}
