namespace MyCompany.MyApp.Common.Application.Dto

type Answer = { value: int }

module Answer =
    open MyCompany.MyApp.Common

    let fromDomain a = { value = Domain.Answer.value a }

    let toDomain a = Domain.Answer.create (a.value)
