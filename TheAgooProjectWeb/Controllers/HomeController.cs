using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Security.Claims;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.DTOs;
using TheAgooProjectModel.Models;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Controllers
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer},{SD.Role_Accountant}")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HomeController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [AcceptVerbs(["Post","Get"])]
        public IActionResult StudentData()
        {
            try
            {
                IEnumerable<StudentsData> userData;
                userData = dbContext.StudentTable.Include(h=>h.ApplicationUser).ToList();

                userData = userData.OrderByDescending(k => k.CreateDate).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.StudentTable.Include(k=>k.ApplicationUser).Where(m => (m.FirstName.Contains(searchValue)
                                                || m.Gender.Contains(searchValue)
                                                || m.SurName.Contains(searchValue)
                                                || m.StudentRegNo.Contains(searchValue)
                                                || m.State.Contains(searchValue)
                                                || m.LocalGov.Contains(searchValue)
                                                || m.OtherName.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AcceptVerbs(["Post", "Get"])]
        public IActionResult SessionData()
        {
            try
            {
                IEnumerable<SessionYear> userData;
                userData = dbContext.SessionYears.ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.SessionYears.Where(m => (m.Name.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult ClassData()
        {
            try
            {
                IEnumerable<SchoolClasses> userData;
                userData = dbContext.SchoolClasses.ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.SchoolClasses.Where(m => (m.Name.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult SubClassData()
        {
            try
            {
                IEnumerable<SubClasses> userData;
                userData = dbContext.SubClasses.ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.SubClasses.Where(m => (m.Name.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //SubjectData
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult SubjectData()
        {
            try
            {
                IEnumerable<Subjects> userData;
                userData = dbContext.Subjects.ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.Subjects.Where(m => (m.Name.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Term Registration
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult TermRegistration(termregDTO response)
        {
            try
            {
                IEnumerable<TermRegistration> userData;
                if (ModelState.IsValid)
                {
                    userData = dbContext.TermRegistrations.Include(k => k.SessionYear).Include(k => k.SubClasses).Include(k => k.Schoolclasses).Include(k => k.StudentsData).Include(k => k.ResultTable)
                        .Where(d => d.Term == response.term && d.ClassesInSchoolId == response.classes && d.SubClassId == response.subclass && d.SessionYearId == response.sessionYear).ToList();
                }
                else
                {
                    userData = dbContext.TermRegistrations.Include(k => k.SessionYear).Include(k => k.SubClasses).Include(k => k.Schoolclasses).Include(k => k.StudentsData).Include(k => k.ResultTable).ToList();
                }

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.TermRegistrations.Include(k => k.SessionYear).Include(k => k.SubClasses).Include(k => k.Schoolclasses).Include(k => k.StudentsData).Include(k => k.ResultTable).Where(m => (m.StudentsData.StudentRegNo.Contains(searchValue)
                                                || m.SessionYear.Name.Contains(searchValue)
                                                || m.Schoolclasses.Name.Contains(searchValue)
                                                || m.StudentsData.SurName.Contains(searchValue)
                                                || m.Term.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //Logic Statement about putting result data together and reading meaning out of it
        [HttpPost]
        public JsonResult ResultComputation()
        {
            var ClaimFinder = (ClaimsIdentity)User.Identity;
            var claim = ClaimFinder.FindFirst(ClaimTypes.NameIdentifier);
            var settings = dbContext.Settings.FirstOrDefault(h => h.ApplicationUserId == claim.Value);
            var collectResult = new Collection<ComputeResultVM>();
            if (settings != null)
            {
                var termregister = dbContext.TermRegistrations.Include(h => h.SessionYear).Include(h => h.RemarkPositions).Include(h => h.Schoolclasses).Include(h => h.SubClasses).Include(h => h.ResultTable).Include(l => l.StudentsData).Where(d => d.ClassesInSchoolId == settings.Classes && d.SubClassId == settings.subclass && d.SessionYearId == settings.sessionYear && d.Term == settings.Term).ToList();

                foreach (var item in termregister)
                {
                    int outcome = 0;
                    var compute = new ComputeResultVM()
                    {
                        Term = item.Term,
                        Fullname = item.StudentsData.SurName + " " + item.StudentsData.OtherName + " " + item.StudentsData.FirstName,
                        StudentRegNo = item.StudentsData.StudentRegNo,
                        SessionYear = item.SessionYear.Name,
                        Classes = item.Schoolclasses.Name,
                        SubClass = item.SubClasses.Name,
                        TermRegId = item.Id,
                        PostStatus = item.RemarkPositions.P_Status,
                        RemarkStatus = item.RemarkPositions.R_Status,
                        Attendance = SD.Attendances((item.RemarkPositions.Student_Attendance??0))
                    };
                    foreach (var item1 in item.ResultTable)
                    {
                        if (item1.Status)
                        {
                            outcome++;
                        }
                    }
                    if (item.ResultTable.Count() == item.ResultTable.Where(k => k.Status).Count())
                    {
                        compute.Status = true;
                    }
                    else { compute.Status = false; }
                    compute.SubjectsState = item.ResultTable.Where(k=>k.Status).Count() + "/" + item.ResultTable.Count();
                    collectResult.Add(compute);
                }
                //var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                //var draw = Request.Form["draw"].FirstOrDefault();
                //var start = Request.Form["start"].FirstOrDefault();
                //var length = Request.Form["length"].FirstOrDefault();
                //var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                //var searchValue = Request.Form["search[value]"].FirstOrDefault();
                //int pageSize = length != null ? Convert.ToInt32(length) : 0;
                //int skip = start != null ? Convert.ToInt32(start) : 0;
                //int recordsTotal = 0;
                
                //recordsTotal = collectResult.Count();
                //var data = collectResult.Skip(skip).Take(pageSize).ToList();
                var jsonData = new {data = collectResult };
                return Json(jsonData);
            }
            return Json(new { data = collectResult });
        }
        //ResultComputation
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult GeneralInfo()
        {
            try
            {
                IEnumerable<GeneralClassTable> userData;
                userData = dbContext.GeneralClassTables.Include(k => k.SessionYear).Include(k => k.SubClasses).Include(k => k.Classcchool).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.GeneralClassTables.Include(k => k.SessionYear).Include(k => k.SubClasses).Include(k => k.Classcchool).Where(m => m.SessionYear.Name.Contains(searchValue)
                                                || m.Classcchool.Name.Contains(searchValue)
                                                || m.SubClasses.Name.Contains(searchValue)
                                                || m.Term.Contains(searchValue));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //SystemUserz
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult SystemUserz()
        {
            try
            {
                IEnumerable<ApplicationUser> userData;
                userData = dbContext.ApplicationUsers.Where(j=>j.Position ==SD.Role_Admin || j.Position==SD.Role_ExamOfficer || j.Position == SD.Role_Accountant).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.ApplicationUsers.Where(m => m.Fullname.Contains(searchValue)
                                                || m.Position.Contains(searchValue)
                                                || m.UserName.Contains(searchValue));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult TermlyFeesData()
        {
            try
            {
                IEnumerable<TermlyFeesPay> userData;
                userData = dbContext.TermLyFeesPays.Include(k => k.SessionYear).Include(k => k.Schoolclasses).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.TermLyFeesPays.Include(k => k.SessionYear).Include(k => k.Schoolclasses).Where(m => (m.Schoolclasses.Name.Contains(searchValue)
                                                || m.SessionYear.Name.Contains(searchValue)
                                                || m.Term.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //FeesPayemtData
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult FeesPayemtData()
        {
            try
            {
                IEnumerable<Payment> userData;
                userData = dbContext.Payments.Include(k => k.Termregistrations.SessionYear).Include(k => k.Termregistrations.Schoolclasses).Include(k => k.Termregistrations.SubClasses).Include(k => k.TermlyFees).Include(k => k.Termregistrations.StudentsData).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.Payments.Include(k => k.Termregistrations.SessionYear).Include(k => k.Termregistrations.Schoolclasses).Include(k => k.Termregistrations.SubClasses).Include(k => k.TermlyFees).Include(k => k.Termregistrations.StudentsData).Where(m => (m.Termregistrations.Schoolclasses.Name.Contains(searchValue)
                                                || m.Termregistrations.SessionYear.Name.Contains(searchValue)
                                                || m.Termregistrations.StudentsData.SurName.Contains(searchValue)
                                                || m.Termregistrations.StudentsData.FirstName.Contains(searchValue)
                                                || m.Termregistrations.SessionYear.Name.Contains(searchValue)
                                                || m.Termregistrations.StudentsData.StudentRegNo.Contains(searchValue)
                                                || m.ReceiptNo.Contains(searchValue)
                                                || m.Termregistrations.Term.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult ExpenditureType()
        {
            try
            {
                IEnumerable<TheAgooProjectModel.ExpenditureTypes> userData;
                userData = dbContext.ExpenditureType.ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.ExpenditureType.Where(m => (m.Name.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult ExpenditureTableData()
        {
            try
            {
                IEnumerable<TheAgooProjectModel.ExpenditureTable> userData;
                userData = dbContext.ExpenditureTables.Include(m=>m.ApplicationUser).Include(n=>n.expenditureTypex).Include(n => n.SessionYear).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.ExpenditureTables.Include(m => m.ApplicationUser).Include(n => n.expenditureTypex).Include(n => n.SessionYear).Where(m => (m.SessionYear.Name.Contains(searchValue)
                    || m.Term.Contains(searchValue)|| m.Description.Contains(searchValue)|| m.expenditureTypex.Name.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult LoadSmsData()
        {
            try
            {
                IEnumerable<SmsTables> userData;
                userData = dbContext.SmsTable.Include(m => m.ApplicationUser).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.SmsTable.Include(m => m.ApplicationUser).Where(m => (m.Sender.Contains(searchValue)
                    || m.Subject.Contains(searchValue) || m.ApplicationUser.Fullname.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AcceptVerbs(["Post", "Get"])]
        public IActionResult ParentStudentData()
        {
            try
            {
                IEnumerable<ParentStudent> userData;
                userData = dbContext.ParentStudents.Include(m => m.ParentTable).Include(m => m.Studentsdata).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.ParentStudents.Include(m => m.ParentTable).Include(m => m.Studentsdata).Where(m => (m.ParentTable.Name.Contains(searchValue)
                                                          || m.ParentTable.Occupation.Contains(searchValue)
                                                          || m.ParentTable.Relationship.Contains(searchValue)
                                                          || m.ParentTable.PhoneNumber.Contains(searchValue)
                                                          || m.Studentsdata.SurName.Contains(searchValue)
                                                          || m.Studentsdata.StudentRegNo.Contains(searchValue)
                                                          || m.Studentsdata.OtherName.Contains(searchValue)
                                                          || m.Studentsdata.FirstName.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var par = new List<ParentStudent>();
                foreach (var item in data)
                {
                    var stud = new ParentStudent
                    {
                        Studentsdata = item.Studentsdata,
                        ParentTable = dbContext.ParentTables.FirstOrDefault(j => j.Id == item.ParentTableId)
                    };
                    par.Add(stud);
                }
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = par };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AcceptVerbs(["Post", "Get"])]
        public IActionResult PTAFeesData()
        {
            try
            {
                IEnumerable<PTAFees> userData;
                userData = dbContext.PTAfees.Include(m => m.SessionYear).Include(m => m.ApplicationUser).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.PTAfees.Include(m => m.SessionYear).Include(m => m.ApplicationUser).Where(m => (m.Term.Contains(searchValue)
                    || m.SessionYear.Name.Contains(searchValue) || m.ApplicationUser.Fullname.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //PTA TERMLY FEES PAYMENT
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult PTAFeesPayemtData()
        {
            try
            {
                IEnumerable<PTATable> userData;
                userData = dbContext.PTATables.Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.Schoolclasses).Include(k => k.Termregistration.SubClasses).Include(k => k.PTAFee).Include(k => k.Termregistration.StudentsData).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.PTATables.Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.Schoolclasses).Include(k => k.Termregistration.SubClasses).Include(k => k.PTAFee).Include(k => k.Termregistration.StudentsData).Where(m => (m.Termregistration.Schoolclasses.Name.Contains(searchValue)
                                                || m.Termregistration.SessionYear.Name.Contains(searchValue)
                                                || m.Termregistration.StudentsData.SurName.Contains(searchValue)
                                                || m.Termregistration.StudentsData.FirstName.Contains(searchValue)
                                                || m.Termregistration.SessionYear.Name.Contains(searchValue)
                                                || m.Termregistration.StudentsData.StudentRegNo.Contains(searchValue)
                                                || m.ReceiptNo.Contains(searchValue)
                                                || m.Termregistration.Term.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult PTAExpenditureData()
        {
            try
            {
                IEnumerable<PTAExpenditureTable> userData;
                userData = dbContext.PTAExpenditureTables.Include(m => m.ApplicationUser).Include(n => n.ExpenditureType).Include(n => n.SessionYear).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.PTAExpenditureTables.Include(m => m.ApplicationUser).Include(n => n.ExpenditureType).Include(n => n.SessionYear).Where(m => (m.SessionYear.Name.Contains(searchValue)
                                                                    || m.Term.Contains(searchValue) 
                                                                    || m.Description.Contains(searchValue) 
                                                                    || m.ExpenditureType.Name.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult GetFeedBack()
        {
            try
            {
                //var user = await userManager.GetUserAsync(User);
                IEnumerable<FeedBack> userData = dbContext.FeedBacks.OrderByDescending(k => k.CreatedDate).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    var userDat = dbContext.FeedBacks.OrderByDescending(k => k.CreatedDate).Where(m => m.Name.Contains(searchValue)
                                                || m.Subject.Contains(searchValue)
                                                || m.Message.Contains(searchValue)
                                                || m.Email.Contains(searchValue)
                                                || m.Email.Contains(searchValue)).ToList();
                    userData = userDat;
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AcceptVerbs(["Post", "Get"])]
        public IActionResult OtherpayData()
        {
            try
            {
                IEnumerable<OtherPayTable> userData;
                userData = dbContext.OtherPayTables.ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.OtherPayTables.Where(m => (m.Name.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AcceptVerbs(["Post", "Get"])]
        public IActionResult SpecialPaymentData()
        {
            try
            {
                IEnumerable<SpecialPaySetUp> userData;
                userData = dbContext.SpecialPaySetUps.Include(m=>m.OtherPayTables).Include(k => k.SessionYear).Include(k => k.Schoolclasses).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.SpecialPaySetUps.Include(m => m.OtherPayTables).Include(k => k.SessionYear).Include(k => k.Schoolclasses).Where(m => (m.Schoolclasses.Name.Contains(searchValue)
                                                || m.SessionYear.Name.Contains(searchValue)
                                                || m.OtherPayTables.Name.Contains(searchValue))
                                                || m.Term.Contains(searchValue));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //Special payment Data
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult AditionalPaymentData()
        {
            try
            {
                IEnumerable<AdditionalPayment> userData;
                userData = dbContext.AdditionalPayments.Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.Schoolclasses).Include(k => k.Termregistration.SubClasses).Include(k => k.SpecialPay.OtherPayTables).Include(k => k.Termregistration.StudentsData).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.AdditionalPayments.Include(k => k.Termregistration.SessionYear).Include(k => k.Termregistration.Schoolclasses).Include(k => k.Termregistration.SubClasses).Include(k => k.SpecialPay).Include(k => k.Termregistration.StudentsData).Where(m =>             (m.Termregistration.Schoolclasses.Name.Contains(searchValue)
                                                || m.SpecialPay.OtherPayTables.Name.Contains(searchValue)
                                                || m.Termregistration.SessionYear.Name.Contains(searchValue)
                                                || m.Termregistration.StudentsData.SurName.Contains(searchValue)
                                                || m.Termregistration.StudentsData.FirstName.Contains(searchValue)
                                                || m.Termregistration.SessionYear.Name.Contains(searchValue)
                                                || m.Termregistration.StudentsData.StudentRegNo.Contains(searchValue)
                                                || m.Termregistration.Term.Contains(searchValue)));
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //TeamManagers
        [AcceptVerbs(["Post", "Get"])]
        public IActionResult TeamManagers()
        {
            try
            {
                IEnumerable<TeamManagement> userData;
                userData = dbContext.TeamManagements.ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.TeamManagements.Where(m => (m.Fullname.Contains(searchValue)
                                                || m.Position.Contains(searchValue)
                                                || m.Email.Contains(searchValue)
                                                || m.Phone.Contains(searchValue))).ToList();
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AcceptVerbs(["Post", "Get"])]
        public IActionResult WebGalleryPictures()
        {
            try
            {
                IEnumerable<GalleryTable> userData;
                userData = dbContext.GalleryTables.Include(j => j.Imagestable).ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.GalleryTables.Include(j=>j.Imagestable).Where(m => (m.Message.Contains(searchValue)
                                                || m.PostedBy.Contains(searchValue))).ToList();
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //GetnewsInformation

        [AcceptVerbs(["Post", "Get"])]
        public IActionResult GetnewsInformation()
        {
            try
            {
                IEnumerable<NewsTable> userData;
                userData = dbContext.NewsTables.ToList();

                userData = userData.OrderByDescending(k => k.Id).ToList();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    userData = userData.OrderBy(s => sortColumn + " " + sortColumnDirection);
                }
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = dbContext.NewsTables.Where(m => (m.Reporter.Contains(searchValue)
                                                || m.Headline.Contains(searchValue)
                                                || m.Category.Contains(searchValue)
                                                || m.NewBody.Contains(searchValue))).ToList();
                }
                recordsTotal = userData.Count();
                var data = userData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Json(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
