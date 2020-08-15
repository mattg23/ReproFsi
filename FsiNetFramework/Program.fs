open System.Text
open System.IO
open FSharp.Compiler.Interactive.Shell

let smallLambda = fun x -> let p = x
                           p

let code = """
let smallLambda = fun x -> let p = x
                           p
"""

[<EntryPoint>]
let main argv =

    printfn "try compiling %A" smallLambda

    let sbOut = StringBuilder()
    let sbErr = StringBuilder()

    let fsi =
        let inStream = new StringReader("")
        let outStream = new StringWriter(sbOut)
        let errStream = new StringWriter(sbErr)
        let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()       
        let argv = [| "fsi.exe"; "--noninteractive"; "--quiet";"--nologo"; |]
        let fsi_noloaded = FsiEvaluationSession.Create(fsiConfig, argv, inStream, outStream, errStream)
        let system = fsi_noloaded.EvalInteractionNonThrowing "open System;;"
        //fsi_noloaded.EvalInteractionNonThrowing "#indent \"off\";;"
        fsi_noloaded

    let evalresult, errors = fsi.EvalExpressionNonThrowing(code)

    match evalresult with
    | Choice1Of2(Some fsiValue) -> printfn "compile successful: %A" fsiValue
    | Choice2Of2(e) -> printfn "%A" e

    errors |> Array.map (printfn "%A") |> ignore

    0 // return an integer exit code