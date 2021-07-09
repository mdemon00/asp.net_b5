using Autofac;
using WebProject.Training.Context;
using WebProject.Training.Repositories;
using WebProject.Training.UnitOfWorks;

namespace WebProject.Training
{
    public class TrainingModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public TrainingModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TrainingContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();
            builder.RegisterType<TrainingContext>().As<ITrainingContext>()
     .WithParameter("connectionString", _connectionString)
     .WithParameter("migrationAssemblyName", _migrationAssemblyName)
     .InstancePerLifetimeScope();

            builder.RegisterType<StudentRepository>().As<IStudentRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CourseRepository>().As<ICourseRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<TrainingUnitOfWork>().As<ITrainingUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterType<CourseService>().As<ICourseService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
