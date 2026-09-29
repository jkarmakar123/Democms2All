using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using EPiServer.Personalization.VisitorGroups;
using Microsoft.AspNetCore.Http;

namespace Democms2.Models.VisitorGroups
{
[VisitorGroupCriterion(
    Category = "User",
    DisplayName = "Authenticated user role",
    Description = "Matches the visitor when the logged-in user belongs to the selected role."
)]
public class AuthenticatedUserRoleCriterion
    : CriterionBase<AuthenticatedUserRoleCriterionModel>
{
    public override bool IsMatch(IPrincipal principal, HttpContext httpContext)
    {
        if (principal?.Identity?.IsAuthenticated != true)
        {
            return false;
        }

        return !string.IsNullOrWhiteSpace(Model.RoleName)
            && principal.IsInRole(Model.RoleName);
    }
}

public class AuthenticatedUserRoleCriterionModel : CriterionModelBase
{
    [Required]
    public string RoleName { get; set; } = string.Empty;

    public override ICriterionModel Copy()
    {
        return ShallowCopy();
    }
}
}