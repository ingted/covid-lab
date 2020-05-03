namespace Infrastructure

open System
open Domain
open Domain.Types
open FSharp.Control

module Storage =
    type private DbOperation =
        | Store of AllCases
        | Fetch of AsyncReplyChannel<AllCases option>

    let private db =
        MailboxProcessor.Start(fun inbox ->
            let rec loop cases =
                async {
                    match! inbox.Receive() with
                    | Store(msg) ->
                        return! loop(Some(msg))
                    | Fetch(reply) ->
                        reply.Reply(cases)
                        return! loop(cases)
                }
            loop None)

    let getCovidCases(): Async<AllCases> =
        async {
            match! db.PostAndAsyncReply(fun ch -> Fetch(ch)) with
            | Some(x) ->
                return x
            | None ->
                let! res = Github.loadData() |> AsyncSeq.toArrayAsync
                db.Post(Store(res))
                return res |> Array.toSeq
        }

    let findByCountry(coutntry: string): Async<AllCases> =
        async {
            let! data = getCovidCases()
            return data |> Seq.filter(fun x -> x.Country = coutntry)
        }