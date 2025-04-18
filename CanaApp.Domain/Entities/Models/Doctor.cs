﻿using CanaApp.Domain.Entities.Comunity;

namespace CanaApp.Domain.Entities.Models
{
    public class Doctor
    {
        public string? MedicalSyndicatePhoto { get; set; } 
        public string? ImageId { get; set; } 
        public ApplicationUser ApplicationUser { get; set; } = default!;
        public bool IsConfirmedByAdmin { get; set; }
        public bool IsDisabled { get; set; }
        public int NumberOfWarrings { get; set; }
        public string UserId { get; set; } = default!;
        public bool IsCompletedProfileFailed { get; set; }

        // Community
        public ICollection<Post> Posts = new List<Post>();
        public ICollection<Reaction> Reactions = new List<Reaction>();
        public ICollection<Comment> Comments = new List<Comment>(); 
    }
}
