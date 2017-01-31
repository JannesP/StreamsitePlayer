using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SeriesPlayer.Streamsites.Sites
{
    class NineAnimeStreamingSite : StreamingSite
    {
        public const string NAME = "nineanimesite";

        public NineAnimeStreamingSite(string link) : base(link)
        {
        }

        public override string GetSiteName()
        {
            return "nineanime";
        }

        public override int GetRemainingWaitTime()
        {
            return 0;
        }

        public override int GetEstimateWaitTime()
        {
            return 1000;
        }

        public override bool IsJwLinkSupported()
        {
            return IsFileDownloadSupported();
        }

        public override bool IsFileDownloadSupported()
        {
            return true;
        }

        public override async Task<string> RequestJwDataAsync(IProgress<int> progress, CancellationToken ct)
        {
            return await RequestFileAsync(progress, ct) + "\",\ntype: \"mp4";
        }

        public override async Task<string> RequestFileAsync(IProgress<int> progress, CancellationToken ct)
        {
            string[] parts = base.link.Split('/');
            string videoId = parts[parts.Length - 1];
            string infoResponse =
                await Util.RequestSimplifiedHtmlSiteAsync($"https://9anime.to/ajax/episode/info?id={videoId}&update=0");
            var requestUrl = "";
            try
            {
                dynamic infoObj = JsonConvert.DeserializeObject(infoResponse);
                dynamic parameters = infoObj.@params;
                requestUrl = "https:" + infoObj.grabber + "?";
                requestUrl += "id=" + parameters.id;
                string token = parameters.token;
                requestUrl += "&token=" + WebUtility.UrlEncode(token);
                string options = parameters.options;
                requestUrl += "&options=" + WebUtility.UrlEncode(options);
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) { return ""; }
            string idResponse = await Util.RequestSimplifiedHtmlSiteAsync(requestUrl);
            try
            {
                dynamic idObj = JsonConvert.DeserializeObject(idResponse);
                var data = idObj.data;
                string file = data.Last.file;
                return file;
            }
            catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) { return ""; }
        }
    }
}
