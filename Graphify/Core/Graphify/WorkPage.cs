using System;
using System.Text.Json.Serialization;

namespace Core.Graphify
{
    public class WorkPage
    {
        public int OwnerID { get; set; }
        public string Identifier { get; set; }
        public string PageName { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Edge> Edges { get; set; }
        [JsonIgnore]
        public string Path { get; private set; }

        public WorkPage(int ownerID, string identifier, string pageName)
        {
            this.OwnerID = ownerID;
            this.Identifier = identifier;
            this.PageName = pageName;
            this.Nodes = new List<Node>();
            this.Edges = new List<Edge>();
            this.Path = System.IO.Path.Combine("Server", "Workpages", identifier + ".json");
        }
    }
}
