Imports System.Linq
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Model
Imports DevExpress.ExpressApp.Web
Imports FriendlyUrlSample.Module.Web

Namespace FriendlyUrlSample.Web
    Public Class CustomViewUrlManager
        Implements IViewUrlManager

        Private innerUrlManager As IViewUrlManager
        Private application As WebApplication

        Public Sub New()
            innerUrlManager = New ViewUrlManager()
            application = WebApplication.Instance
        End Sub

        Private Function IViewUrlManager_GetUrl(shortcut As ViewShortcut, Optional additionalParams As IDictionary(Of String, String) = Nothing) As String Implements IViewUrlManager.GetUrl
            Dim localShortcut As ViewShortcut = New ViewShortcut()

            For i As Integer = 0 To shortcut.Count - 1
                localShortcut(shortcut.GetKey(i)) = shortcut(i)
            Next

            Dim modelView As IModelView = application.FindModelView(localShortcut.ViewId)

            If modelView IsNot Nothing Then
                localShortcut.ViewId = (CType(modelView, IModelViewWebExtender)).ViewUrlAlias
            End If

            Return innerUrlManager.GetUrl(localShortcut, additionalParams)
        End Function

        Private Function IViewUrlManager_GetViewShortcut() As ViewShortcut Implements IViewUrlManager.GetViewShortcut
            Dim shortcut As ViewShortcut = innerUrlManager.GetViewShortcut()
            Dim modelView As IModelView = application.Model.Views.SingleOrDefault(Function(m) (CType(m, IModelViewWebExtender)).ViewUrlAlias = shortcut.ViewId)

            If modelView IsNot Nothing Then
                shortcut.ViewId = modelView.Id
            End If

            Return shortcut
        End Function
    End Class
End Namespace
