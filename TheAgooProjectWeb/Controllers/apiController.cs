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
using ExcelDataReader;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Text;

namespace TheAgooProjectWeb.Controllers
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer},{SD.Role_Accountant}")]
    public class apiController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IOptions<AppConfigurations> appConfigurations;

        public apiController(UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext,
            IWebHostEnvironment webHostEnvironment,
            IOptions<AppConfigurations> appConfigurations)
        {
            this.userManager = userManager;
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
            this.appConfigurations = appConfigurations;
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
                    return Json(new { success = false, message = "An error occurred while processing your request " + ex.Message });
                }
            }
            return Json(new { success = false, message = "Data not found" });
        }
        [Authorize(Roles = SD.Role_Admin)]
        [HttpPost]
        public async Task<JsonResult> StudentDelete(string id)
        {
            var termreg = dbContext.TermRegistrations.Include(k => k.StudentsData.ApplicationUser).Where(k => k.StudentsData.ApplicationUserId == id).Count();
            if (termreg > 0)
            {
                return Json(new { success = false, message = "This is student is registered for various terms, remove such records before proceeding" });
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
                return Json(new { success = false, message = "Admission is given under this session/year, remove all records before proceeding" });
            }
            var getinfo1 = dbContext.TermRegistrations.Where(j => j.SessionYearId == id);
            if (getinfo1.Any())
            {
                return Json(new { success = false, message = "Students are registered in the term under this session/year, remove all records before proceeding" });
            }
            var getinfo4 = dbContext.TermLyFeesPays.Where(j => j.SessionYearId == id);
            if (getinfo4.Any())
            {
                return Json(new { success = false, message = "Termly Fees is asigned to this session/year, remove all records before proceeding" });
            }
            var getinfo3 = dbContext.PTAfees.Where(j => j.SessionYearId == id);
            if (getinfo3.Any())
            {
                return Json(new { success = false, message = "PTA Fees is asigned to this session/year, remove all records before proceeding" });
            }
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
            var getinfo4 = dbContext.TermLyFeesPays.Where(j => j.ClassesInSchoolId == id);
            if (getinfo4.Any())
            {
                return Json(new { success = false, message = "Termly Fees is asigned to this Class, remove all payments before proceeding" });
            }
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
                return Json(new { success = false, message = "Students are registered in the term under this Sub-Class, remove all students before proceeding" });
            }

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
                return Json(new { success = false, message = "Students are registered in the term under this Subject, remove all students before proceeding" });
            }
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
        [HttpPost]
        public JsonResult ManageTermReg(int id)
        {
            if (dbContext.AdditionalPayments.Any(k => k.TermRegId == id))
                return Json(new { success = false, message = "This student cannot be removed due to having paid some Additional payment for this term." });

            if (dbContext.PTATables.Any(k => k.TermRegId == id))
                return Json(new { success = false, message = "This student cannot be removed due to having paid PTA fees for this term." });

            if (dbContext.Payments.Any(k => k.TermRegId == id))
                return Json(new { success = false, message = "This student cannot be removed due to having paid school fees for this term." });

            if (dbContext.ResultTable.Any(k => k.TermRegId == id && k.Status))
                return Json(new { success = false, message = "This student cannot be removed due to having term result record computed." });

            var student = dbContext.TermRegistrations
                .Include(k => k.ResultTable)
                .Include(k => k.StudentRatings)
                .FirstOrDefault(k => k.Id == id);

            if (student == null)
                return Json(new { success = false, message = "Error while delete" });

            if (student.StudentRatings != null)
                dbContext.Remove(student.StudentRatings);

            if (student.ResultTable?.Any() == true)
                dbContext.RemoveRange(student.ResultTable);

            dbContext.Remove(student);
            dbContext.SaveChanges();

            return Json(new { success = true, message = "Student Successfully Removed from the Term" });
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

                return Json(new { success = true, message = "Subject successfully removed from the Term" });
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
                    return Json(new { success = true, message = update + $" results successfully updated!{msg}" });
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

                return Json(new { success = true, message = "Class general information has been successfully deleted" });
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
                return Json(new { success = false, message = "Something went wrong " });
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

            string[] allowedExtensions = new string[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg", ".tiff" };

            if (!allowedExtensions.Contains(extension.ToLower()))
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
        //Delete TEMLY FEES
        [HttpDelete]
        public JsonResult TermlyFeesManager(int id)
        {
            //FIRST CHECK IF THE FEES IS IN USE
            if (id > 0)
            {
                var getfeespaid = dbContext.Payments.Where(k => k.TermlyFeesId == id).ToList();
                if (getfeespaid.Count() > 0)
                {
                    return Json(new { success = false, message = "Fees have been recorded under this amount. This is not appropriate. Please remove all fees recorded under it before proceeding." });
                }
                var termlyfees = dbContext.TermLyFeesPays.FirstOrDefault(k => k.Id == id);
                if (termlyfees != null)
                {
                    dbContext.Remove(termlyfees);
                    dbContext.SaveChanges();

                    return Json(new { success = true, message = "Class Termly Fees successfully removed!" });
                }
            }
            return Json(new { success = false, message = "We could not identify this record!" });
        }
        //Delete TEMLY FEES
        [HttpDelete]
        public JsonResult FeesPaymentManager(int id)
        {
            if (id > 0)
            {
                var feepay = dbContext.Payments.FirstOrDefault(k => k.Id == id);
                if (feepay != null)
                {
                    var getreport = dbContext.PaymentReports.FirstOrDefault(k => k.TermRegId == feepay.TermRegId);
                    if (getreport != null)
                    {
                        getreport.AccummulatedPay -= feepay.Paid;
                        if (getreport.AccummulatedPay == 0)
                        {
                            dbContext.Remove(getreport);
                        }
                        else
                        {
                            dbContext.Update(getreport);
                        }
                    }
                    dbContext.Remove(feepay);
                    dbContext.SaveChanges();

                    return Json(new { success = true, message = "Fees payment successfully deleted!" });
                }
            }
            return Json(new { success = false, message = "We could not identify this record!" });
        }

        //Delete TEMLY FEES
        [HttpDelete]
        public JsonResult ExpendiTypemanager(int id)
        {
            //FIRST CHECK IF THE FEES IS IN USE
            if (id > 0)
            {
                var expentype = dbContext.ExpenditureTables.Where(k => k.ExpendId == id).ToList();
                if (expentype.Any())
                {
                    return Json(new { success = false, message = "This expenditure is in use by expenditure table, removing it is not possible" });
                }

                //var ptaexpen = dbContext.ExpenditureTables.Where(k => k.ExpendId == id).ToList();
                //if (expentype.Any())
                //{
                //    return Json(new { success = false, message = "This expenditure is in use by expenditure table, removing it is not possible" });
                //}
                var expendiT = dbContext.ExpenditureType.FirstOrDefault(k => k.Id == id);
                if (expendiT != null)
                {
                    dbContext.Remove(expendiT);
                    dbContext.SaveChanges();

                    return Json(new { success = true, message = "Expenditure successfully removed!" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record." });
        }

        //Delete TEMLY FEES
        [HttpDelete]
        public JsonResult ParentManager(int id)
        {
            //FIRST CHECK IF THE FEES IS IN USE
            if (id > 0)
            {
                var studentp = dbContext.ParentStudents.Include(b => b.ParentTable).FirstOrDefault(k => k.Id == id);
                if (studentp == null)
                {
                    return Json(new { success = false, message = "We couldn't identify the data send for processing" });
                }
                var parent = studentp.ParentTable;
                if (parent != null)
                {
                    dbContext.Remove(parent);
                    dbContext.Remove(studentp);
                    dbContext.SaveChanges();
                    return Json(new { success = true, message = "Parent successfully deleted" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }
        [HttpDelete]
        public JsonResult ManagerPTAFees(int id)
        {
            if (id > 0)
            {
                var studentp = dbContext.PTATables.Where(k => k.PTAFeesId == id).ToList();
                if (studentp.Any())
                {
                    return Json(new { success = false, message = "Payment has been made under this PTA Fees, therefore it cannot be removed." });
                }
                var ptafees = dbContext.PTAfees.FirstOrDefault(k => k.Id == id);
                if (ptafees != null)
                {
                    dbContext.Remove(ptafees);
                    dbContext.SaveChanges();
                    return Json(new { success = true, message = "PTA Fees successfully deleted" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }

        [HttpDelete]
        public JsonResult PTAFeesPaymentManager(int id)
        {
            if (id > 0)
            {
                var ptafees = dbContext.PTATables.FirstOrDefault(k => k.Id == id);
                if (ptafees != null)
                {
                    var getreport = dbContext.PTAFeesReports.FirstOrDefault(k => k.TermRegId == ptafees.TermRegId);
                    if (getreport != null)
                    {
                        getreport.CulmulativePay -= ptafees.Amount;
                        if (getreport.CulmulativePay == 0)
                        {
                            dbContext.Remove(getreport);
                        }
                        else
                        {
                            dbContext.Update(getreport);
                        }
                    }
                    dbContext.Remove(ptafees);
                    dbContext.SaveChanges();
                    return Json(new { success = true, message = "PTA Fees Payment successfully deleted" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record!" });
        }

        [HttpDelete]
        public JsonResult PTAExpenditureManager(int id)
        {
            if (id > 0)
            {
                var expentype = dbContext.PTAExpenditureTables.FirstOrDefault(k => k.Id == id);
                if (expentype != null)
                {
                    dbContext.Remove(expentype);
                    dbContext.SaveChanges();

                    return Json(new { success = true, message = "PTA Expenditure successfully deleted!" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record." });
        }
        [HttpDelete]
        public JsonResult ExpenditureManager(int id)
        {
            if (id > 0)
            {
                var expentype = dbContext.ExpenditureTables.FirstOrDefault(k => k.Id == id);
                if (expentype != null)
                {
                    dbContext.Remove(expentype);
                    dbContext.SaveChanges();

                    return Json(new { success = true, message = "Expenditure successfully deleted!" });
                }
            }
            return Json(new { success = false, message = "We couldn't identify this record." });
        }

        [HttpDelete]
        public IActionResult DeleteFeedback(int id)
        {
            var getinfo = dbContext.FeedBacks.FirstOrDefault(j => j.Id == id);
            if (getinfo == null)
            {
                return Json(new { success = false, message = "An error occurred while deleting the data" });
            }
            dbContext.Remove(getinfo);
            dbContext.SaveChanges();
            return Json(new { success = true, message = "Feed back Successfully Removed!!" });
        }
        //othertypemanager
        [HttpDelete]
        public IActionResult othertypemanager(int id)
        {
            var getinfo2 = dbContext.SpecialPaySetUps.Where(j => j.OtherPayId == id);
            if (getinfo2.Any())
            {
                return Json(new { success = false, message = "Some crucial information is dependent on this data. Removing it will distort some information" });
            }
            var otherpayz = dbContext.OtherPayTables.FirstOrDefault(k => k.Id == id);
            if (otherpayz == null)
            {
                return Json(new { success = false, message = "Error while deleting data" });
            }
            dbContext.Remove(otherpayz);
            dbContext.SaveChanges();
            return Json(new { success = true, message = "Data Successfully Removed!" });
        }
        //DELETE SPECIAL PAYMENT
        [HttpDelete]
        public IActionResult OtherPayManager(int id)
        {
            var getinfo1 = dbContext.AdditionalPayments.Where(j => j.Id == id);
            if (getinfo1.Any())
            {
                return Json(new { success = false, message = "Payments have been made under this amount. Deleting it would affect the accuracy of the information. Please remove all payments associated with this amount before continuing." });
            }
            var specialPay = dbContext.SpecialPaySetUps.FirstOrDefault(k => k.Id == id);
            if (specialPay == null)
            {
                return Json(new { success = false, message = "Error while deleting data" });
            }
            dbContext.Remove(specialPay);
            dbContext.SaveChanges();
            return Json(new { success = true, message = "Special Payment Successfully Removed!" });
        }

        //AdditionaPayMnanager
        [HttpDelete]
        public IActionResult AdditionaPayMnanager(int id)
        {
            var specialPay = dbContext.AdditionalPayments.FirstOrDefault(k => k.Id == id);
            if (specialPay == null)
            {
                return Json(new { success = false, message = "Error while deleting data" });
            }
            dbContext.Remove(specialPay);
            dbContext.SaveChanges();
            return Json(new { success = true, message = "Additional Payment Successfully Removed!" });
        }

        // ==================== BULK STUDENT IMPORT ====================

        public class BulkImportRow
        {
            public int RowNumber { get; set; }
            public string? SurName { get; set; }
            public string? FirstName { get; set; }
            public string? OtherName { get; set; }
            public string? Gender { get; set; }
            public string? DateOfBirth { get; set; }
            public string? State { get; set; }
            public string? LocalGov { get; set; }
            public string? Address { get; set; }
            public string? SessionName { get; set; }
            public string? FileSession { get; set; }
            public string Status { get; set; } = "OK";
            public List<string> Errors { get; set; } = new();
            public List<string> ErrorFields { get; set; } = new();

            internal DateTime? ParsedDateOfBirth { get; set; }
        }

        private class BulkImportParseResult
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public string SessionName { get; set; } = string.Empty;
            public List<BulkImportRow> Rows { get; set; } = new();
        }

        [HttpPost]
        public async Task<JsonResult> BulkImportPreview(IFormFile? excelFile, int sessionYearId)
        {
            var parsed = await ParseBulkImportFileAsync(excelFile, sessionYearId);
            if (!parsed.Success)
            {
                return Json(new { success = false, message = parsed.Message });
            }

            var errorCount = parsed.Rows.Count(r => r.Errors.Count > 0);
            return Json(new
            {
                success = true,
                sessionName = parsed.SessionName,
                rows = parsed.Rows,
                errorCount,
                successCount = parsed.Rows.Count - errorCount
            });
        }

        [HttpPost]
        public async Task<JsonResult> BulkImportCommit(IFormFile? excelFile, int sessionYearId)
        {
            var parsed = await ParseBulkImportFileAsync(excelFile, sessionYearId);
            if (!parsed.Success)
            {
                return Json(new { success = false, message = parsed.Message });
            }

            var invalidCount = parsed.Rows.Count(r => r.Errors.Count > 0);
            if (invalidCount > 0)
            {
                return Json(new
                {
                    success = false,
                    errorCount = invalidCount,
                    message = $"Import cancelled. {invalidCount} row(s) still have validation errors. Fix the file and preview again.",
                    rows = parsed.Rows
                });
            }

            var defaultPassword = appConfigurations.Value.DefaultPassword;
            if (string.IsNullOrWhiteSpace(defaultPassword))
            {
                return Json(new { success = false, message = "No default password is configured for new student accounts. Set AppConfigurations:DefaultPassword and try again." });
            }

            var admissionPrefix = appConfigurations.Value.AdmissionPrefix;
            if (string.IsNullOrWhiteSpace(admissionPrefix))
            {
                return Json(new { success = false, message = "No admission prefix is configured for student registration numbers. Set AppConfigurations:AdmissionPrefix and try again." });
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Json(new { success = false, message = "Unable to identify the signed-in user. Please sign in again." });
            }

            var importedCount = 0;
            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                foreach (var row in parsed.Rows)
                {
                    var username = await GenerateStudentUsernameAsync(admissionPrefix);

                    var user = new ApplicationUser
                    {
                        UserName = username,
                        Email = username,
                        Fullname = string.Join(" ", new[] { row.SurName, row.OtherName }.Where(v => !string.IsNullOrWhiteSpace(v))),
                        Position = SD.IsStudent
                    };

                    var createResult = await userManager.CreateAsync(user, defaultPassword);
                    if (!createResult.Succeeded)
                    {
                        throw new InvalidOperationException($"Could not create the account for {row.SurName} (row {row.RowNumber}): {string.Join("; ", createResult.Errors.Select(e => e.Description))}");
                    }

                    var roleResult = await userManager.AddToRoleAsync(user, SD.IsStudent);
                    if (!roleResult.Succeeded)
                    {
                        throw new InvalidOperationException($"The account for {row.SurName} (row {row.RowNumber}) could not be assigned the student role: {string.Join("; ", roleResult.Errors.Select(e => e.Description))}");
                    }

                    dbContext.StudentTable.Add(new StudentsData
                    {
                        FirstName = row.FirstName,
                        OtherName = row.OtherName,
                        SurName = row.SurName!,
                        Gender = row.Gender!,
                        State = row.State!,
                        LocalGov = row.LocalGov!,
                        Address = row.Address!,
                        StudentRegNo = username,
                        SessionYearId = sessionYearId,
                        DateOfBirth = row.ParsedDateOfBirth!.Value,
                        ApplicationUserId = user.Id,
                        UserID = currentUserId
                    });

                    importedCount++;
                }

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                try
                {
                    await transaction.RollbackAsync();
                }
                catch
                {
                    // The original import error is more useful than a rollback failure.
                }

                var message = ex is InvalidOperationException
                    ? $"{ex.Message} No records were saved."
                    : "An error occurred while importing students. No records were saved. " + ex.Message;
                return Json(new { success = false, message });
            }

            return Json(new
            {
                success = true,
                importedCount,
                message = $"{importedCount} student(s) successfully imported into {parsed.SessionName}."
            });
        }

        private async Task<string> GenerateStudentUsernameAsync(string admissionPrefix)
        {
            var studentCount = await dbContext.StudentTable.CountAsync();
            var year = DateTime.Now.Year;

            for (var attempt = 0; attempt < 100000; attempt++)
            {
                var candidate = $"{admissionPrefix}/{year}/{100 + studentCount + attempt}";
                if (await userManager.FindByNameAsync(candidate) == null)
                {
                    return candidate;
                }
            }

            throw new InvalidOperationException("Could not generate a unique student registration number.");
        }

        private async Task<BulkImportParseResult> ParseBulkImportFileAsync(IFormFile? excelFile, int sessionYearId)
        {
            var result = new BulkImportParseResult();

            if (excelFile == null || excelFile.Length == 0)
            {
                result.Message = "Please choose an Excel file (.xlsx) to import.";
                return result;
            }

            if (excelFile.Length > 5L * 1024 * 1024)
            {
                result.Message = "The uploaded file is larger than the 5MB limit.";
                return result;
            }

            if (!string.Equals(Path.GetExtension(excelFile.FileName), ".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                result.Message = "Only Excel .xlsx files are accepted. Save the workbook as .xlsx and try again.";
                return result;
            }

            var session = await dbContext.SessionYears.AsNoTracking().FirstOrDefaultAsync(s => s.Id == sessionYearId);
            if (session == null)
            {
                result.Message = "Please select a valid session/year before importing.";
                return result;
            }

            result.SessionName = session.Name;

            DataTable table;
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using var stream = excelFile.OpenReadStream();
                using var reader = ExcelReaderFactory.CreateReader(stream);
                var config = new ExcelDataSetConfiguration
                {
                    UseColumnDataType = false,
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };
                var dataSet = reader.AsDataSet(config);
                if (dataSet.Tables.Count == 0)
                {
                    result.Message = "The uploaded workbook does not contain any worksheet.";
                    return result;
                }

                table = dataSet.Tables[0];
            }
            catch (Exception ex)
            {
                result.Message = "The file could not be read as an Excel workbook. Make sure it is a valid .xlsx file. " + ex.Message;
                return result;
            }

            var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (var i = 0; i < table.Columns.Count; i++)
            {
                var header = NormalizeHeader(table.Columns[i].ColumnName);
                if (header.Length > 0 && !headerMap.ContainsKey(header))
                {
                    headerMap[header] = i;
                }
            }

            var requiredHeaders = new[] { "SURNAME", "FIRST NAME", "GENDER", "DOB", "STATE", "LOCAL GOVERNMENT", "ADDRESS" };
            var missingHeaders = requiredHeaders.Where(h => !headerMap.ContainsKey(h)).ToList();
            if (missingHeaders.Count > 0)
            {
                result.Message = $"The uploaded file is missing the following column(s): {string.Join(", ", missingHeaders)}. Download the template and try again.";
                return result;
            }

            var excelRowNumber = 1;
            foreach (DataRow dataRow in table.Rows)
            {
                excelRowNumber++;

                var surName = GetCellText(dataRow, headerMap, "SURNAME");
                var firstName = GetCellText(dataRow, headerMap, "FIRST NAME");
                var otherName = GetCellText(dataRow, headerMap, "OTHER NAME");
                var genderRaw = GetCellText(dataRow, headerMap, "GENDER");
                var dateOfBirthRaw = GetCellValue(dataRow, headerMap, "DOB");
                var dateOfBirthText = GetCellText(dataRow, headerMap, "DOB");
                var stateRaw = GetCellText(dataRow, headerMap, "STATE");
                var localGovRaw = GetCellText(dataRow, headerMap, "LOCAL GOVERNMENT");
                var address = GetCellText(dataRow, headerMap, "ADDRESS");
                var fileSession = GetCellText(dataRow, headerMap, "SESSION");

                if (IsEmptyRow(surName, firstName, otherName, genderRaw, dateOfBirthText, stateRaw, localGovRaw, address, fileSession))
                {
                    continue;
                }

                // Skip the guidance row ("e.g. Okonkwo") and the green demo row that ship with the template.
                if (surName != null && surName.StartsWith("e.g.", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (IsTemplateDemoRow(surName, otherName, stateRaw, localGovRaw, address))
                {
                    continue;
                }

                var row = new BulkImportRow
                {
                    RowNumber = excelRowNumber,
                    SurName = surName,
                    FirstName = firstName,
                    OtherName = otherName,
                    Gender = genderRaw,
                    DateOfBirth = dateOfBirthText,
                    State = stateRaw,
                    LocalGov = localGovRaw,
                    Address = address,
                    SessionName = session.Name,
                    FileSession = fileSession
                };

                // Surname
                if (string.IsNullOrWhiteSpace(surName))
                {
                    AddRowError(row, "surName", "Surname is required.");
                }
                else if (surName.Length > 50)
                {
                    AddRowError(row, "surName", "Surname must not exceed 50 characters.");
                }

                // First name
                if (string.IsNullOrWhiteSpace(firstName))
                {
                    AddRowError(row, "firstName", "First Name is required.");
                }
                else if (firstName.Length > 50)
                {
                    AddRowError(row, "firstName", "First Name must not exceed 50 characters.");
                }

                // Other name (optional)
                if (!string.IsNullOrWhiteSpace(otherName) && otherName.Length > 50)
                {
                    AddRowError(row, "otherName", "Other name must not exceed 50 characters.");
                }

                // Gender
                if (string.IsNullOrWhiteSpace(genderRaw))
                {
                    AddRowError(row, "gender", "Gender is required. Use 'Male' or 'Female'.");
                }
                else if (genderRaw.Equals("Male", StringComparison.OrdinalIgnoreCase))
                {
                    row.Gender = "Male";
                }
                else if (genderRaw.Equals("Female", StringComparison.OrdinalIgnoreCase))
                {
                    row.Gender = "Female";
                }
                else
                {
                    var suggestion = genderRaw.StartsWith("mal", StringComparison.OrdinalIgnoreCase)
                        ? " Did you mean 'Male'?"
                        : genderRaw.StartsWith("fem", StringComparison.OrdinalIgnoreCase)
                            ? " Did you mean 'Female'?"
                            : string.Empty;
                    AddRowError(row, "gender", $"Gender must be 'Male' or 'Female' (got: '{genderRaw}').{suggestion}");
                }

                // Date of birth
                if (TryParseDateOfBirth(dateOfBirthRaw, out var parsedDateOfBirth))
                {
                    row.ParsedDateOfBirth = parsedDateOfBirth;
                    row.DateOfBirth = parsedDateOfBirth.ToString("yyyy-MM-dd");
                }
                else
                {
                    AddRowError(row, "dateOfBirth", "Date of Birth is invalid or in the future. Use a date between 1900-01-01 and today (e.g. 2010-05-20).");
                }

                // State
                if (string.IsNullOrWhiteSpace(stateRaw))
                {
                    AddRowError(row, "state", "State is required.");
                }
                else if (NigeriaGeoData.TryGetState(stateRaw, out var canonicalState))
                {
                    row.State = canonicalState;
                }
                else
                {
                    var suggestion = NigeriaGeoData.SuggestState(stateRaw);
                    AddRowError(row, "state", $"State '{stateRaw}' is not a valid Nigerian state." +
                        (suggestion != null ? $" Did you mean '{suggestion}'?" : string.Empty));
                }

                // Local government
                if (string.IsNullOrWhiteSpace(localGovRaw))
                {
                    AddRowError(row, "localGov", "Local Government is required.");
                }
                else if (row.State != null && NigeriaGeoData.TryGetState(row.State, out var resolvedState))
                {
                    if (NigeriaGeoData.TryGetLocalGov(resolvedState, localGovRaw, out var canonicalLocalGov))
                    {
                        row.LocalGov = canonicalLocalGov;
                    }
                    else
                    {
                        var suggestion = NigeriaGeoData.SuggestLocalGov(resolvedState, localGovRaw);
                        AddRowError(row, "localGov", $"'{localGovRaw}' is not a valid LGA for {resolvedState}." +
                            (suggestion != null ? $" Did you mean '{suggestion}'?" : string.Empty));
                    }
                }

                // Address
                if (string.IsNullOrWhiteSpace(address))
                {
                    AddRowError(row, "address", "Address is required.");
                }
                else if (address.Length > 150)
                {
                    AddRowError(row, "address", "Address must not exceed 150 characters.");
                }

                row.Status = row.Errors.Count == 0 ? "OK" : "ERROR";
                result.Rows.Add(row);
            }

            if (result.Rows.Count == 0)
            {
                result.Message = "No student rows were found in the uploaded file. Add student data beneath the header row and try again.";
                return result;
            }

            result.Success = true;
            return result;
        }

        private static void AddRowError(BulkImportRow row, string field, string message)
        {
            row.Errors.Add(message);
            if (!row.ErrorFields.Contains(field))
            {
                row.ErrorFields.Add(field);
            }
        }

        private static string NormalizeHeader(string? header)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
                return string.Empty;
            }

            return string.Join(" ", header.Trim().ToUpperInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        private static object? GetCellValue(DataRow row, IReadOnlyDictionary<string, int> headerMap, string header)
        {
            if (!headerMap.TryGetValue(header, out var index) || index < 0 || index >= row.Table.Columns.Count)
            {
                return null;
            }

            var value = row[index];
            return value == null || value == DBNull.Value ? null : value;
        }

        private static string? GetCellText(DataRow row, IReadOnlyDictionary<string, int> headerMap, string header)
        {
            var value = GetCellValue(row, headerMap, header);
            if (value == null)
            {
                return null;
            }

            var text = value switch
            {
                DateTime date => date.ToString("yyyy-MM-dd"),
                double number => number.ToString(CultureInfo.InvariantCulture),
                _ => value.ToString()
            };

            text = text?.Trim();
            return string.IsNullOrEmpty(text) ? null : text;
        }

        private static bool IsEmptyRow(params string?[] values)
        {
            return values.All(string.IsNullOrWhiteSpace);
        }

        private static bool IsTemplateDemoRow(string? surName, string? otherName, string? state, string? localGov, string? address)
        {
            return string.Equals(surName, "Adaeze", StringComparison.OrdinalIgnoreCase)
                && string.Equals(otherName, "Chisom", StringComparison.OrdinalIgnoreCase)
                && string.Equals(state?.Trim(), "Anambra", StringComparison.OrdinalIgnoreCase)
                && string.Equals(localGov?.Trim(), "Awka South", StringComparison.OrdinalIgnoreCase)
                && string.Equals(address?.Trim(), "14 Zik Avenue, Awka", StringComparison.OrdinalIgnoreCase);
        }

        private static bool TryParseDateOfBirth(object? value, out DateTime dateOfBirth)
        {
            dateOfBirth = default;
            if (value == null)
            {
                return false;
            }

            if (value is DateTime dateValue)
            {
                dateOfBirth = dateValue.Date;
            }
            else if (value is double serialDate)
            {
                try
                {
                    dateOfBirth = DateTime.FromOADate(serialDate).Date;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                var text = value.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(text))
                {
                    return false;
                }

                var formats = new[]
                {
                    "yyyy-MM-dd", "yyyy/MM/dd", "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "d-M-yyyy",
                    "dd/MM/yy", "d/M/yy", "yyyy-MM-dd HH:mm:ss", "dd/MM/yyyy HH:mm:ss",
                    "d MMMM yyyy", "d MMM yyyy", "MMMM d, yyyy", "M/d/yyyy"
                };

                if (!DateTime.TryParseExact(text, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth)
                    && !DateTime.TryParse(text, CultureInfo.CurrentCulture, DateTimeStyles.None, out dateOfBirth))
                {
                    return false;
                }

                dateOfBirth = dateOfBirth.Date;
            }

            return dateOfBirth >= new DateTime(1900, 1, 1) && dateOfBirth <= DateTime.Today;
        }
    }
}
