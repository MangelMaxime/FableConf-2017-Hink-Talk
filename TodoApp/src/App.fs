module TodoApp

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Hink.Gui
open Hink.Inputs
open Hink.Widgets
open System

let canvas = Browser.document.getElementById "application" :?> Browser.HTMLCanvasElement

let mainWindow = { WindowInfo.Default with Width = 900.
                                           Height = 600.
                                           Title = Some "Main window" }

let mutable detailWindow : WindowInfo option = None

let comboDisplay = { ComboInfo.Default with SelectedIndex = Some 0 }

let newNote = InputInfo.Default

type Note =
    { Title : string
      Description : string
      Guid : Guid
      StateCheckbox : CheckboxInfo
      DetailsWindow : WindowInfo }

    static member Create title =
        { Title = title
          Description = ""
          Guid = Guid.NewGuid()
          StateCheckbox = CheckboxInfo.Default
          DetailsWindow = { WindowInfo.Default with X = 50.
                                                    Y = 50.
                                                    Width = 200.
                                                    Height = 100.
                                                    Title = Some title
                                                    Draggable = true
                                                    Closable = true
                                                    Closed = true } }

type Memory =
    { mutable Notes : Note list }

    member this.Remove guid =
        this.Notes <-
            this.Notes
            |> List.filter(fun note -> note.Guid <> guid)

    member this.Add title =
        this.Notes <- Note.Create(newNote.Value) :: this.Notes

    member this.GetDetailsWindows () =
        this.Notes
        |> List.filter(fun note ->
            not note.DetailsWindow.Closed
        )

let memory = { Notes = [] }

let keyboardCaptureHandler (keyboard: Keyboard.Record) =
    match keyboard.Modifiers with
    | _ ->
        match keyboard.LastKey with
        | Keyboard.Keys.Enter ->
            if newNote.Value <> "" then
                memory.Add newNote.Value
                newNote.Value <- ""
                newNote.Selection <- None
                newNote.CursorOffset <- 0
                newNote.TextStartOrigin <- 0

            true // Key has been captured
        | _ -> false

let ui = Hink.Create(canvas, keyboardCaptureHandler = keyboardCaptureHandler)

let rec render (_: float) =

    ui.ApplicationContext.clearRect(0., 0., ui.Canvas.width, ui.Canvas.height)
    ui.ApplicationContext.fillStyle <- !^"#fff"

    ui.Prepare()

    if ui.Window(mainWindow) then
        ui.Row([|1./6.; 3./6.; 1./6.; 1./6.|])
        ui.Empty()
        ui.Input(newNote) |> ignore
        ui.Combo(comboDisplay, ["All"; "Active"; "Completed"], None, labelAlign = Center)
        |> ignore
        ui.Empty()
        // Make space between input and list
        ui.Empty()

    for note in memory.Notes do
        ui.Row([| 1./8.; 5./8.; 1./8.; 1./8. |])
        ui.Checkbox(note.StateCheckbox, "") |> ignore
        ui.Label(note.Title)

        if ui.Button("Show") then
            note.DetailsWindow.Closed <- false

        if ui.Button("Delete") then
            memory.Remove(note.Guid)

    for note in memory.GetDetailsWindows() do
        if ui.Window(note.DetailsWindow, "purple") then
            ui.Label("maxime")


    ui.Finish()


    Browser.window.requestAnimationFrame(Browser.FrameRequestCallback(render))
    |> ignore

render 0.
