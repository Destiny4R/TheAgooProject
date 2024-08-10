using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheAgooProjectDataAccess;
using TheAgooProjectModel;
using TheAgooProjectDataAccess.Data;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using TheAgooProjectModel.ViewModels;
using System.Reflection;

namespace TheAgooProjectWeb.Controllers
{
    public class apiController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public apiController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        public class Input
        {
            public string term { get; set; }
            public int session { get; set; }
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<JsonResult> LockUnlockAccount(string id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "Data not found" });
            }
            var getSchool = await userManager.FindByIdAsync(id);
            if (getSchool != null)
            {
                if (await userManager.IsInRoleAsync(getSchool, SD.Role_Admin))
                {
                    return Json(new { success = false, message = "Admin Account Can't Be Locked!" });
                }
                string msg = "";
                if (getSchool.Active)
                {
                    getSchool.Active = false;
                    msg = "Account Successfully Lock From Accessing The Website!";
                }
                else
                {
                    getSchool.Active = true;
                    msg = "Account Successfully Unlock to Use The System!";
                }
                try
                {
                    var result = await userManager.UpdateAsync(getSchool);
                    if (result.Succeeded)
                    {
                        return Json(new { success = true, message = msg });
                    }
                    return Json(new { success = false, message = "An error occurred while processing your request" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "An error occurred while processing your request "+ex.Message });
                }
            }
            return Json(new { success = false, message = "Data not found" });
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpDelete]
        public async Task<JsonResult> StudentDelete(string id)
        {
            var termreg = dbContext.TermRegistrations.Include(k=>k.StudentsData.ApplicationUser).Where(k => k.StudentsData.ApplicationUserId == id).Count();
            if (termreg > 0)
            {
                return Json(new { success = false, message = "This is student is registered for various terms, remove such before proceeding" });
            }
            var obj = await userManager.FindByIdAsync(id);
            if (obj == null)
            {
                return Json(new { success = false, message = "Error while delete" });
            }
            var student = dbContext.StudentTable.FirstOrDefault(k => k.ApplicationUserId == id);
            if (student == null)
            {
                return Json(new { success = false, message = "Unidentify student" });
            }
            
            string deleteOldImage = webHostEnvironment.WebRootPath + student.Passport;
            if (System.IO.File.Exists(deleteOldImage))
            {
                System.IO.File.Delete(deleteOldImage);
            }

            var dBRole = userManager.GetRolesAsync(obj).Result.FirstOrDefault();
            if (dBRole != null)
            {
                await userManager.RemoveFromRoleAsync(obj, dBRole);
            }
            var result = await userManager.DeleteAsync(obj);
            if (result.Succeeded)
            {
                return Json(new { success = true, message = $"Student Account Successfully Deleted" });
            }

            return Json(new { success = false, message = "Something went wrong" });
        }

        [HttpDelete]
        public IActionResult sessionmanager(int id)
        {
            var getinfo2 = dbContext.StudentTable.Where(j => j.SessionYearId == id);
            if (getinfo2.Any())
            {
                return Json(new { success = false, message = "Admission is given under this session/year therefore it can't be deleted" });
            }
            var getinfo1 = dbContext.TermRegistrations.Where(j => j.SessionYearId == id);
            if (getinfo1.Any())
            {
                return Json(new { success = false, message = "Students are registered in the term under this session/year, therefore it can't be deleted" });
            }
            //var getinfo4 = dbContext.TermlyFees.Where(j => j.SessionYearId == id);
            //if (getinfo4.Any())
            //{
            //    return Json(new { success = false, message = "Termly Fees is asigned to this session/year therefore it can't be deleted" });
            //}
            //var getinfo3 = dbContext.PTAFees.Where(j => j.SessionYearId == id);
            //if (getinfo3.Any())
            //{
            //    return Json(new { success = false, message = "PTA Fees is asigned to this session/year therefore it can't be deleted" });
            //}
            var session = dbContext.SessionYears.FirstOrDefault(k => k.Id == id);
            if (session == null)
            {
                return Json(new { success = false, message = "Error while deleting data" });
            }
            dbContext.SessionYears.Remove(session);
            dbContext.SaveChanges();
            return Json(new { success = true, message = "Session/Year Successfully Removed!" });
        }
        [HttpDelete]
        public IActionResult classmanager(int id)
        {
            var getinfo1 = dbContext.TermRegistrations.Where(j => j.ClassesInSchoolId == id);
            if (getinfo1.Any())
            {
                return Json(new { success = false, message = "Students are registered in the term under this Class, therefore it can't be deleted" });
            }
            //var getinfo4 = dbContext.TermlyFees.Where(j => j.SessionYearId == id);
            //if (getinfo4.Any())
            //{
            //    return Json(new { success = false, message = "Termly Fees is asigned to this session/year therefore it can't be deleted" });
            //}
            //var getinfo3 = dbContext.PTAFees.Where(j => j.SessionYearId == id);
            //if (getinfo3.Any())
            //{
            //    return Json(new { success = false, message = "PTA Fees is asigned to this session/year therefore it can't be deleted" });
            //}
            var _class = dbContext.SchoolClasses.FirstOrDefault(k => k.Id == id);
            if (_class == null)
            {
                return Json(new { success = false, message = "Error while deleting data" });
            }
            dbContext.Remove(_class);
            dbContext.SaveChanges();
            return Json(new { success = true, message = "Class Successfully Removed!" });
        }
        [HttpDelete]
        public IActionResult subclassmanager(int id)
        {
            var getinfo1 = dbContext.TermRegistrations.Where(j => j.SubClassId == id);
            if (getinfo1.Any())
            {
                return Json(new { success = false, message = "Students are registered in the term under this Sub-Class, therefore it can't be deleted" });
            }
            //var getinfo4 = dbContext.TermlyFees.Where(j => j.SessionYearId == id);
            //if (getinfo4.Any())
            //{
            //    return Json(new { success = false, message = "Termly Fees is asigned to this session/year therefore it can't be deleted" });
            //}
            //var getinfo3 = dbContext.PTAFees.Where(j => j.SessionYearId == id);
            //if (getinfo3.Any())
            //{
            //    return Json(new { success = false, message = "PTA Fees is asigned to this session/year therefore it can't be deleted" });
            //}
            var _class = dbContext.SubClasses.FirstOrDefault(k => k.Id == id);
            if (_class == null)
            {
                return Json(new { success = false, message = "Error while deleting data" });
            }
            dbContext.Remove(_class);
            dbContext.SaveChanges();
            return Json(new { success = true, message = "Sub-Class Successfully Removed!" });
        }
        //Subjectmanager
        [HttpDelete]
        public IActionResult Subjectmanager(int id)
        {
            var getinfo1 = dbContext.ResultTable.Where(j => j.SubjectId == id);
            if (getinfo1.Any())
            {
                return Json(new { success = false, message = "Students are registered in the term under this Subject, therefore it can't be deleted" });
            }
            //var getinfo4 = dbContext.TermlyFees.Where(j => j.SessionYearId == id);
            //if (getinfo4.Any())
            //{
            //    return Json(new { success = false, message = "Termly Fees is asigned to this session/year therefore it can't be deleted" });
            //}
            //var getinfo3 = dbContext.PTAFees.Where(j => j.SessionYearId == id);
            //if (getinfo3.Any())
            //{
            //    return Json(new { success = false, message = "PTA Fees is asigned to this session/year therefore it can't be deleted" });
            //}
            var _class = dbContext.Subjects.FirstOrDefault(k => k.Id == id);
            if (_class == null)
            {
                return Json(new { success = false, message = "Error while deleting data" });
            }
            dbContext.Remove(_class);
            dbContext.SaveChanges();
            return Json(new { success = true, message = "Subject Successfully Removed!" });
        }
        //ManageTermReg
        [HttpDelete]
        public JsonResult ManageTermReg(int id)
        {
            //CHECK IF HE OR SHE HAS PAID FOR PTA FEES
            //var studentPTA = dbContext.PTATables.FirstOrDefault(k => k.TermRegId == id);
            //if (studentPTA != null)
            //{
            //    return Json(new { success = false, message = "This student cannot be removed due to having paid PTA fees for this term." });
            //}
            //CHECK IF HE OR SHE HAS PAID TERM SCHOOL FEES
            //var studentFees = dbContext.Payments.FirstOrDefault(k => k.TermRegId == id);
            //if (studentFees != null)
            //{
            //    return Json(new { success = false, message = "This student cannot be removed due to having paid school fees for this term." });
            //}
            //CHECK IF HE OR HAS EXAMS RECORD
            var studentResult = dbContext.ResultTable.Where(k => k.TermRegId == id).ToList();
            if (studentResult.Count() > 0)
            {
                int ready = 0;
                foreach (var item in studentResult)
                {
                    if (item.Status)
                    {
                        ready++;
                    }
                }
                if (ready > 0)
                {
                    return Json(new { success = false, message = "This student cannot be removed due to having term result record and computed." });
                }
                dbContext.RemoveRange(studentResult);
            }
            var student = dbContext.TermRegistrations.Include(k=>k.ResultTable).Include(m=>m.StudentRatings).FirstOrDefault(k => k.Id == id);
            if (student == null)
            {
                return Json(new { success = false, message = "Error while delete" });
            }
            if (student.StudentRatings!=null)
            {
                dbContext.Remove(student.StudentRatings);
            }
            if (student.ResultTable.Count() > 0)
            {
                dbContext.RemoveRange(student.ResultTable);
            }
            dbContext.Remove(student);
            dbContext.SaveChanges();

            return Json(new { success = true, message = "Student Successfully Removed From the Term" });
        }
        //DeleteSubject
        [HttpDelete]
        public JsonResult DeleteSubject(int id)
        {
            var studentResult = dbContext.ResultTable.FirstOrDefault(k => k.Id == id);
            if (studentResult != null)
            {
                if (studentResult.Status)
                {
                    return Json(new { success = false, message = "This student cannot be removed due to having term result record." });
                }
                dbContext.Remove(studentResult);
                dbContext.SaveChanges();

                return Json(new { success = true, message = "Student subject successfully removed from the Term" });
            }
            return Json(new { success = false, message = "We could not identify this record!" });
        }
        [HttpPost]
        public async Task<IActionResult> PublistResult(Input input)
        {
            if (input.term != null && input.session > 0)
            {
                int _noOfPublished = 0, _NoOfSentSms = 0, _feesNotCompleted = 0, _resultNotAdded = 0;
                var termReg = await dbContext.TermRegistrations.Include(u => u.StudentsData).Include(c => c.Schoolclasses).Include(u => u.SessionYear).Include(h => h.SubClasses).Where(u => u.Term == input.term && u.SessionYearId == input.session && u.ResultIsReady == false).ToListAsync();
                if (termReg.Count() == 0)
                {
                    return Json(new { success = false, message = "No Result available for the selected Term and Session or Result already published!" });
                }
                var settings = new Settings();
                var usersInRole = await userManager.GetUsersInRoleAsync(SD.Role_Admin);
                if (usersInRole != null)
                {
                    string id = "";
                    foreach (var user in usersInRole)
                    {
                        id = user.Id;
                        break;
                    }
                    settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == id);
                }
                //var payment = new Payment();
                foreach (var item in termReg)
                {
                    //Publishing Result Base On Students with Completed Fees Payment
                    //if (settings != null)
                    //{
                    //    if (settings.CanPrintResult)
                    //    {
                    //        payment = await dbContext.Payments.AsNoTracking().FirstOrDefaultAsync(u => u.TermRegId == item.Id);
                    //    }
                    //    else
                    //    {
                    //        payment = await dbContext.Payments.AsNoTracking().FirstOrDefaultAsync(u => u.Status == SD.Fees_Status_Completed && u.TermRegId == item.Id);
                    //    }
                    //}

                    //if (payment != null)
                    //{
                    //GET PARENT PHONE NUMBER
                    //var getnumber = await dbContext.ParentTablez.FirstOrDefaultAsync(u => u.StudentRegNo == item.ApplicationUser.StudentRegNo);
                    //if (getnumber != null)
                    //{
                    if (!item.ResultIsReady)
                    {
                        item.ResultIsReady = true;
                        dbContext.Update(item);
                        _noOfPublished++;
                    }


                            //PROCESSING SMS SENDER
                            //string number1 = getnumber.PhoneNumber, finalmessage = "";
                            //string anotherPhone = number1.Remove(0, 1);
                            //string recipient = 234 + anotherPhone;
                            //string message = $"Dear parent, we're pleased to announce ${item.StudentsData.FirstName}'s ${term(item.Term)} term result (${item.SessionYear.Name} session) is ready. Visit www.aggocollegemkd.com.ng 4 details. Thanks!";


                            ///MESSAGE LENGTH SHOULD NOT NOT BE MORE THAN 160 CHARATERS
                            //finalmessage = message.Length < 160 ? message : message.Substring(0, 160);
                            ///
                            //string sender_name = "AggoCollege";
                            //string username = "";
                            //string password = "";
                            //var intervals = Configuration.GetSection("Credentials").Get<List<SmsCredentials>>();

                            //username = intervals.FirstOrDefault().userName;
                            //password = intervals.FirstOrDefault().password;
                            //SEND SMS AT THIS POINT
                            //var data = await sender.SendMessage(username, password, finalmessage, sender_name, recipient);
                            //if (data.Contains("status") || data.Contains("Ok"))
                            //{
                            //    _NoOfSentSms++;
                            //}
                        //}
                        else
                        {
                            _resultNotAdded++;
                        }
                    //}
                    //else
                    //{
                    //    _feesNotCompleted++;
                    //}
                }
                TempData["success"] = $"{_noOfPublished} has been published, {_NoOfSentSms} message Send to Parent, {_feesNotCompleted} result(s) has not been publish due to fees not completed or paid and {_resultNotAdded} has not been added to the Portal!";
                await dbContext.SaveChangesAsync();
                return Json(new { success = true, message = TempData["success"] });
            }
            return Json(new { success = false, message = "Error in publishing result try again!" });
        }
        //ONLINE RESULT COMPUTATION OR ADDING
        [HttpPost]
        public JsonResult SendResult(List<ResultHolderVM> resultsExams)
        {
            if (resultsExams.Count() > 0)
            {
                int update = 0, notupdate = 0;
                foreach (var item in resultsExams)
                {
                    var data = dbContext.ResultTable.FirstOrDefault(k => k.SubjectId == item.SubjectsCode && k.TermRegId == item.TermRegId);
                    if (data != null)
                    {
                        data.Assignment = item.Assignment;
                        data.Test = item.Test;
                        data.Project = item.Project;
                        data.ClassWork = item.ClassWork;
                        data.Examination = item.Examination;
                        data.Total = item.Total;
                        data.Grade = SD.Grade((double)item.Total);
                        data.Remark = SD.Remark((double)item.Total);
                        data.Status = true;
                        dbContext.Update(data);
                        update++;
                    }
                    else { notupdate++; }
                }
                string msg = "";
                if (notupdate > 0)
                {
                    msg = $", but {notupdate} result not updated, this may be due subject(s) not registered for this student";
                }
                if (update > 0)
                {
                    dbContext.SaveChanges();
                    return Json(new { success = true, message = update+$" results successfully updated!{msg}" });
                }
            }
            return Json(new { success = false, message = "No result send to the server for processing!" });
        }

        [HttpDelete]
        public JsonResult ManageGeneralInfo(int id)
        {
            var general = dbContext.GeneralClassTables.FirstOrDefault(k => k.Id == id);
            if (general != null)
            {
                dbContext.Remove(general);
                dbContext.SaveChanges();

                return Json(new { success = true, message = "Class general information has been successfully deleted"});
            }
            return Json(new { success = false, message = "We could not identify this record!" });
        }
        [HttpDelete]
        public async Task<JsonResult> ManageSystemUser(string id)
        {
            try
            {
                var obj = await userManager.FindByIdAsync(id);
                if (obj == null)
                {
                    return Json(new { success = false, message = "Unknow system user account" });
                }
                if (await userManager.IsInRoleAsync(obj, SD.Role_Admin))
                {
                    return Json(new { success = false, message = "Admin Account Cannot be removed!" });
                }
                string status = "Student";

                var dBRole = userManager.GetRolesAsync(obj).Result.FirstOrDefault();
                if (dBRole != null)
                {
                    await userManager.RemoveFromRoleAsync(obj, dBRole);
                    status = dBRole;
                }
                var result = await userManager.DeleteAsync(obj);
                if (result.Succeeded)
                {
                    return Json(new { success = true, message = $"{status}Account Successfully Deleted" });
                }
                return Json(new { success = false, message = "Something went wrong "});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Something went wrong " + ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> CanprintResult(string id)
        {
            try
            {
                var settingz = dbContext.Settings.FirstOrDefault(j => j.ApplicationUserId == id);
                if (settingz != null)
                {
                    string msg = "";
                    if (settingz.CanPrintResult)
                    {
                        settingz.CanPrintResult = false;
                        msg = "Results are now available for printing by students who have completed their school fees.";
                    }
                    else
                    {
                        settingz.CanPrintResult = true;
                        msg = "Results are now available for printing by all students.";
                    }
                    dbContext.Update(settingz);
                    dbContext.SaveChanges();
                    return Json(new { success = true, message = msg });
                }
                var set = new Settings()
                {
                    CanPrintResult = true,
                    ApplicationUserId = id
                };
                dbContext.Add(set);
                dbContext.SaveChanges();
                return Json(new { success = true, message = "Results are now available for printing by students who have completed their school fees." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Unidentified user " });
            }
            //return Json(new { success = false, message = "Unidentified user " });
        }
        [AcceptVerbs("Post", "Get")]
        public async Task<IActionResult> SettingsSignature(string id, string head, IFormFile file)
        {
            string msg = "";
            string dataFileName = Path.GetFileName(file.FileName);

            string extension = Path.GetExtension(dataFileName);

            string[] allowedExtsnions = new string[] { ".jpg", ".png", ".jpeg" };

            if (!allowedExtsnions.Contains(extension.ToLower()))
            {
                TempData["error"] = "Sorry! This file is not allowed. Please make sure the file has one of these extensions: .jpg or .png.";
                return Json(new { success = false, message = TempData["error"] });
            }
            var settingz = await dbContext.Settings.FirstOrDefaultAsync(j => j.ApplicationUserId == id);
            if (settingz != null)
            {

                if (head == "Head")
                {
                    settingz.PrincipalSignature = GetFilePathAndCopy(file, settingz.PrincipalSignature);
                    dbContext.Update(settingz);
                    dbContext.SaveChanges();

                    return Json(new { success = true, message = "Principal's signature successfully updated!" });
                }
                settingz.CashierSignature = GetFilePathAndCopy(file, settingz.CashierSignature);
                dbContext.Update(settingz);
                dbContext.SaveChanges();

                return Json(new { success = true, message = "Account/Casheir signature successfully updated!" });
            }
            if (head == "Head")
            {
                var set = new Settings()
                {
                    PrincipalSignature = GetFilePathAndCopy(file, settingz.PrincipalSignature),
                    ApplicationUserId = id
                };
                dbContext.Add(set);
                msg = "Principal's signature successfully uploaded!";
            }
            else
            {
                var set = new Settings()
                {
                    CashierSignature = GetFilePathAndCopy(file, settingz.CashierSignature),
                    ApplicationUserId = id
                };
                dbContext.Add(set);
                msg = "Account/Casheir signature successfully uploaded!";
            }
            dbContext.SaveChanges();
            return Json(new { success = true, message = msg });
        }
        private string GetFilePathAndCopy(IFormFile file, string imagePath)
        {
            try
            {
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var upload = webHostEnvironment.WebRootPath + @"\image\signature\";
                    var extention = Path.GetExtension(file.FileName);
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }
                    using (var fileStream = new FileStream(upload + fileName + extention, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    return @"\image\signature\" + fileName + extention;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
