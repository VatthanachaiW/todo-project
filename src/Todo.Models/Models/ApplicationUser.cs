using System;
using Microsoft.AspNetCore.Identity;

namespace Todo.Models.Models
{
  public class ApplicationUser : IdentityUser<Guid>
  {
    public string Firstname { get; set; }
    public string Lastname { get; set; }
  }
}