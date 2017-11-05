﻿open Configuration
open Server

printfn "Starting Test Server"
let configuration = Configuration.create {
        Configuration.createEmpty() with 
            Port = 20000; 
            WebRoot = "/home/uwe/Projekte/Node/WebServerElectron/web" 
    }

try
    let server = create configuration
    server.start ()
    stdin.ReadLine() |> ignore
    server.stop ()
with
    | ex -> printfn "Fehler: %s" ex.Message


