using GymSystemBLL.Services.Interfaces;
using GymSystemBLL.ViewModels;
using GymSystemDAL.Entities;
using GymSystemDAL.Repositroies.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Classes
{
    internal class MemberService : IMemberService
    {
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<Membership> _membershipRepository;
        private readonly IPlanRepository _planRepository;

        public MemberService
            (IGenericRepository<Member> memberRepository,
            IGenericRepository<Membership> membershipRepository,
            IPlanRepository planRepository

            )
        {
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
           _planRepository = planRepository;
        }
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            #region First Way Of Mapping
            ////var members = _memberRepository.GetAll() ?? [];
            //var members = _memberRepository.GetAll();
            //if (members is null || members.Any()) return [];
            //var memberViewModels = new List<MemberViewModel>();
            //foreach (var member in members)
            //{
            //    var memberViewModel = new MemberViewModel()
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Email = member.Email,
            //        Phone = member.Phone,
            //        Gender = member.Gender.ToString(),
            //    };
            //    memberViewModels.Add(memberViewModel);
            //}
            //return memberViewModels;
            #endregion

            var members = _memberRepository.GetAll();
            if (members is null || !members.Any()) return [];
            var memberViewModels = members.Select(member => new MemberViewModel
            {
                Id = member.Id,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),

            });
            return memberViewModels;
        }

        public bool CreateMembers(CreateMemberViewModel createMember)
        {
            //Check if Email or Phone are unique
            try
            {
                var EmailExists = _memberRepository.GetAll(m => m.Email == createMember.Email).Any();
                var PhoneExists = _memberRepository.GetAll(m => m.Phone == createMember.Phone).Any();

                if (EmailExists || PhoneExists) return false;

                var member = new Member
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    Gender = createMember.Gender,
                    DateOfBirth = createMember.DateOfBirth,
                    Address = new Address
                    {
                        BuildingNumber = createMember.BuildingNumber,
                        Street = createMember.Street,
                        City = createMember.City,
                    },
                    HealthRecord = new HealthRecord
                    {
                        Height = createMember.HealthViewModel.Height,
                        Weight = createMember.HealthViewModel.Weight,
                        BloodType = createMember.HealthViewModel.BloodType,
                        Note = createMember.HealthViewModel.Note,
                    }

                };
                return _memberRepository.Add(member) > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public MemberViewModel? GetMemberDetails(int id)
        {
            //IPlanRepository 
            //Inject PlanRepository & MembershipRepository

            var Member = _memberRepository.GetById(id);
            if (Member is null) return null;
            var ViewModel = new MemberViewModel
            {
                Name = Member.Name,
                Email = Member.Email,
                Phone = Member.Phone,
                Photo = Member.Photo,
                Gender = Member.Gender.ToString(),
                DateOfBirth = Member.DateOfBirth.ToShortDateString(),
                Address = $"{Member.Address?.BuildingNumber} - {Member.Address?.Street} - {Member.Address?.City}"

            };
            var ActiveMembership = _membershipRepository.GetAll(m => m.MemberId == id && m.Status == "Active").FirstOrDefault();

            if (ActiveMembership is  not null) //StartDate , EndDate , PlanName
            {
                ViewModel.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                ViewModel.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();

                //plans
                var plan = _planRepository.GetById(ActiveMembership.PlanId);
                ViewModel.PlanNmae = plan?.Name;
            }
            return ViewModel;
        }
    }
}
