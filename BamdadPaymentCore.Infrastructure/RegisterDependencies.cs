using BamdadPaymentCore.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Infrastructure
{
    public static class RegisterDependencies
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
       => services.AddDbContext<NimkatOnlinePaymentContext>(options =>
          options.UseSqlServer("Persist Security Info=True;Initial Catalog=NimkatOnlinePayment;User ID=sa;password=andIShe2019$$;data source=185.13.229.227"));
    }
}
