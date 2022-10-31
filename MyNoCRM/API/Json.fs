module Json

open Thoth.Json.Net
open Types

let leadDecoder: Decoder<Lead> =
    Decode.object (fun get ->
        { id = get.Required.Field "id" Decode.int |> string
          title = get.Required.Field "title" Decode.string
          pipeline = get.Required.Field "pipeline" Decode.string
          step = get.Required.Field "step" Decode.string
          step_id = get.Required.Field "step_id" Decode.int
          status = get.Required.Field "status" Decode.string
          amount = get.Optional.Field "amount" Decode.float
          probability = get.Optional.Field "probability" Decode.float
          currency = get.Optional.Field "currency" Decode.string
          starred = get.Required.Field "starred" Decode.bool
          remind_date = get.Required.Field "remind_date" Decode.string
          remind_time = get.Optional.Field "remind_time" Decode.string
          created_at = get.Required.Field "created_at" Decode.string
          estimated_closing_date = get.Optional.Field "estimated_closing_date" Decode.string
          updated_at = get.Required.Field "updated_at" Decode.string
          description = get.Required.Field "description" Decode.string
          html_description = get.Required.Field "html_description" Decode.string
          tags = get.Required.Field "tags" (Decode.list Decode.string)
          created_from = get.Optional.Field "created_from" Decode.string
          closed_at = get.Optional.Field "closed_at" Decode.string
          attachment_count = get.Required.Field "attachment_count" Decode.int
          created_by_id = get.Required.Field "created_by_id" Decode.int
          user_id = get.Required.Field "user_id" Decode.int
          client_folder_id = get.Optional.Field "client_folder_id" Decode.int
          client_folder_name = get.Optional.Field "client_folder_name" Decode.string
          team_id = get.Optional.Field "team_id" Decode.int
          team_name = get.Optional.Field "team_name" Decode.string })

let leadsDecoder: Decoder<list<Lead>> = Decode.list leadDecoder

let tryDecodeLeads jsonLeads =
    match jsonLeads |> Decode.fromString leadsDecoder with
    | Ok leads -> leads
    | Error err -> failwith err
