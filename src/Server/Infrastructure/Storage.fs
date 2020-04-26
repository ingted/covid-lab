namespace Infrastructure

open System
open Domain
open Domain.Types

module Storage =
    let getCovidCases(): Async<CovidCases> =
        async {
            return Some
                       (seq
                           [ { CountryName = "Poland"
                               Location =
                                   { Longitude = 21.017532
                                     Latitude = 52.237049 }
                               WholeCountryCases =
                                   seq
                                       [ { Date = DateTime.Now
                                           Confirmed = 10
                                           Gain = -1 } ]
                               Provinces = None } ])
        }
