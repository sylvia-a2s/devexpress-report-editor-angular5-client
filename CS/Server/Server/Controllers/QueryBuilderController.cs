using System;
using System.Linq;
using System.Web.Mvc;

using DevExpress.Web.Mvc.Controllers;

namespace Server.Controllers {
    public class QueryBuilderController : QueryBuilderApiController {
        public override ActionResult Invoke() {
            var result = base.Invoke();
            // Allow cross-domain requests.
            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return result;
        }
    }
}