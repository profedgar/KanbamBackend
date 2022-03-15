using System;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
