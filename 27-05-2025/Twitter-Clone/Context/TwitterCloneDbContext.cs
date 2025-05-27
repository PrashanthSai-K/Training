using System;
using Microsoft.EntityFrameworkCore;
using Twitter_Clone.Models;

namespace Twitter_Clone.Context;

public class TwitterCloneDbContext : DbContext
{
    public TwitterCloneDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FollowList>()
            .HasOne(f => f.Follower)
            .WithMany(f => f.Followings)
            .HasForeignKey(f => f.FollowerId);
        modelBuilder.Entity<FollowList>()
            .HasOne(f => f.Following)
            .WithMany(f => f.Followers)
            .HasForeignKey(f => f.FollowingId);
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Hashtag> Hashtags { get; set; }
    public DbSet<PostHashtags> PostHashtags { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<FollowList> FollowLists { get; set; }

}
