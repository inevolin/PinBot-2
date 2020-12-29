using PinBot2.Model.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Algorithms.Helpers
{
    public static class QueueHelper
    {
        public static string GetRandomSourceUrl(PinConfiguration _config)
        {
            if (_config.SourceUrlRate <= 0)
                return "";

            var c = _config.SourceUrls;
            if (c != null && c.Count > 0)
            {
                Random r = new Random();
                if (r.Next(0, 100) <= _config.SourceUrlRate)
                {
                    var f = c[r.Next(0, c.Count)];
                    return f;
                }
                else
                    return "";
            }
            return "";

        }

        public static string GetRandomDescUrl(IPinRepinConfiguration _config)
        {
            if (_config.DescUrlRate <= 0)
                return "";

            var c = _config.DescUrls;
            if (c != null && c.Count > 0)
            {
                Random r = new Random();
                if (r.Next(0, 100) <= _config.DescUrlRate)
                {
                    var f = c[r.Next(0, c.Count)];
                    return f;
                }
                else
                    return "";
            }
            return "";

        }

    }
}
