using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MiniHRIS.Services;

namespace MiniHRIS.Attributes
{
    /// <summary>
    /// Custom attribute to enforce role-based access control on controllers and actions
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireRoleAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// Creates a new RequireRoleAttribute that enforces role-based access
        /// </summary>
        /// <param name="roles">The allowed roles for accessing the decorated controller/action</param>
        public RequireRoleAttribute(params string[] roles) : base(typeof(RoleAuthorizationFilter))
        {
            Arguments = new object[] { roles };
        }
    }

    /// <summary>
    /// Filter that validates user role and returns 403 Forbidden if unauthorized
    /// </summary>
    public class RoleAuthorizationFilter : IAuthorizationFilter
    {
        private readonly string[] _allowedRoles;
        private readonly IUserContextService _userContext;

        public RoleAuthorizationFilter(string[] allowedRoles, IUserContextService userContext)
        {
            _allowedRoles = allowedRoles;
            _userContext = userContext;
        }

        /// <summary>
        /// Validates that the current user has one of the allowed roles
        /// </summary>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userRole = _userContext.Role;

            if (string.IsNullOrEmpty(userRole) || !_allowedRoles.Contains(userRole, StringComparer.OrdinalIgnoreCase))
            {
                context.Result = new ObjectResult(new
                {
                    success = false,
                    message = "Access denied. Insufficient permissions.",
                    errors = new[] { "You do not have permission to access this resource." }
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
