using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendlyUrlSample.Module.BusinessObjects {
    [DefaultClassOptions]
    public class Contact : Person {
        public Contact(Session session) : base(session) { }


        [Association("Contact-DemoTask")]
        public XPCollection<DemoTask> Tasks {
            get {
                return GetCollection<DemoTask>(nameof(Tasks));
            }
        }

    }
}
