using Bitirme.DAL.Entities.Courses;
using Bitirme.DAL.Entities.Medias;
using Bitirme.DAL.Entities.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL
{
    public class BitirmeDbContext:DbContext
    {
        public BitirmeDbContext(DbContextOptions<BitirmeDbContext> options) : base(options)
        {

        }

        #region USERS
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<UserMailCode> UserMailCodes { get; set; }

        #endregion

        #region COURSES
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Lesson> Lesson { get; set; }
        public DbSet<LessonStudent> LessonStudents { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }
        public DbSet<Question> Questions { get; set; }
        #endregion

        #region Medias
        public DbSet<ClassMedia> Medias { get; set; }
        #endregion

    }
}
