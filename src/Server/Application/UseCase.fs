namespace Application


module UseCase =
    open Domain.Types

    let getCovidData () =
        async {
            return { Country = "Poland"; Province = "Lodzkie"; Cases = Seq.empty }
        }