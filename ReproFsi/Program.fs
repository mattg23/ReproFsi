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
        let argv = [| "fsi.exe"; "--noninteractive"; "--quiet";"--nologo";"--langversion:latest" |]
        let session = FsiEvaluationSession.Create(fsiConfig, argv, inStream, outStream, errStream)
        session.EvalInteractionNonThrowing "open System;;" |> ignore
        session.EvalInteractionNonThrowing "#indent \"off\";;" |> ignore
        session

    let evalresult, errors = fsi.EvalExpressionNonThrowing(code)

    match evalresult with
    | Choice1Of2(Some fsiValue) -> printfn "compile successful: %A" fsiValue
    | Choice2Of2(e) -> printfn "%A" e

    errors |> Array.map (printfn "%A") |> ignore

    0 // return an integer exit code