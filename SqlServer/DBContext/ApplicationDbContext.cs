using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreEntities.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace SqlServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //public DbSet<User> Users { get; set; }
        public DbSet<TaskFL> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TaskTag> TaskTags { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Space> Spaces { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
        public DbSet<UserFL> UserFLs { get; set; }
        public DbSet<TaskAssignee> TaskAssignees { get; set; }
        public DbSet<GoalFL> Goals { get; set; }
        public DbSet<ChecklistFL> Checklists { get; set; }
        public DbSet<ChecklistItemFL> ChecklistItems { get; set; }
        public DbSet<TimeEntryFL> TimeEntries { get; set; }
        public DbSet<CustomFieldFL> CustomFields { get; set; }
        public DbSet<TaskCustomFieldValueFL> TaskCustomFieldValues { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; } // Lưu ý tên DbSet
        public DbSet<AIAction> AIActions { get; set; }
        public DbSet<KnowledgeChunk> KnowledgeChunks { get; set; }
        public DbSet<ConversationSummary> ConversationSummaries { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ActivityLog> activityLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Đảm bảo Task có khóa chính
            modelBuilder.Entity<TaskFL>()
                .HasKey(t => t.Id);
            modelBuilder.Entity<TaskTag>()
                .HasKey(tt => new { tt.TaskId, tt.TagId });


            // --- Cấu hình cho bảng Comments ---
            modelBuilder.Entity<TaskFL>()
            .HasKey(t => t.Id);

            modelBuilder.Entity<TaskFL>()
                .HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TaskAssignee>(entity =>
            {
                // 1. Chỉ định khóa chính (nếu là khóa phức hợp gồm TaskId và UserId)
                entity.HasKey(e => new { e.TaskId, e.UserId });

                // 2. Cấu hình mối quan hệ với Task
                entity.HasOne(d => d.Tasks)       // Biến điều hướng trong TaskAssignee là TaskFL
                      .WithMany(p => p.TaskAssignees)  // Biến danh sách trong Task là Assignees
                      .HasForeignKey(d => d.TaskId) // <--- QUAN TRỌNG NHẤT: Bắt buộc dùng cột TaskId
                      .OnDelete(DeleteBehavior.Cascade); // (Tùy chọn) Xóa Task thì xóa luôn phân công

                // 3. Cấu hình mối quan hệ với User (nếu cần)
                // entity.HasOne(d => d.UserFLs)...
            });


            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.TaskId);

            modelBuilder.Entity<TaskFL>()
                .HasOne(t => t.List)
                .WithMany(l => l.Tasks)
                .HasForeignKey(t => t.ListId);

            modelBuilder.Entity<TeamMember>()
                .HasKey(tm => new { tm.TeamId, tm.UserId });

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Teams)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tm => tm.TeamId);

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.UserFLs)
                .WithMany(u => u.TeamMembers)
                .HasForeignKey(tm => tm.UserId);

            modelBuilder.Entity<TaskAssignee>()
                .HasKey(a => new { a.TaskId, a.UserId });

            modelBuilder.Entity<TaskAssignee>()
                .HasOne(a => a.Tasks)
                .WithMany(t => t.TaskAssignees)
                .HasForeignKey(a => a.TaskId);

            modelBuilder.Entity<TaskAssignee>()
                .HasOne(a => a.UserFLs)
                .WithMany(u => u.TaskAssignees)
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<TaskAssignee>()
                .HasOne(ta => ta.Tasks)       // Một Assignee thuộc về 1 TaskFL
                .WithMany(t => t.TaskAssignees)    // Một Task có nhiều Assignees
                .HasForeignKey(ta => ta.TaskId); // <--- CHỐT: Khóa ngoại là TaskId

            modelBuilder.Entity<GoalFL>()
                .HasOne(g => g.Teams)
                .WithMany(t => t.Goals)
                .HasForeignKey(g => g.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChecklistFL>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Checklists)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChecklistItemFL>()
                .HasOne(i => i.Checklist)
                .WithMany(c => c.ChecklistItems)
                .HasForeignKey(i => i.ChecklistId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ChecklistItemFL>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.ResolvedBy)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TimeEntryFL>()
                .HasOne(t => t.Task)
                .WithMany()
                .HasForeignKey(t => t.TaskId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TimeEntryFL>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<TaskCustomFieldValueFL>()
                .HasKey(x => new { x.TaskId, x.FieldId });

            modelBuilder.Entity<TaskCustomFieldValueFL>()
                .HasOne(v => v.Task)
                .WithMany(t => t.CustomFieldValues)
                .HasForeignKey(v => v.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskCustomFieldValueFL>()
                .HasOne(v => v.CustomField)
                .WithMany(f => f.Values)
                .HasForeignKey(v => v.FieldId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Teams)
                .WithMany(t => t.TeamMembers)
                .HasForeignKey(tm => tm.TeamId)
                .OnDelete(DeleteBehavior.Cascade); // <--- QUAN TRỌNG

            // 2. Cấu hình Team -> Spaces (Xóa Team là xóa sạch Space)
            modelBuilder.Entity<Space>()
                .HasOne(s => s.Teams)
                .WithMany(t => t.Spaces)
                .HasForeignKey(s => s.TeamId)
                .OnDelete(DeleteBehavior.Cascade); // <--- QUAN TRỌNG

            modelBuilder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa hội thoại thì xóa luôn tin nhắn

            // Config relationship Message -> AIActions (1-1 hoặc 1-n tùy logic, ở đây để 1-n cho chắc)
            modelBuilder.Entity<ChatMessage>()
                .HasMany<AIAction>()
                .WithOne(a => a.Message)
                .HasForeignKey(a => a.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConversationSummary>()
                .HasKey(x => x.ConversationId);

            modelBuilder.Entity<ConversationSummary>()
                .ToTable("ConversationSummaries");
        }
    }
}