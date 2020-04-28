namespace Infrastructure

open System
open Domain
open Domain.Types
open FSharp.Control

module Storage =

    let getCovidCases(): Async<CovidCases> =
        async {
            let! data = Github.loadData() |> AsyncSeq.toArrayAsync
            return Some(data |> Array.toSeq)
        }