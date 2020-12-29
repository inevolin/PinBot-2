using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBot2.Common
{
    public static class ParsePatterns
    {
        /*
        private static string unicode = "([\\x00-\\x7F]|\\p{L}|.)*?";
        public static string[] Boards = { 
                     "\"url\":\"/(?<url>.+?)/\", "
                    + "\"pin_count\": (?<pins>\\d+), "
                    + "\"pin_thumbnail_urls\": \\[\"?[.a-zA-Z0-9\\-/_:\\.,\\\" ]*\"?\\], "
                    + "\"image_thumbnail_url\":\"[.a-zA-Z0-9\\-/_:\\.]+?\", "
                    + "\"access\": \\[(\"write\", \"delete\")*\\], "
                    + "\"collaborated_by_me\": (?<collaborated_by_me>true|false), "
                    + "\"owner\": \\{\"username\":\"(?<username>.+?)\", "
                    + "\"verified_identity\": \\{(\"verified\": (true|false))?\\}, "
                    + "\"image_medium_url\":\"[a-zA-Z0-9\\-/_:\\.]+?\", "
                    + "\"explicitly_followed_by_me\": (true|false), "
                    + "\"full_name\":\"(.+?)\", "
                    + "\"domain_verified\": (true|false), "
                    + "\"type\":\"user\", "
                    + "\"id\":\"(?<userId>\\d+)\"\\}, "
                    + "\"followed_by_me\": (?<followed_by_me>true|false), "
                    + "\"type\":\"board\", "
                    + "\"id\":\"(?<boardId>\\d+)\", "
                    + "\"name\":\"(?<boardname>.+?)\"\\}"                
                   };

        public static string[] Pinners = { 
                "\\{\"username\":\"(?<username>[a-zA-Z0-9_-]+)\", "
                +"\"domain_verified\": (true|false), "
                +"\"is_default_image\": (true|false), "
                +"\"pin_count\": (?<pins>\\d+), "
                +"\"pin_thumbnail_urls\": \\[" + unicode + "\\], "
                +"\"image_medium_url\":\"" + unicode + "\", "
                +"\"explicitly_followed_by_me\": (?<followed_by_me>true|false), "
                +"\"full_name\":\"" + unicode + "\", "//[a-z0-9A-Z\\._/:\\-\", \\(\\)\\+]+
                +"\"follower_count\": (?<followers>\\d+), "
                + "\"verified_identity\": \\{(\"verified\": (true|false))?\\}, "
                +"\"type\":\"user\", "
                +"\"id\":\"(?<id>\\d+)\", "
                +"\"image_large_url\":\"" + unicode + "\"\\}"   
            ,
                "\\{\"username\":\"(?<username>[a-zA-Z0-9_-]+)\", "
                +"\"domain_verified\": (true|false), "
                +"\"blocked_by_me\": false, "
                +"\"is_default_image\": (true|false), "
                +"\"type\":\"user\", "
                +"\"pin_thumbnail_urls\": \\[" + unicode+ "\\], "
                +"\"image_medium_url\":\"" + unicode + "\", "
                +"\"explicitly_followed_by_me\": (?<followed_by_me>true|false), "
                +"\"full_name\":\"" + unicode + "\", "//[a-z0-9A-Z\\._/:\\-\", \\(\\)\\+]+
                +"\"follower_count\": (?<followers>\\d+), "
                + "\"verified_identity\": \\{(\"verified\": (true|false))?\\}, "
                +"\"pin_count\": (?<pins>\\d+), "
                +"\"id\":\"(?<id>\\d+)\", "
                +"\"image_large_url\":\"" + unicode + "\"\\}"

            ,
                "\\{\"username\":\"(?<username>[a-zA-Z0-9_-]+)\", "
                +"\"repins_from\": \\[\\], "
                +"\"image_medium_url\":\"" + unicode + "\", "
                +"\"full_name\":\"" + unicode + "\", "//[a-z0-9A-Z\\._/:\\-\", \\(\\)\\+]+
                +"\"image_small_url\":\"" + unicode + "\", "
                +"\"id\":\"(?<id>\\d+)\"\\},"

            };

        public static string[] Pins = {

                "\"orig\": \\{\"url\":\"(?<image>.+?)\","
                + "(.+?)\\}\\}, "
                + "\"id\":\"(?<id>.+?)\", "
                + "\"price_currency\":\"(.+?)\", "
                + "\"description_html\":\"(.+?)\", "
                + "\"privacy\":\"(.+?)\", "
                + "\"buyable_product\": (.+?), "
                + "\"comments\": \\{\"bookmark\": (.+?), "
                + "\"data\": \\[(.*?)\\], "
                + "\"uri\":\"(.+?)\"\\}, "
                + "\"access\": \\[(.*?)\\], "
                + "\"comment_count\": \\d+?, "
                + "(.+?)\"description\":\"(?<description>.+?)\", "
                + "\"price_value\": (.+?), "
                + "\"is_playable\": (true|false), "
                + "\"link\":\"(?<link>.+?)\", "
                + "(\"canonical_likes\": null|\"view_tags\": \\[.*?\\]), "
                + "\"is_repin\": (true|false), "
                + "\"liked_by_me\": (?<liked_by_me>true|false), "

            ,

            "\"orig\": \\{\"url\":\"(?<image>.+?)\","
                + "(.+?)\\}\\}, "
                + "\"id\":\"(?<id>.+?)\", "
                + "\"price_currency\":\"(.+?)\", "
                + "\"description_html\":\"(.+?)\", "
                + "\"privacy\":\"(.+?)\", "
                + "\"comments\": \\{\"bookmark\": (.+?), "
                + "\"data\": \\[(.*?)\\], "
                + "\"uri\":\"(.+?)\"\\}, "
                + "\"access\": \\[(.*?)\\], "
                + "\"comment_count\": \\d+?, "
                + "(.+?)\"description\":\"(?<description>.+?)\", "
                + "\"price_value\": (.+?), "
                + "\"is_playable\": (true|false), "
                + "\"link\":\"(?<link>.+?)\", "
                + "(\"canonical_likes\": null|\"view_tags\": \\[.*?\\]), "
                + "\"is_repin\": (true|false), "
                + "\"liked_by_me\": (?<liked_by_me>true|false), "
            };

        
        */



        public static string[] IndividualPins = {
              "\"rich_metadata\":"
              + "\\{\"site_name\":\".+?\","
              + "\"description\":\"(?<desc>.+?)\","
              + "\"title\":\".+?\","
              + "\"locale\":\".+?\","
              + "\"type\":\".+?\","
              + "\"amp_url\":(null|\".+?\"),"
              + "\"url\":(?<source>(null|\".+?\")),"
            };

        public static string[] IndividualUsers = {
                "\"id\":\"(?<id>\\d+)\","
                + "\"first_name\":\".+?\","
                + "\"explicitly_followed_by_me\":(?<followed_by_me>true|false)"
            };


        public static string[] Comments = { 
                "\\{\"text\":\"(?<text>.+?)\","
                + "\"created_at\":\"(.+?)\","
                + "\"commenter\":\\{"
                + "\"username\":\"(?<username>.+?)\","
                + "\"type\":\"user\","
                + "\"id\":\"(?<id>\\d+?)\","
                + "\"full_name\":\"(.+?)\","
                + "\"image_medium_url\":\"(.+?)\"\\},"
                + "\"deletable_by_me\":(true|false),"
                + "\"type\":\"comment\","
                + "\"id\":\"(\\d+?)\"\\}"
            };

        public static string[] ImgFave = {
                "<img class=\"lazy\" data-href=\"(?<img>.+?)\" src=\"(.+?)\" width=\"\\d+\" height=\"\\d+\" alt=\"(?<title>.+?)\"/></a>"
                + "("
                + "<a class=\"(.+?)\" href=\"(.+?)\" style=\"(.+?)\">(.+?)</a>"
                + "|"
                + "<div class=\"image_tags_container\" id=\"(.+?)\">(?<tags><span class=\"image_tags\">)(<a class=\"(.+?)\" href=\"(.+?)\">(?<tag>.+?)</a>)+</span><div class=\"clearfix\"></div></div>"
                + ")?"
                + "</div>"
            };

        public static string[] Tumblr = {
                "\"high_res\":\"(?<img>.+?)\",\"height\":.+?,\"width\":.+?\\}'>(.|\\n|\\r|\\t)+?<p>(?<desc>.+?)</p>"
            };

    }
}
