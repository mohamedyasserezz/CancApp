﻿using CanaApp.Domain.Entities.Models;

namespace CanaApp.Domain.Entities.Comunity
{
    public class Post
    {
        public int Id { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        public string Content { get; set; } = default!;

        // Foreign keys and navigation properties
        public ApplicationUser User { get; set; } = default!;

        public string? Image { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public bool IsReported { get; set; }

        public ICollection<Reaction> Reactions = new List<Reaction>();
        public ICollection<Comment> Comments = new List<Comment>();
    }
}
