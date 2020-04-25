namespace Application

module UseCase =
    open Shared
    open Domain

    let private mapToDto (domainCovid: Domain.Types.CountryCases): Shared.CountryCases =
        { Country = domainCovid.Country
          Province = domainCovid.Province
          Cases =
              domainCovid.Cases
              |> Seq.map (fun case ->
                  { Date = case.Date
                    Confirmed = case.Confirmed
                    Gain = case.Gain }) }

    let getCovidData (getCovidCases: GetCovidCases) () =
        async {
            let! cases = getCovidCases()
            return cases |> Option.map(fun x -> x |> Seq.map(mapToDto))
        }
