Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo

Namespace FriendlyUrlSample.[Module].BusinessObjects
    <DefaultClassOptions>
    Public Class Contact
        Inherits Person

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        <Association("Contact-DemoTask")>
        Public ReadOnly Property Tasks As XPCollection(Of DemoTask)
            Get
                Return GetCollection(Of DemoTask)(NameOf(Tasks))
            End Get
        End Property
    End Class
End Namespace
