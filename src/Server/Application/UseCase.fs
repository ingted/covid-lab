namespace Application

module UseCase =
    open Shared
    open Domain


    let private mapToSummaryDto (domainCovid: Domain.Types.CountryCovidCasesSummary): Shared.CountryCovidCasesSummary =
        { Country = domainCovid.Country
          Confirmed = domainCovid.Confirmed
          Deaths = domainCovid.Deaths
          Recovered = domainCovid.Recovered }

    let private mapToDto (domainCovid: Domain.Types.CountryCovidCasesDay): Shared.CountryCovidCasesDay =
        { Country = domainCovid.Country
          Province = domainCovid.Province
          Date = domainCovid.Date
          Confirmed = domainCovid.Confirmed
          Deaths = domainCovid.Deaths
          Recovered = domainCovid.Recovered }

    let getCovidDataSummary (getCovidCases: GetCovidDataSummary) () =
        async {
            let! cases = getCovidCases()
            return cases |> Seq.map (mapToSummaryDto) }

    let getCovidDataByCountry (getCovidCases: GetCovidCasesByCountry) (country: string) =
        async {
            let! cases = getCovidCases(country)
            return cases |> Seq.map (mapToDto) }

