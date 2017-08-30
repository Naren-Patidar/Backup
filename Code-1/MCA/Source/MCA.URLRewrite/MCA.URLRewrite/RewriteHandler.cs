using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using Newtonsoft.Json;

namespace Tesco.ClubcardProducts.MCA.UrlRewrite
{
    public class RewriteHandler : IHttpModule
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                HttpContext context = ((HttpApplication)sender).Context;
                string rulesFile = HttpContext.Current.Server.MapPath("~/rules.json");

                var rules = this.ParseRules(rulesFile);
                string redirectTo = this.ProcessRules(context.Request.RawUrl, rules);
                if (!String.IsNullOrWhiteSpace(redirectTo))
                {
                    HttpContext.Current.Response.Redirect(redirectTo, true);
                }
            }
            catch (Exception)
            {
                //Need to add logging statements for error handling
            }
        }

        private string ProcessRules(string sourceUrl, List<Rule> list)
        {
            foreach (Rule r in list)
            {
                if (this.FindRegExMatch(sourceUrl, r.source))
                {
                    if (r.exceptions != null && r.exceptions.Count > 0)
                    {
                        foreach (string exp in r.exceptions)
                        {
                            if (this.FindRegExMatch(sourceUrl, exp))
                            {
                                return String.Empty;
                            }
                        }
                    }

                    return r.target;
                }
            }
            return string.Empty;
        }

        private bool FindRegExMatch(string input, string expression)
        {
            Regex regex = new Regex(expression);
            Match match = regex.Match(input);
            return match.Success;
        }

        private List<Rule> ParseRules(string filePath)
        {
            string fileContent = File.ReadAllText(filePath);
            List<Rule> mcaRules = JsonConvert.DeserializeObject<List<Rule>>(fileContent);
            return mcaRules;
        }
    }

    public class Rule
    {
        public string source { get; set; }
        public string target { get; set; }
        public List<string> exceptions { get; set; }
    }
}
