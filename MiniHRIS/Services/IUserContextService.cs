namespace MiniHRIS.Services
{
    public interface IUserContextService
    {
        /// <summary>
        /// Gets the user's role from the X-User-Role header
        /// </summary>
        string? Role { get; }

        /// <summary>
        /// Gets the user's employee ID from the X-User-Id header
        /// </summary>
        string? EmployeeId { get; }

        /// <summary>
        /// Indicates whether the current user has the HR role
        /// </summary>
        bool IsHR { get; }

        /// <summary>
        /// Indicates whether the current user has the Employee role
        /// </summary>
        bool IsEmployee { get; }
    }
}
