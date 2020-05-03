namespace Domain
open Domain.Types

type CovidCases = AllCases seq
type GetCovidDataSummary = unit -> Async<CountryCovidCasesSummary seq>
type GetCovidCasesByCountry = string -> Async<AllCases>