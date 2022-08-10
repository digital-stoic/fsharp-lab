//-----------------------------------------------------------------------------
namespace Sketch.Common.Domain.SimpleType

open System

//=============================================================================
// Types
//=============================================================================
type Id = private Id of Guid

type String42 = private String42 of string

type ZipCode = private ZipCode of string

//=============================================================================
// Modules
//=============================================================================
module private ConstrainedType =
    let createString (s: string) = s

    // TODO: Add regex
    let createLike pattern (s: string) = s

    let createInt (i: int) = i

// FIXME: Can Id generation be in domain?
module internal Id =

    let create id = id |> Id

    let createWithNewId = Guid.NewGuid() |> Id

    let value (Id id) = id

module internal String42 =
    let create s =
        s |> ConstrainedType.createString |> String42

    let value (String42 s) = s

module internal ZipCode =
    let create s =
        s
        |> ConstrainedType.createLike "^[0-9]{5}$"
        |> ZipCode

    let value (ZipCode s) = s

//-----------------------------------------------------------------------------
namespace Sketch.Common.Domain.ValueObject

open Sketch.Common.Domain.SimpleType

//=============================================================================
// Types
//=============================================================================
// TODO: Add Country
type internal Address =
    { Street: String42
      City: String42
      State: String42
      Zip: ZipCode }

//=============================================================================
// Modules
//=============================================================================
module internal Address =
    let create street city state zip =
        { Street = String42.create street
          City = String42.create city
          State = String42.create state
          Zip = ZipCode.create zip }

//-----------------------------------------------------------------------------
namespace Sketch.Module.OrderPlacing.Domain.Public.ValueObject

open Sketch.Common.Domain.ValueObject

//=============================================================================
// Types
//=============================================================================
type internal UnvalidatedAddress = UnvalidatedAddress of Address

type internal ValidatedAddress = ValidatedAddress of Address

//=============================================================================
// Modules
//=============================================================================
module internal UnvalidatedAddress =
    let create address = address |> UnvalidatedAddress

    let value (UnvalidatedAddress a) = a

module internal ValidatedAddress =
    let create address = address |> ValidatedAddress

    let value (ValidatedAddress a) = a

//-----------------------------------------------------------------------------
namespace Sketch.Module.OrderPlacing.Domain.Public.Entity

open Sketch.Common.Domain.SimpleType

//=============================================================================
// Types
//=============================================================================
type internal Customer =
    { CustomerId: Id
      FirstName: String42
      LastName: String42 }

//=============================================================================
// Modules
//=============================================================================
module internal Customer =
    let create id first last =
        { CustomerId = Id.create id
          FirstName = String42.create first
          LastName = String42.create last }

//-----------------------------------------------------------------------------
namespace Sketch.Module.OrderPlacing.Domain.Public.AggregateRoot

open Sketch.Common.Domain.SimpleType
open Sketch.Module.OrderPlacing.Domain.Public.ValueObject
open Sketch.Module.OrderPlacing.Domain.Public.Entity

//=============================================================================
// Types
//=============================================================================
type internal ValidatedOrder =
    { OrderId: Id
      Customer: Customer
      ShippingAddress: ValidatedAddress }

type internal ShippedOrder =
    { OrderId: Id
      Customer: Customer
      ShippingAddress: ValidatedAddress
      BillingAddress: ValidatedAddress }

type internal OrderSubmit = Customer -> UnvalidatedAddress -> ValidatedOrder

type internal OrderShip = ValidatedOrder -> ShippedOrder

//=============================================================================
// Workflow Modules
//=============================================================================
module internal Order =
    let submit: OrderSubmit =
        fun customer address ->
            { OrderId = Id.createWithNewId
              Customer = customer
              // TODO: With external address check
              ShippingAddress =
                  address
                  |> UnvalidatedAddress.value
                  |> ValidatedAddress.create }

    let ship: OrderShip =
        fun order ->
            { OrderId = order.OrderId
              Customer = order.Customer
              ShippingAddress = order.ShippingAddress
              // TODO: Business logic = only in authorized countries + billing address = shipping address
              BillingAddress = order.ShippingAddress }

//-----------------------------------------------------------------------------








//-----------------------------------------------------------------------------
// FIXME: Add workflow for possible states? AKA Business Rules (input/output/events/validationError)
// TODO: Add business logic validation ROP
// TODO: Add domain events (other module?)
