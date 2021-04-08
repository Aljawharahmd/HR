using System;
using System.Collections.Generic;
using System.Text;

namespace HR.Data.DTO.Enums
{
    public enum DepartmentValidationStatus
    {
        Success = 0,
        DepartmentNameAlreadyExists = 1,
        DepartmentManagerAlreadyExists = 2
    }
}
