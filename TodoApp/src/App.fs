module TodoApp

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Hink.Gui
open Hink.Inputs
open Hink.Widgets
open System

/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////
/////
///// IMPORTANT: This application is just to demonstrate Hink potentail
/////                    and serve as a demo for FableConf
/////          This doesn't represent final application written with Hink !!!
/////
/////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////////////////////////////////////////

let canvas = Browser.document.getElementById "application" :?> Browser.HTMLCanvasElement

let mainWindow = { WindowInfo.Default with Width = 1024.
                                           Height = 600.
                                           Title = Some "Main window" }

type EditWindow =
    { Window : WindowInfo
      Input : InputInfo }

    static member Create title =
        { Window = { WindowInfo.Default with X = 50.
                                             Y = 50.
                                             Width = 500.
                                             Height = 200.
                                             Title = Some title
                                             Draggable = true
                                             Closable = true
                                             Closed = true }
          Input = { InputInfo.Default with Value = title } }

type Note =
    { mutable Title : string
      Guid : Guid
      StateCheckbox : CheckboxInfo
      EditWindow : EditWindow }

    static member Create (title, ?completed) =
        { Title = title
          Guid = Guid.NewGuid()
          StateCheckbox = { CheckboxInfo.Default with Value = defaultArg completed false }
          EditWindow = EditWindow.Create title  }

type AppState () as this =
    let newNoteInput : InputInfo = InputInfo.Default
    let mutable notes : Note list = []
    let filters = ComboInfo.Default

    do
        newNoteInput.KeyboardCaptureHandler <-
            Some (fun inputInfo keyboard ->
                    match keyboard.Modifiers with
                    | _ ->
                        match keyboard.LastKey with
                        | Keyboard.Keys.Enter ->
                            if inputInfo.Value <> "" then
                                this.Add this.NewNoteInput.Value
                                this.NewNoteInput.Empty()
                            true // Key has been captured
                        | _ -> false )

    member this.Notes
        with get() = notes
        and set(value) = notes <- value

    member this.NewNoteInput = newNoteInput

    member this.Filters = filters

    member this.Add (title, ?completed) =
        this.Notes <- Note.Create(title, ?completed = completed) :: this.Notes

    member this.Remove guid =
        this.Notes <-
            this.Notes
            |> List.filter(fun note -> note.Guid <> guid)

    member this.GetDetailsWindows () =
        this.Notes
        |> List.filter(fun note ->
            not note.EditWindow.Window.Closed
        )

    member this.FilteredNotes
        with get () =
            match this.Filters.SelectedIndex with
            | None | Some 0 ->
                this.Notes
            | Some 1 ->
                this.Notes
                |> List.filter(fun note -> not note.StateCheckbox.Value )
            | Some 2 ->
                this.Notes
                |> List.filter(fun note -> note.StateCheckbox.Value )
            | Some index ->
                 failwithf "Unkown index: %i" index

    member this.OpenDetailWindow guid =
        this.NewNoteInput.IsActive <- false
        for note in this.Notes do
            if note.Guid = guid then
                note.EditWindow.Window.Closed <- false
            else
                note.EditWindow.Window.Closed <- true
                note.EditWindow.Input.IsActive <- false

let appState = AppState()

appState.Add("Create a demo app for #FableConf", true)
appState.Add("I am a really long note. So my text will be cut... That's really sad")

let ui = Hink.Create(canvas)

let rec render (_: float) =

    ui.ApplicationContext.clearRect(0., 0., ui.Canvas.width, ui.Canvas.height)
    ui.ApplicationContext.fillStyle <- !^"#fff"

    ui.Prepare()

    if ui.Window(mainWindow) then
        ui.Row([|1./6.; 1./6.; 3./6.; 1./6.|])
        ui.Empty()
        ui.Combo(appState.Filters, ["All"; "Active"; "Completed"], Some "Filter", labelAlign = Center)
        |> ignore
        ui.Input(appState.NewNoteInput) |> ignore
        ui.Empty()
        // Make space between input and list
        ui.Empty()

    for note in appState.FilteredNotes do
        ui.Row([| 1./15.; 1./15.; 7./15.; 1./15.; 2./15.; 2./15.; 1./15. |])
        ui.Empty()
        if ui.Checkbox(note.StateCheckbox) then
            if not note.EditWindow.Window.Closed then
                note.EditWindow.Window.ShouldRedraw <- true
        ui.Label(note.Title)
        ui.Empty()

        if ui.Button("Edit") then
            appState.OpenDetailWindow note.Guid

        if ui.Button("Delete") then
            appState.Remove(note.Guid)

        ui.Empty()

    for note in appState.GetDetailsWindows() do
        if ui.Window(note.EditWindow.Window, "#9b59b6") then
            ui.Label("Edit title:")
            if ui.Input(note.EditWindow.Input) then
                note.Title <- note.EditWindow.Input.Value
                note.EditWindow.Window.Title <- Some note.EditWindow.Input.Value
            ui.Empty()

            ui.Row([|1./4.; 1./2.; 1./4.|])
            ui.Empty()
            if note.StateCheckbox.Value then
                if ui.Button("Mark as active") then
                    note.StateCheckbox.Value <- false
            else
                if ui.Button("Mark as completed") then
                    note.StateCheckbox.Value <- true
            ui.Empty()

    ui.Finish()

    Browser.window.requestAnimationFrame(Browser.FrameRequestCallback(render))
    |> ignore

render 0.
