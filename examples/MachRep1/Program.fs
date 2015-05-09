﻿
(* It corresponds to model MachRep1 described in document 
   Introduction to Discrete-Event Simulation and the SimPy Language
   [http://heather.cs.ucdavis.edu/~matloff/156/PLN/DESimIntro.pdf]. 
   SimPy is available on [http://simpy.sourceforge.net/].
   
   The model description is as follows.

   Two machines, which sometimes break down.
   Up time is exponentially distributed with mean 1.0, and repair time is
   exponentially distributed with mean 0.5. There are two repairpersons,
   so the two machines can be repaired simultaneously if they are down
   at the same time.

   Output is long-run proportion of up time. Should get value of about
   0.66. *)

open System

open Simulation.Aivika

let specs = {

    StartTime=0.0; StopTime=1000.0; DT=1.0; 
    Method=RungeKutta4; GeneratorType=StrongGenerator
}

let meanUpTime = 1.0
let meanRepairTime = 0.5

let model: Simulation<float> = simulation {

    // total up time for all machines
    let totalUpTime = ref 0.0

    let machine = proc {
    
        while true do

            let! upTime = Proc.randomExponential meanUpTime
            totalUpTime := !totalUpTime + upTime
             
            do! Proc.randomExponential_ meanRepairTime
    }
    
    do! Proc.runInStartTime machine
    do! Proc.runInStartTime machine

    let upTimeProp = 
        eventive {
            let! t = Dynamics.time |> Dynamics.lift
            return (!totalUpTime / (2.0 * t))
        }

    return! upTimeProp |> Eventive.runInStopTime
}

[<EntryPoint>]
let main argv = 
    
    let x = model |> Simulation.run specs 

    Console.WriteLine ("The long-run proption of up time (~ 0.66): {0}", x)
    Console.ReadLine () |> ignore

    0
