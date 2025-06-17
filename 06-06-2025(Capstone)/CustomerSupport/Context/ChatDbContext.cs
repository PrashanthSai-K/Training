using System;
using CustomerSupport.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

namespace CustomerSupport.Context;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Agent> Agents { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<ChatMessage> ChatMessages { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasKey(u => u.Username);
        // modelBuilder.Entity<Image>().HasKey(i => i.ImageName);


        modelBuilder.Entity<Agent>()
                    .HasOne(a => a.User)
                    .WithOne(u => u.Agent)
                    .HasForeignKey<Agent>(a => a.Email)
                    .HasConstraintName("FK_User_Agent")
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Customer>()
                    .HasOne(c => c.User)
                    .WithOne(u => u.Customer)
                    .HasForeignKey<Customer>(c => c.Email)
                    .HasConstraintName("FK_User_Customer")
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Chat>()
                    .HasOne(c => c.Agent)
                    .WithMany(a => a.Chats)
                    .HasForeignKey(c => c.AgentId)
                    .HasConstraintName("FK_Chat_Agent")
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Chat>()
                    .HasOne(c => c.Customer)
                    .WithMany(cu => cu.Chats)
                    .HasForeignKey(c => c.CustomerId)
                    .HasConstraintName("FK_Chat_Customer")
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatMessage>()
                    .HasOne(cm => cm.Chat)
                    .WithMany(c => c.ChatMessages)
                    .HasForeignKey(cm => cm.ChatId)
                    .HasConstraintName("FK_ChatMessage_Chat")
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatMessage>()
                    .HasOne(cm => cm.User)
                    .WithMany(u => u.ChatMessages)
                    .HasForeignKey(cm => cm.UserId)
                    .HasConstraintName("FK_ChatMessage_User")
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatMessage>()
                    .HasOne(cm => cm.Image)
                    .WithOne(cm => cm.ChatMessages)
                    .HasForeignKey<ChatMessage>(cm => cm.ImageName)
                    .HasConstraintName("FK_ChatMessage_Image")
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditLog>()
                    .HasOne(al => al.User)
                    .WithMany(u => u.AuditLogs)
                    .HasForeignKey(al => al.UserId)
                    .HasConstraintName("FK_AuditLog_User")
                    .OnDelete(DeleteBehavior.Restrict);
    }
}


