namespace MyCompany.Meeting.Common.Domain

open MyCompany.Meeting.Common.Architecture.Attribute
open MyCompany.Meeting.Common.Domain.SimpleType

// FIXME: Private still accessible in same assembly?
type String200 = private String200 of string

[<IsLayer(Layer.Domain)>]
[<IsPattern(Pattern.ConstrainedType)>]
module String200 =
    let value (String200 str) = str

    let create = ConstrainedType.createString String200 200

    let createOption = ConstrainedType.createStringOption String200 200

    let createMappError fieldName str =
        create fieldName str
        |> Result.mapError ValidationError
