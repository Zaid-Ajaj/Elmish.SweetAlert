namespace Elmish.SweetAlert

open Fable.Core
open Fable.Core.JsInterop

/// InputAlert lets you create modals with textual input, along with validation of confirmation of the input.
type InputAlert<'a>(handler: InputAlertResult -> 'a) =
    let config = obj()
    do
        Interop.setProp "input" "text" config
        Interop.setProp "showCancelButton" true config

    /// Sets the input type for the alert
    member this.InputType(inputType: InputAlertType) =
        Interop.setProp "input" (Interop.stringifyInputType inputType) config
        this

    /// Add a placeholder for the input element
    member this.Placeholder(placeholder: string) =
        Interop.setProp "inputPlaceholder" placeholder config
        this

    /// Validate the input before submitting
    member this.Validate(validate: string -> Result<string, string>) =
        let innerValidator =
            fun (value:string) ->
                JS.Constructors.Promise.Create <| fun res rej ->
                    match validate value with
                    | Ok _ -> (res (unbox<string> ()))
                    | Error errorMsg -> res(errorMsg)

        Interop.setProp "inputValidator" innerValidator config
        this

    /// Sets the text for the modal
    member this.Text(text: string) =
        Interop.setProp "text" text config
        this

    /// If set to true, the cancel button becomes focussed when the modal appears. This is set to false by default
    member this.FocusCancel(enable: bool) =
        Interop.setProp "focusCancel" enable config
        this

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

    /// Sets a custom class for the cancel button, the property `ButtonStyling` must be set to false
    member this.CancelButtonClass(className: string) =
        Interop.setProp "cancelButtonClass" className config
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

    /// Adds an image to the dialog from the given image url
    member this.ImageUrl(url: string) =
        Interop.setProp "imageUrl" url config
        this

    /// Sets the height of the dialog image, if one is set.
    member this.ImageHeight(height: int) =
        Interop.setProp "imageHeight" height config
        this

    /// Sets the width of the dialog image, if one is set.
    member this.ImageWidth(width: int) =
        Interop.setProp "imageWidth" width config
        this

    /// Sets a custom alt for the image, if any
    member this.ImageAlt(alt: string) =
        Interop.setProp "imageAlt" alt config
        this

    /// Set to false to disable body padding adjustment when the page scrollbar gets hidden while the modal is shown. This is set to true by default
    member this.ScrollbarPadding(enable: bool) =
        Interop.setProp "scrollbarPadding" enable config
        this

    interface ISweetAlert<'a> with
        member this.Run(dispatch) =
            async {
                let! result = Async.AwaitPromise (unbox (Interop.fire config))
                let keys = (JS.Constructors.Object.keys result).ToArray()
                let handle confirmResult = dispatch (handler confirmResult)
                if not (Array.contains "dismiss" keys)
                then handle (InputAlertResult.Confirmed (Interop.getAs<string> result "value"))
                else
                    let dismiss = Interop.getAs<obj> result "dismiss"
                    if dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "cancel"
                    then handle (InputAlertResult.Dismissed DismissalReason.Cancel)
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "esc"
                    then handle (InputAlertResult.Dismissed DismissalReason.PressedEscape)
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "close"
                    then handle (InputAlertResult.Dismissed DismissalReason.Close)
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "backdrop"
                    then handle (InputAlertResult.Dismissed DismissalReason.ClickedOutsideDialog)
                    else handle (InputAlertResult.Dismissed DismissalReason.TimedOut)

            } |> Async.StartImmediate