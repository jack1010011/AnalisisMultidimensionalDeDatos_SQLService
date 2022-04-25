using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;
using TableDependency.SqlClient.Base.Messages;
using System.Configuration;
using System.ServiceProcess;


namespace WorkerService1
{
    class ModelClass
    {
        public class Message
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public string UserId { get; set; }
            public DateTime Timestamp { get; set; }
        }

    }


}





