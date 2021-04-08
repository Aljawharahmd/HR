namespace HR.Data.DTO.Enums
{
    public enum EmployeeLeavesValidationStatus
    {
        Success = 0,
        RequestAlreadyExists = 1,
        NotApplicable = 2,
        LeaveBalanceExceeded = 3,
        NotAuthorized = 4,
        DraftAlreadyExists = 5,
    }
}
