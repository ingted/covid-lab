namespace Shared
open System


type CountryName = string

type CovidCaseDay = {
    Date: DateTime
    Confirmed: int
    Gain: int
}

type CovidCasesPerDay = CovidCaseDay seq

type Location = { Longitude: double; Latitude: double }

type Province = { Name: string; Location: Location; Cases: CovidCasesPerDay }

type Provinces = Province seq

type ProvicesDataOrNone = Provinces option

type CountryCases = { CountryName: CountryName; Location: Location; WholeCountryCases: CovidCasesPerDay;  Provinces: ProvicesDataOrNone; }

module Route =
    /// Defines how routes are generated on server and mapped from client
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

/// A type that specifies the communication protocol between client and server
/// to learn more, read the docs at https://zaid-ajaj.github.io/Fable.Remoting/src/basics.html
type ICovidDataApi =
    { init : unit -> Async<CountryCases seq option> }

