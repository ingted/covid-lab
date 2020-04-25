namespace Infrastructure
open Domain
open Domain.Types

module Storage =
    let getCovidCases (): Async<CovidCases> =
        async {
            return Some(seq[{ Country = "Poland"; Province = "Lodzkie"; Cases = Seq.empty }])
        }