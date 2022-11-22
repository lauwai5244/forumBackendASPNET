using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace forum.Models;

public partial class ForumContext : DbContext
{
    public ForumContext()
    {
    }

    public ForumContext(DbContextOptions<ForumContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Article> Articles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server= forumnet.database.windows.net,1433;Initial Catalog= forum;Persist Security Info=False;User ID= lauuai5244;Password= Aa51291112;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Article>(entity =>
        {
            entity.ToTable("article");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("date")
                .HasColumnName("creationTime");
            entity.Property(e => e.CreationUserId).HasColumnName("creationUserId");
            entity.Property(e => e.Text)
                .HasColumnType("ntext")
                .HasColumnName("text");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreationTime)
                .HasColumnType("date")
                .HasColumnName("creationTime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Salt)
                .HasMaxLength(50)
                .HasColumnName("salt");
            entity.Property(e => e.Sex)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("sex");
            entity.Property(e => e.UserImage)
                .HasColumnType("text")
                .HasColumnName("userImage");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
