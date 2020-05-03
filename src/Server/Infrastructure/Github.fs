namespace Infrastructure

open FSharp.Control

module Github =
    open System
    open FSharp.Data
    open Domain.Types

    type private Record =
        { CountryRegion: string
          ProvinceState: string
          LastUpdate: DateTime
          Confirmed: int option
          Deaths: int option
          Recovered: int option }

    let private firstReportDate = DateTime(2020, 1, 22).AddDays(0.)
    let private lastReportDate = DateTime.Today

    let private dateCount = (lastReportDate - firstReportDate).Days

    let private reportBaseUrl =
        "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/"
    let private toDailyReportUrl (dt: DateTime) = sprintf "%s%s.csv" reportBaseUrl (dt.ToString("MM-dd-yyyy"))

    let private dates = seq [ 0 .. dateCount - 1 ] |> Seq.map (float >> firstReportDate.AddDays)

    let private dailyReportUrls = dates |> Seq.map toDailyReportUrl


    let private toShortDate (dt: DateTime) = dt.ToShortDateString()
    let private noTime (dt: DateTime) = DateTime(dt.Year, dt.Month, dt.Day)

    let private parseConfirmed =
        function
        | "" -> None
        | str -> Some(int str)

    let private parseRecord (row: CsvRow) =
        try
            Some
                ({ CountryRegion = row.["Country/Region"]
                   ProvinceState = row.["Province/State"]
                   LastUpdate = row.["Last Update"].AsDateTime() |> noTime
                   Confirmed = row.["Confirmed"] |> parseConfirmed
                   Recovered = row.["Recovered"] |> parseConfirmed
                   Deaths = row.["Deaths"] |> parseConfirmed  })
        with ex -> None


    let private loadCsv url =
        async {
            try
                let! data = CsvFile.AsyncLoad url
                return data |> Some
            with ex -> return None
        }


    let loadData() : AsyncSeq<CountryCovidCasesDay> =
        dailyReportUrls
        |> AsyncSeq.ofSeq
        |> AsyncSeq.mapAsyncParallel (loadCsv)
        |> AsyncSeq.choose id
        |> AsyncSeq.collect (fun x -> x.Rows |> AsyncSeq.ofSeq)
        |> AsyncSeq.map (parseRecord)
        |> AsyncSeq.choose id
        |> AsyncSeq.map(fun x-> { Country = x.CountryRegion
                                  Province = x.ProvinceState |> Option.ofObj
                                  Date = x.LastUpdate
                                  Confirmed = x.Confirmed
                                  Deaths = x.Deaths
                                  Recovered = x.Recovered })
