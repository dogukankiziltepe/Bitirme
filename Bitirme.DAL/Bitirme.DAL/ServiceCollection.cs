using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bitirme.DAL.Abstracts;
using Bitirme.DAL.Concretes;
using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Concretes.Courses;
using Bitirme.DAL.Abstracts.Medias;
using Bitirme.DAL.Concretes.Medias;
using Bitirme.DAL.Concretes.Users;
using Bitirme.DAL.Abstracts.Users;

namespace Bitirme.DAL
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddDAL(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure DbContext with PostgreSQL
            services.AddDbContext<BitirmeDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            // Register repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();
            services.AddScoped<ILessonStudentRepository,LessonStudentRepository>();
            services.AddScoped<IClassStudentRepository, ClassStudentRepository>();


            return services;
        }
    }
}
