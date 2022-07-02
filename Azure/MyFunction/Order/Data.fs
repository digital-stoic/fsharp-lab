module Order.Data.Test

open Order.Types

let order1: UnvalidatedOrder =
    { OrderId = "1"
      CustomerInfo = "John Doe"
      ShippingAddress = "123 Main St"
      BillingAddress = "123 Main St"
      Lines = [ "Item 1"; "Item 2" ] }
