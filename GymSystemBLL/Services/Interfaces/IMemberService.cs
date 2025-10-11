using GymSystemBLL.ViewModels;
using GymSystemDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    internal interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMembers(CreateMemberViewModel CreateMemberViewModel);

        MemberViewModel? GetMemberDetails(int id);

        //Get Health Record
        HealthViewModel GetMemberHealthRecordDetails(int MemberId);

        //GetMemberId To Update view
        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);
        //apply update
        bool UpdateMemberDetails(int id, MemberToUpdateViewModel updateMember);

        bool RemoveMember(int id);
    }
}
