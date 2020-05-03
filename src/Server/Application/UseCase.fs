namespace Application

module UseCase =
    open Shared
    open Domain


    let private mapToDto (domainCovid: Domain.Types.CountryCovidCasesDay): Shared.CountryCovidCasesDay =
        { Country = domainCovid.Country
          Province = domainCovid.Province
          Date = domainCovid.Date
          Confirmed = domainCovid.Confirmed
          Deaths = domainCovid.Deaths
          Recovered = domainCovid.Recovered }

    let getCovidData (getCovidCases: GetCovidCases) () =
        async {
            let! cases = getCovidCases()
            return cases |> Seq.map (mapToDto) }

    let getCovidDataByCountry (getCovidCases: GetCovidCasesByCountry) (country: string) =
        async {
            let! cases = getCovidCases(country)
            return cases |> Seq.map (mapToDto) }

