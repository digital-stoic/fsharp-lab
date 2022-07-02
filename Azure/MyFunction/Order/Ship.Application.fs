module Ship.Application

open System
open Order.Types
open Order.Data.Test

module Domain = Order.Ship.Domain

let shipOrder input =
    let shippedDate = DateTime.Now

    let order =
        { OrderId = order1.OrderId
          CustomerInfo = order1.CustomerInfo :> ValidatedCustomerInfo
          ShippingAddress = order1.ShippingAddress :> ValidatedShippingAddress
          BillingAddress = order1.BillingAddress :> ValidatedBillingAddress
          Lines = order1.Lines :> ValidatedLines
          ShippedDate = shippedDate }

    Domain.shipOrder order
