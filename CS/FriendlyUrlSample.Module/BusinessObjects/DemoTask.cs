using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FriendlyUrlSample.Module.BusinessObjects {
    public enum Priority {
        [ImageName("State_Priority_Low")]
        Low = 0,
        [ImageName("State_Priority_Normal")]
        Normal = 1,
        [ImageName("State_Priority_High")]
        High = 2
    }
    [DefaultClassOptions]
    public class DemoTask : Task {
        private Priority priority;
        public DemoTask(Session session) : base(session) { }
        public Priority Priority {
            get {
                return priority;
            }
            set {
                SetPropertyValue(nameof(Priority), ref priority, value);
            }
        }

        [Association("Contact-DemoTask")]
        public XPCollection<Contact> Contacts {
            get {
                return GetCollection<Contact>(nameof(Contacts));
            }
        }
    }
}
