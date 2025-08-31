using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Data.Entities.AspIdentity;

public class Role : IdentityRole
{
    [MaxLength(256)]
    public string AlternativeName { get; set; }
    public bool? System { get; set; } = false;
    public bool? Preview { get; set; } = true;
    public bool? Active { get; set; } = true;
    public bool? Deleted { get; set; } = false;
    public DateTime? DateCreated { get; set; } = DateTime.UtcNow;
    [MaxLength(56)]
    public string? CreatedBy { get; set; }
    public DateTime? DateModified { get; set; }
    [MaxLength(56)]
    public string? ModifiedBy { get; set; }
}
