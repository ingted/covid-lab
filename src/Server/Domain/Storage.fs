namespace Domain
open Domain.Types

type GetCovidCases = unit -> Async<CountryCases seq>