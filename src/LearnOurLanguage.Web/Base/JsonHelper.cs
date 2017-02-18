using Microsoft.AspNetCore.Mvc;
using Model.Const;

namespace LearnOurLanguage.Web.Base
{
    /// <summary>
    /// Helper do obsugi JSON
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Odpowiedź JSON z sukcesem
        /// </summary>
        /// <param name="data">Obiekt danych</param>
        /// <returns></returns>
        public static ActionResult Success(object data)
        {
            return Response(true, data);
        }

        /// <summary>
        /// Odpowiedź JSON z blędem
        /// </summary>
        /// <param name="data">Opis blędu</param>
        /// <returns></returns>
        public static ActionResult Error(object data)
        {
            return Response(false, data);
        }

        /// <summary>
        /// Odpowiedź JSON z parameterm sukcesu
        /// </summary>
        /// <param name="success">Czy operacja się powiodla</param>
        /// <param name="data">Dane do zwrócenia</param>
        /// <returns></returns>
        public static ActionResult Response(bool success, object data)
        {
            return new JsonResult(new { success, data });
        }

        /// <summary>
        /// Odpowiedź JSON z parametrem sukcesu oraz danymi sukcesu i bledu
        /// </summary>
        /// <param name="success">Czy operacja się powiodla</param>
        /// <param name="dataSuccess">Dane dla sukcesu</param>
        /// <param name="dataError">Dane dla bledu</param>
        /// <returns></returns>
        public static ActionResult Response(bool success, object dataSuccess, object dataError)
        {
            return success ? Success(dataSuccess) : Error(dataError);
        }

        /// <summary>
        /// Odpowiedź domyslna JSON
        /// </summary>
        /// <param name="success">Czy operacja się powiodla</param>
        /// <returns></returns>
        public static ActionResult Response(bool success)
        {
            return Response(success, MessagesConst.DataSaved, MessagesConst.SaveError);
        }
    }
}