open Monoids

type Demo = Monoid of Monoids.Demo

[<EntryPoint>]
let main argv =
    let demo = Monoid MonoidalValidation

    match demo with
    | Monoid OrdersUsingFold1 -> OrdersUsingFold1.run ()
    | Monoid OrdersUsingFold2 -> OrdersUsingFold2.run ()
    | Monoid StringMonoid -> StringMonoid.run ()
    | Monoid MappingDifferentStructure -> MappingDifferentStructure.run ()
    | Monoid MonoidHomorphism -> MonoidHomorphism.run ()
    | Monoid WordCountTest -> WordCountTest.run ()
    | Monoid FrequentWordTest -> FrequentWordTest.run ()
    | Monoid MonoidChar -> MonoidChar.run ()
    | Monoid Validation -> Validation.run ()
    | Monoid MonoidalValidation -> MonoidalValidation.run ()

    0
