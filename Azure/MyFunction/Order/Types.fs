module Order.Types

// TODO: Simple, Compound and public types
// TODO: Workflow error, exception, async

open System

type OrderId = string
type UnvalidatedCustomerInfo = string
type UnvalidatedAddress = string
type UnvalidatedOrderLine = string
type ValidatedCustomerInfo = string
type ValidatedAddress = string
type ValidatedOrderLine = string

type UnvalidatedOrder =
    { OrderId: OrderId
      CustomerInfo: UnvalidatedCustomerInfo
      ShippingAddress: UnvalidatedAddress
      BillingAddress: UnvalidatedAddress
      Lines: UnvalidatedOrderLine list }

type PlacedOrder =
    { OrderId: OrderId
      CustomerInfo: ValidatedCustomerInfo
      ShippingAddress: ValidatedAddress
      BillingAddress: ValidatedAddress
      Lines: ValidatedOrderLine list }

type ShippedOrder =
    { OrderId: OrderId
      CustomerInfo: ValidatedCustomerInfo
      ShippingAddress: ValidatedAddress
      BillingAddress: ValidatedAddress
      Lines: ValidatedOrderLine list
      ShippedDate: DateTime }

type PlaceOrder = UnvalidatedOrder -> PlacedOrder
type ShipOrder = PlacedOrder -> ShippedOrder
