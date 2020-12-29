using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PinBot2.Algorithms.Scraping;
using PinBot2.Model.PinterestObjects;
using PinBot2.Model;

using PinBot2.Model.Configurations;
using PinBot2.Common;
using System.Web;
using PinBot2.Algorithms.Scraping.Queue;
using PinBot2.Model.Configurations.SpecialFeatures;

namespace PinBot2.Algorithms.Scraping
{
    public class ScrapeSessionManager
    {
        public IList<PinterestObject> UsedPinterestObjects { get; set; }
        private int MAX_COUNT_RESTART = 2; //if this limit is reached for a session, it will delete itself from list.
        private IConfiguration Config;
        private http request;
        internal IList<ScrapeSession> Sessions { get; private set; }
        private IAccount account;

        private bool abort = false;
        public void Abort() {
            abort = true;
        }

        public ScrapeSessionManager(Algo TypeAlgo, IConfiguration Config, http request, IAccount account)
        {
            try
            {
                this.Config = Config;
                this.request = request;
                this.account = account;
                Sessions = new List<ScrapeSession>();
                UsedPinterestObjects = new List<PinterestObject>();

                if (TypeAlgo.GetType() == typeof(LikeAlgo))
                    LikeAlgo();
                else if (TypeAlgo.GetType() == typeof(FollowAlgo))
                    FollowAlgo();
                else if (TypeAlgo.GetType() == typeof(UnfollowAlgo))
                    UnfollowAlgo();
                else if (TypeAlgo.GetType() == typeof(RepinAlgo))
                    RepinAlgo();
                else if (TypeAlgo.GetType() == typeof(InviteAlgo))
                    InviteAlgo();
                else if (TypeAlgo.GetType() == typeof(CommentAlgo))
                    CommentAlgo();
                else if (TypeAlgo.GetType() == typeof(PinAlgo))
                    PinAlgo();
                else if (TypeAlgo.GetType() == typeof(PinQueueAlgo))
                    PinQueueAlgo();
                else if (TypeAlgo.GetType() == typeof(RepinQueueAlgo))
                    RepinAlgo();
                else if (TypeAlgo.GetType() == typeof(ScrapeUsersExportAlgo))
                    ScrapeUsersExportAlgo();

            }
            catch (Exception ex)
            {
                string msg = "Error SSM58." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        private void LikeAlgo()
        {
            try
            {
                ScrapeSession ses = null;
                foreach (var kv in ((LikeConfiguration)Config).Queries)
                {
                    switch (kv.Value)
                    {
                        case PinterestObjectResources.SearchResource:       //scrape keywords {pins}
                            ses = new ScrapePins_SearchResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.BoardFeedResource:    //scrape /username/boardname/
                            ses = new ScrapePins_BoardFeedResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.UserPinsResource:     //scrape /username/pins/
                            ses = new ScrapePins_UserPinsResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SSM88." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        private void RepinAlgo()
        {
            try
            {
                ScrapeSession ses = null;
                foreach (var kv in ((IPinRepinConfiguration)Config).Queries)
                {
                    switch (kv.Value)
                    {
                        case PinterestObjectResources.SearchResource:       //scrape keywords {pins}
                            ses = new ScrapePins_SearchResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.BoardFeedResource:    //scrape /username/boardname/
                            ses = new ScrapePins_BoardFeedResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.UserPinsResource:     //scrape /username/pins/
                            ses = new ScrapePins_UserPinsResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.IndividualPin:     //scrape /pin/0123456789/
                            ses = new IndividualPin(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SSM118." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        private void FollowAlgo()
        {
            try
            {
                ScrapeSession ses = null;
                foreach (var kv in ((FollowConfiguration)Config).Queries)
                {
                    switch (kv.Value)
                    {
                        case PinterestObjectResources.SearchResource: //scrape keywords {pinners,boards}
                            if (!((FollowConfiguration)Config).FollowBoards.HasValue ||
                                ((FollowConfiguration)Config).FollowBoards == true)
                            {
                                ses = new ScrapeBoards_SearchResource(kv.Key, request, account, Config);
                                Sessions.Add(ses);
                            }
                            if (!((FollowConfiguration)Config).FollowUsers.HasValue ||
                                ((FollowConfiguration)Config).FollowUsers == true)
                            {
                                ses = new ScrapePinners_SearchResource(kv.Key, request, account, Config);
                                Sessions.Add(ses);
                            }
                            break;
                        case PinterestObjectResources.UserFollowersResource:    //scrape /username/followers/
                            ses = new ScrapePinners_UserFollowersResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.BoardFollowersResource:   //scrape /username/boardname/followers/
                            ses = new ScrapePinners_BoardFollowersResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.ProfileBoardsResource:   //scrape /username/boards/
                            ses = new ScrapeBoards_ProfileBoardsResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.UserFollowingResource:    //scrape /username/following/people/
                            ses = new ScrapePinners_UserFollowingResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.BoardFollowingResource:   //scrape /username/following/boards/
                            ses = new ScrapeBoards_BoardFollowingResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.IndividualUser:   //scrape /username/
                            ses = new IndividualUser(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SSM168." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        private void UnfollowAlgo()
        {
            try
            {
                ScrapeSession ses = null;

                if (((UnfollowConfiguration)Config).UnfollowUsers)
                {
                    ses = new ScrapePinners_UserFollowingResource("/" + account.Username + "/following/people/", request, account, Config);
                    Sessions.Add(ses);
                }

                if (((UnfollowConfiguration)Config).UnfollowBoards)
                {
                    ses = new ScrapeBoards_BoardFollowingResource("/" + account.Username + "/following/boards/", request, account, Config);
                    Sessions.Add(ses);
                }

                /*if (((UnfollowConfiguration)Config).UnfollowNonFollowers)
                {
                    ses = new ScrapePinners_UserFollowingResource("/" + account.Username + "/following/people/", request, account, Config);
                    Sessions.Add(ses);
                }*/
            }
            catch (Exception ex)
            {
                string msg = "Error SSM198." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        private void InviteAlgo()
        {
            try
            {
                ScrapeSession ses = null;

                foreach (var kv in ((InviteConfiguration)Config).Queries)
                {
                    /*if (kv.Value == true)
                    {*/
                    ses = new ScrapePinners_BoardFollowersResource(kv.Key, request, account, Config);
                    Sessions.Add(ses);
                    /*}*/
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SSM219." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        private void CommentAlgo()
        {
            try
            {
                ScrapeSession ses = null;
                foreach (var kv in ((CommentConfiguration)Config).Queries)
                {
                    switch (kv.Value)
                    {
                        case PinterestObjectResources.SearchResource:       //scrape keywords {pins}
                            ses = new ScrapePins_SearchResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.BoardFeedResource:    //scrape /username/boardname/
                            ses = new ScrapePins_BoardFeedResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.UserPinsResource:     //scrape /username/pins/
                            ses = new ScrapePins_UserPinsResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SSM249." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        private void PinAlgo()
        {
            try
            {
                ScrapeSession ses = null;
                foreach (var kv in ((IPinRepinConfiguration)Config).Queries)
                {
                    switch (kv.Value)
                    {
                        case PinterestObjectResources.External:       //scrape keywords {pins}
                            //ses = new ScrapeExternal_imgfave(kv.Key, request, account, Config);
                            ses = new ScrapeExternal_tumblr(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SSM271." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        private void PinQueueAlgo()
        {
            try
            {
                ScrapeSession ses = null;
                foreach (var kv in ((IPinRepinConfiguration)Config).Queries)
                {
                    switch (kv.Value)
                    {
                        case PinterestObjectResources.External:       //scrape keywords {pins}
                            //ses = new ScrapeExternal_imgfave(kv.Key, request, account, Config);
                            ses = new ScrapeExternal_tumblr(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SSM293." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }
        private void ScrapeUsersExportAlgo()
        {
            try
            {
                ScrapeSession ses = null;
                foreach (var kv in ((ScrapeUsersExportConfiguration)Config).Queries)
                {
                    switch (kv.Value)
                    {
                        case PinterestObjectResources.SearchResource: //scrape keywords {pinners,boards}
                            ses = new ScrapePinners_SearchResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.UserFollowersResource:    //scrape /username/followers/
                            ses = new ScrapePinners_UserFollowersResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.UserFollowingResource:    //scrape /username/following/people/
                            ses = new ScrapePinners_UserFollowingResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                        case PinterestObjectResources.BoardFollowersResource:   //scrape /username/boardname/followers/
                            ses = new ScrapePinners_BoardFollowersResource(kv.Key, request, account, Config);
                            Sessions.Add(ses);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = "Error SSM333." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg);
            }
        }

        public IList<PinterestObject> Scrape(string ForBoard)
        {
            if (Sessions.Count == 0) return null;

            Random r = new Random();
            ScrapeSession ses = Sessions[r.Next(Sessions.Count)];
            string LoggingAppendix = " for :: " + ForBoard + " :: " + ses.Query;
            Logging.Log("user or auto", ses.GetType().ToString(), "scraping" + LoggingAppendix);

            try
            {
                ses.Scrape();
                Logging.Log(account.Username, ses.GetType().ToString(), "scraped " + ses.PinterestObjects.Count + LoggingAppendix);

                while (ses.PinterestObjects.Count <= 0 && ses.CountRestarted < MAX_COUNT_RESTART)
                {
                    ses.Scrape();
                    Logging.Log(account.Username, ses.GetType().ToString(), "scraped " + ses.PinterestObjects.Count + LoggingAppendix);
                    if (abort == true) { abort = false; return new List<PinterestObject>(ses.PinterestObjects); }
                    if (ses.EndReached)
                    {
                        Logging.Log(account.Username, ses.GetType().ToString(), "EndReached retrying #" + ses.CountRestarted + LoggingAppendix);
                        ses.FirstRequest = true; //nothing to scrape, start again.
                        ++ses.CountRestarted;
                        ses.EndReached = false;
                    }
                }
                ses.CountRestarted = 0;
                ManageSession(ses, ForBoard);

                if (Sessions.Count <= 0 && ses.PinterestObjects.Count <= 0)
                    return null;
                return new List<PinterestObject>(ses.PinterestObjects);
            }
            catch (Exception ex)
            {
                string msg = "Error SSM325." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, this.GetType().ToString(), msg + LoggingAppendix);
                return null;
            }
        }

        public void Reset() //after autopilot, make it start from start
        {
            if (Sessions != null)
                foreach (var s in Sessions)
                    s.FirstRequest = true;
        }
        private void ManageSession(ScrapeSession ses, string ForBoard)
        {
            try
            {
                if (ses.PinterestObjects.Count <= 0 && (ses.CountRestarted >= MAX_COUNT_RESTART || ses.FirstRequest))
                {
                    Sessions.Remove(ses); //nothing to scrape, and already started again many times.
                    Logging.Log(account.Username, ses.GetType().ToString(), ForBoard + " :: removed from Sessions");
                }
                else if (ses.PinterestObjects.Count <= 0 && ses.EndReached)
                {
                    ses.FirstRequest = true; //nothing to scrape, start again.
                    ++ses.CountRestarted;
                    Logging.Log(account.Username, ses.GetType().ToString(), " :: nothing to scrape, starts again ");
                }
                else if (ses.PinterestObjects.All(x => x.Resource.Equals(PinterestObjectResources.IndividualUser) || x.Resource.Equals(PinterestObjectResources.IndividualUser)))
                {
                    Sessions.Remove(ses);
                }

                Sessions = new List<ScrapeSession>(Sessions);
            }
            catch (Exception ex)
            {
                string msg = "Error SSM347." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, ses.GetType().ToString(), msg);
            }
        }

        public IList<PinterestObject> ScrapeComments(PinterestObject p)
        {
            ScrapePinComments session = null;
            try
            {
                session = new ScrapePinComments((Pin)p, request, account, Config);
                session.Scrape();

                return session.PinterestObjects;
            }
            catch (Exception ex)
            {
                string msg = "Error 363." + Environment.NewLine + ex.Message + Environment.NewLine + (ex.InnerException != null ? ex.InnerException.Message : "");
                Logging.Log(account.Username, session.GetType().ToString(), msg);
                return null;
            }
        }



    }
}
