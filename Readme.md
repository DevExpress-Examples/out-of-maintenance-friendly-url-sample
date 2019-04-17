﻿# ASP.NET WebForms User-Friendly URLs for Views - Early Access Preview v19.1

## How it works

The **DevExpress.ExpressApp.Web.BrowserHistoryMode** enumeration specifies the current routing mode and URL representation:

**Hash** (Default Mode)
* /Default.aspx#ViewID=Contact_ListView
* /Default.aspx#ViewID=Contact_DetailView&ObjectKey=ContactId

**QueryString**
* /Default.aspx?ViewID=Contact_ListView
* /Default.aspx?ViewID=Contact_DetailView&ObjectKey=ContactId

**FriendlyUrl** (Enabled in this sample)
* /Contact_ListView/
* /Contact_DetailView/ContactId/

To change mode, set the static **DevExpress.ExpressApp.Web.RouteManager.BrowserHistoryMode** property.


**WebApplication.RouteManager** provides the following methods:

```csharp
//C#
public virtual ViewShortcut GetViewShortcut(string parameter);
public virtual string GetRelativeUrl(ViewShortcut shortcut, IDictionary<string, string> additionalParams = null);
```
```vb
'VB
Public Overridable Function GetViewShortcut(ByVal parameter As String) As ViewShortcut
Public Overridable Function GetRelativeUrl(ByVal shortcut As ViewShortcut, ByVal Optional additionalParams As IDictionary(Of String, String) = Nothing) As String
```
GetViewShortcut returns [ViewShortcut](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewShortcut) by current URL. The string parameter is required for backward compatibility with the default BrowserHistoryMode.Hash mode.

GetRelativeUrl returns a relative URL by [ViewShortcut](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ViewShortcut) and the dictionary with additional parameters.

#### Important note
The QueryString and FriendlyUrl modes are based on the standard routing mechanism using [query string](https://en.wikipedia.org/wiki/Query_string) and [History API](https://developer.mozilla.org/en-US/docs/Web/API/History_API). It allows getting full page content via one request and gives us some significant improvements. For example, you will be able to achieve the functionality described in the [How to open a View specified in an external link after logging in to a Web application with the security system enabled?](https://isc.devexpress.com/Thread/WorkplaceDetails/B222208) ticket without any customization.

## Routing customizations

### 1. Enable BrowserHistoryMode.FriendlyUrl mode in the Web application.
In the YourSolutionName.Web project ([Global.asax.cs](./CS/FriendlyUrlSample.Web/Global.asax.cs)/[Global.asax.vb](./VB/FriendlyUrlSample.Web/Global.asax.vb)), check out the Application_Start method:

```csharp
//C#
protected void Application_Start(Object sender, EventArgs e) {
  RouteManager.BrowserHistoryMode = BrowserHistoryMode.FriendlyUrl;
  //
}
```
```vb
'VB
Protected Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
    RouteManager.BrowserHistoryMode = BrowserHistoryMode.FriendlyUrl
    '
End Sub
```
### 2. Slightly change the default route format for BrowserHistoryMode.FriendlyUrl mode.
To have the '/YourCustomString/Contact_DetailView/ContactId' URL representation (just add a prefix and keep the rest), remove the default route and add a custom one using the following code:
```csharp
//C#
RouteManager.RegisterRoutes(RouteTable.Routes);
RouteTable.Routes.Remove(RouteTable.Routes[RouteManager.ViewRouteName]);
RouteTable.Routes.MapPageRoute(RouteManager.ViewRouteName, "YourCustomString/{ViewID}/{ObjectKey}/", "~/Default.aspx", false, new RouteValueDictionary() { { "ObjectKey", string.Empty } });
```
```vb
'VB
RouteManager.RegisterRoutes(RouteTable.Routes)
RouteTable.Routes.Remove(RouteTable.Routes(RouteManager.ViewRouteName))
Dim routeValueDictionary As RouteValueDictionary = New RouteValueDictionary()
routeValueDictionary.Add("ObjectKey", String.Empty)
RouteTable.Routes.MapPageRoute(RouteManager.ViewRouteName, "YourCustomString/{ViewID}/{ObjectKey}/", "~/Default.aspx", False, routeValueDictionary)
```


### 3. Fully change the default route format for BrowserHistoryMode.FriendlyUrl mode.
To customize the default routing completely, create a custom RouteManager class ([CustomRouteManager.cs](./CS/FriendlyUrlSample.Web/CustomRouteManager.cs)/[CustomRouteManager.vb](./VB/FriendlyUrlSample.Web/CustomRouteManager.vb)) and register it in the overridden **WebApplication.CreateRouteManager** method ([WebApplication.cs](./CS/FriendlyUrlSample.Web/WebApplication.cs)/[WebApplication.vb](./VB/FriendlyUrlSample.Web/WebApplication.vb)). The code in this example performs the following route customizations:

For ListView:  
*  /Contacts/ instead of /Contact_ListView/
*  /Tasks/  instead of /DemoTask_ListView/
               
For DetailView:
*  /Contact/ContactId/ instead of /Contact_DetailView/ContactId/
*  /Task/TaskId/ instead of /DemoTask_DetailView/TaskId/


 
### 4. Open a DetailView in the new window from the client-side in BrowserHistoryMode.FriendlyUrl mode.
You can use the **WebApplication.ViewUrlManager.GetUrl(viewShortcut)** method to obtain a URL by the ViewShortcut object corresponding to a required View ([CustomLinkController.cs](./CS/FriendlyUrlSample.Module.Web/Controllers/CustomLinkController.cs) / [CustomLinkController.vb](./VB/FriendlyUrlSample.Module.Web/Controllers/CustomLinkController.vb)).
