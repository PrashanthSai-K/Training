
using Microsoft.EntityFrameworkCore;
using Twitter_Clone.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<TwitterCloneDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();


/*

----Twitter clone----

-- Login
    Id, username, password, userId

-- User
    Id, Name, email, phone, dob

-- Posts
    Id, Post title, description, userId, createdAt

-- Likes
    Id, PostId, UserId, CreatedAt

-- Hashtags
    Id, Tag

-- PostHashtags
    Id, PostId, HashTagId

-- Comments
    Id, userId, postId, Content, createdAt

-- Follow
    id, FollowerId, FollowingId, createdAt

*/