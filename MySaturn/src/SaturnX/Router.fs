module Router

open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters
open Giraffe.HttpStatusCodeHandlers


let browser =
    pipeline {
        plug acceptHtml
        plug putSecureBrowserHeaders
        plug fetchSession
        set_header "x-pipeline-type" "Browser"
    }

let defaultView =
    router {
        get "/" (htmlView Index.layout)
        get "/index.html" (redirectTo false "/")
        get "/default.html" (redirectTo false "/")
    }

let browserRouter =
    router {
        not_found_handler (htmlView NotFound.layout) //Use the default 404 webpage
        pipe_through browser //Use the default browser pipeline
        forward "" defaultView
        forward "/books" Books.Controller.resource
    }

let api =
    pipeline {
        plug acceptJson
        set_header "x-pipeline-type" "Api"
    }

let apiRouter =
    router {
        not_found_handler (text "Api 404")
        pipe_through api
        get "/test1" (Successful.OK "Test 1 OK")
    }

let appRouter =
    router {
        forward "" browserRouter
        forward "/api" apiRouter
    }
