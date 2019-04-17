Imports DevExpress.Data.Filtering
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Actions
Imports DevExpress.ExpressApp.Web
Imports DevExpress.Persistent.Base
Imports FriendlyUrlSample.Module.FriendlyUrlSample.Module.BusinessObjects

Namespace FriendlyUrlSample.[Module].Web.Controllers
    Public Class CustomLinkController
        Inherits ViewController

        Private goToJohnNilsenAction As SimpleAction

        Protected Overrides Sub OnActivated()
            MyBase.OnActivated()
            Dim johnNilsen As Contact = ObjectSpace.FindObject(Of Contact)(CriteriaOperator.Parse("LastName == 'Nilsen'"))

            If johnNilsen IsNot Nothing Then
                goToJohnNilsenAction.Active.RemoveItem("JohnNilsenIsNotExist")
                Dim viewShortcut As ViewShortcut = New ViewShortcut(Application.GetDetailViewId(GetType(Contact)), ObjectSpace.GetKeyValueAsString(johnNilsen))
                Dim url As String = (CType(Application, WebApplication)).ViewUrlManager.GetUrl(viewShortcut)
                goToJohnNilsenAction.SetClientScript($"window.open('{url}', '_blank')", False)
            Else
                goToJohnNilsenAction.Active("JohnNilsenIsNotExist") = False
            End If
        End Sub

        Public Sub New()
            goToJohnNilsenAction = New SimpleAction(Me, "GoToJohnNilsen", PredefinedCategory.View)
        End Sub

        Private Sub InitializeComponent()
        End Sub
    End Class
End Namespace
