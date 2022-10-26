using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace BhutaneseNews.Controllers;

public class KuenselController : ControllerBase
{
    // GET
    [Route("[controller]")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var uri = "https://kuenselonline.com/";
        var client = new HttpClient();
        var response = await client.GetAsync(uri);
        var pageContents = await response.Content.ReadAsStringAsync();
        
        var title = new List<string>();
        var links = new List<string>();
        var data = new List<Dictionary<string, string>>();
        var dateList = new List<string>();
        
        var h3Regex = new Regex(@"<h3 class=""post-title""><a href=""(?<link>.*?)"">(?<title>.*?)</a></h3>");
        var h5Regex = new Regex(@"<h5 class=""mt-0 post-title""><a href=""(?<link>.*?)"">(?<title>.*?)</a></h5>");
        var dateRegex = new Regex(@"<p class=""post-date"">(?<date>.*?)</p>");
        
        
        foreach (Match match in dateRegex.Matches(pageContents))
        {
            var date = match.Groups["date"].Value;
            dateList.Add(date);
        }

        foreach (Match match in h3Regex.Matches(pageContents))
        {
            title.Add(match.Groups["title"].Value);
            links.Add(match.Groups["link"].Value);
        }
        
        foreach (Match match in h5Regex.Matches(pageContents))
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