﻿using AutoMapper;
using GymSystemBLL.ViewModels.SessionsViewModel;
using GymSystemBLL.ViewModels.SessionViewModels;
using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL
{
    public class MappingProfiles:Profile
    {
        //Profile must be CTOR
        public MappingProfiles()
        {

            CreateMap<Session, SessionViewModel>()
                .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.SessionCategory.CategoryName))
                .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.SessionTrainer.Name))
                .ForMember(dest => dest.AvailableSlot, Options => Options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();
            CreateMap< UpdateSessionViewModel,Session>().ReverseMap();

        }
    }
}
