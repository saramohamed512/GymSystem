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
        private readonly IGenericRepository<HealthRecord> _healthRecordRepository;

        public MemberService
            (IGenericRepository<Member> memberRepository,
            IGenericRepository<Membership> membershipRepository,
            IPlanRepository planRepository,
            IGenericRepository<HealthRecord> healthRecordRepository

            )
        {
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
            _planRepository = planRepository;
            _healthRecordRepository = healthRecordRepository;
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
                if (IsEmailExist(createMember.Email) || IsPhoneExist(createMember.Phone))
                    return false;
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

    

        public HealthViewModel GetMemberHealthRecordDetails(int MemberId)
        {
            var MemeberHealthRecord = _healthRecordRepository.GetById(MemberId);
            if (MemeberHealthRecord is null) return null;
            var healthViewModel = new HealthViewModel()
            {
                Weight = MemeberHealthRecord.Weight,
                Height = MemeberHealthRecord.Height,
                BloodType = MemeberHealthRecord.BloodType,
                Note = MemeberHealthRecord.Note,
            };
            return healthViewModel;
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var Member = _memberRepository.GetById(MemberId);
            if (Member is null) return null;
            return new MemberToUpdateViewModel
            {
                Name = Member.Name,
                Photo = Member.Photo,
                Email = Member.Email,
                Phone = Member.Phone,
                BuildingNumber = Member.Address.BuildingNumber,
                Street = Member.Address.Street,
                City = Member.Address.City,
            };
        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel updateMember)
        {

            try
            {

                if (IsEmailExist(updateMember.Email) || IsPhoneExist(updateMember.Phone))
                    return false;
                var Member = _memberRepository.GetById(id);
                if (Member is null) return false;

                Member.Email = updateMember.Email;
                Member.Phone = updateMember.Phone;
                Member.Photo = updateMember.Photo;
                Member.Address.BuildingNumber = updateMember.BuildingNumber;
                Member.Address.Street = updateMember.Street;
                Member.Address.City = updateMember.City;
                Member.UpdatedAt = DateTime.Now;
                return _memberRepository.Update(Member) > 0;

            }
            catch (Exception)
            {

                return false;
            }


        }
        #region Helper Methods
        private bool IsEmailExist(string email)
        {
           return _memberRepository.GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _memberRepository.GetAll(m => m.Phone == phone).Any();
        }
        #endregion
    }
}
