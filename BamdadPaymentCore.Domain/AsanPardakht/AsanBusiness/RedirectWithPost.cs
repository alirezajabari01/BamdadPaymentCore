using System.Text;

namespace BamdadPaymentCore.Domain.AsanPardakht.AsanBusiness
{
    public class RedirectWithPost
    {

        public static string PreparePostForm(string url, Dictionary<string, string> data)
        {
            var formBuilder = new StringBuilder();
            formBuilder.AppendLine("<html>");
            formBuilder.AppendLine("<body onload='document.forms[\"redirectForm\"].submit()'>");
            formBuilder.AppendLine($"<form name='redirectForm' method='post' action='{url}'>");

            foreach (var key in data.Keys)
            {
                formBuilder.AppendLine($"<input type='hidden' name='{key}' value='{data[key]}' />");
            }

            formBuilder.AppendLine("</form>");
            formBuilder.AppendLine("</body>");
            formBuilder.AppendLine("</html>");

            return formBuilder.ToString();
        }
    }
}
