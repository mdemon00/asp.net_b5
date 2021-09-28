using Autofac;
using DataImporter.Areas.Member.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataImporter.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<GroupListModel>().AsSelf();
            builder.RegisterType<EditGroupModel>().AsSelf();
            builder.RegisterType<CreateGroupModel>().AsSelf();
            builder.RegisterType<ContactListModel>().AsSelf();
            builder.RegisterType<ImportContactModel>().AsSelf();
            builder.RegisterType<HistoryListModel>().AsSelf();

            base.Load(builder);
        }
    }
}
