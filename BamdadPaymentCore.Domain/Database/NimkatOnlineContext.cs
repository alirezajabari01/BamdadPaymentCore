using BamdadPaymentCore.Domain.Entites;
using BamdadPaymentCore.Domain.Models.StoreProceduresModels.Response;
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
        public DbSet<OnlinePay> OnlinePay { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<SiteError> SiteErrors { get; set; }
        public DbSet<User> Users { get; set; }

        public DbQuery<SiteAuthenticationResult> SiteAuthenticationResult { get; set; }

        public DbQuery<InsertIntoOnlinePayResult> InsertOnlinePayResult { get; set; }

        public DbQuery<SelectBankIdResult> SelectBankIdResult { get; set; }

        public DbQuery<UpdateOnlinePayRefundResult> UpdateOnlinePayRefundResult { get; set; }

        public DbQuery<UpdateOnlinePayReversalResult> UpdateOnlinePayReversalResult { get; set; }

    }
}
