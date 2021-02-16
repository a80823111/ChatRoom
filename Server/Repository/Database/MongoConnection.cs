
using Model.BaseModels.Configuration;
using MongoDB.Driver;

namespace Repository.Database
{
    public class MongoConnection
    {
        //public IMongoCollection<T> MongoDbServer<T>(string collection)
        //{
        //    MongoClient client = new MongoClient(ConnectionStrings.MongoDbServer.ConnectionString);
        //    IMongoDatabase database = client.GetDatabase(ConnectionStrings.MongoDbServer.DatabaseName);

        //    return database.GetCollection<T>(collection);
        //}

        public IMongoDatabase MongoDbServer()
        {
            MongoClient client = new MongoClient(ConnectionStrings.MongoDbServer.ConnectionString);
            IMongoDatabase database = client.GetDatabase(ConnectionStrings.MongoDbServer.DatabaseName);

            return database;
        }
    }
}
