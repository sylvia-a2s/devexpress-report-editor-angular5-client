using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;

namespace Server
{
    public class MyReportStorage : ReportStorageWebExtension
    {
        public Dictionary<string, XtraReport> Reports = new Dictionary<string, XtraReport>();

        public MyReportStorage()
        {
            this.Reports.Add("Products", new XtraReport1());
        }

        public override bool CanSetData(string url)
        {
            return true;
        }

        public override byte[] GetData(string url)
        {
            var report = this.Reports[url];
            using (MemoryStream stream = new MemoryStream())
            {
                report.SaveLayoutToXml(stream);
                return stream.ToArray();
            }
        }

        public override Dictionary<string, string> GetUrls()
        {
            return this.Reports.ToDictionary(x => x.Key, y => y.Key);
        }

        public override void SetData(XtraReport report, string url)
        {
            if (this.Reports.ContainsKey(url))
            {
                this.Reports[url] = report;
            }
            else
            {
                this.Reports.Add(url, report);
            }
        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            SetData(report, defaultUrl);
            return defaultUrl;
        }

        public override bool IsValidUrl(string url)
        {
            return true;
        }
    }
}