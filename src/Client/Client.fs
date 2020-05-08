module Client

open Elmish
open Elmish.React
open Elmish.Streams
open FSharp.Control
open Fable.React
open Fable.React.Props
open Fulma
open Thoth.Json

open Shared

type Page =
    | Countries
    | Country of string

// The model holds data that you want to keep track of while the application is running
// in this case, we are keeping track of a counter
// we mark it as optional, because initially it will not be available from the client
// the initial value will be requested from server
type Model = { Countries: CountryCovidCasesSummary seq option }

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
type Msg =
    | InitialCountriesLoaded of CountryCovidCasesSummary seq

module Server =

    open Shared
    open Fable.Remoting.Client

    /// A proxy you can use to talk to server directly
    let api : ICovidDataApi =
      Remoting.createApi()
      |> Remoting.withRouteBuilder Route.builder
      |> Remoting.buildProxy<ICovidDataApi>
let initialCounter = Server.api.summary

// defines the initial state
let init () : Model =
    { Countries = None }
// The update function computes the next state of the application based on the current state and the incoming events/messages
let update (msg : Msg) (currentModel : Model) : Model =
    match currentModel.Countries, msg with
    | _, InitialCountriesLoaded countries ->
        { Countries = Some(countries) }
    | _ -> currentModel


let load = AsyncRx.ofAsync (initialCounter ())

let loadCount =
    load
    |> AsyncRx.map InitialCountriesLoaded
    |> AsyncRx.toStream "loading"

let stream model msgs =
    match model.Countries with
    | None -> loadCount
    | _ -> msgs

let safeComponents =
    let components =
        span [ ]
           [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ]
               [ str "SAFE  "
                 str Version.template ]
             str ", "
             a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
             str ", "
             a [ Href "http://fable.io" ] [ str "Fable" ]
             str ", "
             a [ Href "https://elmish.github.io" ] [ str "Elmish" ]
             str ", "
             a [ Href "https://fulma.github.io/Fulma" ] [ str "Fulma" ]
             str ", "
             a [ Href "http://elmish-streams.rtfd.io/" ] [ str "Elmish.Streams" ]
             str ", "
             a [ Href "https://zaid-ajaj.github.io/Fable.Remoting/" ] [ str "Fable.Remoting" ]

           ]

    span [ ]
        [ str "Version "
          strong [ ] [ str Version.app ]
          str " powered by: "
          components ]

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

let view (model : Model) (dispatch : Msg -> unit) =
    div []
        [ Navbar.navbar [ Navbar.Color IsPrimary ]
            [ Navbar.Item.div [ ]
                [ Heading.h2 [ ]
                    [ str "SAFE Template" ] ] ]


          Table.table [ Table.IsBordered
                        Table.IsFullWidth
                        Table.IsStriped ]
              [ thead [ ]
                  [ tr [ ]
                       [ th [ ] [ str "Country" ]
                         th [ ] [ str "Cases" ]
                         th [ ] [ str "Deaths" ] ] ]
                tbody [ ] (show model) ]
          Footer.footer [ ]
                [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
                    [ safeComponents ] ] ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkSimple init update view
|> Program.withStream stream "msgs"
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
