module MyCompany.MyApp.Deploy.Console

open Spectre.Console

let printStatus message =
    async { AnsiConsole.Markup $"[bold aqua]{message}\n[/]" }

let printError message =
    async { AnsiConsole.Markup $"[bold red]Error: {message}\n[/]" }

// FIXME: https://spectreconsole.net/live/status
let printLiveStatus message f =
    AnsiConsole
        .Status()
        .Start(
            message,
            (fun (ctx: StatusContext) ->
                AnsiConsole.MarkupLine("Doing some more work...")
                // ctx.Status("ok")
                f)
        )
