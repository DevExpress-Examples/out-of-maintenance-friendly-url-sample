# Friendly URL Sample

The **DevExpress.ExpressApp.Web.BrowserHistoryMode** enumeration contains values that specify the current routing mode.
```csharp
public enum BrowserHistoryMode { Hash, QueryString, FriendlyUrl }
```
The URL representation depends on the static **WebApplication.RouteManager.BrowserHistoryMode** property value and looks like this:

**Hash** (Default Mode):
* /Default.aspx#ViewID=Contact_ListView
* /Default.aspx#ViewID=Contact_DetailView&ObjectKey=ContactId

**QueryString**:
* /Default.aspx?ViewID=Contact_ListView
* /Default.aspx?ViewID=Contact_DetailView&ObjectKey=ContactId

**FriendlyUrl** (Enabled in this demo):
* /Contact_ListView/
* /Contact_DetailView/ContactId/

*Files to look at*:
### [Global.asax.cs](./FriendlyUrlSample.Web/Global.asax.cs)

The FriendlyUrl mode is enabled in the Application_Start method. 
```csharp
protected void Application_Start(Object sender, EventArgs e) {
  RouteManager.BrowserHistoryMode = BrowserHistoryMode.FriendlyUrl;
  //
}
```
You can also change the routing mode and check another modes. Note, the **RouteManager.RegisterRoutes(RouteTable.Routes)** method should be also called when the FriendlyUrl mode is enabled. It requires to register the default route "{ViewID}/{ObjectKey}/" internally. Registration of the default route looks like this:
```csharp
RouteTable.Routes.Add("ViewRouteName", "{ViewID}/{ObjectKey}/", "~/Default.aspx", false, new RouteValueDictionary() { { ViewShortcut.ObjectKeyParamName, string.Empty } });
```
If you uncomment the following lines, the default route will be replaced to '/XAF/Contact_DetailView/ContactId'.
```csharp
//RouteTable.Routes.Remove(RouteTable.Routes["ViewRouteName"]);
//RouteTable.Routes.MapPageRoute("ViewRouteName", "XAF/{ViewID}/{ObjectKey}/", "~/Default.aspx", false, new RouteValueDictionary() { { ViewShortcut.ObjectKeyParamName, string.Empty } });
```


### [CustomRouteManager.cs](./FriendlyUrlSample.Web/CustomRouteManager.cs) and [WebApplication.cs]
Check these files to see how to customize the default routing. In this case, the URLs will look like this:

For ListView:  
*  /Contacts/ instead of /Contact_ListView/
*  /Tasks/  instead of /DemoTask_ListView/
               
For DetailView:
*  /Contact/ContactId/ instead of /Contact_DetailView/ContactId/
*  /Task/TaskId/ instead of /DemoTask_DetailView/TaskId/
 
### [CustomLinkController.cs](./FriendlyUrlSample.Module.Web/Controllers/CustomLinkController.cs)
This controller demostrates how to open a DetailView in the new window using the **WebApplication.RouteManager.GetRelativeUrl(viewShortcut)** method.
