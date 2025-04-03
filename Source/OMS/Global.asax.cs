using System;
using System.Reflection ;

namespace OMS
{
    public class Global : System.Web.HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            PropertyInfo p = typeof(System.Web.HttpRuntime).GetProperty("FileChangesMonitor", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            object o = p.GetValue(null, null);
            FieldInfo f = o.GetType().GetField("_dirMonSubdirs", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            object monitor = f.GetValue(o);
            MethodInfo m = monitor.GetType().GetMethod("StopMonitoring", BindingFlags.Instance | BindingFlags.NonPublic);
            m.Invoke(monitor, new object[] { });
        }

        void Application_End(object sender, EventArgs e)
        {
            //  アプリケーションのシャットダウンで実行するコードです

        }

        void Application_Error(object sender, EventArgs e)
        {
            // ハンドルされていないエラーが発生したときに実行するコードです
            //Write log
            Exception ex = Server.GetLastError();
            Utilities.Log.Instance.WriteLog(ex);
            if (ex != null)
            {
                System.Web.HttpContext.Current.Cache["LST_EX"] = ex;
            }
            var httpEx = ex as System.Web.HttpException;
            
            //URL not found
            if (httpEx != null && httpEx.GetHttpCode() == 404)
            {
                Response.Redirect("~/Menu/FrmMainMenu.aspx");
            }

        }

        void Session_Start(object sender, EventArgs e)
        {
            // 新規セッションを開始したときに実行するコードです

        }

        void Session_End(object sender, EventArgs e)
        {
            // セッションが終了したときに実行するコードです 
            // メモ: Web.config ファイル内で sessionstate モードが InProc に設定されているときのみ、
            // Session_End イベントが発生します。 session モードが StateServer か、または 
            // SQLServer に設定されている場合、イベントは発生しません。

        }

    }
}
