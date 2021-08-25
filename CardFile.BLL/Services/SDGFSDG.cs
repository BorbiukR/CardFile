using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardFile.BLL.Services
{
    public class SDGFSDG
    {
        //public FileResult Download(string path, string fileName)
        //{
        //    byte[] fileBytes = System.IO.File.ReadAllBytes(path);
        //    var response = new FileContentResult(fileBytes, "application/octet-stream");
        //    response.FileDownloadName = fileName;
        //    return response;
        //}



        //public ActionResult UploadFile(int id, PostedFileViewModel model)
        //{
        //    string userId = User.Identity.GetUserId();

        //    string path = Server.MapPath("~/Uploads/");
        //    var mapperUploads = new MapperConfiguration(cfg => cfg.CreateMap<PostedFileViewModel, CardFileUploadDTO>()
        //    .ForMember(dest => dest.CardType, opt => opt.MapFrom(src => src.FileAttach.ContentType))
        //    .ForMember(dest => dest.Data, opt => opt.MapFrom(src => path + Path.GetFileName(src.FileAttach.FileName)))
        //    .ForMember(dest => dest.CardFileId, opt => opt.MapFrom(src => id))
        //    .ForMember(dest => dest.FileName, opt => opt.MapFrom(src => src.FileAttach.FileName)))
        //     .CreateMapper();

        //    var cardFileUploads = mapperUploads.Map<PostedFileViewModel, CardFileUploadDTO>(model);
        //    cardFileUploadService.CreateNew(cardFileUploads);

        //    if (model.FileAttach != null)
        //    {
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }

        //        model.FileAttach.SaveAs(path + Path.GetFileName(model.FileAttach.FileName));
        //    }
        //    model.Message = "'" + model.FileAttach.FileName + "' file has been successfuly!! uploaded";
        //    model.IsValid = true;

        //    return this.View(model);

        //}
    }
}
