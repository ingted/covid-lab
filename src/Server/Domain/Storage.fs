namespace Domain
open Domain.Types

type CovidCases = CountryCases seq option
type GetCovidCases = unit -> Async<CovidCases>