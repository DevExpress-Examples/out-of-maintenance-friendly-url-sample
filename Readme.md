# Friendly URL Sample

## How it works

The **DevExpress.ExpressApp.Web.BrowserHistoryMode** enumeration contains values that specify the current routing mode.

#### CS
```csharp
public enum BrowserHistoryMode { Hash, QueryString, FriendlyUrl }
```
#### VB
```vb
Public Enum BrowserHistoryMode
    Hash
    QueryString
    FriendlyUrl
End Enum
```

The URL representation depends on the static **WebApplication.RouteManager.BrowserHistoryMode** property value and looks like this:

**Hash** (Default Mode):
* /Default.aspx#ViewID=Contact_ListView
* /Default.aspx#ViewID=Contact_DetailView&ObjectKey=ContactId

**QueryString**:
* /Default.aspx?ViewID=Contact_ListView
* /Default.aspx?ViewID=Contact_DetailView&ObjectKey=ContactId

**FriendlyUrl** (Enabled in this sample):
* /Contact_ListView/
* /Contact_DetailView/ContactId/

**WebApplication.RouteManager** contains following methods:

#### CS
```csharp
public virtual ViewShortcut GetViewShortcut(string parameter);
public virtual string GetRelativeUrl(ViewShortcut shortcut, IDictionary<string, string> additionalParams = null);
```
#### VB
```vb
Public Overridable Function GetViewShortcut(ByVal parameter As String) As ViewShortcut
    Public Overridable Function GetRelativeUrl(ByVal shortcut As ViewShortcut, ByVal Optional additionalParams As IDictionary(Of String, String) = Nothing) As String
```
The GetViewShortcut method returns [ViewShortcut](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewShortcut) by current URL. The parameter is required for backward compatibility with the Hash mode.

The GetRelativeUrl method returns relative URL by [ViewShortcut](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewShortcut) and the dictionary with additional parameters.


## The Customization

*Files to look at*:

### [Global.asax.cs](./CS/FriendlyUrlSample.Web/Global.asax.cs) / [Global.asax.vb](./VB/FriendlyUrlSample.Web/Global.asax.vb)

The FriendlyUrl mode is enabled in the Application_Start method. 

#### CS
```csharp
protected void Application_Start(Object sender, EventArgs e) {
  RouteManager.BrowserHistoryMode = BrowserHistoryMode.FriendlyUrl;
  //
}
```
#### VB
```vb
Protected Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
    RouteManager.BrowserHistoryMode = BrowserHistoryMode.FriendlyUrl
    '
End Sub
```

You can also change the routing mode and check other modes. Note, the **RouteManager.RegisterRoutes(RouteTable.Routes)** method should be also called when the FriendlyUrl mode is enabled. This method registers the default route ("{ViewID}/{ObjectKey}/"). Internally, registration of the default route looks like this:
#### CS
```csharp
RouteTable.Routes.Add("ViewRouteName", "{ViewID}/{ObjectKey}/", "~/Default.aspx", false, new RouteValueDictionary() { { "ObjectKey", string.Empty } });
```
#### VB
```vb
Dim routeValueDictionary As RouteValueDictionary = New RouteValueDictionary()
routeValueDictionary.Add("ObjectKey", String.Empty)
RouteTable.Routes.Add("ViewRouteName", "{ViewID}/{ObjectKey}/", "~/Default.aspx", false, routeValueDictionary)
```
If you uncomment the following lines, the default route will be replaced to '/XAF/Contact_DetailView/ContactId'.
#### CS
```csharp
//RouteTable.Routes.Remove(RouteTable.Routes["ViewRouteName"]);
//RouteTable.Routes.MapPageRoute("ViewRouteName", "XAF/{ViewID}/{ObjectKey}/", "~/Default.aspx", false, new RouteValueDictionary() { { "ObjectKey", string.Empty } });
```
#### VB
```vb
'RouteTable.Routes.Remove(RouteTable.Routes("ViewRouteName"))
'Dim routeValueDictionary As RouteValueDictionary = New RouteValueDictionary()
'routeValueDictionary.Add("ObjectKey", String.Empty)
'RouteTable.Routes.MapPageRoute("ViewRouteName", "XAF/{ViewID}/{ObjectKey}/", "~/Default.aspx", False, routeValueDictionary)
```

### [CustomRouteManager.cs](./CS/FriendlyUrlSample.Web/CustomRouteManager.cs) and [WebApplication.cs](./CS/FriendlyUrlSample.Web/WebApplication.cs) / [CustomRouteManager.vb](./VB/FriendlyUrlSample.Web/CustomRouteManager.vb) and [WebApplication.vb](./VB/FriendlyUrlSample.Web/WebApplication.vb) /
Check these files to see how to customize the default routing. In this sample, the URLs looks like this:

For ListView:  
*  /Contacts/ instead of /Contact_ListView/
*  /Tasks/  instead of /DemoTask_ListView/
               
For DetailView:
*  /Contact/ContactId/ instead of /Contact_DetailView/ContactId/
*  /Task/TaskId/ instead of /DemoTask_DetailView/TaskId/
 
### [CustomLinkController.cs](./CS/FriendlyUrlSample.Module.Web/Controllers/CustomLinkController.cs) / [CustomLinkController.vb](./VB/FriendlyUrlSample.Module.Web/Controllers/CustomLinkController.vb)
This controller demostrates how to open a DetailView in the new window using the **WebApplication.RouteManager.GetRelativeUrl(viewShortcut)** method.
