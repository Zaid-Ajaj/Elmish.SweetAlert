namespace Elmish.SweetAlert

open Fable.Core
open Fable.Core.JsInterop

/// SelectAlert lets create modals where the user can select from a dropdown.
type SelectAlert<'a>(options: FSharp.Collections.Map<string, string>, handler: SelectAlertResult -> 'a) =
    let config = obj()
    do
        Interop.setProp "input" "select" config
        Interop.setProp "showCancelButton" true config
        let inputOptions = obj()
        options |> Map.iter (fun key value -> Interop.setProp key value inputOptions)
        Interop.setProp "inputOptions" inputOptions config

    /// Sets the input type for the alert
    member this.InputType(inputType: InputAlertType) =
        Interop.setProp "input" (Interop.stringifyInputType inputType) config
        this

    /// Validate the key of value from the options
    member this.Validate(validate: (string * string) -> Result<string * string, string>) =
        let innerValidator =
            fun (key:string) ->
                let value = Map.find key options
                JS.Constructors.Promise.Create(fun res rej ->
                    match validate (key, value) with
                    | Ok _ -> (res (unbox<string> ()))
                    | Error errorMsg -> res(errorMsg)
                )

        Interop.setProp "inputValidator" innerValidator config
        this

    /// Add a placeholder for the input element
    member this.Placeholder(placeholder: string) =
        Interop.setProp "inputPlaceholder" placeholder config
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
                then
                    let key = Interop.getAs<string> result "value"
                    let value = Map.find key options
                    return handle (SelectAlertResult.Confirmed (key, value))
                else
                    let dismiss = Interop.getAs<obj> result "dismiss"
                    if dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "cancel"
                    then handle (SelectAlertResult.Dismissed DismissalReason.Cancel)
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "esc"
                    then handle (SelectAlertResult.Dismissed DismissalReason.PressedEscape)
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "close"
                    then handle (SelectAlertResult.Dismissed DismissalReason.Close)
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "backdrop"
                    then handle (SelectAlertResult.Dismissed DismissalReason.ClickedOutsideDialog)
                    else handle (SelectAlertResult.Dismissed DismissalReason.TimedOut)

            } |> Async.StartImmediate