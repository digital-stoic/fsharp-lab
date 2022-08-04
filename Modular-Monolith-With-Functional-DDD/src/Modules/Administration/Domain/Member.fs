namespace MyCompany.Meeting.Module.Administration.Domain

open System
open FsToolkit.ErrorHandling
open MyCompany.Meeting.Common.Domain
open MyCompany.Meeting.Common.Domain.SimpleType

// module M =
//     let x = String50 "x"

// Entity, Aggregate Root
type Member =
    { Id: Id
      Login: String50
      Email: ToDo
      FirstName: String50
      LastName: String50
      CreateDate: DateTime }

// TODO: No create for Admin module (DTO != Repository)
module Member =
    // TODO: Move to DTO?
    type CreateArgs =
        { Login: string
          Email: string
          FirstName: string
          LastName: string }

    let create arg =
        result {
            let! login = arg.Login |> String50.createMappError "Login"
            // let! email =
            let! firstName = arg.Login |> String50.createMappError "FirstName"
            let! lastName = arg.Login |> String50.createMappError "LastName"
            let id = Entity.createId
            let createDate = Entity.createCreateDate

            let _member =
                { Id = id
                  Login = login
                  Email = arg.Email
                  FirstName = firstName
                  LastName = lastName
                  CreateDate = createDate }

            return _member
        }
