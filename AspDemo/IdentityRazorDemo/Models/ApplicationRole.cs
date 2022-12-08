using Microsoft.AspNetCore.Identity;

namespace IdentityRazorDemo.Models;

/// <summary>
///
/// </summary>
public class ApplicationRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
}
