namespace Elmish.SweetAlert

open Fable.Core
open Fable.Core.JsInterop

/// SimpleAlert lets you create highly customizable SweetAlert modals that show information.
type SimpleAlert<'a>(text: string) =
    let config = obj()
    do Interop.setProp "text" text config

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
        member this.Run (dispatch) =
            Interop.fire config |> ignore