using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.Base;
using FriendlyUrlSample.Module.BusinessObjects;

namespace FriendlyUrlSample.Module.Web.Controllers {
    public class CustomLinkController : ViewController {
        private SimpleAction goToJohnNilsenAction;

        protected override void OnActivated() {
            base.OnActivated();
            Contact johnNilsen = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("LastName == 'Nilsen'"));
            if(johnNilsen != null) {
                goToJohnNilsenAction.Active.RemoveItem("JohnNilsenIsNotExist");
                ViewShortcut viewShortcut = new ViewShortcut(Application.GetDetailViewId(typeof(Contact)), ObjectSpace.GetKeyValueAsString(johnNilsen));
                string url = ((WebApplication)Application).ViewUrlManager.GetUrl(viewShortcut);
                goToJohnNilsenAction.SetClientScript($"window.open('{url}', '_blank')", false);
            }
            else {
                goToJohnNilsenAction.Active["JohnNilsenIsNotExist"] = false;
            }
        }
        public CustomLinkController() {
            goToJohnNilsenAction = new SimpleAction(this, "GoToJohnNilsen", PredefinedCategory.View);
        }
    }
}
