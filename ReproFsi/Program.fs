open System.Text
open System.IO
open FSharp.Compiler.Interactive.Shell

let smallLambda = fun x -> let p = x
                           p

let smallLambda2 =    
    fun x ->
        let p = x
        p

let codeSmallLambda = """
let smallLambda = fun x -> let p = x
                           p

"""

// this works in F# Interactive window
let codeSmallLambda2 = """
let smallLambda2 =    
    fun x -> x
        let p = x
        p
"""

[<EntryPoint>]
let main argv =

    printfn "smallLambda: %A , smallLambda2: %A" smallLambda smallLambda2

    let sbOut = StringBuilder()
    let sbErr = StringBuilder()

    let fsi =
        let inStream = new StringReader "" 
        let outStream = new StringWriter(sbOut)
        let errStream = new StringWriter(sbErr)
        let fsiConfig = FsiEvaluationSession.GetDefaultConfiguration()       
        let argv = [| "fsi.exe"; "--noninteractive"; "--quiet";"--nologo";"--langversion:latest" |]
        let session = FsiEvaluationSession.Create(fsiConfig, argv, inStream, outStream, errStream)
        session.EvalInteractionNonThrowing "open System;;" |> ignore
        session.EvalInteractionNonThrowing "#indent \"off\";;" |> ignore
        session

    printfn "TEST (EvalExpressionNonThrowing): %s" codeSmallLambda

    let evalresult, errors = fsi.EvalExpressionNonThrowing codeSmallLambda 

    match evalresult with
    | Choice1Of2(Some fsiValue) -> printfn "compile successful: %A" fsiValue
    | Choice2Of2(e) -> printfn "%A" e

    errors |> Array.map (printfn "%A") |> ignore

    printfn "TEST (EvalExpressionNonThrowing): %s" codeSmallLambda2

    let evalresult, errors = fsi.EvalExpressionNonThrowing codeSmallLambda2 

    match evalresult with
    | Choice1Of2(Some fsiValue) -> printfn "compile successful: %A" fsiValue
    | Choice2Of2(e) -> printfn "%A" e

    errors |> Array.map (printfn "%A") |> ignore

    printfn "TEST (EvalInteractionNonThrowing): %s" codeSmallLambda

    let evalresult, errors = fsi.EvalInteractionNonThrowing codeSmallLambda 

    match evalresult with
    | Choice1Of2(Some fsiValue) -> printfn "compile successful: %A" fsiValue
    | Choice2Of2(e) -> printfn "%A" e

    errors |> Array.map (printfn "%A") |> ignore

    printfn "TEST (EvalInteractionNonThrowing): %s" codeSmallLambda2

    let evalresult, errors = fsi.EvalInteractionNonThrowing codeSmallLambda2 

    match evalresult with
    | Choice1Of2(Some fsiValue) -> printfn "compile successful: %A" fsiValue
    | Choice2Of2(e) -> printfn "%A" e

    errors |> Array.map (printfn "%A") |> ignore

    0 // return an integer exit code