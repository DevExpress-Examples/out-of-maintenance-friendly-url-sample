using System.Collections.Generic;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using FriendlyUrlSample.Module.Web;

namespace FriendlyUrlSample.Web {
    public class CustomViewUrlManager : IViewUrlManager {
        private readonly IViewUrlManager innerUrlManager;
        private WebApplication application;
        public CustomViewUrlManager() {
            innerUrlManager = new ViewUrlManager();
            application = WebApplication.Instance;
        }
        public string GetUrl(ViewShortcut shortcut, IDictionary<string, string> additionalParams = null) {
            ViewShortcut localShortcut = new ViewShortcut();
            for(int i = 0; i < shortcut.Count; i++) {
                localShortcut[shortcut.GetKey(i)] = shortcut[i];
            }
            IModelView modelView = application.FindModelView(localShortcut.ViewId);
            if(modelView != null) {
                localShortcut.ViewId = ((IModelViewWebExtender)modelView).ViewUrlAlias;
            }
            return innerUrlManager.GetUrl(localShortcut, additionalParams);
        }
        public ViewShortcut GetViewShortcut() {
            ViewShortcut shortcut = innerUrlManager.GetViewShortcut();
            IModelView modelView = application.Model.Views.SingleOrDefault(m => ((IModelViewWebExtender)m).ViewUrlAlias == shortcut.ViewId);
            if(modelView != null) {
                shortcut.ViewId = modelView.Id;
            }
            return shortcut;
        }
    }
}