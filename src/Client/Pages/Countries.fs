module Countries
open Shared
open Elmish.Streams
open FSharp.Control
open Shared

type State = { Countries: CountryCovidCasesSummary seq option }

type Msg =
    | InitialCountriesLoaded of CountryCovidCasesSummary seq

let init() = { Countries = None }

let update (msg : Msg) (state : State) : State =
    match state.Countries, msg with
    | _, InitialCountriesLoaded countries ->
        { state with Countries = Some(countries)  }
    | _ -> state

let load (api : ICovidDataApi) = api.summary() |> AsyncRx.ofAsync

let loadCountries (api : ICovidDataApi)  =
    load(api)
    |> AsyncRx.map InitialCountriesLoaded
    |> AsyncRx.toStream "loading"

let stream (api : ICovidDataApi) model msgs =
    match model.Countries with
    | None -> loadCountries (api)
    | _ -> msgs
