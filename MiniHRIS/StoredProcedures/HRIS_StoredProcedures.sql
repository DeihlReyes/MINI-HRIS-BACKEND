-- ================================================
-- MiniHRIS Stored Procedures
-- ================================================
-- Execute these stored procedures on your MSSQL database after running migrations

USE [EmployeesDB]
GO

-- ================================================
-- Stored Procedure: Get Employee Leave Balance Summary
-- Description: Retrieves comprehensive leave balance information for an employee
-- ================================================
IF OBJECT_ID('sp_GetEmployeeLeaveBalance', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetEmployeeLeaveBalance
GO

CREATE PROCEDURE sp_GetEmployeeLeaveBalance
    @EmployeeId UNIQUEIDENTIFIER,
    @Year INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        ela.Id AS AllocationId,
        e.Id AS EmployeeId,
        e.EmployeeNumber,
        e.FirstName + ' ' + e.LastName AS EmployeeName,
        lt.Id AS LeaveTypeId,
        lt.Name AS LeaveTypeName,
        lt.Code AS LeaveTypeCode,
        ela.AllocatedDays,
        ela.UsedDays,
        ela.RemainingDays,
        ela.Year,
        ela.IsActive,
        ela.ExpiryDate,
        -- Calculate pending days (leaves in pending status)
        ISNULL((
            SELECT SUM(l.TotalDays)
            FROM Leaves l
            WHERE l.EmployeeId = @EmployeeId
                AND l.LeaveTypeId = lt.Id
                AND YEAR(l.StartDate) = @Year
                AND l.Status = 'Pending'
        ), 0) AS PendingDays,
        -- Calculate approved days for the year
        ISNULL((
            SELECT SUM(l.TotalDays)
            FROM Leaves l
            WHERE l.EmployeeId = @EmployeeId
                AND l.LeaveTypeId = lt.Id
                AND YEAR(l.StartDate) = @Year
                AND l.Status = 'Approved'
        ), 0) AS ApprovedDays
    FROM 
        EmployeeLeaveAllocations ela
        INNER JOIN Employees e ON ela.EmployeeId = e.Id
        INNER JOIN LeaveTypes lt ON ela.LeaveTypeId = lt.Id
    WHERE 
        ela.EmployeeId = @EmployeeId
        AND ela.Year = @Year
        AND ela.IsActive = 1
    ORDER BY 
        lt.Name;
END
GO

-- ================================================
-- Stored Procedure: Get Department Statistics
-- Description: Retrieves comprehensive statistics for a department
-- ================================================
IF OBJECT_ID('sp_GetDepartmentStatistics', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetDepartmentStatistics
GO

CREATE PROCEDURE sp_GetDepartmentStatistics
    @DepartmentId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        d.Id AS DepartmentId,
        d.Name AS DepartmentName,
        d.Code AS DepartmentCode,
        -- Total employees
        COUNT(DISTINCT e.Id) AS TotalEmployees,
        -- Active employees
        SUM(CASE WHEN e.EmploymentStatus = 'Active' THEN 1 ELSE 0 END) AS ActiveEmployees,
        -- Employees on leave
        SUM(CASE WHEN e.EmploymentStatus = 'OnLeave' THEN 1 ELSE 0 END) AS EmployeesOnLeave,
        -- Terminated employees
        SUM(CASE WHEN e.EmploymentStatus = 'Terminated' THEN 1 ELSE 0 END) AS TerminatedEmployees,
        -- Average salary
        AVG(e.Salary) AS AverageSalary,
        -- Total salary cost
        SUM(e.Salary) AS TotalSalaryCost,
        -- Pending leave requests
        (
            SELECT COUNT(*)
            FROM Leaves l
            WHERE l.EmployeeId IN (SELECT Id FROM Employees WHERE DepartmentId = @DepartmentId)
                AND l.Status = 'Pending'
        ) AS PendingLeaveRequests
    FROM 
        Departments d
        LEFT JOIN Employees e ON d.Id = e.DepartmentId
    WHERE 
        d.Id = @DepartmentId
    GROUP BY 
        d.Id, d.Name, d.Code;
END
GO

-- ================================================
-- Stored Procedure: Get Leave Summary Report
-- Description: Generates leave summary report for a date range
-- ================================================
IF OBJECT_ID('sp_GetLeaveSummaryReport', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetLeaveSummaryReport
GO

CREATE PROCEDURE sp_GetLeaveSummaryReport
    @StartDate DATE,
    @EndDate DATE,
    @DepartmentId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        d.Name AS DepartmentName,
        e.EmployeeNumber,
        e.FirstName + ' ' + e.LastName AS EmployeeName,
        lt.Name AS LeaveTypeName,
        l.StartDate,
        l.EndDate,
        l.TotalDays,
        l.Status,
        l.Reason,
        l.ApproverName,
        l.ApprovedAt
    FROM 
        Leaves l
        INNER JOIN Employees e ON l.EmployeeId = e.Id
        INNER JOIN Departments d ON e.DepartmentId = d.Id
        INNER JOIN LeaveTypes lt ON l.LeaveTypeId = lt.Id
    WHERE 
        l.StartDate >= @StartDate
        AND l.EndDate <= @EndDate
        AND (@DepartmentId IS NULL OR e.DepartmentId = @DepartmentId)
    ORDER BY 
        d.Name, e.LastName, l.StartDate;
END
GO

-- ================================================
-- Stored Procedure: Update Leave Balances After Approval
-- Description: Updates employee leave balance when a leave is approved
-- Note: This is called automatically by the application, but can be used for manual corrections
-- ================================================
IF OBJECT_ID('sp_UpdateLeaveBalanceOnApproval', 'P') IS NOT NULL
    DROP PROCEDURE sp_UpdateLeaveBalanceOnApproval
GO

CREATE PROCEDURE sp_UpdateLeaveBalanceOnApproval
    @LeaveId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRANSACTION;
    
    BEGIN TRY
        DECLARE @EmployeeId UNIQUEIDENTIFIER;
        DECLARE @LeaveTypeId UNIQUEIDENTIFIER;
        DECLARE @TotalDays DECIMAL(18,2);
        DECLARE @Year INT;
        
        -- Get leave details
        SELECT 
            @EmployeeId = EmployeeId,
            @LeaveTypeId = LeaveTypeId,
            @TotalDays = TotalDays,
            @Year = YEAR(StartDate)
        FROM 
            Leaves
        WHERE 
            Id = @LeaveId;
        
        -- Update the allocation
        UPDATE EmployeeLeaveAllocations
        SET 
            UsedDays = UsedDays + @TotalDays,
            RemainingDays = AllocatedDays - (UsedDays + @TotalDays),
            UpdatedAt = GETUTCDATE(),
            UpdatedBy = 'StoredProcedure'
        WHERE 
            EmployeeId = @EmployeeId
            AND LeaveTypeId = @LeaveTypeId
            AND Year = @Year
            AND IsActive = 1;
        
        COMMIT TRANSACTION;
        
        SELECT 'Success' AS Result, 'Leave balance updated successfully' AS Message;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        
        SELECT 'Error' AS Result, ERROR_MESSAGE() AS Message;
    END CATCH
END
GO

-- ================================================
-- Stored Procedure: Get Employees with Low Leave Balance
-- Description: Finds employees with leave balance below a threshold
-- ================================================
IF OBJECT_ID('sp_GetEmployeesWithLowLeaveBalance', 'P') IS NOT NULL
    DROP PROCEDURE sp_GetEmployeesWithLowLeaveBalance
GO

CREATE PROCEDURE sp_GetEmployeesWithLowLeaveBalance
    @Year INT,
    @ThresholdDays DECIMAL(18,2) = 3.0
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        e.EmployeeNumber,
        e.FirstName + ' ' + e.LastName AS EmployeeName,
        e.Email,
        d.Name AS DepartmentName,
        lt.Name AS LeaveTypeName,
        ela.AllocatedDays,
        ela.UsedDays,
        ela.RemainingDays
    FROM 
        EmployeeLeaveAllocations ela
        INNER JOIN Employees e ON ela.EmployeeId = e.Id
        INNER JOIN Departments d ON e.DepartmentId = d.Id
        INNER JOIN LeaveTypes lt ON ela.LeaveTypeId = lt.Id
    WHERE 
        ela.Year = @Year
        AND ela.IsActive = 1
        AND ela.RemainingDays <= @ThresholdDays
        AND e.EmploymentStatus = 'Active'
    ORDER BY 
        ela.RemainingDays ASC, e.LastName;
END
GO

PRINT 'All stored procedures created successfully!';
PRINT 'You can now use these stored procedures in your application.';
PRINT '';
PRINT 'Example usage:';
PRINT '  EXEC sp_GetEmployeeLeaveBalance @EmployeeId = ''your-guid-here'', @Year = 2026';
PRINT '  EXEC sp_GetDepartmentStatistics @DepartmentId = ''your-guid-here''';
PRINT '  EXEC sp_GetLeaveSummaryReport @StartDate = ''2026-01-01'', @EndDate = ''2026-12-31''';
PRINT '  EXEC sp_GetEmployeesWithLowLeaveBalance @Year = 2026, @ThresholdDays = 5.0';
GO

