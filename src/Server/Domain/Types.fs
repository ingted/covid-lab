namespace Domain.Types
open System

type Country = string
type Province = string

type CovidCaseDay = {
    Date: DateTime
    Confirmed: int
    Gain: int
}

type CountryCases = { Country: Country; Province: Province; Cases: CovidCaseDay seq }

