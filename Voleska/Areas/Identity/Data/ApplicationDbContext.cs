using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Voleska.Areas.Identity.Data;
using Voleska.Models;


namespace Voleska.Data
{
    public class ApplicationDbContext : IdentityDbContext<Areas.Identity.Data.ApplicationUser, Areas.Identity.Data.ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            modelBuilder.Entity<Material>().ToTable("Material");
            modelBuilder.Entity<TipIzdelka>().ToTable("TipIzdelka");
            modelBuilder.Entity<Izdelek>().ToTable("Izdelek");
            modelBuilder.Entity<Opcija>().ToTable("Opcija");
            modelBuilder.Entity<TipOpcije>().ToTable("TipOpcije");
            modelBuilder.Entity<Narocilo>().ToTable("Narocilo");
            modelBuilder.Entity<Transakcija>().ToTable("Transakcija");
            modelBuilder.Entity<Novice>().ToTable("Novice");
            modelBuilder.Entity<Posta>().ToTable("Posta");
            modelBuilder.Entity<Naslov>().ToTable("Naslov");
            modelBuilder.Entity<Blog>().ToTable("Blog");
            modelBuilder.Entity<Komentar>().ToTable("Komentar");
            modelBuilder.Entity<LajkanjeKomentarjev>().ToTable("LajkanjeKomentarjev");
            modelBuilder.Entity<LajkanjeBlogov>().ToTable("LajkanjeBlogov");
            modelBuilder.Entity<OcenaIzdelka>().ToTable("OceneIzdelkov");
            modelBuilder.Entity<Odgovor>().ToTable("Odgovori");
            modelBuilder.Entity<Sporocilo>().ToTable("Sporocila");
            modelBuilder.Entity<Pogovor>().ToTable("Pogovori");
            modelBuilder.Entity<Dopisovanje>().ToTable("Dopisovanja");

        }

        public DbSet<Material> Materiali { get; set; }
        public DbSet<TipIzdelka> TipiIzdelkov { get; set; }
        public DbSet<Izdelek> Izdelki { get; set; }
        public DbSet<Opcija> Opcije { get; set; }
        public DbSet<TipOpcije> TipiOpcij { get; set; }
        public DbSet<Narocilo> Narocila { get; set; }
        public DbSet<Transakcija> Transakcije { get; set; }
        public DbSet<Posta> Poste { get; set; }
        public DbSet<Naslov> Naslovi { get; set; }
        public DbSet<Blog> Blogi { get; set; }
        public DbSet<Komentar> Komentarji { get; set; }
        public DbSet<LajkanjeKomentarjev> LajkaniKomentarji { get; set; }
        public DbSet<LajkanjeBlogov> LajkaniBlogi { get; set; }
        public DbSet<OcenaIzdelka> OceneIzdelkov { get; set; }
        public DbSet<Novice> Novice { get; set; }
        public DbSet<Odgovor> Odgovori { get; set; }
        public DbSet<Sporocilo> Sporocila { get; set; }
        public DbSet<Pogovor> Pogovori { get; set; }
        public DbSet<Dopisovanje> Dopisovanja { get; set; }



    }
}
