using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.AttachmentService
{
    public interface IAttachmentService
    {

        //Fun To Upload Photo and return the photo name 
        string? Upload(string folderName, IFormFile file);
        //Fun To Delete Photo
        bool Delete(string fileName, string folderName);
    }
}
