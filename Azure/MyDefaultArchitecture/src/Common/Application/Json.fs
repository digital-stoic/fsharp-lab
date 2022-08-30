namespace MyCompany.MyApp.Common.Application.Json

module Answer =
    open Thoth.Json.Net
    open MyCompany.MyApp.Common.Application

    let decoder: Decoder<Dto.Answer> =
        Decode.object (fun get -> { value = get.Required.Field "value" Decode.int })

    let deserialize (json: JsonValue) = Decode.fromValue "$" decoder json

    let encoder (answer: Dto.Answer) =
        Encode.object [ "value", Encode.int (answer.value) ]

    let serialize = encoder
