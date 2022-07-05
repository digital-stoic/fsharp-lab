namespace MyCompany.Meeting.Common.Domain

open System
open MyCompany.Meeting.Common.Domain.SimpleType

module Entity =
    let createId: Id = Guid.NewGuid()
    let createCreateDate: DateTime = DateTime.Now
