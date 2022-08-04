namespace MyCompany.Meeting.Common.Domain

open MyCompany.Meeting.Common.Architecture.Attribute
open MyCompany.Meeting.Common.Domain.SimpleType

// FIXME: Private still accessible in same assembly?
type String50 = private String50 of string

[<IsLayer(Layer.Domain)>]
[<IsPattern(Pattern.ConstrainedType)>]
module String50 =
    let value (String50 str) = str

    let create = ConstrainedType.createString String50 50

    let createOption = ConstrainedType.createStringOption String50 50

    let createMappError fieldName str =
        create fieldName str
        |> Result.mapError ValidationError
