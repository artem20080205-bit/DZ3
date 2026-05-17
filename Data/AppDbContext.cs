using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MuseumApp.Models;

namespace MuseumApp.Data
{
    /// <summary>Контекст базы данных</summary>
    public class AppDbContext : DbContext
    {
        public DbSet<Museum> Museums { get; set; }
        public DbSet<Exhibit> Exhibits { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=museum.db");
            // Включение логирования (ILog)
            options.LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Начальные данные: 4 музея
            modelBuilder.Entity<Museum>().HasData(
                new Museum { Id = 1, Name = "Эрмитаж" },
                new Museum { Id = 2, Name = "Третьяковская галерея" },
                new Museum { Id = 3, Name = "Русский музей" },
                new Museum { Id = 4, Name = "Музей космонавтики" }
            );

            // Начальные данные: 14 экспонатов
            modelBuilder.Entity<Exhibit>().HasData(
                // Эрмитаж (Id=1)
                new Exhibit { Id = 1, MuseumId = 1, Name = "Мадонна Литта", ValueK = 5000 },
                new Exhibit { Id = 2, MuseumId = 1, Name = "Часы Павлин", ValueK = 3000 },
                new Exhibit { Id = 3, MuseumId = 1, Name = "Скифское золото", ValueK = 8000 },
                new Exhibit { Id = 13, MuseumId = 1, Name = "Табакерка", ValueK = 500 },

                // Третьяковка (Id=2)
                new Exhibit { Id = 4, MuseumId = 2, Name = "Утро в сосновом лесу", ValueK = 2000 },
                new Exhibit { Id = 5, MuseumId = 2, Name = "Богатыри", ValueK = 4000 },
                new Exhibit { Id = 6, MuseumId = 2, Name = "Грачи прилетели", ValueK = 1500 },
                new Exhibit { Id = 14, MuseumId = 2, Name = "Иван Грозный убивает сына", ValueK = 8000 },

                // Русский музей (Id=3)
                new Exhibit { Id = 7, MuseumId = 3, Name = "Последний день Помпеи", ValueK = 6000 },
                new Exhibit { Id = 8, MuseumId = 3, Name = "Девятый вал", ValueK = 4500 },
                new Exhibit { Id = 9, MuseumId = 3, Name = "Бурлаки на Волге", ValueK = 3500 },

                // Музей космонавтики (Id=4)
                new Exhibit { Id = 10, MuseumId = 4, Name = "Спутник-1", ValueK = 10000 },
                new Exhibit { Id = 11, MuseumId = 4, Name = "Скафандр Гагарина", ValueK = 7000 },
                new Exhibit { Id = 12, MuseumId = 4, Name = "Луноход", ValueK = 12000 }
            );
        }
    }
}