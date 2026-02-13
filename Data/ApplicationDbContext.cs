using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace demoapp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
//public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    //{    }

}
