using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using FriendlyUrlSample.Module.BusinessObjects;

namespace FriendlyUrlSample.Web {
    public class CustomRouteManager : RouteManager {
        private WebApplication application;
        public CustomRouteManager(WebApplication application) : base(application) {
            this.application = application;
        }
        public override string GetRelativeUrl(ViewShortcut shortcut, IDictionary<string, string> additionalParams = null) {
            if(BrowserHistoryMode != BrowserHistoryMode.FriendlyUrl) {
                return base.GetRelativeUrl(shortcut, additionalParams);
            }
            ViewShortcut localShortcut = new ViewShortcut(shortcut.ViewId, shortcut.ObjectKey);
            if(localShortcut.ViewId == application.FindListViewId(typeof(Contact))) {
                localShortcut.ViewId = "Contacts";
            }
            else if(localShortcut.ViewId == application.FindListViewId(typeof(DemoTask))) {
                localShortcut.ViewId = "Tasks";
            }
            else if(localShortcut.ViewId == application.FindDetailViewId(typeof(Contact))) {
                localShortcut.ViewId = "Contact";
            }
            else if(localShortcut.ViewId == application.FindDetailViewId(typeof(DemoTask))) {
                localShortcut.ViewId = "Task";
            }
            return base.GetRelativeUrl(localShortcut, additionalParams);
        }
        public override ViewShortcut GetViewShortcut(string parameter) {
            if(BrowserHistoryMode != BrowserHistoryMode.FriendlyUrl) {
                return base.GetViewShortcut(parameter);
            }
            ViewShortcut shortcut = base.GetViewShortcut(parameter);
            if(shortcut.ViewId == "Contacts") {
                shortcut.ViewId = application.FindListViewId(typeof(Contact));
            }
            else if(shortcut.ViewId == "Tasks") {
                shortcut.ViewId = application.FindListViewId(typeof(DemoTask));
            }
            else if(shortcut.ViewId == "Contact") {
                shortcut.ViewId = application.FindDetailViewId(typeof(Contact));
            }
            else if(shortcut.ViewId == "Task") {
                shortcut.ViewId = application.FindDetailViewId(typeof(DemoTask));
            }
            return shortcut;
        }
    }
}