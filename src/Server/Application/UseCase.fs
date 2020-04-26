namespace Application

module UseCase =
    open Shared
    open Domain

    let private mapCase (domainCase: Domain.Types.CovidCaseDay): Shared.CovidCaseDay =
        { Date = domainCase.Date
          Confirmed = domainCase.Confirmed
          Gain = domainCase.Gain }

    let mapLocation (domainLocation: Domain.Types.Location): Shared.Location =
        { Latitude = domainLocation.Latitude
          Longitude = domainLocation.Longitude }

    let private mapToDto (domainCovid: Domain.Types.CountryCases): Shared.CountryCases =
        { CountryName = domainCovid.CountryName
          Provinces =
              domainCovid.Provinces
              |> Option.map (fun provinces ->
                  provinces
                  |> Seq.map (fun province ->
                      { Name = province.Name
                        Location = province.Location |> mapLocation
                        Cases = province.Cases |> Seq.map (fun x -> x |> mapCase) }))
          Location = domainCovid.Location |> mapLocation
          WholeCountryCases = domainCovid.WholeCountryCases |> Seq.map (mapCase) }

    let getCovidData (getCovidCases: GetCovidCases) () =
        async {
            let! cases = getCovidCases()
            return cases |> Option.map (fun x -> x |> Seq.map (mapToDto)) }
