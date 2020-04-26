namespace Domain.Types
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

