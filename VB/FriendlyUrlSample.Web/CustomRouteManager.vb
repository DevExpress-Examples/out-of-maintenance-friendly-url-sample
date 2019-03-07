Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Web
Imports FriendlyUrlSample.Module.FriendlyUrlSample.Module.BusinessObjects

Namespace FriendlyUrlSample.Web
    Public Class CustomRouteManager
        Inherits RouteManager

        Private application As WebApplication

        Public Sub New(ByVal application As WebApplication)
            MyBase.New(application)
            Me.application = application
        End Sub

        Public Overrides Function GetRelativeUrl(ByVal shortcut As ViewShortcut, ByVal Optional additionalParams As IDictionary(Of String, String) = Nothing) As String
            If BrowserHistoryMode <> BrowserHistoryMode.FriendlyUrl Then
                Return MyBase.GetRelativeUrl(shortcut, additionalParams)
            End If

            Dim localShortcut As ViewShortcut = New ViewShortcut(shortcut.ViewId, shortcut.ObjectKey)

            If localShortcut.ViewId = application.FindListViewId(GetType(Contact)) Then
                localShortcut.ViewId = "Contacts"
            ElseIf localShortcut.ViewId = application.FindListViewId(GetType(DemoTask)) Then
                localShortcut.ViewId = "Tasks"
            ElseIf localShortcut.ViewId = application.FindDetailViewId(GetType(Contact)) Then
                localShortcut.ViewId = "Contact"
            ElseIf localShortcut.ViewId = application.FindDetailViewId(GetType(DemoTask)) Then
                localShortcut.ViewId = "Task"
            End If

            Return MyBase.GetRelativeUrl(localShortcut, additionalParams)
        End Function

        Public Overrides Function GetViewShortcut(ByVal parameter As String) As ViewShortcut
            If BrowserHistoryMode <> BrowserHistoryMode.FriendlyUrl Then
                Return MyBase.GetViewShortcut(parameter)
            End If

            Dim shortcut As ViewShortcut = MyBase.GetViewShortcut(parameter)

            If shortcut.ViewId = "Contacts" Then
                shortcut.ViewId = application.FindListViewId(GetType(Contact))
            ElseIf shortcut.ViewId = "Tasks" Then
                shortcut.ViewId = application.FindListViewId(GetType(DemoTask))
            ElseIf shortcut.ViewId = "Contact" Then
                shortcut.ViewId = application.FindDetailViewId(GetType(Contact))
            ElseIf shortcut.ViewId = "Task" Then
                shortcut.ViewId = application.FindDetailViewId(GetType(DemoTask))
            End If

            Return shortcut
        End Function
    End Class
End Namespace
