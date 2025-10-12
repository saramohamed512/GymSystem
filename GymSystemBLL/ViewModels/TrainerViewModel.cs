﻿using GymSystemDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string? DateOfBirth { get; set; }

        public string Gender { get; set; } = null!;
        public string? Address { get; set; }
        public Specialites Specialites { get; set; }
    }
}
