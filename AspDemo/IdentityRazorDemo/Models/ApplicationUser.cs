using Microsoft.AspNetCore.Identity;

namespace IdentityRazorDemo.Models;

public class ApplicationUser : IdentityUser
{
    [PersonalData]
    public string? JobNumber { get; set; }

    [PersonalData]
    public DateTime? LeaveAt { get; set; }
}
