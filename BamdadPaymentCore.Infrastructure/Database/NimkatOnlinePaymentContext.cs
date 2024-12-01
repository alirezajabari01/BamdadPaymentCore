using BamdadPaymentCore.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BamdadPaymentCore.Infrastructure.Database
{
    public class NimkatOnlinePaymentContext : DbContext
    {
        public NimkatOnlinePaymentContext(DbContextOptions<NimkatOnlinePaymentContext> options):base(options) 
        {
            
        }

        public DbSet<Bank> Bank { get; set; }
        public DbSet<OnlinePay> OnlinePay { get; set; }
        public DbSet<SiteError> SiteError { get; set; }
        public DbSet<User> User { get; set; }
    }
}
