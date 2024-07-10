using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<CinemaHall> CinemaHalls { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<CinemaMovie> CinemaMovies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // check later
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Seat)
                .WithOne(s => s.Ticket)
                .HasForeignKey<Ticket>(t => t.SeatId) // one to one
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CinemaHall>()
                .HasOne(ch => ch.Cinema)
                .WithMany(c => c.CinemaHalls)
                .HasForeignKey(ch => ch.CinemaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Seat>()
                .HasOne(s => s.CinemaHall)
                .WithMany(ch => ch.Seats)
                .HasForeignKey(s => s.CinemaHallId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CinemaMovie>()
                    .HasKey(cm => new { cm.cinemaId, cm.movieId });

            modelBuilder.Entity<CinemaMovie>()
                    .HasOne(c => c.cinema)
                    .WithMany(cm => cm.CinemaMovies)
                    .HasForeignKey(c => c.cinemaId);

            modelBuilder.Entity<CinemaMovie>()
                    .HasOne(m => m.movie)
                    .WithMany(cm => cm.CinemaMovies)
                    .HasForeignKey(m => m.movieId);
        }
    }
}
