using System;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using FriendlyUrlSample.Module.BusinessObjects;

namespace FriendlyUrlSample.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
            base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            Contact karlJablonski = CreateContact("Karl", "Jablonski");
            if(ObjectSpace.FindObject<DemoTask>(CriteriaOperator.Parse("Subject == 'Check wiring in main electricity panel'")) == null) {
                DemoTask task = ObjectSpace.CreateObject<DemoTask>();
                task.Subject = "Check wiring in main electricity panel";
                task.AssignedTo = karlJablonski;
                task.StartDate = DateTime.Parse("May 03, 2008");
                task.DueDate = DateTime.Parse("September 06, 2008");
                task.Status = DevExpress.Persistent.Base.General.TaskStatus.InProgress;
                task.Priority = Priority.High;
            }

            Contact johnNilsen = CreateContact("John", "Nilsen");
            if(ObjectSpace.FindObject<DemoTask>(CriteriaOperator.Parse("Subject == 'Take kids to park and play baseball on Sunday'")) == null) {
                DemoTask task = ObjectSpace.CreateObject<DemoTask>();
                task.Subject = "Take kids to park and play baseball on Sunday";
                task.AssignedTo = johnNilsen;
                task.StartDate = DateTime.Parse("May 03, 2008");
                task.DueDate = DateTime.Parse("May 04, 2008");
                task.Status = DevExpress.Persistent.Base.General.TaskStatus.Completed;
                task.Priority = Priority.Low;
            }

            Contact maryTallitson = CreateContact("Mary", "Tallitson");
            if(ObjectSpace.FindObject<DemoTask>(CriteriaOperator.Parse("Subject == 'Bake brownies and send them to neighbors'")) == null) {
                DemoTask task = ObjectSpace.CreateObject<DemoTask>();
                task.Subject = "Bake brownies and send them to neighbors";
                task.AssignedTo = maryTallitson;
                task.StartDate = DateTime.Parse("June 03, 2008");
                task.DueDate = DateTime.Parse("June 06, 2008");
                task.Status = DevExpress.Persistent.Base.General.TaskStatus.Completed;
                task.Priority = Priority.High;
            }

            Contact anitaRyan = CreateContact("Anita", "Ryan");
            if(ObjectSpace.FindObject<DemoTask>(CriteriaOperator.Parse("Subject == 'Install an new electric outlet in garage'")) == null) {
                DemoTask task = ObjectSpace.CreateObject<DemoTask>();
                task.Subject = "Install an new electric outlet in garage";
                task.AssignedTo = anitaRyan;
                task.StartDate = DateTime.Parse("July 03, 2008");
                task.DueDate = DateTime.Parse("July 06, 2008");
                task.Status = DevExpress.Persistent.Base.General.TaskStatus.Completed;
                task.Priority = Priority.Low;
            }

            ObjectSpace.CommitChanges();
        }
        private Contact CreateContact(string firstName, string lastName) {
            Contact contact = ObjectSpace.FindObject<Contact>(CriteriaOperator.Parse("LastName=?", lastName));
            if(contact == null) {
                contact = ObjectSpace.CreateObject<Contact>();
                contact.LastName = lastName;
                contact.FirstName = firstName;
                contact.Email = $"{firstName}{lastName}@example.com";
            }
            return contact;
        }
        public override void UpdateDatabaseBeforeUpdateSchema() {
            base.UpdateDatabaseBeforeUpdateSchema();
            //if(CurrentDBVersion < new Version("1.1.0.0") && CurrentDBVersion > new Version("0.0.0.0")) {
            //    RenameColumn("DomainObject1Table", "OldColumnName", "NewColumnName");
            //}
        }
    }
}
