using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace cattoapi.Models;

public partial class CattoDbContext : DbContext
{
    public CattoDbContext()
    {
    }

    public CattoDbContext(DbContextOptions<CattoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<LikedPost> LikedPosts { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\apic\\cattoapi_Copy\\cattoapi_DB\\cattoDB.mdf;Integrated Security=True;Connect Timeout=30;Encrypt=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Accounts__F267253E0923382B");

            entity.HasIndex(e => e.UserName, "UQ__Accounts__66DCF95C88B7E2C9").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Accounts__AB6E6164B4DC7439").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Pfp).HasColumnName("pfp");
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .HasColumnName("role");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .HasColumnName("userName");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__CDDE91BD3C86D971");

            entity.Property(e => e.CommentId).HasColumnName("commentID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.CommentText)
                .HasMaxLength(255)
                .HasColumnName("commentText");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.PostId).HasColumnName("postID");

            entity.HasOne(d => d.Account).WithMany(p => p.Comments)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_comments_accountID");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_comments_postID");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => new { e.Participant1Id, e.Participant2Id }).HasName("PK__Conversa__7C56DCBB4DB7F8AC");

            entity.Property(e => e.Participant1Id).HasColumnName("Participant1ID");
            entity.Property(e => e.Participant2Id).HasColumnName("Participant2ID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Participant1).WithMany(p => p.ConversationParticipant1s)
                .HasForeignKey(d => d.Participant1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Conversat__Parti__2DE6D218");

            entity.HasOne(d => d.Participant2).WithMany(p => p.ConversationParticipant2s)
                .HasForeignKey(d => d.Participant2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Conversat__Parti__2EDAF651");
        });

        modelBuilder.Entity<LikedPost>(entity =>
        {
            entity.HasKey(e => new { e.AccountId, e.PostId }).HasName("PK__Likedpos__4FB7E205A8DF8E01");

            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.PostId).HasColumnName("postID");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");

            entity.HasOne(d => d.Account).WithMany(p => p.LikedPosts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_likedposts_accountID");

            entity.HasOne(d => d.Post).WithMany(p => p.LikedPosts)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK_likedposts_postID");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.Participant1Id).HasColumnName("Participant1ID");
            entity.Property(e => e.Participant2Id).HasColumnName("Participant2ID");
            entity.Property(e => e.SenderId).HasColumnName("SenderID");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Sender).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderId)
                .HasConstraintName("FK__Messages__Sender__40F9A68C");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages)
                .HasForeignKey(d => new { d.Participant1Id, d.Participant2Id })
                .HasConstraintName("FK__Messages__40058253");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__posts__DD0C73BAC72F2DD8");

            entity.Property(e => e.PostId).HasColumnName("postID");
            entity.Property(e => e.AccountId).HasColumnName("accountID");
            entity.Property(e => e.CommentsCount).HasColumnName("commentsCount");
            entity.Property(e => e.DateCreated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("dateCreated");
            entity.Property(e => e.LikesCount).HasColumnName("likesCount");
            entity.Property(e => e.PostPictrue).HasColumnName("postPictrue");
            entity.Property(e => e.PostText)
                .HasMaxLength(500)
                .HasColumnName("postText");

            entity.HasOne(d => d.Account).WithMany(p => p.Posts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("posts_accountID_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
