namespace Elmish.SweetAlert 

open Fable.Core.JS

/// SimpleAlert lets you create highly customizable SweetAlert modals that show information. 
type SimpleAlert<'a>(text: string) = 
    let config = obj()
    do Interop.setProp "text" text config 

    /// Adds a title to the alert dialog.
    member this.Title(title: string) = 
        Interop.setProp "title" title config
        this 

    /// Specify the dialog alert type.
    member this.Type(alertType: AlertType) = 
        Interop.setProp "type"  (Interop.stringifyAlertType alertType) config
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

    /// Sets whether the dialog uses animation or not, it is set to true by default.
    member this.UseAnimation(enable: bool) = 
        Interop.setProp "animation" enable config 
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

    interface ISweetAlert<'a> with 
        member this.Run (dispatch) = 
            Interop.fire config |> ignore 