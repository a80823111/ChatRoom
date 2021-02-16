using Model.BaseModels.Configuration;
using Model.Migrations;
using MongoDB.Driver;
using Repository.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UsersRepository
    {
        private readonly IMongoCollection<Users> _conn;

        public UsersRepository(MongoConnection mongoDb)
        {
           _conn = mongoDb.MongoDbServer().GetCollection<Users>("Users");



        }

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task InsertUserAsync(Users users)
        {
            await _conn.InsertOneAsync(users);
        }

        /// <summary>
        /// 取得使用者
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Users> GetUserAsync(string userId)
        {
            var filter = Builders<Users>.Filter.Eq(x => x.Id, userId);

            return await _conn.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 取得使用者
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Users> GetUserByNickNameAsync(string nickName)
        {
            var filter = Builders<Users>.Filter.Eq(x => x.NickName, nickName);

            
            return await _conn.Find(filter).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 取得使用者
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        public async Task<List<Users>> GetListUserAsync(List<string> userIds)
        {
            var filter = Builders<Users>.Filter.In(x => x.Id, userIds);

            return await _conn.Find(filter).ToListAsync();
        }
    }
}
