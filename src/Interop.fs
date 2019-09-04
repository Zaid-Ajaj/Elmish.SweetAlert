namespace Elmish.SweetAlert

open Fable.Core
open Fable.Core.JsInterop
open Fable.Core.JS

[<RequireQualifiedAccess>]
module internal Interop =

    [<Emit("$2[$0] = $1")>]
    let setProp (propName: string) (propValue: obj) (any: obj) : unit = jsNative

    [<Emit("$0[$1]")>]
    let getAs<'a> (x: obj) (key: string) : 'a = jsNative
    [<Emit("console.log($0)")>]
    let log (x: 'a) : unit = jsNative
    let stringifyAlertType = function
        | AlertType.Info -> "info"
        | AlertType.Error -> "error"
        | AlertType.Question -> "question"
        | AlertType.Warning -> "warning"
        | AlertType.Success -> "success"

    let stringifyPosition = function
        | AlertPosition.Bottom -> "bottom"
        | AlertPosition.BottomEnd -> "bottom-end"
        | AlertPosition.BottomStart -> "bottom-start"
        | AlertPosition.Top -> "top"
        | AlertPosition.TopEnd -> "top-end"
        | AlertPosition.TopStart -> "top-start"
        | AlertPosition.Center -> "center"
        | AlertPosition.CenterEnd -> "center-end"
        | AlertPosition.CenterStart -> "center-start"

    let stringifyInputType = function
        | InputAlertType.Text -> "text"
        | InputAlertType.Password -> "password"
        | InputAlertType.TextArea -> "textarea"

    let swal : obj -> Promise<obj> = importDefault "sweetalert2"
    let fire (x: obj) : Promise<obj> = swal?fire(x)