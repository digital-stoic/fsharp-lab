namespace MyCompany.Meeting.Common.Domain

open FsToolkit.ErrorHandling
open MyCompany.Meeting.Common.Domain.SimpleType

type String50 = private String50 of string

module String50 =
    let value (String50 str) = str

    let create fieldName str =
        ConstrainedType.createString fieldName String50 50 str

    let createOption fieldName str =
        ConstrainedType.createStringOption fieldName String50 50 str

    let createMappError fieldName str = 
        create fieldName str |> Result.mapError ValidationError