using System;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Updating;

namespace FriendlyUrlSample.Module.Web {

    public interface IModelViewWebExtender {
        string ViewUrlAlias { get; set; }
    }

    [DomainLogic(typeof(IModelViewWebExtender))]
    public static class ModelViewWebExtenderLogic {
        public static string Get_ViewUrlAlias(IModelViewWebExtender model) {
            return ((IModelView)model).Id;
        }
    }

    [ToolboxItemFilter("Xaf.Platform.Web")]
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class FriendlyUrlSampleAspNetModule : ModuleBase {
        public FriendlyUrlSampleAspNetModule() {
            InitializeComponent();
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            return ModuleUpdater.EmptyModuleUpdaters;
        }
        public override void ExtendModelInterfaces(ModelInterfaceExtenders extenders) {
            base.ExtendModelInterfaces(extenders);
            extenders.Add<IModelView, IModelViewWebExtender>();
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }
    }
}
