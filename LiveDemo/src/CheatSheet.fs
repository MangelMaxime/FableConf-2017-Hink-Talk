ui.Row([| 1./3.; 1./3.; 1./3. |])
if ui.Button("Click me!") then
    counterValue <- counterValue + 1
ui.Empty()
ui.Label(sprintf "You clicked %i times" counterValue)

let mutable counterValue = 0
