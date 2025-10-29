using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreEntities.Model
{
    public class TaskFL
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("projectid")]
        public int ProjectId { get; set; }
        [Column("title")]
        public string? Title { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("AssignedTo")]
        public int? AssignedTo { get; set; }
        [Column("createdby")]
        public int? CreatedBy { get; set; }
        [Column("status")]
        public string? Status { get; set; }
        [Column("priority")]
        public string? Priority { get; set; }
        [Column("startdate")]
        public DateTime? StartDate { get; set; }
        [Column("duedate")]
        public DateTime? DueDate { get; set; }
        [Column("progress")]
        public int? Progress { get; set; }
        [Column("createdat")]
        public DateTime? CreatedAt { get; set; }
        [Column("updatedat")]
        public DateTime? UpdatedAt { get; set; }
    }
}
