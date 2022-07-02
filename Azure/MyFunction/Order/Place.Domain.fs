module Order.Place.Domain

// TODO: Dependencies
// TODO: Validation

open Order.Types

let placeOrder: PlaceOrder =
    fun order ->
        let validatedOrder =
            { OrderId = order.OrderId
              CustomerInfo = order.CustomerInfo
              ShippingAddress = order.ShippingAddress
              BillingAddress = order.BillingAddress
              Lines = order.Lines }

        validatedOrder
