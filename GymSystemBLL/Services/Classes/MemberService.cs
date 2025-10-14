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
        private readonly IUnitOfWork _unitOfWork;
        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //Don't forget to register IUnitOfWork  Program.cs !!!!!!!

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

            var members = _unitOfWork.GetRepository<Member>().GetAll();
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
                 _unitOfWork.GetRepository<Member>().Add(member) ;
                return _unitOfWork.SaveChanges() > 0;
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

            var Member = _unitOfWork.GetRepository<Member>().GetById(id);
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
            var ActiveMembership = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == id && m.Status == "Active").FirstOrDefault();

            if (ActiveMembership is  not null) //StartDate , EndDate , PlanName
            {
                ViewModel.MembershipStartDate = ActiveMembership.CreatedAt.ToShortDateString();
                ViewModel.MembershipEndDate = ActiveMembership.EndDate.ToShortDateString();

                //plans
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMembership.PlanId);
                ViewModel.PlanNmae = plan?.Name;
            }
            return ViewModel;
        }

    

        public HealthViewModel GetMemberHealthRecordDetails(int MemberId)
        {
            var MemeberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
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
            var Member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
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
                var MemberRepo= _unitOfWork.GetRepository<Member>();

                if (IsEmailExist(updateMember.Email) || IsPhoneExist(updateMember.Phone))
                    return false;
                var Member = MemberRepo.GetById(id);
                if (Member is null) return false;

                Member.Email = updateMember.Email;
                Member.Phone = updateMember.Phone;
                Member.Photo = updateMember.Photo;
                Member.Address.BuildingNumber = updateMember.BuildingNumber;
                Member.Address.Street = updateMember.Street;
                Member.Address.City = updateMember.City;
                Member.UpdatedAt = DateTime.Now;
                MemberRepo.Update(Member);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {

                return false;
            }


        }
        public bool RemoveMember(int id)
        {
            var MemberRepo = _unitOfWork.GetRepository<Member>();
            var MembershipRepo = _unitOfWork.GetRepository<Membership>();
            var MemberSessionRepo = _unitOfWork.GetRepository<MemberSession>();
            var member = MemberRepo.GetById(id);
            if (member is null) return false;

            //check if member has active member sessions
            var HasActiveMemberSessions = MemberSessionRepo.GetAll(m => m.MemberId == id && m.Session.StartDate >DateTime.Now).Any();

            if (HasActiveMemberSessions) return false;

            //Handel to Cascade Delete for Memberships
            var memberMemberships = MembershipRepo.GetAll(m => m.MemberId == id);
            try
            {
                if(memberMemberships.Any()){
                    foreach (var membership in memberMemberships)
                    {
                        MembershipRepo.Delete(membership);
                    }
                }
                MemberRepo.Delete(member);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch (Exception)
            {

                return false;
            }
        }
        #region Helper Methods
        private bool IsEmailExist(string email)
        {
           return _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        }
        private bool IsPhoneExist(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == phone).Any();
        }

       
        #endregion
    }
}
