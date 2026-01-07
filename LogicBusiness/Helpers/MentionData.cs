using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LogicBusiness.Helpers
{
    public class MentionData
    {
        public string Type { get; set; }
        public string Id { get; set; }
    }

    public static class MentionParser
    {
        // Regex bắt pattern: @[Display Name](Type:ID)
        private static readonly Regex _regex = new Regex(@"@\[(.*?)\]\((User|Task):([a-zA-Z0-9-]+)\)");

        public static List<MentionData> GetMentions(string content)
        {
            var list = new List<MentionData>();
            if (string.IsNullOrEmpty(content)) return list;

            var matches = _regex.Matches(content);
            foreach (Match match in matches)
            {
                list.Add(new MentionData
                {
                    Type = match.Groups[2].Value, // Group 2 là (User|Task)
                    Id = match.Groups[3].Value    // Group 3 là ID
                });
            }
            return list;
        }
    }
}
