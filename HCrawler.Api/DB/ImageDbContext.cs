using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HCrawler.Api.DB
{
    public class ImageDbContext : DbContext
    {
        public ImageDbContext(DbContextOptions<ImageDbContext> options) : base(options)
        {
            
        }

        public DbSet<Source> Sources { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetupSource(modelBuilder.Entity<Source>());
            SetupProfile(modelBuilder.Entity<Profile>());
        }

        private void SetupSource(EntityTypeBuilder<Source> source)
        {
            source
                .HasKey(x => x.Id);

            source
                .Property(x => x.Name)
                .IsRequired();

            source
                .Property(x => x.Url)
                .IsRequired();

            source
                .HasMany(x => x.Profiles)
                .WithOne(x => x.Source)
                .HasForeignKey(x => x.SourceId);
        }

        private void SetupProfile(EntityTypeBuilder<Profile> profile)
        {
            profile
                .HasKey(x => x.Id);

            profile
                .Property(x => x.Name)
                .IsRequired();

            profile
                .HasMany(x => x.Images)
                .WithOne(x => x.Profile)
                .HasForeignKey(x => x.ProfileId);
        }

        private void SetupImage(EntityTypeBuilder<Image> image)
        {
            image
                .Property(x => x.Path)
                .IsRequired();
        }
    }
}