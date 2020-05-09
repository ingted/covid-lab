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

    let summary(): Async<CountryCovidCasesSummary seq> =
        let mapCases(countryName: string, cases: CountryCovidCasesDay seq): CountryCovidCasesSummary =
            let lastDay = cases |> Seq.sortByDescending(fun x -> x.Date) |> Seq.tryHead
            let struct (confirmed, deaths, recovered) =
                match lastDay with
                | Some({ Confirmed = Some(confirmed); Deaths = Some(deaths); Recovered = Some(recovered)}) ->
                    struct (confirmed, deaths, recovered)
                | _ -> struct (0, 0, 0)
            { Country = countryName; Confirmed = confirmed; Deaths = deaths; Recovered = recovered}

        async {
            let! data = getCovidCases()
            return data
                    |> Seq.groupBy(fun x -> x.Country)
                    |> Seq.map(mapCases)
        }

    let findByCountry(coutntry: string): Async<AllCases> =
        async {
            let! data = getCovidCases()
            return data |> Seq.filter(fun x -> x.Country = coutntry)
        }