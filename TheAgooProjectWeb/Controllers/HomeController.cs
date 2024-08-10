using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Security.Claims;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Controllers
{
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
        public IActionResult TermRegistration()
        {
            try
            {
                IEnumerable<TermRegistration> userData;
                userData = dbContext.TermRegistrations.Include(k=>k.SessionYear).Include(k => k.SubClasses).Include(k => k.Schoolclasses).Include(k => k.StudentsData).Include(k => k.ResultTable).ToList();

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
                    userData = dbContext.TermRegistrations.Where(m => (m.StudentsData.StudentRegNo.Contains(searchValue)
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
                    userData = dbContext.GeneralClassTables.Where(m => m.SessionYear.Name.Contains(searchValue)
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
    }
}
