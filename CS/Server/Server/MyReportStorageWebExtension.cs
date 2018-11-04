using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;

using Server.CatalogDataSetTableAdapters;

namespace Server
{
    public class MyReportStorageWebExtension : DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
    {
        // public Dictionary<string, XtraReport> Reports = new Dictionary<string, XtraReport>();
        private CatalogDataSet catalogDataSet;
        private DataTable reportsTable;
        private ReportsTableAdapter reportsTableAdapter;

        public MyReportStorageWebExtension()
        {
            catalogDataSet = new CatalogDataSet();
            reportsTableAdapter = new ReportsTableAdapter();
            reportsTableAdapter.Fill(catalogDataSet.Reports);
            reportsTable = catalogDataSet.Tables["Reports"];
        }


        public override bool CanSetData(string url)
        {
            // Check if the URL is available in the report storage.
            //return base.CanSetData(url);
            return reportsTable.Rows.Find(int.Parse(url)) != null;
        }
        public override bool IsValidUrl(string url)
        {
            // Check if the specified URL is valid for the current report storage.
            // In this example, a URL should be a string containing a numeric value that is used as a data row primary key.
            //return base.IsValidUrl(url);
            int n;
            return int.TryParse(url, out n);
        }

        public override byte[] GetData(string url)
        {
            // Get the report data from the storage.
            DataRow row = reportsTable.Rows.Find(int.Parse(url));
            if (row == null) return null;
            byte[] reportData = (Byte[])row["LayoutData"];
            return reportData;
        }

        public override Dictionary<string, string> GetUrls()
        {
            // Get URLs and display names for all reports available in the storage.
            return reportsTable.AsEnumerable()
                .ToDictionary<DataRow, string, string>(dataRow => ((Int32)dataRow["ReportId"]).ToString(),
                                             dataRow => (string)dataRow["DisplayName"]);
        }

        public override void SetData(DevExpress.XtraReports.UI.XtraReport report, string url)
        {
            // Write a report to the storage under the specified URL.
            //base.SetData(report, url);
            DataRow row = reportsTable.Rows.Find(int.Parse(url));

            if (row != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    row["LayoutData"] = ms.GetBuffer();
                }
                reportsTableAdapter.Update(catalogDataSet);
                catalogDataSet.AcceptChanges();
            }
        }

        public override string SetNewData(DevExpress.XtraReports.UI.XtraReport report, string defaultUrl)
        {
            // Save a report to the storage under a new URL. 
            // The defaultUrl parameter contains the report display name specified by a user.
            //return base.SetNewData(report, defaultUrl);
            DataRow row = reportsTable.NewRow();

            row["DisplayName"] = defaultUrl;
            using (MemoryStream ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);
                row["LayoutData"] = ms.GetBuffer();
            }

            reportsTable.Rows.Add(row);
            reportsTableAdapter.Update(catalogDataSet);
            catalogDataSet.AcceptChanges();

            // Refill the dataset to obtain the actual value of the new row's autoincrement key field.
            reportsTableAdapter.Fill(catalogDataSet.Reports);
            return catalogDataSet.Reports.FirstOrDefault(x => x.DisplayName == defaultUrl).ReportId.ToString();
        }
    }
}
