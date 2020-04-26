namespace Infrastructure
open FSharp.Control

module Github =
    open System
    open FSharp.Data

    type Record =
        { CountryRegion: string
          ProvinceState: string
          LastUpdate: DateTime
          Confirmed: int
          Gain: int }

    let firstReportDate = DateTime(2020, 1, 22).AddDays(0.)
    let lastReportDate = DateTime.Today

    let dateCount = (lastReportDate - firstReportDate).Days

    let reportBaseUrl =
        "https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_daily_reports/"
    let toDailyReportUrl (dt: DateTime) = sprintf "%s%s.csv" reportBaseUrl (dt.ToString("MM-dd-yyyy"))

    let dates = seq [ 0 .. dateCount - 1 ] |> Seq.map (float >> firstReportDate.AddDays)

    let dailyReportUrls =
        dates
        |> Seq.map toDailyReportUrl


    let toShortDate (dt: DateTime) = dt.ToShortDateString()
    let noTime (dt: DateTime) = DateTime(dt.Year, dt.Month, dt.Day)

    let parseConfirmed =
        function
        | "" -> 0
        | str -> int str

    let parseRecord (row: CsvRow) =
        try
            Some({ CountryRegion = row.["Country/Region"]
                   ProvinceState = row.["Province/State"]
                   LastUpdate = row.["Last Update"].AsDateTime() |> noTime
                   Confirmed = row.["Confirmed"] |> parseConfirmed
                   Gain = 0 })
        with ex ->
            None


    let loadCsv url =
        async {
             try
                let! data = CsvFile.AsyncLoad url
                return data |> Some
             with ex ->
                return None
        }

    let loadData() =
        dailyReportUrls
            |> AsyncSeq.ofSeq
            |> AsyncSeq.mapAsync(loadCsv)
            |> AsyncSeq.choose id
            |> AsyncSeq.collect (fun x -> x.Rows |> AsyncSeq.ofSeq)
            |> AsyncSeq.map(parseRecord)
            |> AsyncSeq.choose id

