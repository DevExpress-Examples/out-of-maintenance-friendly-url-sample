Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports DevExpress.ExpressApp
Imports DevExpress.Data.Filtering
Imports DevExpress.Persistent.Base
Imports DevExpress.ExpressApp.Updating
Imports DevExpress.Xpo
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.Persistent.BaseImpl
Imports FriendlyUrlSample.Module.FriendlyUrlSample.Module.BusinessObjects

' For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
Public Class Updater
    Inherits ModuleUpdater
    Public Sub New(ByVal objectSpace As IObjectSpace, ByVal currentDBVersion As Version)
        MyBase.New(objectSpace, currentDBVersion)
    End Sub

    Public Overrides Sub UpdateDatabaseAfterUpdateSchema()
        MyBase.UpdateDatabaseAfterUpdateSchema()
        Dim karlJablonski As Contact = CreateContact("Karl", "Jablonski")

        If ObjectSpace.FindObject(Of DemoTask)(CriteriaOperator.Parse("Subject == 'Check wiring in main electricity panel'")) Is Nothing Then
            Dim task As DemoTask = ObjectSpace.CreateObject(Of DemoTask)()
            task.Subject = "Check wiring in main electricity panel"
            task.AssignedTo = karlJablonski
            task.StartDate = DateTime.Parse("May 03, 2008")
            task.DueDate = DateTime.Parse("September 06, 2008")
            task.Status = DevExpress.Persistent.Base.General.TaskStatus.InProgress
            task.Priority = Priority.High
        End If

        Dim johnNilsen As Contact = CreateContact("John", "Nilsen")

        If ObjectSpace.FindObject(Of DemoTask)(CriteriaOperator.Parse("Subject == 'Take kids to park and play baseball on Sunday'")) Is Nothing Then
            Dim task As DemoTask = ObjectSpace.CreateObject(Of DemoTask)()
            task.Subject = "Take kids to park and play baseball on Sunday"
            task.AssignedTo = johnNilsen
            task.StartDate = DateTime.Parse("May 03, 2008")
            task.DueDate = DateTime.Parse("May 04, 2008")
            task.Status = DevExpress.Persistent.Base.General.TaskStatus.Completed
            task.Priority = Priority.Low
        End If

        Dim maryTallitson As Contact = CreateContact("Mary", "Tallitson")

        If ObjectSpace.FindObject(Of DemoTask)(CriteriaOperator.Parse("Subject == 'Bake brownies and send them to neighbors'")) Is Nothing Then
            Dim task As DemoTask = ObjectSpace.CreateObject(Of DemoTask)()
            task.Subject = "Bake brownies and send them to neighbors"
            task.AssignedTo = maryTallitson
            task.StartDate = DateTime.Parse("June 03, 2008")
            task.DueDate = DateTime.Parse("June 06, 2008")
            task.Status = DevExpress.Persistent.Base.General.TaskStatus.Completed
            task.Priority = Priority.High
        End If

        Dim anitaRyan As Contact = CreateContact("Anita", "Ryan")

        If ObjectSpace.FindObject(Of DemoTask)(CriteriaOperator.Parse("Subject == 'Install an new electric outlet in garage'")) Is Nothing Then
            Dim task As DemoTask = ObjectSpace.CreateObject(Of DemoTask)()
            task.Subject = "Install an new electric outlet in garage"
            task.AssignedTo = anitaRyan
            task.StartDate = DateTime.Parse("July 03, 2008")
            task.DueDate = DateTime.Parse("July 06, 2008")
            task.Status = DevExpress.Persistent.Base.General.TaskStatus.Completed
            task.Priority = Priority.Low
        End If

        ObjectSpace.CommitChanges()
    End Sub

    Private Function CreateContact(ByVal firstName As String, ByVal lastName As String) As Contact
        Dim contact As Contact = ObjectSpace.FindObject(Of Contact)(CriteriaOperator.Parse("LastName=?", lastName))

        If contact Is Nothing Then
            contact = ObjectSpace.CreateObject(Of Contact)()
            contact.LastName = lastName
            contact.FirstName = firstName
            contact.Email = $"{firstName}{lastName}@example.com"
        End If

        Return contact
    End Function

    Public Overrides Sub UpdateDatabaseBeforeUpdateSchema()
        MyBase.UpdateDatabaseBeforeUpdateSchema()
        'If (CurrentDBVersion < New Version("1.1.0.0") AndAlso CurrentDBVersion > New Version("0.0.0.0")) Then
        '    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName")
        'End If
    End Sub
End Class