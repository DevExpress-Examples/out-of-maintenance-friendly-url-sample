# ASP.NET WebForms User-Friendly URLs for Views - v19.1

## How it works

The application's URLs are managed by the **WebApplication.ViewUrlManager** object. This object should implement the **IViewUrlManager** interface with two methods - *GetUrl* and *GetViewShortcut*:

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
**GetUrl** - returns a URL based on a [ViewShortcut](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewShortcut) and a dictionary of additional parameters.

**GetViewShortcut** - returns a [ViewShortcut](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewShortcut) by the current URL.

We provide the following implementations of the IViewUrlManager interface:

**ViewUrlHashManager** - implements the classic functionality, where URLs contain a full description of ViewShortcut parameters:
* /Default.aspx#ViewID=Contact_ListView
* /Default.aspx#ViewID=Contact_DetailView&ObjectKey=ContactId

**ViewUrlManager** - implements the built-in User-Friendly URLs mechanism:
* /Contact_ListView/
* /Contact_DetailView/ContactId/

In addition, you can provide a custom implementation.

Override the **WebApplication.CreateViewUrlManager** method to specify the application's URL Manager.

## How to enable User-Friendly URLs

1. Create a ViewUrlManager instance in the overridden CreateViewUrlManager method of the WebApplication descendant:

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
2. Call the static RouteTable.Routes.RegisterDefaultXafRoutes() method in the Application_Start method of the Global.asax file:

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
#### Note
The new User-Friendly URLs feature is based on the standard routing mechanism that uses a [query string](https://en.wikipedia.org/wiki/Query_string) and a [History API](https://developer.mozilla.org/en-US/docs/Web/API/History_API). This allows you to get full page content in a single request and gives us other significant improvements. For example, you will be able to achieve the functionality described in the [How to open a View specified in an external link after logging in to a Web application with the security system enabled?](https://isc.devexpress.com/Thread/WorkplaceDetails/B222208) ticket without any customization.

## Routing customization

### 1. How to change the default format of User-Friendly URLs
The following example shows how to add the *YourCustomString* prefix to the default user-friendly URL format. It removes the default routing rule (YourClass_View/KeyValue) and adds a custom one (YourCustomString/YourClass_View/KeyValue).
```csharp
//C#
using System;
using System.Web.Routing;
using DevExpress.ExpressApp.Web;
//
protected void Application_Start(Object sender, EventArgs e) {
    RouteTable.Routes.RegisterDefaultXafRoutes();
    RouteTable.Routes.Remove(RouteTable.Routes[ViewUrlManager.RouteName]);
    RouteTable.Routes.MapPageRoute(ViewUrlManager.RouteName, "YourCustomString/{ViewID}/{ObjectKey}/", "~/Default.aspx", false, new RouteValueDictionary() { { "ObjectKey", string.Empty } });
    //
}
```
```vb
'VB
Imports System
Imports System.Web.Routing
Imports DevExpress.ExpressApp.Web
'
Protected Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
    RouteTable.Routes.RegisterDefaultXafRoutes()
    RouteTable.Routes.Remove(RouteTable.Routes(ViewUrlManager.RouteName))
    Dim routeValueDictionary As RouteValueDictionary = New RouteValueDictionary()
    routeValueDictionary.Add("ObjectKey", String.Empty)
    RouteTable.Routes.MapPageRoute(ViewUrlManager.RouteName, "YourCustomString/{ViewID}/{ObjectKey}/", "~/Default.aspx", False, routeValueDictionary)
End Sub
```

### 2. How to provide custom URLs
1. Create a custom class that implements the IViewUrlManager interface: [CustomRouteManager.cs](./CS/FriendlyUrlSample.Web/CustomViewUrlManager.cs)/[CustomViewUrlManager.vb](./VB/FriendlyUrlSample.Web/CustomViewUrlManager.vb).
2. Override the **CreateRouteManager** method in the application's WebApplication descendant: [WebApplication.cs](./CS/FriendlyUrlSample.Web/WebApplication.cs)/[WebApplication.vb](./VB/FriendlyUrlSample.Web/WebApplication.vb). 

The attached example uses a model extender to allow specifying user-friendly View identifiers in the Model Editor:

For ListView:  
*  /Contacts/ instead of /Contact_ListView/
*  /Tasks/  instead of /DemoTask_ListView/
               
For DetailView:
*  /Contact/ContactId/ instead of /Contact_DetailView/ContactId/
*  /Task/TaskId/ instead of /DemoTask_DetailView/TaskId/

Check the [WebModule.cs](./CS/FriendlyUrlSample.Module.Web/WebModule.cs) and [WebModule.vb](./VB/FriendlyUrlSample.Module.Web/WebModule.vb) files to see the IModelView extender's implementation.
 
### 3. How to show a View in a new window on the client side
The **WebApplication.ViewUrlManager.GetUrl(viewShortcut)** method allows you to obtain a View's URL by its ViewShortcut. This URL can be used as a parameter of the window.open JavaScript method to open a new browser window. See an example in the [CustomLinkController.cs](./CS/FriendlyUrlSample.Module.Web/Controllers/CustomLinkController.cs) and [CustomLinkController.vb](./VB/FriendlyUrlSample.Module.Web/Controllers/CustomLinkController.vb) files.
