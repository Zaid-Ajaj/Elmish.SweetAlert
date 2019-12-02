module App

open System
open Elmish
open Elmish.React
open Elmish.SweetAlert
open Fable.React
open Fable.React.Props

type Model =
    | SimpleAlertDocs
    | ToastAlertDocs
    | ConfirmAlertDocs
    | InputAlertDocs
    | SelectAlertDocs
    | ConfirmToastAlertDocs
    | Other

let init() = SimpleAlertDocs, Cmd.none

type AppMsg =
    | SwitchToSimpleAlertDocs
    | SwitchToToastAlertDocs
    | SwitchToConfirmAlertDocs
    | SwitchToInputAlertDocs
    | SwitchToSelectAlertDocs
    | SwitchToConfirmToastAlertDocs
    | SimplestAlert
    | SimpleAlertWithTitle
    | SuccessAlert
    | Timeout
    | HideConfirm
    | WithImage
    | SimpleToastAlert
    | ToastTopConfirm
    | CustomConfirmBtnText
    | ScrollbarPaddingFalse
    | ConfirmAlertMsg
    | ConfirmToastAlertMsg
    | ConfirmResultMsg of Result<string, string>
    | InputAlertMsg
    | InputResultMsg of Result<string, string>
    | SelectAlertMsg
    | SelectAlertConfirmed of (string * string)

let update msg state  =
    match msg with
    | SwitchToSimpleAlertDocs ->
        SimpleAlertDocs, Cmd.none

    | SwitchToToastAlertDocs ->
        ToastAlertDocs, Cmd.none

    | SwitchToConfirmAlertDocs ->
        ConfirmAlertDocs, Cmd.none

    | SwitchToInputAlertDocs ->
        InputAlertDocs, Cmd.none

    | SwitchToSelectAlertDocs ->
        SelectAlertDocs, Cmd.none

    | SwitchToConfirmToastAlertDocs ->
        ConfirmToastAlertDocs, Cmd.none

    | SimplestAlert ->
        let alert = SimpleAlert("Simple but sweet")
        state, SweetAlert.Run(alert)

    | SimpleAlertWithTitle ->
        let alert = SimpleAlert("Is that still a thing?").Title("The Internet")
        state, SweetAlert.Run(alert)

    | SuccessAlert ->
        let successAlert =
            SimpleAlert("Your account has been succesfully created!")
                .Title("Server")
                .Icon(AlertIcon.Success)

        state, SweetAlert.Run(successAlert)

    | Timeout ->
        let errorAlert =
            SimpleAlert("That didn't go as we expected")
                .Title("We are sorry...")
                .Icon(AlertIcon.Error)
                .Timeout(3000)

        state, SweetAlert.Run(errorAlert)

    | HideConfirm ->
        let successAlert =
            SimpleAlert("You have levelled up!")
                .Title("Congrats")
                .Icon(AlertIcon.Success)
                .ConfirmButton(false)
                .Timeout(2000)

        state, SweetAlert.Run(successAlert)

    | WithImage ->
        let alert =
            SimpleAlert("Modal with a custom image.")
                .Title("Sweet!")
                .ImageUrl("https://unsplash.it/400/200")
                .ImageHeight(200)
                .ImageWidth(400)
                .DisableAnimation(true)

        state, SweetAlert.Run(alert)

    | SimpleToastAlert ->
        let toastAlert =
            ToastAlert("Things about to go down")
                .Title("Oh boy!")
                .Timeout(3000)
                .ConfirmButton(false)
                .Position(AlertPosition.TopEnd)
                .Icon(AlertIcon.Info)

        state, SweetAlert.Run(toastAlert)

    | CustomConfirmBtnText ->
        let alert =
            SimpleAlert("We are going to launch the missiles")
                .Title("Heads up!")
                .Icon(AlertIcon.Warning)
                .ConfirmButtonText("Oki doki!")

        state, SweetAlert.Run(alert)

    | ScrollbarPaddingFalse ->
        let alert =
            SimpleAlert("A simple alert without padding")
                .ScrollbarPadding(false)

        state, SweetAlert.Run(alert)

    | ToastTopConfirm ->
        let toastAlert =
            ToastAlert("Things are going good!")
                .Position(AlertPosition.Top)
                .Icon(AlertIcon.Success)
                .Timeout(2000)

        state, SweetAlert.Run(toastAlert)

    | ConfirmAlertMsg ->
        let handleConfirm = function
        | ConfirmAlertResult.Confirmed -> AppMsg.ConfirmResultMsg (Ok "You confirmed")
        | ConfirmAlertResult.Dismissed reason ->
            match reason with
            | DismissalReason.Cancel -> AppMsg.ConfirmResultMsg (Error "Clicked cancel")
            | DismissalReason.Close -> AppMsg.ConfirmResultMsg (Error "Clicked close button")
            | DismissalReason.PressedEscape -> AppMsg.ConfirmResultMsg (Error "Pressed escape")
            | DismissalReason.TimedOut -> AppMsg.ConfirmResultMsg (Error "Modal closed after timeout")
            | DismissalReason.ClickedOutsideDialog ->  AppMsg.ConfirmResultMsg (Error "Clicked outside dialog")

        let confirmAlert =
            ConfirmAlert("You won't be able to undo this action", handleConfirm)
                .Title("Are you sure you want to delete the file?")
                .ShowCloseButton(true)
                .Timeout(10000)
                .Icon(AlertIcon.Question)

        state, SweetAlert.Run(confirmAlert)

    | ConfirmToastAlertMsg ->
        let handleConfirm = function
        | ConfirmAlertResult.Confirmed -> AppMsg.ConfirmResultMsg (Ok "You confirmed")
        | ConfirmAlertResult.Dismissed reason -> AppMsg.ConfirmResultMsg (Error "Dismissed")

        let confirmAlert =
            ConfirmToastAlert("You won't be able to undo this action", handleConfirm)
                .Title("Are you sure you want to delete the file?")
                .ShowCloseButton(true)
                .Timeout(10000)
                .Icon(AlertIcon.Question)

        state, SweetAlert.Run(confirmAlert)

    | ConfirmResultMsg (Ok _) ->
        let successAlert =
            SimpleAlert("You imaginary file was succesfully deleted")
                .Title("Success")
                .Icon(AlertIcon.Success)

        state, SweetAlert.Run(successAlert)

    | ConfirmResultMsg (Error errorMsg) ->
        let errorAlert =
            SimpleAlert("Your imaginary file was left alone, reason: " + errorMsg)
                .Title("We didn't delete anything")
                .Icon(AlertIcon.Info)

        state, SweetAlert.Run(errorAlert)

    | InputAlertMsg ->
        let validate input =
            if String.IsNullOrEmpty input
            then Error "Input name cannot be empty"
            else Ok input

        let handleInput = function
        | InputAlertResult.Confirmed input -> AppMsg.InputResultMsg (Ok input)
        | InputAlertResult.Dismissed reason ->
            match reason with
            | DismissalReason.Cancel -> AppMsg.InputResultMsg (Error "Clicked cancel")
            | DismissalReason.Close -> AppMsg.InputResultMsg (Error "Clicked close button")
            | DismissalReason.PressedEscape -> AppMsg.InputResultMsg (Error "Pressed escape")
            | DismissalReason.TimedOut -> AppMsg.InputResultMsg (Error "Modal closed after timeout")
            | DismissalReason.ClickedOutsideDialog ->  AppMsg.InputResultMsg (Error "Clicked outside dialog")

        let inputAlert =
            InputAlert(handleInput)
                .Text("What's your name?")
                .Title("Credentials")
                .Placeholder("Name")
                .Validate(validate)
                .Icon(AlertIcon.Question)

        state, SweetAlert.Run(inputAlert)

    | InputResultMsg (Ok msg) ->
        let successAlert = SimpleAlert("Your name: " + msg).Icon(AlertIcon.Success)
        state, SweetAlert.Run(successAlert)

    | InputResultMsg (Error msg) ->
        let errorAlert = SimpleAlert("Error: " + msg).Icon(AlertIcon.Error)
        state, SweetAlert.Run(errorAlert)

    | SelectAlertMsg ->

        let options =
            [  "coffee", "Coffee Driven Development"
               "domain", "Domain Driven Developement"
               "test", "Test Driven Developement" ]
            |> Map.ofList

        let validate = function
            | (key, value) when key <> "coffee" -> Error "Nope"
            | (key, value) -> Ok (key, value)

        let handleInput = function
        | SelectAlertResult.Confirmed (key, value) -> AppMsg.SelectAlertConfirmed (key, value)
        | SelectAlertResult.Dismissed reason -> AppMsg.InputResultMsg (Error "Dismissed")

        let alert =
            SelectAlert(options, handleInput)
                .Text("What is the best software methodology?")
                .Icon(AlertIcon.Question)
                .Validate(validate)

        state, SweetAlert.Run(alert)

    | SelectAlertConfirmed (key, value) ->
        let alert = SimpleAlert("Everyone loves " + value).Icon(AlertIcon.Success)
        state, SweetAlert.Run(alert)

let simplestAlert = """
| SimplestAlert ->
    let alert = SimpleAlert("Simple but sweet")
    state, SweetAlert.Run(alert)
"""

let simplestAlertWithTitle = """
| SimpleAlertWithTitle ->
    let alert = SimpleAlert("Is that still a thing?").Title("The Internet")
    state, SweetAlert.Run(alert)
"""

let successAlert = """
| SuccessAlert ->
    let successAlert =
        SimpleAlert("Your account has been succesfully created!")
            .Title("Server")
            .Icon(AlertIcon.Success)

    state, SweetAlert.Run(successAlert)
"""

let selectAlertMsg = """
| SelectAlertMsg ->

    let options =
        [  "coffee", "Coffee Driven Development"
           "domain", "Domain Driven Developement"
           "test", "Test Driven Developement" ]
        |> Map.ofList

    let validate = function
        | (key, value) when key <> "coffee" -> Error "Nope"
        | (key, value) -> Ok (key, value)

    let handleInput = function
    | SelectAlertResult.Confirmed (key, value) -> AppMsg.SelectAlertConfirmed (key, value)
    | SelectAlertResult.Dismissed reason -> AppMsg.InputResultMsg (Error "Dismissed")

    let alert =
        SelectAlert(options, handleInput)
            .Text("What is the best software methodology?")
            .Icon(AlertIcon.Question)
            .Validate(validate)

    state, SweetAlert.Run(alert)

| SelectAlertConfirmed (key, value) ->
    let alert = SimpleAlert("Everyone loves " + value).Icon(AlertIcon.Success)
    state, SweetAlert.Run(alert)
"""

let hideConfirm = """
| HideConfirm ->
    let successAlert =
        SimpleAlert("You have levelled up!")
            .Title("Congrats")
            .Icon(AlertIcon.Success)
            .ConfirmButton(false)
            .Timeout(2000)

    state, SweetAlert.Run(successAlert)
"""

let timeout = """
| Timeout ->
    let errorAlert =
        SimpleAlert("That didn't go as we expected")
            .Title("We are sorry...")
            .Icon(AlertIcon.Error)
            .Timeout(3000)

    state, SweetAlert.Run(errorAlert)
"""

let confirmAlertMsg = """
| ConfirmAlertMsg ->
    // map user action to application message, whether they confirm the dialog or not
    // if the dialog is dismissed, you can handle each dismissal reason
    let handleConfirm = function
    | ConfirmAlertResult.Confirmed -> AppMsg.ConfirmResultMsg (Ok "You confirmed")
    | ConfirmAlertResult.Dismissed reason ->
        match reason with
        | DismissalReason.Cancel -> AppMsg.ConfirmResultMsg (Error "Clicked cancel")
        | DismissalReason.Close -> AppMsg.ConfirmResultMsg (Error "Clicked close button")
        | DismissalReason.PressedEscape -> AppMsg.ConfirmResultMsg (Error "Pressed escape")
        | DismissalReason.TimedOut -> AppMsg.ConfirmResultMsg (Error "Modal closed after timeout")
        | DismissalReason.ClickedOutsideDialog ->  AppMsg.ConfirmResultMsg (Error "Clicked outside dialog")

    let confirmAlert =
        ConfirmAlert("You won't be able to undo this action", handleConfirm)
            .Title("Are you sure you want to delete the file?")
            .Icon(AlertIcon.Question)
            .ShowCloseButton(true)
            .Timeout(10000)

    state, SweetAlert.Run(confirmAlert)

| AppMsg.ConfirmResultMsg (Ok _) ->
    let successAlert =
        SimpleAlert("You imaginary file was succesfully deleted")
            .Title("Success")
            .Icon(AlertIcon.Success)

    state, SweetAlert.Run(successAlert)

| AppMsg.ConfirmResultMsg (Error errorMsg) ->
    let errorAlert =
        SimpleAlert("Your imaginary file was left alone, reason: " + errorMsg)
            .Title("We didn't delete anything")
            .Icon(AlertIcon.Info)

    state, SweetAlert.Run(errorAlert)
"""

let customConfirmBtnText = """
| CustomConfirmBtnText ->
    let alert =
        SimpleAlert("We are going to launch the missiles")
            .Title("Heads up!")
            .Icon(AlertIcon.Warning)
            .ConfirmButtonText("Oki doki!")

    state, SweetAlert.Run(alert)
"""

let scrollbarPaddingFalseText = """
| ScrollbarPaddingFalse ->
    let alert =
        SimpleAlert("A simple alert without padding")
            .ScrollbarPadding(false)

    state, SweetAlert.Run(alert)
"""

let withImage = """
| WithImage ->
    let alert =
        SimpleAlert("Modal with a custom image.")
            .Title("Sweet!")
            .ImageUrl("https://unsplash.it/400/200")
            .ImageHeight(200)
            .ImageWidth(400)
            .DisableAnimation(true)

    state, SweetAlert.Run(alert)
"""

let inputAlertMsg = """
| InputAlertMsg ->

    let validate input =
        if String.IsNullOrEmpty input
        then Error "Input name cannot be empty"
        else Ok input

    let handleInput = function
    | InputAlertResult.Confirmed input -> AppMsg.InputResultMsg (Ok input)
    | InputAlertResult.Dismissed reason ->
        match reason with
        | DismissalReason.Cancel -> AppMsg.InputResultMsg (Error "Clicked cancel")
        | DismissalReason.Close -> AppMsg.InputResultMsg (Error "Clicked close button")
        | DismissalReason.PressedEscape -> AppMsg.InputResultMsg (Error "Pressed escape")
        | DismissalReason.TimedOut -> AppMsg.InputResultMsg (Error "Modal closed after timeout")
        | DismissalReason.ClickedOutsideDialog ->  AppMsg.InputResultMsg (Error "Clicked outside dialog")

    let inputAlert =
        InputAlert(handleInput)
            .Text("What's your name?")
            .Title("Credentials")
            .Placeholder("Name")
            .Validate(validate)
            .Icon(AlertIcon.Question)

    state, SweetAlert.Run(inputAlert)

| InputResultMsg (Ok msg) ->
    let successAlert = SimpleAlert("Your name: " + msg).Icon(AlertIcon.Success)
    state, SweetAlert.Run(successAlert)

| InputResultMsg (Error msg) ->
    let errorAlert = SimpleAlert("Error: " + msg).Icon(AlertIcon.Error)
    state, SweetAlert.Run(errorAlert)
"""

let confirmToastAlert = """
| ConfirmToastAlertMsg ->
    let handleConfirm = function
    | ConfirmAlertResult.Confirmed -> AppMsg.ConfirmResultMsg (Ok "You confirmed")
    | ConfirmAlertResult.Dismissed reason -> AppMsg.ConfirmResultMsg (Error "Dismissed")

    let confirmAlert =
        ConfirmToastAlert("You won't be able to undo this action", handleConfirm)
            .Title("Are you sure you want to delete the file?")
            .ShowCloseButton(true)
            .Timeout(10000)
            .Icon(AlertIcon.Question)

    state, SweetAlert.Run(confirmAlert)
"""
let simpleToastAlert = """
| SimpleToastAlert ->
    let toastAlert =
        ToastAlert("Things about to go down")
            .Title("Oh boy!")
            .Timeout(3000)
            .ConfirmButton(false)
            .Position(AlertPosition.TopEnd)
            .Icon(AlertIcon.Info)

    state, SweetAlert.Run(toastAlert)
"""
let toastTopConfirm = """
| ToastTopConfirm ->
    let toastAlert =
        ToastAlert("Things are going good!")
            .Position(AlertPosition.Top)
            .Icon(AlertIcon.Success)
            .Timeout(2000)

    state, SweetAlert.Run(toastAlert)
"""
let closeButtonSample = """
Toastr.message "I have a close button"
|> Toastr.title "Close"
|> Toastr.showCloseButton
|> Toastr.error
"""
let interactive = """
| Interactive ->
    let cmd =
        Toastr.message "Click me to dispatch 'Clicked' message"
        |> Toastr.title "Click Me"
        |> Toastr.onClick Clicked
        |> Toastr.info
    model, cmd

| Clicked ->
    let cmd =
        Toastr.message "I was summoned by a message"
        |> Toastr.title "Clicked"
        |> Toastr.info
    model, cmd
"""

let renderSimpleAlert dispatch =
    div [ Style [ Padding 20 ] ] [
        h3 [ ] [ str "SimpleAlert API" ]
        p [ ] [ str "SimpleAlert lets you create highly customizable SweetAlert modals that show information. " ]
        br [ ]
        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch SimplestAlert) ]
                    [ str "dispatch SimplestAlert" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str simplestAlert ] ]
            ]
        ]

        hr [ ]

        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch SimpleAlertWithTitle) ]
                    [ str "dispatch SimpleAlertWithTitle" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str simplestAlertWithTitle ] ]
            ]
        ]

        hr [ ]

        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch SuccessAlert) ]
                    [ str "dispatch SuccessAlert" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str successAlert ] ]
            ]
        ]

        hr [ ]

        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch Timeout) ]
                    [ str "dispatch Timeout" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str timeout ] ]
            ]
        ]

        hr [ ]

        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch HideConfirm) ]
                    [ str "dispatch HideConfirm" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str hideConfirm ] ]
            ]
        ]

        hr [ ]

        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch WithImage) ]
                    [ str "dispatch WithImage" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str withImage ] ]
            ]
        ]

        hr [ ]

        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch CustomConfirmBtnText) ]
                    [ str "dispatch CustomConfirmBtnText" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str customConfirmBtnText ] ]
            ]
        ]

        hr [ ]

        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch ScrollbarPaddingFalse) ]
                    [ str "dispatch ScrollbarPaddingFalse" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str scrollbarPaddingFalseText ] ]
            ]
        ]

    ]

let renderToastAlert dispatch =
    div [ Style [ Padding 20 ] ] [
        h3 [ ] [ str "ToastAlert API" ]
        p [ ] [ str "ToastAlert lets you create modals that look and act like notification toasts" ]
        br [ ]
        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch SimpleToastAlert) ]
                    [ str "dispatch SimpleToastAlert" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str simpleToastAlert ] ]
            ]
        ]

        hr [ ]

        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch ToastTopConfirm) ]
                    [ str "dispatch ToastTopConfirm" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str toastTopConfirm ] ]
            ]
        ]
    ]


let renderConfirmAlert dispatch =
    div [ Style [ Padding 20 ] ] [
        h3 [ ] [ str "ConfirmAlert API" ]
        p [ ] [ str "ConfirmAlert lets create modals that have both \"OK\" and \"Cancel\" buttons and be able to dispatch messages based on the action the user takes." ]
        p [ ] [ str "The following example demonstrates the whole story" ]
        br [ ]
        div [ ClassName "row"; ] [
            div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                div [ ClassName "btn btn-info";
                      OnClick (fun _ -> dispatch ConfirmAlertMsg ) ]
                    [ str "dispatch ConfirmAlertMsg" ]
            ]

            div [ ClassName "col-md-9" ] [
                code [ ] [ pre [ ] [ str confirmAlertMsg ] ]
            ]
        ]
    ]


let renderInputAlert dispatch =
        div [ Style [ Padding 20 ] ] [
            h3 [ ] [ str "InputAlert API" ]
            p [ ] [ str "InputAlert lets create modals that have textual input with validation capabilities, along with the \"OK\" and \"Cancel\" buttons. "]
            br [ ]
            div [ ClassName "row"; ] [
                div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                    div [ ClassName "btn btn-info";
                          OnClick (fun _ -> dispatch InputAlertMsg ) ]
                        [ str "dispatch InputAlertMsg" ]
                ]

                div [ ClassName "col-md-9" ] [
                    code [ ] [ pre [ ] [ str inputAlertMsg ] ]
                ]
            ]
        ]

let renderSelectAlert dispatch =
        div [ Style [ Padding 20 ] ] [
            h3 [ ] [ str "SelectAlert API" ]
            p [ ] [ str "SelectAlert lets create modals where the user can select from a dropdown. Like InputAlert, it also has validation capabilities, along with the \"OK\" and \"Cancel\" buttons. "]
            br [ ]
            div [ ClassName "row"; ] [
                div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                    div [ ClassName "btn btn-info";
                          OnClick (fun _ -> dispatch SelectAlertMsg ) ]
                        [ str "dispatch SelectAlertMsg" ]
                ]

                div [ ClassName "col-md-9" ] [
                    code [ ] [ pre [ ] [ str selectAlertMsg ] ]
                ]
            ]
        ]

let renderConfirmToastAlert dispatch =
        div [ Style [ Padding 20 ] ] [
            h3 [ ] [ str "ConfirmToastAlert API" ]
            p [ ] [ str "Combines ConfirmAlert with ToastAlert functionality"]
            br [ ]
            div [ ClassName "row"; ] [
                div [ ClassName "col-md-3"; Style [ PaddingTop 20 ] ] [
                    div [ ClassName "btn btn-info";
                          OnClick (fun _ -> dispatch ConfirmToastAlertMsg ) ]
                        [ str "dispatch ConfirmToastAlertMsg" ]
                ]

                div [ ClassName "col-md-9" ] [
                    code [ ] [ pre [ ] [ str confirmToastAlert ] ]
                ]
            ]
        ]

let render state dispatch =
    let currentDocs =
      match state with
      | SimpleAlertDocs -> renderSimpleAlert dispatch
      | ToastAlertDocs -> renderToastAlert dispatch
      | ConfirmAlertDocs -> renderConfirmAlert dispatch
      | InputAlertDocs -> renderInputAlert dispatch
      | SelectAlertDocs -> renderSelectAlert dispatch
      | ConfirmToastAlertDocs -> renderConfirmToastAlert dispatch
      | _ -> h1 [ ] [ str "Unknown API" ]

    div [ Style [ Padding 20 ] ] [
        span [ ] [
            h2 [ ] [ str "Elmish.SweetAlert docs" ]
            a [ Href "https://sweetalert2.github.io/" ] [ str "SweetAlert2" ]
            str " integration in Fable, made with <Heart /> by "
            a [ Href "https://github.com/Zaid-Ajaj" ] [ str "Zaid-Ajaj" ]
            str ". Implemented as Elmish commands, for installation instructions see the repo at "
            a [ Href "https://github.com/Zaid-Ajaj/Elmish.SweetAlert" ] [ str "github." ]
        ]

        hr [ ]

        div [ ClassName "row" ] [
            div [ ClassName (if state = SimpleAlertDocs then "btn btn-success" else "btn btn-secondary")
                  OnClick (fun _ -> dispatch SwitchToSimpleAlertDocs)
                  Style [ Height 70; Padding 20; Margin 10 ] ]
                [ str "SimpleAlert" ]
            div [ ClassName (if state = ToastAlertDocs then "btn btn-success" else "btn btn-secondary")
                  OnClick (fun _ -> dispatch SwitchToToastAlertDocs)
                  Style [ Height 70; Padding 20; Margin 10 ]  ]
                [ str "ToastAlert" ]
            div [ ClassName (if state = ConfirmAlertDocs then "btn btn-success" else "btn btn-secondary")
                  OnClick (fun _ -> dispatch SwitchToConfirmAlertDocs)
                  Style [ Height 70; Padding 20; Margin 10 ]  ]
                [ str "ConfirmAlert" ]
            div [ ClassName (if state = ConfirmToastAlertDocs then "btn btn-success" else "btn btn-secondary")
                  OnClick (fun _ -> dispatch SwitchToConfirmToastAlertDocs)
                  Style [ Height 70; Padding 20; Margin 10 ]  ]
                [ str "ConfirmToastAlert" ]
            div [ ClassName (if state = InputAlertDocs then "btn btn-success" else "btn btn-secondary")
                  OnClick (fun _ -> dispatch SwitchToInputAlertDocs)
                  Style [ Height 70; Padding 20; Margin 10 ]  ]
                [ str "InputAlert" ]
            div [ ClassName (if state = SelectAlertDocs then "btn btn-success" else "btn btn-secondary")
                  OnClick (fun _ -> dispatch SwitchToSelectAlertDocs)
                  Style [ Height 70; Padding 20; Margin 10 ]  ]
                [ str "SelectAlert" ]
        ]

        br [ ]
        currentDocs
    ]

Program.mkProgram init update render
|> Program.withReactSynchronous "root"
|> Program.withConsoleTrace
|> Program.run