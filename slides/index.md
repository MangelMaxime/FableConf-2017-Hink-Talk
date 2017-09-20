- title : Hink
- description : Write web applications without CSS
- author : Maxime Mangel
- theme : night
- transition : default

***

## Hink

<br />
<br />

### Write web applications without CSS

<br />
<br />
Maxime Mangel - [@mangelmaxime](http://www.twitter.com/mangelmaxime)

***

### Maxime Mangel

* Contributor to Fable ecosystem
* Maintainer of:
    * Fulma
    * Fulma.Extensions
    * Fulma.Elmish
    * Hink

***

* Why Hink ?
    * I don't like CSS
    * Retained mode versus Immediate mode
* Key concepts
    * Immediate mode
    * Auto layout
    * Minimal theming
    * Coherent and clean design

***

## Why Hink ?

### I don't like CSS

***

## Why Hink ?

### Retained mode

<img src="images/retained-mode.png" style="background: transparent; border-style: none;"  width=700 />

---

## Why Hink ?

### Immediate mode

<img src="images/immediate-mode.png" style="background: transparent; border-style: none;"  width=700 />

***

## Key concepts

### Immediate mode

```fs
    if ui.Button("Click me") then
        console.log("You clicked on the button")
```

---

## Key concepts

### Auto layout

```fs
    ui.Label("Line n°1")
    ui.Label("Line n°2")
    ui.Label("Line n°3")
```

---

## Key concepts

### Auto layout

```fs
    ui.Row([| 1./3.; 1./3.; 1./3. |])
    if ui.Button("Click me") then
        console.log("You clicked on the button")
    ui.Empty()
    ui.Label("Counter value: " + string counterValue)
```

---

## Key concepts

### Minimal theming

```fs
    Button =
        { Width = 80.
          Height = 34.
          CornerRadius = 4.
          Background =
            { Pressed = Color.rgb 22 160 133
              Hover = Color.rgb 72 201 176
              Default = Color.rgb 26 188 156 } }
```

***

### Demo

***

### TakeAways

* Learn all the FP you can!
* Simple modular design

***

### Thank you!

* https://github.com/MangelMaxime/Hink
* https://ionide.io
* http://fable.io
