using AttendanceSystem.Attending.Contexts;
using AttendanceSystem.Attending.Repositories;
using AttendanceSystem.Attending.Services;
using AttendanceSystem.Attending.UnitOfWorks;
using Autofac;

namespace AttendanceSystem.Attending
{
    public class AttendingModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public AttendingModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AttendingContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<AttendingContext>().As<IAttendingContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<StudentRepository>().As<IStudentRepository>()
                .InstancePerLifetimeScope(); 
            
            builder.RegisterType<AttendingUnitOfWork>().As<IAttendingUnitOfWork>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<StudentService>().As<IStudentService>()
                .InstancePerLifetimeScope();

            builder.RegisterType<AttendanceRepository>().As<IAttendanceRepository>()
            .InstancePerLifetimeScope();

            builder.RegisterType<AttendanceService>().As<IAttendanceService>()
                .InstancePerLifetimeScope();

            base.Load(builder);

        }
    }
}

