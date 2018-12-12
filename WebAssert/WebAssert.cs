namespace WebAssert
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Web;

    public static class WebAssert
    {
        [Conditional("DEBUG")]
        public static void Assert(
            Func<bool> assertCondition,
            string failedAssertMessage = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0,
            [CallerMemberName] string callerMember = "")
        {
            if (assertCondition())
            {
                return;
            }

            //
            // As the assertion has failed, let's check if we are attached to a debugger. If we are, then break into it.
            // That way we give the developer a chance to view the current state of the app i.e. by navigating the call stack.
            //
            // If we are not attached to a debugger, then lets output the failed assertion details to the current http context,
            // via a HTML file template.
            //
            if (Debugger.IsAttached)
            {
                //
                // Give the developer a head start by outputting the failed assert message to the 'Immediate Window'.
                //
                Debug.WriteLine(string.Empty);
                Debug.WriteLine(string.Empty);
                Debug.WriteLine($"******************** Assert Failed ({DateTime.Now:dd/MM/yyyy HH:mm:ss tt}) ********************");
                Debug.WriteLine($"Message: {failedAssertMessage}");

                Debugger.Break();
            }
            else
            {
                RenderFailedAssertDetails(
                    failedAssertMessage,
                    callerFilePath,
                    callerLineNumber,
                    callerMember);
            }
        }

        private static void RenderFailedAssertDetails(
            string failedAssertMessage,
            string callerFilePath,
            int callerLineNumber,
            string callerMember)
        {
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.ContentType = "text/html; charset=utf-8";
            HttpContext.Current.Response.Write(
                LoadFailedAssertViewFileHtml()
                    .Replace("{{AssertMessage}}", failedAssertMessage)
                    .Replace("{{MethodName}}", callerMember)
                    .Replace("{{FilePath}}", callerFilePath)
                    .Replace("{{LineNumber}}", callerLineNumber.ToString())
            );

            HttpContext.Current.Response.End();
        }

        private static string LoadFailedAssertViewFileHtml()
        {
            try
            {
                string templatePath = $@"{HttpContext.Current.Request.PhysicalApplicationPath}\ViewTemplates";

                return File.ReadAllText(templatePath + "\\AssertFailed.cshtml");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(string.Empty);
                Debug.WriteLine(string.Empty);
                Debug.WriteLine($"******************** WebAssert.LoadFailedAssertViewFileHtml Error ({DateTime.Now:dd/MM/yyyy HH:mm:ss tt}) ********************");
                Debug.WriteLine(ex.ToString());

                return string.Empty;
            }
        }
    }
}