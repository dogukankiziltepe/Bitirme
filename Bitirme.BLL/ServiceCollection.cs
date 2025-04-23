using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Services;
using Bitirme.DAL;

namespace Bitirme.BLL
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddBLL(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDAL(configuration);
            
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IClassMediaService, ClassMediaService>();
            services.AddScoped<IClassService, ClassService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();
            
            return services;
        }
    }
}
