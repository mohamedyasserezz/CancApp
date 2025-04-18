﻿using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    public class Psychiatrist
    {
        public string? MedicalSyndicatePhoto { get; set; } 
        public string? ImageId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public bool IsDisabled { get; set; }
        public bool IsCompletedProfileFailed { get; set; }
        public int NumberOfWarrings { get; set; }
        public bool IsConfirmedByAdmin { get; set; }

        // Community
        public ICollection<Post> Posts = new List<Post>();
        public ICollection<Reaction> Reactions = new List<Reaction>();
        public ICollection<Comment> Comments = new List<Comment>();
    }
}
