namespace Shared
open System


type CountryCovidCasesDay =
    { Country: string
      Province: string option
      Date: DateTime
      Confirmed: int option
      Deaths: int option
      Recovered: int option }

    module Route =
    /// Defines how routes are generated on server and mapped from client
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName

/// A type that specifies the communication protocol between client and server
/// to learn more, read the docs at https://zaid-ajaj.github.io/Fable.Remoting/src/basics.html
type ICovidDataApi =
    { init : unit -> Async<CountryCovidCasesDay seq> }

