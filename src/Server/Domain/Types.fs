namespace Domain.Types

open System


type CountryCovidCasesDay =
    { Country: string
      Province: string option
      Date: DateTime
      Confirmed: int option
      Deaths: int option
      Recovered: int option }

type AllCases = CountryCovidCasesDay seq