using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace MusicBlog.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AlbumSong>()
                .HasKey(x => new { x.AlbumId, x.SongId });

            modelBuilder.Entity<AlbumSong>()
                .HasOne(x => x.Album)
                .WithMany(a => a.AlbumSongs)
                .HasForeignKey(x => x.AlbumId);

            modelBuilder.Entity<AlbumSong>()
                .HasOne(x => x.Song)
                .WithMany(s => s.AlbumSongs)
                .HasForeignKey(x => x.SongId);
        }

        public DbSet<Singer> Singers { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<AlbumSong> AlbumSongs { get; set; }

    }


}