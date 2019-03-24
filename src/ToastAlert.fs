namespace Elmish.SweetAlert 

/// ToastAlert lets you create SweetAlert modals the look and act like toasts.
type ToastAlert<'a>(text: string) =
    let config = obj()

    do 
        Interop.setProp "toast" true config 
        Interop.setProp "text" text config 

    /// Sets the title for the toast. 
    member this.Title(title: string) = 
        Interop.setProp "title" title config 
        this

    /// Closes the dialog after the given total of milliseconds. 
    member this.Timeout(ms: int) = 
        Interop.setProp "timer" ms config 
        this

    /// Sets whether or not the dialog shows the confirm/OK button, it is set to true by default.
    member this.ConfirmButton(enable: bool) = 
        Interop.setProp "showConfirmButton" enable config
        this

    /// Sets the text for the (OK) confirm button
    member this.ConfirmButtonText(buttonText: string) = 
        Interop.setProp "confirmButtonText" buttonText config 
        this

    /// Sets the color for the confirm button
    member this.ConfirmButtonColor(color: string) = 
        Interop.setProp "confirmButtonColor" color config 
        this 

    /// Specify the dialog alert type.
    member this.Type(alertType: AlertType) = 
        Interop.setProp "type"  (Interop.stringifyAlertType alertType) config
        this 

    /// Sets the position of the dialog
    member this.Position(pos: AlertPosition) = 
        Interop.setProp "position" (Interop.stringifyPosition pos) config
        this

    /// Hides the OK (confirm) button from the dialog.
    member this.HideConfirmButton() = 
        Interop.setProp "showConfirmButton" false config 
        this

    interface ISweetAlert<'a> with 
        member this.Run(dispatch) : unit = 
            Interop.fire config |> ignore 