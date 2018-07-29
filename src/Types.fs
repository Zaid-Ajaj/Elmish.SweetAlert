namespace Elmish.SweetAlert 

open Elmish 

[<RequireQualifiedAccess>]
type AlertType = 
    | Info 
    | Question 
    | Warning 
    | Error
    | Success

[<RequireQualifiedAccess>]
type AlertPosition = 
    | Top 
    | TopStart
    | TopEnd 
    | Center
    | CenterStart
    | CenterEnd 
    | Bottom 
    | BottomStart
    | BottomEnd 

[<RequireQualifiedAccess>]
type DismissalReason = 
    | Cancel 
    | PressedEscape 
    | TimedOut 
    | Close 
    | ClickedOutsideDialog

[<RequireQualifiedAccess>]
type InputAlertType = 
    | Text 
    | Password 
    | TextArea 

[<RequireQualifiedAccess>]
type ConfirmAlertResult = 
    | Confirmed 
    | Dismissed of reason:DismissalReason

[<RequireQualifiedAccess>]
type InputAlertResult = 
    | Confirmed of string 
    | Dismissed of reason:DismissalReason

[<RequireQualifiedAccess>]
type SelectAlertResult = 
    | Confirmed of string * string
    | Dismissed of reason:DismissalReason

type ISweetAlert<'a> = 
    abstract Run : ('a -> unit) -> unit 

type SweetAlert() = 
    static member inline Run(alert: ISweetAlert<_>) : Cmd<_> = [alert.Run]


