module Order.Place.Application

open Order.Data.Test

module Domain = Order.Place.Domain

let placeOrder input = Domain.placeOrder order1
