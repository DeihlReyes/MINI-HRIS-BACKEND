namespace MiniHRIS.Services
{
    /// <summary>
    /// Service to extract and provide user context information from HTTP headers
    /// </summary>
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets the user's role from the X-User-Role header
        /// </summary>
        public string? Role => _httpContextAccessor.HttpContext?.Request.Headers["X-User-Role"].FirstOrDefault();

        /// <summary>
        /// Gets the user's employee ID from the X-User-Id header
        /// </summary>
        public string? EmployeeId => _httpContextAccessor.HttpContext?.Request.Headers["X-User-Id"].FirstOrDefault();

        /// <summary>
        /// Indicates whether the current user has the HR role
        /// </summary>
        public bool IsHR => Role?.Equals("HR", StringComparison.OrdinalIgnoreCase) ?? false;

        /// <summary>
        /// Indicates whether the current user has the Employee role
        /// </summary>
        public bool IsEmployee => Role?.Equals("Employee", StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
