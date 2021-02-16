using Microsoft.Extensions.DependencyInjection;
using Repository.Database;
using Repository.Repositories;
using Service.Extensions;
using Service.Interfaces.IChat;
using Service.Interfaces.ISocket;
using Service.Interfaces.IUsers;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddTransientServices(this IServiceCollection services)
        {
            #region Service Transient
            services.AddTransient<ILoginService, UsersService>();
            services.AddTransient<IAuthService, UsersService>();
            services.AddTransient<IUsersService, UsersService>();
            

            services.AddTransient<ISocketService, SocketService>();

            services.AddTransient<IChatService, ChatService>();

            

            #endregion


            #region Repository Transient
            services.AddTransient<ChatMessageRepository>();
            services.AddTransient<SocketConnectRepository>();
            services.AddTransient<UsersRepository>();

            #endregion

            #region Hub
 
            #endregion



        }

        /// <summary>
        /// Scoped
        /// </summary>
        /// <param name="services"></param>
        public static void AddScopedServices(this IServiceCollection services)
        {
            services.AddScoped<MongoConnection>();
           
        }

        /// <summary>
        /// Singleton
        /// </summary>
        /// <param name="services"></param>
        public static void AddSingletonServices(this IServiceCollection services)
        {
            services.AddSingleton<AuthExtensions>();
        }


    }
}
