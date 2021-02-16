using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BaseModels.Configuration
{
    public class ConnectionStrings
    {
        public static MongoDb MongoDbServer { get; set; }
    }

    public class MongoDb
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
