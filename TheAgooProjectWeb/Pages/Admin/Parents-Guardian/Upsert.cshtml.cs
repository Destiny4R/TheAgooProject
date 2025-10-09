using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;

namespace TheAgooProjectWeb.Pages.Admin.Parents_Guardian
{
    [Authorize(Roles = $"{SD.Role_Admin},{ SD.Role_ExamOfficer}")]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;

        public string studentName = ""; 
        [BindProperty]
        public string studentRegNo { get; set; }
        [BindProperty]
        public ParentTable Parent {  get; set; }
        public int data = 0;
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet( int id, string studentReg)
        {
            if (!string.IsNullOrWhiteSpace(studentReg))
            {
                var stud = dbContext.StudentTable.FirstOrDefault(i=>i.StudentRegNo==studentReg);
                if (stud != null)
                {
                    studentRegNo = studentReg;
                    studentName = stud.SurName + " " + stud.OtherName + " " + stud.FirstName;
                    data = 2;
                }
            }else if (id > 0)
            {
                Parent = dbContext.ParentTables.Include(m=>m.ParentStudent.Studentsdata).FirstOrDefault(j => j.Id == id);
                studentRegNo = Parent.ParentStudent.Studentsdata.StudentRegNo;
                studentName = Parent.ParentStudent.Studentsdata.SurName + " " + Parent.ParentStudent.Studentsdata.OtherName + " " + Parent.ParentStudent.Studentsdata.FirstName;
                data = 2;
            }
        }
        public IActionResult OnPostSend()
        {
            if (ModelState.IsValid)
            {
                if (Parent.Id > 0)
                {
                    var getparent = dbContext.ParentTables.FirstOrDefault(k => k.Id == Parent.Id);
                    if (getparent!= null)
                    {
                        getparent.Name = Parent.Name;
                        getparent.Occupation = Parent.Occupation;
                        getparent.Address = Parent.Address;
                        getparent.Relationship = Parent.Relationship;
                        getparent.PhoneNumber = Parent.PhoneNumber;
                        dbContext.Update(getparent);
                        int result = dbContext.SaveChanges();
                        if (result > 0)
                        {
                            TempData["success"] = "Guardian Information sucessfully Updated!";
                            return RedirectToPage("Index");
                        }
                        TempData["error"] = "An error occured while updating Gaurdian data";
                        return RedirectToPage("Index");
                    }
                }
                else
                {
                    var ClaimsId = (ClaimsIdentity)User.Identity;
                    var claim = ClaimsId.FindFirst(ClaimTypes.NameIdentifier);
                    var stud = dbContext.StudentTable.FirstOrDefault(h => h.StudentRegNo == studentRegNo);

                    var check = dbContext.ParentTables.FirstOrDefault(M => M.PhoneNumber == Parent.PhoneNumber);
                    if (check == null)
                    {
                        if (stud != null)
                        {
                            var parent = new ParentTable()
                            {
                                Name = Parent.Name,
                                Occupation = Parent.Occupation,
                                Address = Parent.Address,
                                Relationship = Parent.Relationship,
                                PhoneNumber = Parent.PhoneNumber,
                                StudentRegNo = studentRegNo,
                                StaffId = claim.Value
                            };
                            parent.ParentStudent = new ParentStudent
                            {
                                ParentTableId = parent.Id,
                                StudentsdataId = stud.Id
                            };

                            dbContext.Add(parent);
                            int result = dbContext.SaveChanges();
                            if (result > 0)
                            {
                                TempData["success"] = "Guardian Information sucessfully Saved!";
                                return RedirectToPage("Index");
                            }
                            TempData["error"] = "An error occured while updating Gaurdian data";
                            return RedirectToPage("Index");
                        }
                    }
                    else
                    {
                        var studParent = new ParentStudent
                        {
                            ParentTableId = check.Id,
                            StudentsdataId = stud.Id
                        };
                        dbContext.Add(studParent);
                        int result = dbContext.SaveChanges();
                        if (result > 0)
                        {
                            TempData["success"] = "Student successfully Linked to an existing parent!";
                            return RedirectToPage("Index");
                        }
                    }
                }
            }
            TempData["error"] = "Incomplete information send for processing";
            return Page();
        }
        public IActionResult OnPostSearch(string regnumber)
        {
            if (!string.IsNullOrEmpty(regnumber))
            {
                var stud = dbContext.StudentTable.FirstOrDefault(i => i.StudentRegNo == regnumber);
                if (stud != null)
                {
                    studentRegNo = stud.StudentRegNo;
                    studentName = stud.SurName + " " + stud.OtherName + " " + stud.FirstName;
                    TempData["success"] = "Student Found";
                    data = 2;
                    return Page();
                }
            }
            TempData["error"] = "Student with the provided number not found";
            return Page();
        }
    }
}
