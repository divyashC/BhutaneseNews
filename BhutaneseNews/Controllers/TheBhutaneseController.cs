using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace BhutaneseNews.Controllers;

public class TheBhutaneseController : ControllerBase
{
    // GET
    [Route("[controller]")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var uri = "https://thebhutanese.bt/";
        var client = new HttpClient();
        var response = await client.GetAsync(uri);
        var pageContents = await response.Content.ReadAsStringAsync();
        
        var title = new List<string>();
        var links = new List<string>();
        var data = new List<Dictionary<string, string>>();
        var dateList = new List<string>();

        
        var h2Regex = new Regex(@"<h2 class=""post-box-title""><a href=""(?<link>.*?)"" rel=""bookmark"">(?<title>.*?)</a></h2>");
        var h3Regex = new Regex(@"<h3 class=""post-box-title""><a href=""(?<link>.*?)"" rel=""bookmark"">(?<title>.*?)</a></h3>");
        var divRegex = new Regex(@"<div class=""featured-cover""><a href=""(?<link>.*?)""><span>(?<title>.*?)</span></a></div>");
        
        var dateRegex = new Regex(@"<span class=""tie-date""><i class=""fa fa-clock-o""></i>(?<date>.*?)</span>");
        
        foreach (Match match in dateRegex.Matches(pageContents))
        {
            var date = match.Groups["date"].Value;
            dateList.Add(date);
        }
        
        foreach (Match match in divRegex.Matches(pageContents))
        {
            title.Add(match.Groups["title"].Value);
            links.Add(match.Groups["link"].Value);
        }
        
        foreach (Match match in h2Regex.Matches(pageContents))
        {
            title.Add(match.Groups["title"].Value);
            links.Add(match.Groups["link"].Value);
        }
        
        foreach (Match match in h3Regex.Matches(pageContents))
        {
            title.Add(match.Groups["title"].Value);
            links.Add(match.Groups["link"].Value);
        }

        for (var i = 0; i < title.Count; i++)
        {
            data.Add(new Dictionary<string, string>
            {
                { "index", (i + 1).ToString() },
                { "date", dateList[i] },
                { "title", title[i] },
                { "link", links[i] }
            });
        }
        
        return Ok(data);
        
    }
}