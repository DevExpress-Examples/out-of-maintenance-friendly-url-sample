<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/174341135/19.1.3%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T830447)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
# ASP.NET WebForms User-Friendly URLs for Views

## How it works

With v19.1, the application's URLs are managed by the **WebApplication.ViewUrlManager** object. This object should implement the [IViewUrlManager](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Web.IViewUrlManager) interface with two methods - *GetUrl* and *GetViewShortcut*:

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
2. Call the static RouteTable.Routes.RegisterXafRoutes() method in the Application_Start method of the Global.asax file:

```csharp
//C#
protected void Application_Start(Object sender, EventArgs e) {
  System.Web.Routing.RouteTable.Routes.RegisterXafRoutes();
  //
}
```
```vb
'VB
Protected Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
    System.Web.Routing.RouteTable.Routes.RegisterXafRoutes()
    '
End Sub
```
#### Notes

User-Friendly URLs are enabled by default in a new project created using the Wizard.

The new User-Friendly URLs feature is based on the standard routing mechanism that uses a [query string](https://en.wikipedia.org/wiki/Query_string) and a [History API](https://developer.mozilla.org/en-US/docs/Web/API/History_API). This allows you to get full page content in a single request and gives us other significant improvements. For example, you will be able to achieve the functionality described in the [How to open a View specified in an external link after logging in to a Web application with the security system enabled?](https://isc.devexpress.com/Thread/WorkplaceDetails/B222208) ticket without any customization.

When User-Friendly URLs are enabled, the [ViewShortcut](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewShortcut) dictionary generated from URL may contain the following default parameters only: "ViewID", "ObjectClassName", "ObjectKey", "ScrollPosition", "NewObject", "mode". You can extend this list using the static [WebViewShortcutHelper.RegisterParameterName](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Web.WebViewShortcutHelper.RegisterParameterName(System.String)) method.

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
    RouteTable.Routes.RegisterXafRoutes();
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
    RouteTable.Routes.RegisterXafRoutes()
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
