using System;
using System.Collections.Generic;
namespace forum.DTO;
    public class UserInfoUpdateDto
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

       

        public string? Email { get; set; }

        public string? Sex { get; set; }

        public int? Phone { get; set; }

        
    }

