namespace Application


module UseCase =
    open Shared

    let getCovidData () =
        async {
            return seq[{ Country = "Poland"; Province = "Lodzkie"; Cases = Seq.empty }]
        }