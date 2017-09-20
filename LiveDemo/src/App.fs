module TodoApp

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Hink.Gui
open Hink.Inputs
open Hink.Widgets
open System

let canvas = Browser.document.getElementById "application" :?> Browser.HTMLCanvasElement

let mainWindow = { WindowInfo.Default with Width = 1024.
                                           Height = 600.
                                           Title = Some "Main window" }
let ui = Hink.Create(canvas)

let rec render (_: float) =

    ui.ApplicationContext.clearRect(0., 0., ui.Canvas.width, ui.Canvas.height)
    ui.ApplicationContext.fillStyle <- !^"#fff"

    ui.Prepare()

    if ui.Window(mainWindow) then
        ()

    ui.Finish()

    Browser.window.requestAnimationFrame(Browser.FrameRequestCallback(render))
    |> ignore

render 0.

// fun inputInfo keyboard ->
//     match keyboard.Modifiers with
//     | _ ->
//         match keyboard.LastKey with
//         | Keyboard.Keys.Enter ->
//             Browser.console.log("Enter pressed")
//             true
//         | _ -> false
