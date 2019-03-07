Imports DevExpress.Persistent.Base
Imports DevExpress.Persistent.BaseImpl
Imports DevExpress.Xpo

Namespace FriendlyUrlSample.[Module].BusinessObjects
    Public Enum Priority
        <ImageName("State_Priority_Low")>
        Low = 0
        <ImageName("State_Priority_Normal")>
        Normal = 1
        <ImageName("State_Priority_High")>
        High = 2
    End Enum

    <DefaultClassOptions>
    Public Class DemoTask
        Inherits Task

        Private _priority As Priority

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        Public Property Priority As Priority
            Get
                Return _priority
            End Get
            Set(ByVal value As Priority)
                SetPropertyValue(NameOf(Priority), _priority, value)
            End Set
        End Property

        <Association("Contact-DemoTask")>
        Public ReadOnly Property Contacts As XPCollection(Of Contact)
            Get
                Return GetCollection(Of Contact)(NameOf(Contacts))
            End Get
        End Property
    End Class
End Namespace
