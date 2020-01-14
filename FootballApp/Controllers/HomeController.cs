using FootballApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FootballApp.Controllers
{
    public class HomeController : Controller
    {

        List<Teams> teamObject;
        string overallPoints;
        string overallLosses;
        string overallWins;

        public ActionResult Index()
        {
            return View();
        }

        public void DownloadString()
        {
            WebClient client = new WebClient();
            string footballJson = client.DownloadString("https://apiv2.apifootball.com/?action=get_standings&league_id=148&APIkey=0dd8eaf58d95895a548af94223aa8cd343a883d28a2a6ae180d57cd319190dc3");
            teamObject = JsonConvert.DeserializeObject<List<Teams>>(footballJson);
        }
        public void sendTeamValues(string teamName)
        {
            DownloadString();
            int counter = 0;

            while (counter < teamObject.Count)
            {
                if (teamObject[counter].team_name == teamName)
                {
                    overallPoints = teamObject[counter].overall_league_PTS;
                    overallLosses = teamObject[counter].overall_league_L;
                    overallWins = teamObject[counter].overall_league_W;
                    break;
                }
                else
                {
                    counter++;
                }
            }
        }
        public ActionResult TeamStats()
        {
            var model = new Teams();
            model.team_name = Request.Form["TeamName"];
            sendTeamValues(model.team_name);
            model.overall_league_PTS = overallPoints;
            model.overall_league_W = overallWins;
            model.overall_league_L = overallLosses;
            return View(model);
        }
    }
}