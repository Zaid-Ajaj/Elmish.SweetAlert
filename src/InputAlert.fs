namespace Elmish.SweetAlert 

open Fable.PowerPack

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
                Promise.create <| fun res rej -> 
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

    /// Specify the dialog alert type.
    member this.Type(alertType: AlertType) = 
        Interop.setProp "type"  (Interop.stringifyAlertType alertType) config
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
        member this.Run(dispatch) = 
            promise {
                let! result = unbox<Fable.Import.JS.Promise<obj>> (Interop.swal config) 
                let keys = (Fable.Import.JS.Object.keys result).ToArray() 
                if not (Array.contains "dismiss" keys)  
                then return InputAlertResult.Confirmed (Interop.getAs<string> result "value")   
                else 
                    let dismiss = Interop.getAs<obj> result "dismiss" 
                    if dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "cancel" 
                    then return InputAlertResult.Dismissed (DismissalReason.Cancel) 
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "esc" 
                    then return InputAlertResult.Dismissed DismissalReason.PressedEscape 
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "close" 
                    then return InputAlertResult.Dismissed DismissalReason.Close
                    elif dismiss = Interop.getAs<obj> (Interop.getAs<obj> Interop.swal "DismissReason") "backdrop" 
                    then return InputAlertResult.Dismissed DismissalReason.ClickedOutsideDialog
                    else return InputAlertResult.Dismissed DismissalReason.TimedOut

            } |> Fable.PowerPack.Promise.iter (fun confirmResult -> dispatch (handler confirmResult))