namespace Domain
open Domain.Types

type CovidCases = AllCases seq
type GetCovidCases = unit -> Async<AllCases>