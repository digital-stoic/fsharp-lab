namespace Books

open Microsoft.AspNetCore.Http
open Giraffe.GiraffeViewEngine
open Saturn

module Views =
  let index (ctx : HttpContext) (objs : Book list) =
    let cnt = [
      div [_class "container "] [
        h2 [ _class "title"] [encodedText "Listing Books"]

        table [_class "table is-hoverable is-fullwidth"] [
          thead [] [
            tr [] [
              th [] [encodedText "Id"]
              th [] [encodedText "Title"]
              th [] [encodedText "Author"]
              th [] []
            ]
          ]
          tbody [] [
            for o in objs do
              yield tr [] [
                td [] [encodedText (string o.id)]
                td [] [encodedText (string o.title)]
                td [] [encodedText (string o.author)]
                td [] [
                  a [_class "button is-text"; _href (Links.withId ctx o.id )] [encodedText "Show"]
                  a [_class "button is-text"; _href (Links.edit ctx o.id )] [encodedText "Edit"]
                  a [_class "button is-text is-delete"; attr "data-href" (Links.withId ctx o.id ) ] [encodedText "Delete"]
                ]
              ]
          ]
        ]

        a [_class "button is-text"; _href (Links.add ctx )] [encodedText "New Book"]
      ]
    ]
    App.layout ([section [_class "section"] cnt])


  let show (ctx : HttpContext) (o : Book) =
    let cnt = [
      div [_class "container "] [
        h2 [ _class "title"] [encodedText "Show Book"]

        ul [] [
          li [] [ strong [] [encodedText "Id: "]; encodedText (string o.id) ]
          li [] [ strong [] [encodedText "Title: "]; encodedText (string o.title) ]
          li [] [ strong [] [encodedText "Author: "]; encodedText (string o.author) ]
        ]
        a [_class "button is-text"; _href (Links.edit ctx o.id)] [encodedText "Edit"]
        a [_class "button is-text"; _href (Links.index ctx )] [encodedText "Back"]
      ]
    ]
    App.layout ([section [_class "section"] cnt])

  let private form (ctx: HttpContext) (o: Book option) (validationResult : Map<string, string>) isUpdate =
    let validationMessage =
      div [_class "notification is-danger"] [
        a [_class "delete"; attr "aria-label" "delete"] []
        encodedText "Oops, something went wrong! Please check the errors below."
      ]

    let field selector lbl key =
      div [_class "field"] [
        yield label [_class "label"] [encodedText (string lbl)]
        yield div [_class "control has-icons-right"] [
          yield input [_class (if validationResult.ContainsKey key then "input is-danger" else "input"); _value (defaultArg (o |> Option.map selector) ""); _name key ; _type "text" ]
          if validationResult.ContainsKey key then
            yield span [_class "icon is-small is-right"] [
              i [_class "fas fa-exclamation-triangle"] []
            ]
        ]
        if validationResult.ContainsKey key then
          yield p [_class "help is-danger"] [encodedText validationResult.[key]]
      ]

    let buttons =
      div [_class "field is-grouped"] [
        div [_class "control"] [
          input [_type "submit"; _class "button is-link"; _value "Submit"]
        ]
        div [_class "control"] [
          a [_class "button is-text"; _href (Links.index ctx)] [encodedText "Cancel"]
        ]
      ]

    let cnt = [
      div [_class "container "] [
        form [ _action (if isUpdate then Links.withId ctx o.Value.id else Links.index ctx ); _method "post"] [
          if not validationResult.IsEmpty then
            yield validationMessage
          yield field (fun i -> (string i.id)) "Id" "id" 
          yield field (fun i -> (string i.title)) "Title" "title" 
          yield field (fun i -> (string i.author)) "Author" "author" 
          yield buttons
        ]
      ]
    ]
    App.layout ([section [_class "section"] cnt])

  let add (ctx: HttpContext) (o: Book option) (validationResult : Map<string, string>)=
    form ctx o validationResult false

  let edit (ctx: HttpContext) (o: Book) (validationResult : Map<string, string>) =
    form ctx (Some o) validationResult true
