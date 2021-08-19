using PokedexApp.Services.Request;
using PokedexApp.Services.SQLite;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokedexApp.Extenders
{
    public static class ServiceExtension
    {
        internal static void ResolveServices(this IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IRequestService, RequestService>();
            containerRegistry.Register<ISQLite, Database>();
        }
    }
}
