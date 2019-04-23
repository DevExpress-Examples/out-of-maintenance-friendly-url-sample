# ASP.NET WebForms User-Friendly URLs for Views - v19.1

## How it works

The **IViewUrlManager** interface is used to manage routing mechanism in XAF.

```csharp
//C#
public interface IViewUrlManager {
    string GetUrl(ViewShortcut shortcut, IDictionary<string, string> additionalParams = null);
    ViewShortcut GetViewShortcut();
}
```
```vb
'VB
Interface IViewUrlManager
    Function GetUrl(ByVal shortcut As ViewShortcut, ByVal Optional additionalParams As IDictionary(Of String, String) = Nothing) As String
    Function GetViewShortcut() As ViewShortcut
End Interface
```
GetViewShortcut returns [ViewShortcut](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewShortcut) by current URL.

GetUrl returns a URL by [ViewShortcut](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewShortcut) and the dictionary with additional parameters.

WebApplication has the **ViewUrlManager** property for getting access to the current instance and the virtual **CreateViewUrlManager method** for creating a custom functionality.

The ViewUrlManager and ViewUrlHashManager classes implement this interface out of the box. ViewUrlHashManager implements classic functionality via hash and created by default. ViewUrlManager implements the User-Friendly URLs mechanism.

ViewUrlHashManager (default):
* /Default.aspx#ViewID=Contact_ListView
* /Default.aspx#ViewID=Contact_DetailView&ObjectKey=ContactId

ViewUrlManager:
* /Contact_ListView/
* /Contact_DetailView/ContactId/

## How to enable User-Friendly URLs

Perform the following steps to enable User-Friendly URLs in your ASP.NET application:

Create ViewUrlManager instance in the overridden CreateViewUrlManager method of the WebApplication descendant:

```csharp
//C#
protected override IViewUrlManager CreateViewUrlManager() {
    return new ViewUrlManager();
}
```
```vb
'VB
Protected Overrides Function CreateViewUrlManager() As IViewUrlManager
    Return New ViewUrlManager()
End Function
```
Call the static RouteTable.Routes.RegisterDefaultXafRoutes() method in the Application_Start method of the Global.asax file:

```csharp
//C#
protected void Application_Start(Object sender, EventArgs e) {
  RouteTable.Routes.RegisterDefaultXafRoutes();
  //
}
```
```vb
'VB
Protected Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
    RouteTable.Routes.RegisterDefaultXafRoutes()
    '
End Sub
```
#### Important note
The new User-Friendly URLs feature are based on the standard routing mechanism using [query string](https://en.wikipedia.org/wiki/Query_string) and [History API](https://developer.mozilla.org/en-US/docs/Web/API/History_API). It allows getting full page content via one request and gives us some significant improvements. For example, you will be able to achieve the functionality described in the [How to open a View specified in an external link after logging in to a Web application with the security system enabled?](https://isc.devexpress.com/Thread/WorkplaceDetails/B222208) ticket without any customization.

## Routing customizations

### 1. Slightly change the default route format for BrowserHistoryMode.FriendlyUrl mode.
To have the '/YourCustomString/Contact_DetailView/ContactId' URL representation (just add a prefix and keep the rest), remove the default route and add a custom one using the following code:
```csharp
//C#
RouteTable.Routes.RegisterDefaultXafRoutes();
RouteTable.Routes.Remove(RouteTable.Routes[ViewUrlManager.RouteName]);
RouteTable.Routes.MapPageRoute(ViewUrlManager.RouteName, "YourCustomString/{ViewID}/{ObjectKey}/", "~/Default.aspx", false, new RouteValueDictionary() { { "ObjectKey", string.Empty } });
```
```vb
'VB
RouteTable.Routes.RegisterDefaultXafRoutes()
RouteTable.Routes.Remove(RouteTable.Routes(ViewUrlManager.RouteName))
Dim routeValueDictionary As RouteValueDictionary = New RouteValueDictionary()
routeValueDictionary.Add("ObjectKey", String.Empty)
RouteTable.Routes.MapPageRoute(ViewUrlManager.RouteName, "YourCustomString/{ViewID}/{ObjectKey}/", "~/Default.aspx", False, routeValueDictionary)
```


### 2. Fully change the default route format for BrowserHistoryMode.FriendlyUrl mode.
To customize the default routing completely, create a custom class which implements the IViewUrlManager interface ([CustomRouteManager.cs](./CS/FriendlyUrlSample.Web/CustomViewUrlManager.cs)/[CustomViewUrlManager.vb](./VB/FriendlyUrlSample.Web/CustomViewUrlManager.vb)) and register it in the overridden **WebApplication.CreateRouteManager** method ([WebApplication.cs](./CS/FriendlyUrlSample.Web/WebApplication.cs)/[WebApplication.vb](./VB/FriendlyUrlSample.Web/WebApplication.vb)). The code in this example performs the following route customizations:

For ListView:  
*  /Contacts/ instead of /Contact_ListView/
*  /Tasks/  instead of /DemoTask_ListView/
               
For DetailView:
*  /Contact/ContactId/ instead of /Contact_DetailView/ContactId/
*  /Task/TaskId/ instead of /DemoTask_DetailView/TaskId/


 
### 4. Open a DetailView in the new window from the client-side in BrowserHistoryMode.FriendlyUrl mode.
You can use the **WebApplication.ViewUrlManager.GetUrl(viewShortcut)** method to obtain a URL by the ViewShortcut object corresponding to a required View ([CustomLinkController.cs](./CS/FriendlyUrlSample.Module.Web/Controllers/CustomLinkController.cs) / [CustomLinkController.vb](./VB/FriendlyUrlSample.Module.Web/Controllers/CustomLinkController.vb)).
