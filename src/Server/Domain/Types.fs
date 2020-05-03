namespace Domain.Types

open System


type CountryCovidCasesDay =
    { Country: string
      Province: string option
      Date: DateTime
      Confirmed: int option
      Deaths: int option

      Recovered: int option }

type CountryCovidCasesSummary =
    { Country: string
      Confirmed: int
      Deaths: int
      Recovered: int }

type AllCases = CountryCovidCasesDay seq