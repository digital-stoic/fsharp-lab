namespace MyCompany.Meeting.Common.Domain

open System

module SimpleType =
    type ToDo = string

    type Id = Guid

    type ValidationError = ValidationError of string
