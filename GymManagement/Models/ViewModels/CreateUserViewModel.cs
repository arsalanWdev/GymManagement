using RoleBase.Areas.Admin.Controllers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class CreateUserViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public List<RoleSelectionViewModel> Roles { get; set; } = new List<RoleSelectionViewModel>();
}
