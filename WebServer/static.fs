module Static
open Configuration
open Header
open System
open System.IO

type QueryValues = {
    query: string
    url: string
}

type StaticInfo = {
    localFile: string
    redirUrl: string
    isFile: bool
}

let checkFile (header: Header) configuration = 
    let r = header.url.IndexOf '#'

    let url = 
        if r <> -1 then 
            header.url.Substring (0, r)
        else
            header.url

    let qm = url.IndexOf ('?')
    let queryValues = 
        if qm <> -1 then
            {
                query = url.Substring (qm + 1)
                url = url.Substring (0, qm)
            }
        else
            {
                query = ""
                url = url
            }
        
    let queryValues = {
        queryValues with 
            url =
                let unescapedUrl = Uri.UnescapeDataString (queryValues.url)                         
                if unescapedUrl.StartsWith "/" then
                    unescapedUrl.Substring 1
                else
                    unescapedUrl
    }        

    let relativePath = queryValues.url.Replace ('/', Path.PathSeparator)
    let rootDirectory = configuration.WebRoot

    let localFile = 
        try 
            Path.Combine (rootDirectory, relativePath)
        with
            | ex -> 
                printfn "%s Invalid path: %s %O" queryValues.url relativePath ex
                raise ex

    if File.Exists localFile then 
        {
            localFile = localFile
            redirUrl = ""
            isFile = true
        }
    elif Directory.Exists localFile then
        if queryValues.url.EndsWith "/" then
            {
                localFile = ""
                redirUrl = "kommt noch"
                isFile = false
            }
        else
            {
                localFile = ""
                redirUrl = "kommt noch"
                isFile = false
            }
    //elif queryValues.url = "/" then
    else
    {
        localFile = ""
        redirUrl = ""
        isFile = false
    }


    


