namespace Infrastructure

open System
open Domain
open Domain.Types
open FSharp.Control

module Storage =

    let getCovidCases(): Async<AllCases> =
        async {
            let! data = Github.loadData() |> AsyncSeq.toArrayAsync
            return data |> Array.toSeq
        }