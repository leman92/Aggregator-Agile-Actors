namespace Aggregator.Models;

public class GithubUser
{
    public string login { get; set; }
    public int id { get; set; }
    public string nodeId { get; set; }
    public string avatarUrl { get; set; }
    public string gravatarId { get; set; }
    public string url { get; set; }
    public string htmlUrl { get; set; }
    public string followersUrl { get; set; }
    public string followingUrl { get; set; }
    public string gistsUrl { get; set; }
    public string starredUrl { get; set; }
    public string subscriptionsUrl { get; set; }
    public string organizationsUrl { get; set; }
    public string reposUrl { get; set; }
    public string eventsUrl { get; set; }
    public string receivedEventsUrl { get; set; }
    public string type { get; set; }
    public string userViewType { get; set; }
    public bool siteAdmin { get; set; }
    public string name { get; set; }
    public string company { get; set; }
    public string blog { get; set; }
    public string location { get; set; }
    public string email { get; set; }
    public bool? hireable { get; set; }
    public string bio { get; set; }
    public string twitterUsername { get; set; }
    public int publicRepos { get; set; }
    public int publicGists { get; set; }
    public int followers { get; set; }
    public int following { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
}