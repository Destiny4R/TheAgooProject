using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Reflection;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;
using TheAgooProjectDataAccess;
using System.Security.Claims;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Authorization;

namespace TheAgooProjectWeb.Pages.Admin.Admission
{
    [Authorize]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty]
        public StudentDataVM UserVM { get; set; }
        public IEnumerable<SelectListItem>? SessionYearz { get; set; }
        public UpsertModel(ApplicationDbContext dbContext,
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public void OnGet(int id)
        {
            sessionyear();
            if (id > 0)
            {
                UserVM = dbContext.StudentTable.Where(k => k.Id == id).Select(k => new StudentDataVM
                {
                    Id = k.Id,
                    FirstName = k.FirstName,
                    OtherName = k.OtherName,
                    SurName = k.SurName,
                    Gender = k.Gender,
                    State = k.State,
                    LocalGov = k.LocalGov,
                    Address = k.Address,
                    StudentRegNo = k.StudentRegNo,
                    SessionYearId = k.SessionYearId,
                    DateOfBirth = k.DateOfBirth
                }).FirstOrDefault();
            }
            else
            {
                UserVM = new StudentDataVM();
            }
        }
        public async Task<IActionResult> OnPost(IFormFile? passport)
        {
            sessionyear();
            if (ModelState.IsValid)
            {
                    if (passport != null)
                    {
                        string dataFileName = System.IO.Path.GetFileName(passport.FileName);

                        string extension = System.IO.Path.GetExtension(dataFileName.ToLower());

                        string[] allowedExtsnions = new string[] { ".png", ".bitmap", ".jpg", ".jpeg" };

                        if (!allowedExtsnions.Contains(extension))

                        {
                            TempData["error"] = "Sorry! the uploaded picture file is not in a picture or image format, only extension with png, jpg, jpeg, bmp, gif are allowed. Check and try again";
                            return Page();
                        }
                    }
                    string outputPath = @"\image\Passports\";
                var ClaimsId = (ClaimsIdentity)User.Identity;
                var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
                if (UserVM.Id > 0)
                {
                    var student = dbContext.StudentTable.FirstOrDefault(k => k.Id == UserVM.Id);
                    if (student == null)
                    {
                        TempData["error"] = "We couldn't identify this student";
                        return RedirectToPage("Index");
                    }
                    student.FirstName = UserVM.FirstName;
                    student.OtherName = UserVM.OtherName;
                    student.SurName = UserVM.SurName;
                    student.Gender = UserVM.Gender;
                    student.State = UserVM.State;
                    student.LocalGov = UserVM.LocalGov;
                    student.Address = UserVM.Address;
                    student.StudentRegNo = UserVM.StudentRegNo;
                    student.SessionYearId = UserVM.SessionYearId;
                    student.DateOfBirth = UserVM.DateOfBirth;
                    if (passport != null)
                    {
                        student.Passport = await CompressAndSaveImageAsync(passport, outputPath, 180, student.Passport);//GetFilePathAndCopy(passport, student.Passport);
                    }
                    dbContext.Update(student);
                    dbContext.SaveChanges();
                    TempData["success"] = "Student data successfully updated!";
                    return RedirectToPage("Index");
                }
                else
                {
                    var stu_list = dbContext.StudentTable.Count();
                    string username = "";
                    int sub = DateTime.Now.Year;
                    if (stu_list > 0)
                    {
                        username = $"AGG/SM/{sub}/"+(100+stu_list);
                    }
                    else
                    {
                        username = $"AGG/SM/{sub}/100";
                    }
                    var getnewuser = await userManager.FindByNameAsync(username);
                    if (getnewuser != null)
                    {
						username = $"AGG/SM/{sub}/" + (100 + stu_list+1);
					}
                    var user = new ApplicationUser
                    {
                        UserName = username,
                        Email = username,
                        Fullname = UserVM.SurName + " " + UserVM.OtherName + " " + UserVM.FirstName,
                        Position = SD.IsStudent
                    };
                    var result = await userManager.CreateAsync(user, UserVM.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, SD.IsStudent);
                        var student = new StudentsData();
                        student.FirstName = UserVM.FirstName;
                        student.OtherName = UserVM.OtherName;
                        student.SurName = UserVM.SurName;
                        student.Gender = UserVM.Gender;
                        student.State = UserVM.State;
                        student.LocalGov = UserVM.LocalGov;
                        student.Address = UserVM.Address;
                        student.StudentRegNo = username;
                        student.SessionYearId = UserVM.SessionYearId;
                        student.DateOfBirth = UserVM.DateOfBirth;
                        student.ApplicationUserId = user.Id;
                        if (passport != null)
                        {
                            student.Passport = await CompressAndSaveImageAsync(passport, outputPath,180, ""); //GetFilePathAndCopy(passport, string.Empty);
                        }
                        student.UserID = claim.Value;
                        dbContext.Add(student);
                        dbContext.SaveChanges();
                        TempData["success"] = "Student successfully registered!";
                        return RedirectToPage("Index");
                    }
                }
                TempData["error"] = "An error occured while registering student check and try again";
                return Page();
            }
            TempData["error"] = "An error occured while registering student check and try again";
            return Page();
        }
        void sessionyear()
        {
            SessionYearz = dbContext.SessionYears.Select(k => new SelectListItem
            {
                Text = k.Name,
                Value = k.Id.ToString()
            });
        }
        public string GetFilePathAndCopy(IFormFile file, string imagePath)
        {
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString();
                var upload = System.IO.Path.Combine(webHostEnvironment.WebRootPath, @"image\Passports\");
                var extention = System.IO.Path.GetExtension(file.FileName);
                if (imagePath != null)
                {
                    string deleteOldImage = webHostEnvironment.WebRootPath+"/"+imagePath;
                    if (System.IO.File.Exists(deleteOldImage))
                    {
                        System.IO.File.Delete(deleteOldImage);
                    }
                }
                using (var fileStream = new FileStream(System.IO.Path.Combine(upload, fileName + extention), System.IO.FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                return @"\image\Passports\" + fileName + extention;
            }
            return string.Empty;
        }
        public async Task<string> CompressAndSaveImageAsync(IFormFile imageFile, string outputPath, int targetSizeKB = 120, string? oldImagePath=null)
        {

            if (!string.IsNullOrEmpty(oldImagePath))
            {
                string deleteOldImage = System.IO.Path.Combine(webHostEnvironment.WebRootPath, oldImagePath);
                if (System.IO.File.Exists(deleteOldImage))
                {
                    System.IO.File.Delete(deleteOldImage);
                }
            }


            using var imageStream = imageFile.OpenReadStream();
            using var image = await Image.LoadAsync(imageStream);
            string fileName = Guid.NewGuid().ToString();
            var exten = System.IO.Path.GetExtension(imageFile.FileName);
            var upload = webHostEnvironment.WebRootPath+"/"+outputPath;
            // Check the initial file size
            if (imageFile.Length / 1024 <= targetSizeKB)
            {
                // Save the image directly if it's already less than the target size
                var filePath = System.IO.Path.Combine(upload, fileName + exten);
                using var fileStream = new FileStream(filePath, System.IO.FileMode.Create);
                await imageFile.CopyToAsync(fileStream);
                return outputPath + fileName + exten;
            }
            var resizeOptions = new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(image.Width / 2, image.Height / 2) // Adjust the size as needed
            };
            image.Mutate(x => x.Resize(resizeOptions));

            var encoder = new JpegEncoder();

            // Compress the image until it meets the target size
            int quality = 90;
            long fileSize;
            do
            {
                using var memoryStream = new MemoryStream();
                await image.SaveAsJpegAsync(memoryStream, new JpegEncoder { Quality = quality });
                fileSize = memoryStream.Length / 1024; // Size in KB
                quality -= 5; // Reduce quality incrementally
            } while (fileSize > targetSizeKB && quality > 2);
            quality = quality < 1 ? 1 : quality;
            // Save the compressed image to the specified output path
            var filePath1 = System.IO.Path.Combine(upload, fileName + exten);
            var compressedFilePath = System.IO.Path.Combine(outputPath, fileName + exten);
            await image.SaveAsJpegAsync(filePath1, new JpegEncoder { Quality = quality });

            return outputPath+fileName+exten;
        }
    }
}
