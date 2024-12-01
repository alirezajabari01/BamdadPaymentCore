using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.StoreProceduresModels;
using BamdadPaymentCore.Domain.StoreProceduresModels.Response;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;

namespace BamdadPaymentCore.Domain.Database
{
    public class NimkatOnlineContext : DbContext
    {
        public NimkatOnlineContext(DbContextOptions<NimkatOnlineContext> options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine);
        }
        public DbSet<Site> Site { get; set; }

        public DbQuery<SiteAuthenticationResult> SiteAuthenticationResult { get; set; }

        public DbQuery<InsertOnlinePayResult> InsertOnlinePayResult { get; set; }

        public DbQuery<SelectBankIdResult> SelectBankIdResult { get; set; }

        public DbQuery<UpdateOnlinePayRefundResult> UpdateOnlinePayRefundResult { get; set; }

        public DbQuery<UpdateOnlinePayReversalResult> UpdateOnlinePayReversalResult { get; set; }

    }
}
