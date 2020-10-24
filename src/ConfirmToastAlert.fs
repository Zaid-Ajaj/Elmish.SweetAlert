namespace Elmish.SweetAlert 

open Fable.Core
open Fable.Core.JsInterop

/// Combines functionality from ConfirmAlert and ToastAlert
type ConfirmToastAlert<'a>(text: string, handler: ConfirmAlertResult -> 'a) = 
    let config = obj() 

    do 
        Interop.setProp "text" text config
        Interop.setProp "toast" true config 
        Interop.setProp "showCancelButton" true config  

    /// Adds a title to the alert dialog.
    member this.Title(title: string) = 
        Interop.setProp "title" title config
        this 

    /// Specify the dialog alert icon.
    member this.Icon(alertType: AlertIcon) = 
        Interop.setProp "icon" (Interop.stringifyAlertIcon alertType) config
        this 

    /// Set the icon via a html string.
    member this.IconHtml(htmlString: string) = 
        Interop.setProp "iconHtml" htmlString config
        this 

    /// Shows a close button on the dialog
    member this.ShowCloseButton(enable: bool) = 
        Interop.setProp "showCloseButton" enable config 
        this

    /// Sets whether or not the dialog shows the confirm/OK button, it is set to true by default.
    member this.ConfirmButton(enable: bool) = 
        Interop.setProp "showConfirmButton" enable config
        this

    /// Sets the text for the (OK) confirm button
    member this.ConfirmButtonText(buttonText: string) = 
        Interop.setProp "confirmButtonText" buttonText config 
        this

    /// Sets a custom class for the confirmation button, property `ButtonStyling` must be set to false.
    member this.ConfirmButtonClass(className: string) = 
        Interop.setProp "confirmButtonClass" className config 
        this

    /// Disables the default styling for the confirm buttons so you can customize it using the `ConfirmButtonClass` property
    member this.ButtonStyling(enable: bool) = 
        Interop.setProp "buttonsStyling" enable config
        this

    /// If set to true, the focus is active on the confirm button, otherwise, the cancel button gets the focus.
    member this.FocusConfirm(enable: bool) = 
        Interop.setProp "focusConfirm" enable config 
        this 

    /// Sets the color for the cancel button
    member this.CancelButtonColor(color: string) = 
        Interop.setProp "cancelButtonColor" color config 
        this 

    /// Sets the text of the cancel button 
    member this.CancelButtonText(buttonText: string) = 
        Interop.setProp "cancelButtonText" buttonText config
        this

    /// Sets the color for the confirm button
    member this.ConfirmButtonColor(color: string) = 
        Interop.setProp "confirmButtonColor" color config 
        this 

    /// Sets the position of the dialog
    member this.Position(pos: AlertPosition) = 
        Interop.setProp "position" (Interop.stringifyPosition pos) config
        this

    /// Closes the dialog after the given total of milliseconds. 
    member this.Timeout(ms: int) = 
        Interop.setProp "timer" ms config 
        this

    /// Sets a custom class for the dialog
    member this.CustomClass(className: string) =
        Interop.setProp "customClass" className config
        this

    /// Applies CSS class names to their given field based on the updated customClass object.
    member this.CustomClass(overrides: customClass -> unit) =
        Interop.setProp "customClass" (jsOptions<customClass>overrides) config
        this

    /// Disable animations
    member this.DisableAnimation(value: bool) =
        if value then 
            Interop.setProp "showClass" 
                (jsOptions<showClass>(fun o -> 
                    o.popup <- ""
                    o.backdrop <- ""
                    o.icon <- "")) config
        this

    /// Applies CSS class names to their given field, this is used for animation.
    member this.ShowClass(overrides: showClass -> unit) =
        Interop.setProp "showClass" (jsOptions<showClass>overrides) config
        this

    /// Applies CSS class names to their given field, this is used for animation.
    member this.HideClass(overrides: hideClass -> unit) =
        Interop.setProp "hideClass" (jsOptions<hideClass>overrides) config
        this

    /// If set to true, the cancel button becomes focussed when the modal appears. This is set to false by default
    member this.FocusCancel(enable: bool) = 
        Interop.setProp "focusCancel" enable config 
        this 

    interface ISweetAlert<'a> with 
        member this.Run(dispatch) = 
            async {
                let! result = Async.AwaitPromise (unbox (Interop.fire config)) 
                let value = Interop.getAs<bool> result "value"  
                let handle confirmResult = dispatch (handler confirmResult)
                if value then handle ConfirmAlertResult.Confirmed
                else 
                    let dismiss = Interop.getAs<obj> result "dismiss" 
                    if dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "cancel" 
                    then handle (ConfirmAlertResult.Dismissed DismissalReason.Cancel)
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "esc" 
                    then handle (ConfirmAlertResult.Dismissed DismissalReason.PressedEscape)
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "close" 
                    then handle (ConfirmAlertResult.Dismissed DismissalReason.Close)
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "backdrop" 
                    then handle (ConfirmAlertResult.Dismissed DismissalReason.ClickedOutsideDialog)
                    else handle (ConfirmAlertResult.Dismissed DismissalReason.TimedOut)

            } |> Async.StartImmediate
