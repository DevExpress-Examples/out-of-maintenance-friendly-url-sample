using System;
using System.Configuration;
using System.Web.Configuration;
using System.Web;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Web;
using System.Web.Routing;

namespace FriendlyUrlSample.Web {
    public class Global : System.Web.HttpApplication {
        public Global() {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e) {
            RouteManager.BrowserHistoryMode = BrowserHistoryMode.FriendlyUrl;

            switch(RouteManager.BrowserHistoryMode) {
                case BrowserHistoryMode.Hash:
                    // --- The Hash mode is enabled by default 
                    // --- ListView: (/Default.aspx#ViewID=Contact_ListView)
                    // --- DetailView: (/Default.aspx#ViewID=Contact_DetailView&ObjectKey=ContactId)
                    break;
                case BrowserHistoryMode.QueryString:
                    // --- ListView: (/Default.aspx?ViewID=Contact_ListView)
                    // --- DetailView: (/Default.aspx?ViewID=Contact_DetailView&ObjectKey=ContactId)
                    break;
                case BrowserHistoryMode.FriendlyUrl:
                    RouteManager.RegisterRoutes(RouteTable.Routes);
                    // --- You can modify the route params as shown below. In this case, the '/YourCustomString/Contact_DetailView/ContactId' route shows the corresponding view.
                    //RouteTable.Routes.Remove(RouteTable.Routes[RouteManager.ViewRouteName]);
                    //RouteTable.Routes.MapPageRoute(RouteManager.ViewRouteName, "YourCustomString/{ViewID}/{ObjectKey}/", "~/Default.aspx", false, new RouteValueDictionary() { { ViewShortcut.ObjectKeyParamName, string.Empty } });
                    break;
            }

            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif
        }
        protected void Session_Start(Object sender, EventArgs e) {
            Tracing.Initialize();
            WebApplication.SetInstance(Session, new FriendlyUrlSampleAspNetApplication());
            DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit();
            WebApplication.Instance.SwitchToNewStyle();
            if(ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }
        protected void Application_BeginRequest(Object sender, EventArgs e) {
        }
        protected void Application_EndRequest(Object sender, EventArgs e) {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
        }
        protected void Application_Error(Object sender, EventArgs e) {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e) {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e) {
        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
        }
        #endregion
    }
}
