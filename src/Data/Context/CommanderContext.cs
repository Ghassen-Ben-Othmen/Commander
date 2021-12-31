using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class CommanderContext : DbContext
    {
        public CommanderContext(DbContextOptions<CommanderContext> options) : base(options)
        {

        }

        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Command> Commands { get; set; }
        public DbSet<Argument> Arguments { get; set; }
        public DbSet<Attachment> Attchments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Command>()
                .HasOne(c => c.Platform)
                .WithMany(p => p.Commands)
                .HasForeignKey(c => c.PlatformId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Argument>()
                .HasOne(arg => arg.Command)
                .WithMany(c => c.Arguments)
                .HasForeignKey(arg => arg.CommandId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attachment>(entity =>
            {
                entity.HasOne(att => att.Command)
                      .WithMany(c => c.Attachments)
                      .HasForeignKey(att => att.CommandId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(att => att.CreatedAt)
                      .ValueGeneratedOnAdd();
            });
        }
    }
}
