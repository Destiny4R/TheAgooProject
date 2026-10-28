using System.Data;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.Admin.TermRegistration
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_ExamOfficer}")]
    public class Group_RegistrationModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        [BindProperty]
        public CTSSClass CTSSClass { get; set; }
        [BindProperty]
        public string? InputOption { get; set; }
        [BindProperty]
        public IFormFile? ExcelFile { get; set; }
        [BindProperty]
        public string? RegNumbers { get; set; }
        [BindProperty]
        public IList<StudentData> StudentList { get; set; } = new List<StudentData>();
        public List<string> Errors { get; set; } = new List<string>();
        public int hasValue = 0;
        public string? SessionName { get; set; }
        public string? ClassName { get; set; }
        public string? SubClassName { get; set; }
        public ViewSelectModel ViewSelectModel { get; set; }

        public Group_RegistrationModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet()
        {
            getViewSelect();
        }
        public IActionResult OnPostSearch()
        {
            getViewSelect();
            StudentList = new List<StudentData>();
            Errors = new List<string>();
            hasValue = 0;
            if (string.IsNullOrEmpty(InputOption))
            {
                TempData["error"] = "Please select how you want to provide the registration numbers: upload an excel file or enter them in the textarea";
                return Page();
            }
            if (CTSSClass.Classes <= 0 || CTSSClass.SessionYear <= 0 || string.IsNullOrEmpty(CTSSClass.Term) || CTSSClass.Subclass <= 0)
            {
                TempData["error"] = "Please select Term, Session/Year, Class and Sub-Class before verifying the students";
                return Page();
            }
            SetSelectionNames();
            var regNumbers = new List<string>();
            if (InputOption == "Excel")
            {
                if (ExcelFile == null || ExcelFile.Length == 0)
                {
                    TempData["error"] = "Please upload an excel file containing the student registration numbers";
                    return Page();
                }
                regNumbers = GetRegNumbersFromFile(ExcelFile).Distinct().ToList();
                if (regNumbers.Count == 0)
                {
                    TempData["error"] = "No registration number was read from the uploaded file, make sure A1 is the header and the registration numbers start from A2";
                    return Page();
                }
            }
            else if (InputOption == "Text")
            {
                if (string.IsNullOrWhiteSpace(RegNumbers))
                {
                    TempData["error"] = "Please enter at least one student registration number in the textarea";
                    return Page();
                }
                regNumbers = RegNumbers.Split(',').Select(k => k.Trim()).Where(k => !string.IsNullOrEmpty(k)).Distinct().ToList();
            }
            else
            {
                TempData["error"] = "Please select a valid option for providing the registration numbers";
                return Page();
            }

            foreach (var reg in regNumbers)
            {
                var student = dbContext.StudentTable.FirstOrDefault(k => k.StudentRegNo == reg);
                if (student == null)
                {
                    Errors.Add($"'{reg}' was not found in the school records");
                    continue;
                }
                var sameClass = dbContext.TermRegistrations.FirstOrDefault(k => k.StudentId == student.Id && k.SessionYearId == CTSSClass.SessionYear && k.ClassesInSchoolId == CTSSClass.Classes && k.SubClassId == CTSSClass.Subclass && k.Term == CTSSClass.Term);
                if (sameClass != null)
                {
                    Errors.Add($"{student.SurName + " " + student.FirstName} ({reg}) is already registered for the {CTSSClass.Term} term in {ClassName} {SubClassName}");
                    continue;
                }
                var otherClass = dbContext.TermRegistrations.Include(k => k.Schoolclasses).Include(k => k.SubClasses).Include(k => k.SessionYear).FirstOrDefault(k => k.StudentId == student.Id && k.SessionYearId == CTSSClass.SessionYear && (k.ClassesInSchoolId != CTSSClass.Classes || k.SubClassId != CTSSClass.Subclass));
                if (otherClass != null)
                {
                    Errors.Add($"{student.SurName + " " + student.FirstName} ({reg}) is already registered in {otherClass.Schoolclasses.Name} {otherClass.SubClasses.Name} for the {otherClass.SessionYear.Name} session, a student cannot be registered in another class/sub-class in the same session");
                    continue;
                }
                StudentList.Add(new StudentData
                {
                    StudentId = student.Id,
                    StudentRegNo = student.StudentRegNo,
                    Fullname = student.SurName + " " + student.OtherName + " " + student.FirstName
                });
            }

            if (StudentList.Count > 0)
            {
                hasValue = 1;
                TempData["success"] = $"{StudentList.Count} student(s) verified and ready for registration";
                return Page();
            }
            TempData["error"] = "No student could be verified for registration, review the errors below and try again";
            return Page();
        }
        public async Task<IActionResult> OnPostRegister(List<int> subjects, List<int>? selectedStudentIds)
        {
            getViewSelect();
            hasValue = StudentList != null && StudentList.Count > 0 ? 1 : 0;
            if (subjects == null || subjects.Count == 0)
            {
                TempData["error"] = "Select at least one subject to register";
                return Page();
            }
            if (selectedStudentIds == null || selectedStudentIds.Count == 0)
            {
                TempData["error"] = "Please select at least one student to register";
                return Page();
            }
            if (hasValue == 0)
            {
                TempData["error"] = "No student was verified for registration, search and verify the students first";
                return Page();
            }
            SetSelectionNames();
            int registered = 0, skipped = 0;
            var termregisters = new List<TheAgooProjectModel.TermRegistration>();
            var studentsToProcess = StudentList.Where(s => selectedStudentIds.Contains(s.StudentId)).ToList();
            foreach (var item in studentsToProcess)
            {
                var student = dbContext.StudentTable.FirstOrDefault(k => k.Id == item.StudentId);
                if (student == null)
                {
                    skipped++;
                    continue;
                }
                var sameClass = dbContext.TermRegistrations.FirstOrDefault(k => k.StudentId == student.Id && k.SessionYearId == CTSSClass.SessionYear && k.ClassesInSchoolId == CTSSClass.Classes && k.SubClassId == CTSSClass.Subclass && k.Term == CTSSClass.Term);
                if (sameClass != null)
                {
                    skipped++;
                    continue;
                }
                var otherClass = dbContext.TermRegistrations.FirstOrDefault(k => k.StudentId == student.Id && k.SessionYearId == CTSSClass.SessionYear && (k.ClassesInSchoolId != CTSSClass.Classes || k.SubClassId != CTSSClass.Subclass));
                if (otherClass != null)
                {
                    skipped++;
                    continue;
                }
                var termreg = new TheAgooProjectModel.TermRegistration()
                {
                    StudentId = student.Id,
                    SessionYearId = CTSSClass.SessionYear,
                    ClassesInSchoolId = CTSSClass.Classes,
                    SubClassId = CTSSClass.Subclass,
                    Term = CTSSClass.Term
                };
                termreg.RemarkPositions = new RemarkPosition
                {
                    TermRegId = termreg.Id
                };
                termreg.StudentRatings = new StudentRating()
                {
                    TermRegId = termreg.Id
                };
                termreg.ResultTable = new List<ResultTable>();
                foreach (var subjectId in subjects)
                {
                    termreg.ResultTable.Add(new ResultTable()
                    {
                        TermRegId = termreg.Id,
                        SubjectId = subjectId
                    });
                }
                termregisters.Add(termreg);
                registered++;
            }
            if (termregisters.Count > 0)
            {
                await dbContext.AddRangeAsync(termregisters);
                await dbContext.SaveChangesAsync();
                var message = $"{registered} student(s) successfully registered for the term";
                if (skipped > 0)
                {
                    message += $", {skipped} student(s) were skipped because they are already registered for this selection";
                }
                TempData["success"] = message;
                return RedirectToPage("Index");
            }
            TempData["error"] = $"No student was registered. {skipped} student(s) were skipped because they are either already registered in this class for the term or in another class/sub-class for the same session";
            return Page();
        }
        private List<string> GetRegNumbersFromFile(IFormFile file)
        {
            var regNumbers = new List<string>();
            var extension = Path.GetExtension(file.FileName);
            string[] allowedExtensions = { ".xls", ".xlsx" };
            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                return regNumbers;
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = file.OpenReadStream())
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var config = new ExcelDataSetConfiguration
                {
                    UseColumnDataType = false,
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = false
                    }
                };
                var dataSet = reader.AsDataSet(config);
                var dataTable = dataSet.Tables[0];
                int rowIndex = 0;
                foreach (DataRow row in dataTable.Rows)
                {
                    rowIndex++;
                    if (rowIndex == 1)
                    {
                        continue;
                    }
                    var value = row[0]?.ToString()?.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        regNumbers.Add(value);
                    }
                }
            }
            return regNumbers;
        }
        private void SetSelectionNames()
        {
            SessionName = dbContext.SessionYears.Find(CTSSClass.SessionYear)?.Name;
            ClassName = dbContext.SchoolClasses.Find(CTSSClass.Classes)?.Name;
            SubClassName = dbContext.SubClasses.Find(CTSSClass.Subclass)?.Name;
        }
        private void getViewSelect()
        {
            ViewSelectModel = new()
            {
                Class = dbContext.SchoolClasses.Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SubClass = dbContext.SubClasses.OrderBy(k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                SessionYearz = dbContext.SessionYears.OrderByDescending(k => k.Name).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                }),
                Subjects = dbContext.Subjects.Where(j => j.Register_State == true).Select(k => new SelectListItem
                {
                    Text = k.Name,
                    Value = k.Id.ToString()
                })
            };
        }
        public class StudentData
        {
            public int StudentId { get; set; }
            public string? StudentRegNo { get; set; }
            public string? Fullname { get; set; }
        }
    }
}
