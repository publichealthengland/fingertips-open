﻿
namespace FingertipsUploadService.ProfileData.Entities.JobError
{
    public enum UploadJobErrorType
    {
        PermissionError = 40001,
        WorkSheetValidationError = 40002,
        DuplicateRowInSpreadsheetError = 40003,
        DuplicateRowInDatabaseError = 40004,
        ValidationFailureError = 40005,
        SmallNumberWarning = 40006,
        WrongColumnError = 40007,
        UnexpectedError = 40008
    }
}
