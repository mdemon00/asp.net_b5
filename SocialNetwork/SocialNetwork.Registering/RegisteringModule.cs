using Autofac;
using SocialNetwork.Registering.Contexts;
using SocialNetwork.Registering.Repositories;
using SocialNetwork.Registering.Services;
using SocialNetwork.Registering.UnitOfWorks;

namespace SocialNetwork.Registering
{
    public class RegisteringModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public RegisteringModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisteringContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            base.Load(builder);
            builder.RegisterType<RegisteringContext>().As<IRegisteringContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<MemberRepository>().As<IMemberRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<PhotoRepository>()
                .As<IPhotoRepository>()
            .InstancePerLifetimeScope();

            builder.RegisterType<RegisteringUnitOfWork>()
                .As<IRegisteringUnitOfWork>()
            .InstancePerLifetimeScope();

            builder.RegisterType<MemberService>()
                 .As<IMemberService>()
            .InstancePerLifetimeScope();

            builder.RegisterType<PhotoService>()
            .As<IPhotoService>()
            .InstancePerLifetimeScope();
        }
    }
}
