using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TheAgooProjectDataAccess;
using TheAgooProjectDataAccess.Data;
using TheAgooProjectModel;
using TheAgooProjectModel.ViewModels;

namespace TheAgooProjectWeb.Pages.WebsiteManager.TeamManagers
{
    [Authorize(Roles = $"{SD.Role_Admin},{SD.Role_Accountant},{SD.Role_ExamOfficer}")]
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext dbContext;
        [BindProperty]
        public TeamManageVM Team { get; set; }
        [BindProperty]
        public IFormFile File { get; set; }
        public UpsertModel(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void OnGet(int id)
        {
            if (id > 0)
            {
                Team = dbContext.TeamManagements.Where(j => j.Id == id).Select(q=> new TeamManageVM
                {
                    Id = q.Id, Fullname = q.Fullname, Position = q.Position, Email = q.Email, Phone = q.Phone, Hierarchy= q.Hierarchy, Note= q.Note, ImagePath = q.ImagePath
                }).FirstOrDefault();
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var httpClient = new HttpClient())
            {
                //https://localhost:44327
                var endpoint = "https://aggocollegemkd.com.ng/api/TeamManagements";

                var content = new MultipartFormDataContent();
                content.Add(new StringContent(Team.Id.ToString()), "Id");
                content.Add(new StringContent(Team.Fullname), "Fullname");
                content.Add(new StringContent(Team.Phone), "Phone");
                content.Add(new StringContent(Team.Email), "Email");
                content.Add(new StringContent(Team.Hierarchy.ToString()), "Hierarchy");
                content.Add(new StringContent(Team.Position), "Position");
                content.Add(new StringContent(Team.Note), "Note");

                if (File != null)
                {
                    var fileContent = new StreamContent(File.OpenReadStream());
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "file",
                        FileName = File.FileName
                    };
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(File.ContentType);
                    content.Add(fileContent);
                }

                var response = await httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    if (Team.Id > 0)
                    {
                        TempData["success"] = "Team member successfully updated";
                    }
                    else { TempData["success"] = "Team member successfully added"; }
                    return RedirectToPage("Index");
                }
                else
                {
                    // Handle failure (e.g., show an error message)
                    TempData["error"] = "An error occurred while processing your request.";
                    return Page();
                }
            }
        }
    }
}
