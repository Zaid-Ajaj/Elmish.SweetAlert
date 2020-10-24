namespace Elmish.SweetAlert 

open Elmish 

[<RequireQualifiedAccess>]
type AlertIcon = 
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

type customClass =
    abstract container: string with get, set
    abstract popup: string with get, set
    abstract header: string with get, set
    abstract title: string with get, set
    abstract closeButton: string with get, set
    abstract icon: string with get, set
    abstract image: string with get, set
    abstract content: string with get, set
    abstract input: string with get, set
    abstract actions: string with get, set
    abstract confirmButton: string with get, set
    abstract cancelButton: string with get, set
    abstract footer: string with get, set

[<RequireQualifiedAccess>]
type DismissalReason = 
    | Cancel 
    | PressedEscape 
    | TimedOut 
    | Close 
    | ClickedOutsideDialog

type hideClass =
    abstract popup: string with get, set
    abstract backdrop: string with get, set
    abstract icon: string with get, set

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

type showClass =
    abstract popup: string with get, set
    abstract backdrop: string with get, set
    abstract icon: string with get, set

type ISweetAlert<'a> = 
    abstract Run : ('a -> unit) -> unit 

type SweetAlert() = 
    static member inline Run(alert: ISweetAlert<_>) : Cmd<_> = [alert.Run]


