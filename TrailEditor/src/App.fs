module TodoApp

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Hink.Gui
open Hink.Inputs
open Hink.Widgets
open System

// Example converted from: http://lab.hakim.se/trail/03

[<Emit("Math.random()")>]
let random = jsNative

let screenWidth = 1024.
let screenHeight = 600.

// Variables to mutate for demo
let mutable quantity = 1000
let speedFactorInput = { InputInfo.Default with Value = "4" }

type Helper =
    class end
    static member SpeedFactor
        with get () = float speedFactorInput.Value

type Point =
    { mutable X : float
      mutable Y : float }

type Particle =
    { mutable Position : Point
      mutable Velocity : Point
      mutable FillColor : string }

    member this.Draw (context : Browser.CanvasRenderingContext2D ) =
        context.fillStyle <- !^this.FillColor
        context.fillRect(this.Position.X, this.Position.Y, 2., 2.)

    member this.Update () =
        this.Position.X <- this.Position.X + this.Velocity.X
        this.Position.Y <- this.Position.Y + this.Velocity.Y

        if this.Position.X < 0. || this.Position.X > screenWidth then
            this.Velocity.X <- -this.Velocity.X

        if this.Position.Y < 0. || this.Position.Y > screenHeight then
            this.Velocity.Y <- -this.Velocity.Y

let mutable particles = []

let randomColor () =
    let mutable r = 0.
    let mutable g = 0.
    let mutable b = 0.

    while (r < 100. && g < 100. && b < 100.) do
        r <- Math.Floor(random * 256.)
        g <- Math.Floor(random * 256.)
        b <- Math.Floor(random * 256.)

    sprintf "rgb(%i, %i, %i)" (int r) (int g) (int b)

let createParticles () =
    particles <- []
    for i = 0 to quantity do
        particles <-
        { Position =
            { X = screenWidth * random
              Y = screenHeight * random }
          Velocity =
            { X = Helper.SpeedFactor * random - (Helper.SpeedFactor / 2.)
              Y = Helper.SpeedFactor * random - (Helper.SpeedFactor /2.) }
          FillColor = randomColor () } :: particles

let canvas = Browser.document.getElementById "application" :?> Browser.HTMLCanvasElement
let context = canvas.getContext_2d()

// Init the particles
createParticles()

let rec render (_: float) =

    context.clearRect(0., 0., canvas.width, canvas.height)
    // ui.ApplicationContext.fillStyle <- !^"#fff"

    for particle in particles do
        particle.Update()
        particle.Draw(context)

    Browser.window.requestAnimationFrame(Browser.FrameRequestCallback(render))
    |> ignore

render 0.
