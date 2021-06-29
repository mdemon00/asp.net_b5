using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProject.Areas.Admin.Models;

namespace WebProject
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CourseListModel>().AsSelf();

            base.Load(builder);
        }
    }
}
