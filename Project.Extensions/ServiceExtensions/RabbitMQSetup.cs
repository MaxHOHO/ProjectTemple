using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Extensions.ServiceExtensions
{
    public static class RabbitMQSetup
    {

        public static void AddRabbitMQSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.AddSingleton(EventBusRabbitMQ);
        }

    }
}
