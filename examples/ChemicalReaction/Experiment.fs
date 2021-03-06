
namespace Simulation.Aivika.Examples

open System
open System.Web.UI

open Simulation.Aivika
open Simulation.Aivika.Results
open Simulation.Aivika.Experiments
open Simulation.Aivika.Experiments.Web
open Simulation.Aivika.Charting.Web

module Experiment =

    [<EntryPoint>]
    let main args =

        let experiment = Experiment ()

        experiment.Specs <- Model.specs
        experiment.RunCount <- 1

        let provider1 = ExperimentSpecsProvider ()
        let provider2 = TimeSeriesProvider ()
        let provider3 = LastValueProvider ()
        let provider4 = TableProvider ()

        let providers =
            [ provider1 :> IExperimentProvider<HtmlTextWriter>;
              provider2 :> IExperimentProvider<HtmlTextWriter>;
              provider3 :> IExperimentProvider<HtmlTextWriter>;
              provider4 :> IExperimentProvider<HtmlTextWriter> ]

        experiment.RenderHtml (Model.model, providers)
            |> Async.RunSynchronously

        Console.WriteLine()
        Console.WriteLine("Press Enter...")
        Console.ReadLine () |> ignore

        0
