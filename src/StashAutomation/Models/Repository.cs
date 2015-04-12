using Newtonsoft.Json;

namespace StashAutomation.Models
{
    public class Repository
    {
        public string Slug { get; set; }
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("scmId")]
        public string ScmId { get; set; }
        public string State { get; set; }
        public string StatusMessage { get; set; }
        [JsonProperty("forkable")]
        public bool Forkable { get; set; }
        [JsonProperty("public")]
        public bool @Public { get; set; }
        public string CloneUrl { get; set; }
        public Link Link { get; set; }
        public Links Links { get; set; }
        public Project Project { get; set; }
    }
}