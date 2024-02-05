using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasks.Application.Implementation.Task;
using Tasks.Application.Implementation.User;
using Tasks.Application.Interfaces.Task;
using Tasks.Application.Interfaces.User;

namespace Tasks.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ITaskService, TaskService>();

            return services;
        }
    }
}
