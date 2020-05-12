module Countries
open Shared
open Elmish.Streams
open FSharp.Control
open Shared
open Fable.React
open Fable.React.Props
open Fulma
open Thoth.Json

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


let tableRow (model: CountryCovidCasesSummary) =
    tr [ ]
       [ th [ ] [ str model.Country ]
         th [ ] [ str (model.Confirmed.ToString()) ]
         th [ ] [ str (model.Deaths.ToString()) ] ]

let show = function
    | { Countries = Some countries } ->
        countries |> Seq.sortByDescending(fun x -> x.Confirmed) |> Seq.map(fun c -> c |> tableRow ) |> Seq.toList
    | { Countries = None   } ->
        [ tr [][]]

let button txt onClick =
    Button.button
        [ Button.IsFullWidth
          Button.Color IsPrimary
          Button.OnClick onClick ]
        [ str txt ]

let view (model : State) (dispatch : Msg -> unit) =
    div []
        [ Table.table [ Table.IsBordered
                        Table.IsFullWidth
                        Table.IsStriped ]
              [ thead [ ]
                  [ tr [ ]
                       [ th [ ] [ str "Country" ]
                         th [ ] [ str "Cases" ]
                         th [ ] [ str "Deaths" ] ] ]
                tbody [ ] (show model) ]]