# MiniHRIS API Documentation

## Base URL
```
http://localhost:xxxx/api
```

---

## Employees Endpoint

Base Route: `/api/employees`

### 1. Create Employee
**POST** `/api/employees`

**Description:** Create a new employee record

**Request Body:**
```json
{
  "employeeNumber": "string (required, max 50 chars)",
  "firstName": "string (required, 2-100 chars)",
  "lastName": "string (required, 2-100 chars)",
  "name": "string (required, 2-200 chars)",
  "email": "string (required, valid email)",
  "phone": "string (optional, valid phone format, max 20 chars)",
  "position": "string (required, max 100 chars)",
  "salary": "decimal (required, must be positive)",
  "hireDate": "DateTime (required)",
  "departmentId": "Guid (required)",
  "employmentStatus": "string (optional, default: 'Active', max 20 chars)"
}
```

**Response:** `201 Created`
```json
{
  "id": "Guid",
  "employeeNumber": "string",
  "firstName": "string",
  "lastName": "string",
  "name": "string",
  "email": "string",
  "phone": "string",
  "position": "string",
  "salary": "decimal",
  "hireDate": "DateTime",
  "employmentStatus": "string",
  "departmentId": "Guid",
  "departmentName": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `201 Created` - Employee successfully created
- `400 Bad Request` - Invalid input data

---

### 2. Get Employee by ID
**GET** `/api/employees/{id}`

**Description:** Retrieve a specific employee by ID

**Parameters:**
- `id` (path, Guid) - Employee ID

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeNumber": "string",
  "firstName": "string",
  "lastName": "string",
  "name": "string",
  "email": "string",
  "phone": "string",
  "position": "string",
  "salary": "decimal",
  "hireDate": "DateTime",
  "employmentStatus": "string",
  "departmentId": "Guid",
  "departmentName": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Employee found
- `404 Not Found` - Employee not found

---

### 3. Get All Employees
**GET** `/api/employees`

**Description:** Retrieve all employees

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "employeeNumber": "string",
    "firstName": "string",
    "lastName": "string",
    "name": "string",
    "email": "string",
    "phone": "string",
    "position": "string",
    "salary": "decimal",
    "hireDate": "DateTime",
    "employmentStatus": "string",
    "departmentId": "Guid",
    "departmentName": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Employees retrieved successfully

---

### 4. Update Employee
**PUT** `/api/employees/{id}`

**Description:** Update an existing employee

**Parameters:**
- `id` (path, Guid) - Employee ID

**Request Body:**
```json
{
  "firstName": "string (required, 2-100 chars)",
  "lastName": "string (required, 2-100 chars)",
  "name": "string (required, 2-200 chars)",
  "email": "string (required, valid email)",
  "phone": "string (optional, valid phone format, max 20 chars)",
  "position": "string (required, max 100 chars)",
  "salary": "decimal (required, 0-10,000,000)",
  "hireDate": "DateTime (required)",
  "departmentId": "Guid (required)",
  "employmentStatus": "string (optional, default: 'Active', max 20 chars)"
}
```

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeNumber": "string",
  "firstName": "string",
  "lastName": "string",
  "name": "string",
  "email": "string",
  "phone": "string",
  "position": "string",
  "salary": "decimal",
  "hireDate": "DateTime",
  "employmentStatus": "string",
  "departmentId": "Guid",
  "departmentName": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Employee updated successfully
- `400 Bad Request` - Invalid input data
- `404 Not Found` - Employee not found

---

### 5. Delete Employee
**DELETE** `/api/employees/{id}`

**Description:** Delete an employee

**Parameters:**
- `id` (path, Guid) - Employee ID

**Response:** `204 No Content`

**Status Codes:**
- `204 No Content` - Employee deleted successfully
- `404 Not Found` - Employee not found

---

### 6. Search Employees
**GET** `/api/employees/search`

**Description:** Search employees by term

**Query Parameters:**
- `term` (query, string, required) - Search term

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "employeeNumber": "string",
    "firstName": "string",
    "lastName": "string",
    "name": "string",
    "email": "string",
    "phone": "string",
    "position": "string",
    "salary": "decimal",
    "hireDate": "DateTime",
    "employmentStatus": "string",
    "departmentId": "Guid",
    "departmentName": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Search completed
- `400 Bad Request` - Search term is empty

---

## Departments Endpoint

Base Route: `/api/departments`

### 1. Create Department
**POST** `/api/departments`

**Description:** Create a new department

**Request Body:**
```json
{
  "name": "string (required, 2-100 chars)",
  "code": "string (required, 2-20 chars)",
  "description": "string (optional, max 500 chars)",
  "managerId": "Guid (optional)"
}
```

**Response:** `201 Created`
```json
{
  "id": "Guid",
  "name": "string",
  "code": "string",
  "description": "string",
  "managerId": "Guid",
  "isActive": "boolean",
  "createdAt": "DateTime",
  "employeeCount": "integer"
}
```

**Status Codes:**
- `201 Created` - Department created successfully
- `400 Bad Request` - Invalid input or duplicate code

---

### 2. Get Department by ID
**GET** `/api/departments/{id}`

**Description:** Retrieve a specific department

**Parameters:**
- `id` (path, Guid) - Department ID

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "name": "string",
  "code": "string",
  "description": "string",
  "managerId": "Guid",
  "isActive": "boolean",
  "createdAt": "DateTime",
  "employeeCount": "integer"
}
```

**Status Codes:**
- `200 OK` - Department found
- `404 Not Found` - Department not found

---

### 3. Get All Departments
**GET** `/api/departments`

**Description:** Retrieve all departments

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "name": "string",
    "code": "string",
    "description": "string",
    "managerId": "Guid",
    "isActive": "boolean",
    "createdAt": "DateTime",
    "employeeCount": "integer"
  }
]
```

**Status Codes:**
- `200 OK` - Departments retrieved successfully

---

### 4. Get Department Employees
**GET** `/api/departments/{id}/employees`

**Description:** Retrieve all employees in a department

**Parameters:**
- `id` (path, Guid) - Department ID

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "employeeNumber": "string",
    "firstName": "string",
    "lastName": "string",
    "name": "string",
    "email": "string",
    "phone": "string",
    "position": "string",
    "salary": "decimal",
    "hireDate": "DateTime",
    "employmentStatus": "string",
    "departmentId": "Guid",
    "departmentName": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Employees retrieved
- `404 Not Found` - Department not found

---

### 5. Update Department
**PUT** `/api/departments/{id}`

**Description:** Update a department

**Parameters:**
- `id` (path, Guid) - Department ID

**Request Body:**
```json
{
  "name": "string (required, 2-100 chars)",
  "code": "string (required, 2-20 chars)",
  "description": "string (optional, max 500 chars)",
  "managerId": "Guid (optional)",
  "isActive": "boolean (optional, default: true)"
}
```

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "name": "string",
  "code": "string",
  "description": "string",
  "managerId": "Guid",
  "isActive": "boolean",
  "createdAt": "DateTime",
  "employeeCount": "integer"
}
```

**Status Codes:**
- `200 OK` - Department updated successfully
- `400 Bad Request` - Invalid input
- `404 Not Found` - Department not found

---

### 6. Delete Department
**DELETE** `/api/departments/{id}`

**Description:** Delete a department

**Parameters:**
- `id` (path, Guid) - Department ID

**Response:** `204 No Content`

**Status Codes:**
- `204 No Content` - Department deleted successfully
- `400 Bad Request` - Cannot delete department (e.g., has employees)
- `404 Not Found` - Department not found

---

## Leave Types Endpoint

Base Route: `/api/leavetypes`

### 1. Create Leave Type
**POST** `/api/leavetypes`

**Description:** Create a new leave type

**Request Body:**
```json
{
  "name": "string (required, 2-100 chars)",
  "code": "string (required, 2-50 chars)",
  "description": "string (optional, max 500 chars)",
  "defaultDays": "integer (required, 0-365)",
  "isPaid": "boolean (optional, default: true)",
  "requiresApproval": "boolean (optional, default: true)",
  "maxConsecutiveDays": "integer (optional, 0-365)",
  "minNoticeDays": "integer (optional, 0-365)",
  "gender": "string (optional, max 20 chars)"
}
```

**Response:** `201 Created`
```json
{
  "id": "Guid",
  "name": "string",
  "code": "string",
  "description": "string",
  "defaultDays": "integer",
  "isPaid": "boolean",
  "requiresApproval": "boolean",
  "maxConsecutiveDays": "integer",
  "minNoticeDays": "integer",
  "isActive": "boolean",
  "gender": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `201 Created` - Leave type created successfully
- `400 Bad Request` - Invalid input

---

### 2. Get Leave Type by ID
**GET** `/api/leavetypes/{id}`

**Description:** Retrieve a specific leave type

**Parameters:**
- `id` (path, Guid) - Leave Type ID

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "name": "string",
  "code": "string",
  "description": "string",
  "defaultDays": "integer",
  "isPaid": "boolean",
  "requiresApproval": "boolean",
  "maxConsecutiveDays": "integer",
  "minNoticeDays": "integer",
  "isActive": "boolean",
  "gender": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Leave type found
- `404 Not Found` - Leave type not found

---

### 3. Get All Leave Types
**GET** `/api/leavetypes`

**Description:** Retrieve all leave types

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "name": "string",
    "code": "string",
    "description": "string",
    "defaultDays": "integer",
    "isPaid": "boolean",
    "requiresApproval": "boolean",
    "maxConsecutiveDays": "integer",
    "minNoticeDays": "integer",
    "isActive": "boolean",
    "gender": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Leave types retrieved successfully

---

### 4. Get Active Leave Types
**GET** `/api/leavetypes/active`

**Description:** Retrieve only active leave types

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "name": "string",
    "code": "string",
    "description": "string",
    "defaultDays": "integer",
    "isPaid": "boolean",
    "requiresApproval": "boolean",
    "maxConsecutiveDays": "integer",
    "minNoticeDays": "integer",
    "isActive": "boolean",
    "gender": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Active leave types retrieved

---

### 5. Update Leave Type
**PUT** `/api/leavetypes/{id}`

**Description:** Update a leave type

**Parameters:**
- `id` (path, Guid) - Leave Type ID

**Request Body:**
```json
{
  "name": "string (required, 2-100 chars)",
  "code": "string (required, 2-50 chars)",
  "description": "string (optional, max 500 chars)",
  "defaultDays": "integer (required, 0-365)",
  "isPaid": "boolean (optional, default: true)",
  "requiresApproval": "boolean (optional, default: true)",
  "maxConsecutiveDays": "integer (optional, 0-365)",
  "minNoticeDays": "integer (optional, 0-365)",
  "isActive": "boolean (optional, default: true)",
  "gender": "string (optional, max 20 chars)"
}
```

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "name": "string",
  "code": "string",
  "description": "string",
  "defaultDays": "integer",
  "isPaid": "boolean",
  "requiresApproval": "boolean",
  "maxConsecutiveDays": "integer",
  "minNoticeDays": "integer",
  "isActive": "boolean",
  "gender": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Leave type updated successfully
- `400 Bad Request` - Invalid input
- `404 Not Found` - Leave type not found

---

### 6. Delete Leave Type
**DELETE** `/api/leavetypes/{id}`

**Description:** Delete a leave type

**Parameters:**
- `id` (path, Guid) - Leave Type ID

**Response:** `204 No Content`

**Status Codes:**
- `204 No Content` - Leave type deleted successfully
- `400 Bad Request` - Cannot delete leave type
- `404 Not Found` - Leave type not found

---

## Leaves Endpoint

Base Route: `/api/leaves`

### 1. Create Leave (Apply for Leave)
**POST** `/api/leaves`

**Description:** Apply for a new leave request

**Request Body:**
```json
{
  "employeeId": "Guid (required)",
  "leaveTypeId": "Guid (required)",
  "startDate": "DateTime (required)",
  "endDate": "DateTime (required)",
  "totalDays": "decimal (required, 0.5-365)",
  "reason": "string (required, 10-1000 chars)",
  "attachmentPath": "string (optional, max 200 chars)"
}
```

**Response:** `201 Created`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "employeeName": "string",
  "leaveTypeId": "Guid",
  "leaveTypeName": "string",
  "startDate": "DateTime",
  "endDate": "DateTime",
  "totalDays": "decimal",
  "reason": "string",
  "status": "string",
  "approvedBy": "Guid",
  "approverName": "string",
  "approvedAt": "DateTime",
  "approverComments": "string",
  "rejectionReason": "string",
  "cancelledAt": "DateTime",
  "cancellationReason": "string",
  "attachmentPath": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `201 Created` - Leave request created successfully
- `400 Bad Request` - Invalid input or insufficient leave balance

---

### 2. Get Leave by ID
**GET** `/api/leaves/{id}`

**Description:** Retrieve a specific leave request

**Parameters:**
- `id` (path, Guid) - Leave ID

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "employeeName": "string",
  "leaveTypeId": "Guid",
  "leaveTypeName": "string",
  "startDate": "DateTime",
  "endDate": "DateTime",
  "totalDays": "decimal",
  "reason": "string",
  "status": "string",
  "approvedBy": "Guid",
  "approverName": "string",
  "approvedAt": "DateTime",
  "approverComments": "string",
  "rejectionReason": "string",
  "cancelledAt": "DateTime",
  "cancellationReason": "string",
  "attachmentPath": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Leave found
- `404 Not Found` - Leave not found

---

### 3. Get All Leaves
**GET** `/api/leaves`

**Description:** Retrieve all leave requests

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "employeeId": "Guid",
    "employeeName": "string",
    "leaveTypeId": "Guid",
    "leaveTypeName": "string",
    "startDate": "DateTime",
    "endDate": "DateTime",
    "totalDays": "decimal",
    "reason": "string",
    "status": "string",
    "approvedBy": "Guid",
    "approverName": "string",
    "approvedAt": "DateTime",
    "approverComments": "string",
    "rejectionReason": "string",
    "cancelledAt": "DateTime",
    "cancellationReason": "string",
    "attachmentPath": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Leaves retrieved successfully

---

### 4. Get Employee Leaves
**GET** `/api/leaves/employee/{employeeId}`

**Description:** Retrieve all leave requests for an employee

**Parameters:**
- `employeeId` (path, Guid) - Employee ID

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "employeeId": "Guid",
    "employeeName": "string",
    "leaveTypeId": "Guid",
    "leaveTypeName": "string",
    "startDate": "DateTime",
    "endDate": "DateTime",
    "totalDays": "decimal",
    "reason": "string",
    "status": "string",
    "approvedBy": "Guid",
    "approverName": "string",
    "approvedAt": "DateTime",
    "approverComments": "string",
    "rejectionReason": "string",
    "cancelledAt": "DateTime",
    "cancellationReason": "string",
    "attachmentPath": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Employee leaves retrieved

---

### 5. Get Leaves by Status
**GET** `/api/leaves/status/{status}`

**Description:** Retrieve leave requests by status

**Parameters:**
- `status` (path, string) - Status: "Pending", "Approved", "Rejected", or "Cancelled"

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "employeeId": "Guid",
    "employeeName": "string",
    "leaveTypeId": "Guid",
    "leaveTypeName": "string",
    "startDate": "DateTime",
    "endDate": "DateTime",
    "totalDays": "decimal",
    "reason": "string",
    "status": "string",
    "approvedBy": "Guid",
    "approverName": "string",
    "approvedAt": "DateTime",
    "approverComments": "string",
    "rejectionReason": "string",
    "cancelledAt": "DateTime",
    "cancellationReason": "string",
    "attachmentPath": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Leaves retrieved

---

### 6. Update Leave Request
**PUT** `/api/leaves/{id}`

**Description:** Update a pending leave request (only pending leaves can be updated)

**Parameters:**
- `id` (path, Guid) - Leave ID

**Request Body:**
```json
{
  "startDate": "DateTime (required)",
  "endDate": "DateTime (required)",
  "totalDays": "decimal (required, 0.5-365)",
  "reason": "string (required, 10-1000 chars)",
  "attachmentPath": "string (optional, max 200 chars)"
}
```

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "employeeName": "string",
  "leaveTypeId": "Guid",
  "leaveTypeName": "string",
  "startDate": "DateTime",
  "endDate": "DateTime",
  "totalDays": "decimal",
  "reason": "string",
  "status": "string",
  "approvedBy": "Guid",
  "approverName": "string",
  "approvedAt": "DateTime",
  "approverComments": "string",
  "rejectionReason": "string",
  "cancelledAt": "DateTime",
  "cancellationReason": "string",
  "attachmentPath": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Leave updated successfully
- `400 Bad Request` - Invalid input or leave is not pending
- `404 Not Found` - Leave not found

---

### 7. Delete Leave Request
**DELETE** `/api/leaves/{id}`

**Description:** Delete a leave request

**Parameters:**
- `id` (path, Guid) - Leave ID

**Response:** `204 No Content`

**Status Codes:**
- `204 No Content` - Leave deleted successfully
- `400 Bad Request` - Cannot delete non-pending leave
- `404 Not Found` - Leave not found

---

### 8. Approve or Reject Leave
**POST** `/api/leaves/{id}/approval`

**Description:** Approve or reject a leave request

**Parameters:**
- `id` (path, Guid) - Leave ID

**Request Body:**
```json
{
  "status": "string (required, 'Approved' or 'Rejected')",
  "approvedBy": "Guid (required)",
  "approverName": "string (optional, max 200 chars)",
  "comments": "string (optional, max 500 chars)",
  "rejectionReason": "string (optional, max 500 chars, required if status is 'Rejected')"
}
```

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "employeeName": "string",
  "leaveTypeId": "Guid",
  "leaveTypeName": "string",
  "startDate": "DateTime",
  "endDate": "DateTime",
  "totalDays": "decimal",
  "reason": "string",
  "status": "string",
  "approvedBy": "Guid",
  "approverName": "string",
  "approvedAt": "DateTime",
  "approverComments": "string",
  "rejectionReason": "string",
  "cancelledAt": "DateTime",
  "cancellationReason": "string",
  "attachmentPath": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Leave approved/rejected successfully
- `400 Bad Request` - Invalid status or leave already processed
- `404 Not Found` - Leave not found

---

### 9. Cancel Leave Request
**POST** `/api/leaves/{id}/cancel`

**Description:** Cancel an approved or pending leave request

**Parameters:**
- `id` (path, Guid) - Leave ID

**Request Body:**
```
Raw string: cancellation reason
```

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "employeeName": "string",
  "leaveTypeId": "Guid",
  "leaveTypeName": "string",
  "startDate": "DateTime",
  "endDate": "DateTime",
  "totalDays": "decimal",
  "reason": "string",
  "status": "string",
  "approvedBy": "Guid",
  "approverName": "string",
  "approvedAt": "DateTime",
  "approverComments": "string",
  "rejectionReason": "string",
  "cancelledAt": "DateTime",
  "cancellationReason": "string",
  "attachmentPath": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Leave cancelled successfully
- `400 Bad Request` - Cannot cancel leave in current status
- `404 Not Found` - Leave not found

---

## Leave Allocations Endpoint

Base Route: `/api/leaveallocations`

### 1. Create Leave Allocation
**POST** `/api/leaveallocations`

**Description:** Create a leave allocation for an employee

**Request Body:**
```json
{
  "employeeId": "Guid (required)",
  "leaveTypeId": "Guid (required)",
  "allocatedDays": "decimal (required, 0-365)",
  "year": "integer (required, 2020-2100)",
  "expiryDate": "DateTime (optional)",
  "notes": "string (optional, max 500 chars)"
}
```

**Response:** `201 Created`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "employeeName": "string",
  "leaveTypeId": "Guid",
  "leaveTypeName": "string",
  "leaveTypeCode": "string",
  "allocatedDays": "decimal",
  "usedDays": "decimal",
  "remainingDays": "decimal",
  "year": "integer",
  "isActive": "boolean",
  "expiryDate": "DateTime",
  "notes": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `201 Created` - Allocation created successfully
- `400 Bad Request` - Invalid input

---

### 2. Get Leave Allocation by ID
**GET** `/api/leaveallocations/{id}`

**Description:** Retrieve a specific leave allocation

**Parameters:**
- `id` (path, Guid) - Allocation ID

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "employeeName": "string",
  "leaveTypeId": "Guid",
  "leaveTypeName": "string",
  "leaveTypeCode": "string",
  "allocatedDays": "decimal",
  "usedDays": "decimal",
  "remainingDays": "decimal",
  "year": "integer",
  "isActive": "boolean",
  "expiryDate": "DateTime",
  "notes": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Allocation found
- `404 Not Found` - Allocation not found

---

### 3. Get All Leave Allocations
**GET** `/api/leaveallocations`

**Description:** Retrieve all leave allocations

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "employeeId": "Guid",
    "employeeName": "string",
    "leaveTypeId": "Guid",
    "leaveTypeName": "string",
    "leaveTypeCode": "string",
    "allocatedDays": "decimal",
    "usedDays": "decimal",
    "remainingDays": "decimal",
    "year": "integer",
    "isActive": "boolean",
    "expiryDate": "DateTime",
    "notes": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Allocations retrieved successfully

---

### 4. Get Employee Leave Allocations
**GET** `/api/leaveallocations/employee/{employeeId}`

**Description:** Retrieve leave allocations for an employee (optionally filtered by year)

**Parameters:**
- `employeeId` (path, Guid) - Employee ID
- `year` (query, integer, optional) - Filter by year

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "employeeId": "Guid",
    "employeeName": "string",
    "leaveTypeId": "Guid",
    "leaveTypeName": "string",
    "leaveTypeCode": "string",
    "allocatedDays": "decimal",
    "usedDays": "decimal",
    "remainingDays": "decimal",
    "year": "integer",
    "isActive": "boolean",
    "expiryDate": "DateTime",
    "notes": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Allocations retrieved

---

### 5. Get Employee Leave Balance Summary
**GET** `/api/leaveallocations/employee/{employeeId}/balance/{year}`

**Description:** Get comprehensive leave balance summary for an employee and year

**Parameters:**
- `employeeId` (path, Guid) - Employee ID
- `year` (path, integer) - Year

**Response:** `200 OK`
```json
{
  "employeeId": "Guid",
  "employeeName": "string",
  "employeeNumber": "string",
  "year": "integer",
  "leaveBalances": [
    {
      "leaveTypeId": "Guid",
      "leaveTypeName": "string",
      "leaveTypeCode": "string",
      "allocatedDays": "decimal",
      "usedDays": "decimal",
      "remainingDays": "decimal",
      "pendingDays": "decimal",
      "isActive": "boolean",
      "expiryDate": "DateTime"
    }
  ],
  "generatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Balance summary retrieved
- `404 Not Found` - Employee not found

---

### 6. Update Leave Allocation
**PUT** `/api/leaveallocations/{id}`

**Description:** Update a leave allocation

**Parameters:**
- `id` (path, Guid) - Allocation ID

**Request Body:**
```json
{
  "allocatedDays": "decimal (required, 0-365)",
  "isActive": "boolean (optional, default: true)",
  "expiryDate": "DateTime (optional)",
  "notes": "string (optional, max 500 chars)"
}
```

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "employeeName": "string",
  "leaveTypeId": "Guid",
  "leaveTypeName": "string",
  "leaveTypeCode": "string",
  "allocatedDays": "decimal",
  "usedDays": "decimal",
  "remainingDays": "decimal",
  "year": "integer",
  "isActive": "boolean",
  "expiryDate": "DateTime",
  "notes": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Allocation updated successfully
- `404 Not Found` - Allocation not found

---

### 7. Delete Leave Allocation
**DELETE** `/api/leaveallocations/{id}`

**Description:** Delete a leave allocation

**Parameters:**
- `id` (path, Guid) - Allocation ID

**Response:** `204 No Content`

**Status Codes:**
- `204 No Content` - Allocation deleted successfully
- `400 Bad Request` - Cannot delete allocation
- `404 Not Found` - Allocation not found

---

### 8. Auto-Allocate Leave Types to Employee
**POST** `/api/leaveallocations/employee/{employeeId}/auto-allocate/{year}`

**Description:** Automatically allocate all active leave types to an employee for a specific year

**Parameters:**
- `employeeId` (path, Guid) - Employee ID
- `year` (path, integer) - Year

**Response:** `200 OK`
```json
{
  "message": "string"
}
```

**Status Codes:**
- `200 OK` - Allocations completed or already exist
- `400 Bad Request` - Invalid employee or year

---

## Employee Information Endpoint

Base Route: `/api/employeeinformation`

### 1. Create Employee Information
**POST** `/api/employeeinformation`

**Description:** Create detailed information for an employee

**Request Body:**
```json
{
  "employeeId": "Guid (required)",
  "address": "string (optional, max 500 chars)",
  "city": "string (optional, max 100 chars)",
  "state": "string (optional, max 100 chars)",
  "postalCode": "string (optional, max 20 chars)",
  "country": "string (optional, max 100 chars)",
  "phoneNumber": "string (required, valid phone format, max 20 chars)",
  "mobileNumber": "string (optional, valid phone format, max 20 chars)",
  "dateOfBirth": "DateTime (required)",
  "gender": "string (optional, max 20 chars)",
  "maritalStatus": "string (optional, max 50 chars)",
  "nationality": "string (optional, max 50 chars)",
  "emergencyContactName": "string (required, max 200 chars)",
  "emergencyContactRelationship": "string (required, max 100 chars)",
  "emergencyContactPhone": "string (required, valid phone format, max 20 chars)",
  "ssn": "string (optional, max 50 chars)",
  "passportNumber": "string (optional, max 50 chars)",
  "taxId": "string (optional, max 50 chars)",
  "bankName": "string (optional, max 100 chars)",
  "bankAccountNumber": "string (optional, max 50 chars)",
  "bankRoutingNumber": "string (optional, max 50 chars)"
}
```

**Response:** `201 Created`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "address": "string",
  "city": "string",
  "state": "string",
  "postalCode": "string",
  "country": "string",
  "phoneNumber": "string",
  "mobileNumber": "string",
  "dateOfBirth": "DateTime",
  "gender": "string",
  "maritalStatus": "string",
  "nationality": "string",
  "emergencyContactName": "string",
  "emergencyContactRelationship": "string",
  "emergencyContactPhone": "string",
  "ssn": "string",
  "passportNumber": "string",
  "taxId": "string",
  "bankName": "string",
  "bankAccountNumber": "string",
  "bankRoutingNumber": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `201 Created` - Employee information created successfully
- `400 Bad Request` - Invalid input

---

### 2. Get Employee Information by ID
**GET** `/api/employeeinformation/{id}`

**Description:** Retrieve employee information by record ID

**Parameters:**
- `id` (path, Guid) - Employee Information ID

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "address": "string",
  "city": "string",
  "state": "string",
  "postalCode": "string",
  "country": "string",
  "phoneNumber": "string",
  "mobileNumber": "string",
  "dateOfBirth": "DateTime",
  "gender": "string",
  "maritalStatus": "string",
  "nationality": "string",
  "emergencyContactName": "string",
  "emergencyContactRelationship": "string",
  "emergencyContactPhone": "string",
  "ssn": "string",
  "passportNumber": "string",
  "taxId": "string",
  "bankName": "string",
  "bankAccountNumber": "string",
  "bankRoutingNumber": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Information found
- `404 Not Found` - Information not found

---

### 3. Get Employee Information by Employee ID
**GET** `/api/employeeinformation/employee/{employeeId}`

**Description:** Retrieve employee information using employee ID

**Parameters:**
- `employeeId` (path, Guid) - Employee ID

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "address": "string",
  "city": "string",
  "state": "string",
  "postalCode": "string",
  "country": "string",
  "phoneNumber": "string",
  "mobileNumber": "string",
  "dateOfBirth": "DateTime",
  "gender": "string",
  "maritalStatus": "string",
  "nationality": "string",
  "emergencyContactName": "string",
  "emergencyContactRelationship": "string",
  "emergencyContactPhone": "string",
  "ssn": "string",
  "passportNumber": "string",
  "taxId": "string",
  "bankName": "string",
  "bankAccountNumber": "string",
  "bankRoutingNumber": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Information found
- `404 Not Found` - Information not found

---

### 4. Get All Employee Information
**GET** `/api/employeeinformation`

**Description:** Retrieve all employee information records

**Response:** `200 OK`
```json
[
  {
    "id": "Guid",
    "employeeId": "Guid",
    "address": "string",
    "city": "string",
    "state": "string",
    "postalCode": "string",
    "country": "string",
    "phoneNumber": "string",
    "mobileNumber": "string",
    "dateOfBirth": "DateTime",
    "gender": "string",
    "maritalStatus": "string",
    "nationality": "string",
    "emergencyContactName": "string",
    "emergencyContactRelationship": "string",
    "emergencyContactPhone": "string",
    "ssn": "string",
    "passportNumber": "string",
    "taxId": "string",
    "bankName": "string",
    "bankAccountNumber": "string",
    "bankRoutingNumber": "string",
    "createdAt": "DateTime",
    "updatedAt": "DateTime"
  }
]
```

**Status Codes:**
- `200 OK` - Information retrieved successfully

---

### 5. Update Employee Information
**PUT** `/api/employeeinformation/{id}`

**Description:** Update employee information

**Parameters:**
- `id` (path, Guid) - Employee Information ID

**Request Body:**
```json
{
  "address": "string (optional, max 500 chars)",
  "city": "string (optional, max 100 chars)",
  "state": "string (optional, max 100 chars)",
  "postalCode": "string (optional, max 20 chars)",
  "country": "string (optional, max 100 chars)",
  "phoneNumber": "string (required, valid phone format, max 20 chars)",
  "mobileNumber": "string (optional, valid phone format, max 20 chars)",
  "dateOfBirth": "DateTime (required)",
  "gender": "string (optional, max 20 chars)",
  "maritalStatus": "string (optional, max 50 chars)",
  "nationality": "string (optional, max 50 chars)",
  "emergencyContactName": "string (required, max 200 chars)",
  "emergencyContactRelationship": "string (required, max 100 chars)",
  "emergencyContactPhone": "string (required, valid phone format, max 20 chars)",
  "ssn": "string (optional, max 50 chars)",
  "passportNumber": "string (optional, max 50 chars)",
  "taxId": "string (optional, max 50 chars)",
  "bankName": "string (optional, max 100 chars)",
  "bankAccountNumber": "string (optional, max 50 chars)",
  "bankRoutingNumber": "string (optional, max 50 chars)"
}
```

**Response:** `200 OK`
```json
{
  "id": "Guid",
  "employeeId": "Guid",
  "address": "string",
  "city": "string",
  "state": "string",
  "postalCode": "string",
  "country": "string",
  "phoneNumber": "string",
  "mobileNumber": "string",
  "dateOfBirth": "DateTime",
  "gender": "string",
  "maritalStatus": "string",
  "nationality": "string",
  "emergencyContactName": "string",
  "emergencyContactRelationship": "string",
  "emergencyContactPhone": "string",
  "ssn": "string",
  "passportNumber": "string",
  "taxId": "string",
  "bankName": "string",
  "bankAccountNumber": "string",
  "bankRoutingNumber": "string",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

**Status Codes:**
- `200 OK` - Information updated successfully
- `404 Not Found` - Information not found

---

### 6. Delete Employee Information
**DELETE** `/api/employeeinformation/{id}`

**Description:** Delete employee information

**Parameters:**
- `id` (path, Guid) - Employee Information ID

**Response:** `204 No Content`

**Status Codes:**
- `204 No Content` - Information deleted successfully
- `404 Not Found` - Information not found

---

## Common Status Codes

| Status Code | Description |
|------------|-------------|
| `200 OK` | Request successful, data returned |
| `201 Created` | Resource created successfully |
| `204 No Content` | Request successful, no content to return |
| `400 Bad Request` | Invalid input or validation error |
| `404 Not Found` | Resource not found |
| `500 Internal Server Error` | Server error |

---

## Authentication

Current API does not have authentication configured. Add authentication middleware as needed.

---

## Error Response Format

When an error occurs, the API returns a response in the following format:

```json
{
  "message": "Error description"
}
```

---

## Notes

- All GUIDs are in standard UUID format
- All DateTime values are in UTC ISO 8601 format
- Decimal values for days can be in 0.5 increments (representing half-days)
- Leave status values: "Pending", "Approved", "Rejected", "Cancelled"
- Employment status typically: "Active", "Inactive", "On Leave", etc.
- Department codes and leave type codes must be unique
