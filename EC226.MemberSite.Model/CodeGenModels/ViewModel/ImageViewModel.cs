using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace EC226.MemberSite.Model.CodeGenModels.Models.ViewModel
{
    public class UploadImagesFormBody {
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
        public string Key { get; set; }
    }

    public class DeleteImagesReqBody {
        public string Key {  set; get; }
        public string[] Filenames { get; set; } = new string[0];
    }

    public class DeleteImagesResponseData {
        public string KeyFolderName { get; set; }
        public List<string> DeletedFiles { get; set; } = new List<string>();
        public List<string> NonexistentFiles { get; set; } = new List<string>();
    }
}
