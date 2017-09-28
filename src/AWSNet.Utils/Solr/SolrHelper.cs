using AWSNet.Utils.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AWSNet.Utils.Solr
{
    public static class SolrHelper
    {
        public static async Task<dynamic> ExecuteQuery(SolrCore core, string q)
        {
            var solrCoreUrl = GetCoreUrl(core);

            if (!string.IsNullOrWhiteSpace(solrCoreUrl) && !string.IsNullOrWhiteSpace(q))
            {
                if (q.Contains("wt="))
                {
                    q = q.Replace("wt=json", string.Empty).Replace("wt=xml", string.Empty).Replace("wt=python", string.Empty)
                         .Replace("wt=ruby", string.Empty).Replace("wt=php", string.Empty).Replace("wt=csv", string.Empty);
                }

                string address = string.Format("{0}select?{1}&wt=json", solrCoreUrl, q);

                using (var client = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    try
                    {
                        return await Task.FromResult(JsonConvert.DeserializeObject<dynamic>(client.DownloadString(new Uri(address))));
                    }
                    catch (WebException)
                    { }
                    catch (Exception)
                    { }
                }
            }

            return await Task.FromResult(JsonConvert.DeserializeObject<dynamic>("{}"));
        }

        public static async Task<dynamic> DataImport(SolrCore core, bool isFull = false)
        {
            var solrCoreUrl = GetCoreUrl(core);

            if (!string.IsNullOrWhiteSpace(solrCoreUrl))
            {
                if (isFull)
                {
                    var address = string.Format("{0}dataimport?command=full-import", solrCoreUrl);

                    using (var client = new WebClient())
                    {
                        try
                        {
                            await Task.FromResult(JsonConvert.DeserializeObject<dynamic>(client.DownloadString(new Uri(address))));
                            return true;
                        }
                        catch (WebException)
                        { }
                        catch (Exception)
                        { }
                    }
                }
                else
                {
                    var address = string.Format("{0}dataimport?command=delta-import", solrCoreUrl);

                    using (var client = new WebClient())
                    {
                        try
                        {
                            await Task.FromResult(JsonConvert.DeserializeObject<dynamic>(client.DownloadString(new Uri(address))));
                            return true;
                        }
                        catch (WebException)
                        { }
                        catch (Exception)
                        { }
                    }
                }
            }

            return false;
        }

        public static async Task<dynamic> DeleteDocumentById(SolrCore core, int id)
        {
            var solrCoreUrl = GetCoreUrl(core);

            if (!string.IsNullOrWhiteSpace(solrCoreUrl))
            {
                var address = string.Format("{0}update?stream.body=<delete><query>Id:{1}</query></delete>&commit=true", solrCoreUrl, id);

                using (var client = new WebClient())
                {
                    try
                    {
                        await Task.FromResult(JsonConvert.DeserializeObject<dynamic>(client.DownloadString(new Uri(address))));
                        return true;
                    }
                    catch (WebException)
                    { }
                    catch (Exception)
                    { }
                }
            }

            return false;
        }

        public static async Task<dynamic> DeleteDocumentByQuery(SolrCore core, string q)
        {
            var solrCoreUrl = GetCoreUrl(core);

            if (!string.IsNullOrWhiteSpace(solrCoreUrl))
            {
                var address = string.Format("{0}update?stream.body=<delete><query>{1}</query></delete>&commit=true", solrCoreUrl, q);

                using (var client = new WebClient())
                {
                    try
                    {
                        await Task.FromResult(JsonConvert.DeserializeObject<dynamic>(client.DownloadString(new Uri(address))));
                        return true;
                    }
                    catch (WebException)
                    { }
                    catch (Exception)
                    { }
                }
            }

            return false;
        }

        public static async Task<dynamic> DeleteAllDocuments(SolrCore core)
        {
            var solrCoreUrl = GetCoreUrl(core);

            if (!string.IsNullOrWhiteSpace(solrCoreUrl))
            {
                var address = string.Format("{0}update?stream.body=<delete><query>*:*</query></delete>&commit=true", solrCoreUrl);

                using (var client = new WebClient())
                {
                    try
                    {
                        await Task.FromResult(JsonConvert.DeserializeObject<dynamic>(client.DownloadString(new Uri(address))));
                        return true;
                    }
                    catch (WebException)
                    { }
                    catch (Exception)
                    { }
                }
            }

            return false;
        }

        public static async Task<bool> AddLayoutInstanceByNode(SolrCore core, object obj)
        {
            var solrCoreUrl = GetCoreUrl(core);

            if (!string.IsNullOrWhiteSpace(solrCoreUrl))
            {
                var address = string.Format("{0}update?commit=true", solrCoreUrl);

                using (var client = new HttpClient())
                {
                    try
                    {
                        var json = JsonConvert.SerializeObject(obj, new Newtonsoft.Json.Converters.IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-ddTHH:mm:ssZ" });
                        var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                        var result = await client.PostAsync(address, stringContent);

                        if (result.IsSuccessStatusCode)
                            return true;
                    }
                    catch (WebException)
                    { }
                    catch (Exception)
                    { }
                }
            }

            return false;
        }

        private static string GetCoreUrl(SolrCore core)
        {
            switch (core)
            {

                case SolrCore.CATEGORY: return ConfigurationHelper.GetValue<string>("AWSNet.Solr.Cores.Category.Url");
                case SolrCore.PRODUCT: return ConfigurationHelper.GetValue<string>("AWSNet.Solr.Cores.Product.Url");
                case SolrCore.USER: return ConfigurationHelper.GetValue<string>("AWSNet.Solr.Cores.User.Url");

            }

            return string.Empty;
        }
    }

    public enum SolrCore
    {
        CATEGORY,
        PRODUCT,
        USER
    }
}