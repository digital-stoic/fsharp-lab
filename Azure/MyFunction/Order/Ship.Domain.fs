module Order.Ship.Domain

open System
open Order.Types

let shipOrder: ShipOrder =
    fun order ->
        let shippedDate = DateTime.Now

        let shippedOrder =
            { OrderId = order.OrderId
              CustomerInfo = order.CustomerInfo
              ShippingAddress = order.ShippingAddress
              BillingAddress = order.BillingAddress
              Lines = order.Lines
              ShippedDate = shippedDate }

        shippedOrder
