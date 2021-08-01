using Autofac;

namespace TicketBookingSystem.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<>().AsSelf();

            base.Load(builder);
        }
    }
}