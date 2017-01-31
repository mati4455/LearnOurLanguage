using Microsoft.AspNetCore.Mvc;
using Model.Const;

namespace LearnOurLanguage.Web.Base
{
    public static class JsonHelper
    {
        public static ActionResult Success(object data)
        {
            return Response(true, data);
        }

        public static ActionResult Error(object data)
        {
            return Response(false, data);
        }

        public static ActionResult Response(bool success, object data)
        {
            return new JsonResult(new {success, data});
        }

        public static ActionResult Response(bool success, object dataSuccess, object dataError)
        {
            return success ? Success(dataSuccess) : Error(dataError);
        }

        public static ActionResult Response(bool success)
        {
            return Response(success, MessagesConst.DataSaved, MessagesConst.SaveError);
        }
    }
}