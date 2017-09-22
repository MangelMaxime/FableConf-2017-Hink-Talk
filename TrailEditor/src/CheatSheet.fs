
    ui.Prepare()

    if ui.Window(mainWindow) then
        ui.Label("Quantity:")

        ui.Row(([| 1./2.; 1./2. |]))
        if ui.Button("+1000") then
            quantity <- quantity + 1000
            createParticles()

        if ui.Button("-1000") then
            quantity <- quantity - 1000
            createParticles()

        ui.Label("Speed factor:")
        let previousSpeedFactor = speedFactorInput.Value
        if ui.Input(speedFactorInput) then
            if speedFactorInput.Value <> previousSpeedFactor then
                createParticles()

        if ui.Button("Reset") then
            speedFactorInput.Value <- "4"
            quantity <- 1000
            createParticles()


    ui.Finish()

speedFactorInput.KeyboardCaptureHandler <- Some (fun inputInfo keyboard ->
                                                    match keyboard.LastKey with
                                                    | Keyboard.Keys.ArrowUp ->
                                                        inputInfo.Value <- string (Helper.SpeedFactor + 1.)
                                                        true
                                                    | Keyboard.Keys.ArrowDown ->
                                                        inputInfo.Value <- string (Helper.SpeedFactor - 1.)
                                                        true
                                                    | _ -> false )


let mainWindow = { WindowInfo.Default with Width = 300.
                                           Height = 600.
                                           Title = Some "Particle editor" }
let ui = Hink.Create(canvas)
