module MyCompany.Meeting.Common.Architecture.Attribute

open System

type Layer =
    | Application = 0
    | Domain = 1
    | Infrastructure = 2

[<AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)>]
type IsLayer(l: Layer) =
    inherit System.Attribute()

type Pattern =
    | ConstrainedType = 0

[<AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)>]
type IsPattern(p: Pattern) =
    inherit System.Attribute()
