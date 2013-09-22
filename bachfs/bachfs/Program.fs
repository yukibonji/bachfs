﻿module main

///////////////////////////////////////////////////////////////
// Originally by:                                            //
// Functional Composition by Chris Ford (@ctford)            //
// ThoughtWorks Uganda                                       //
//                                                           //
// http://github.com/ctford/functional-composition           //
// http://github.com/ctford/leipzig                          //
// http://github.com/overtone/overtone                       //
//                                                           //
// Converted to f# by Chris Holt                             //
// @lefthandedgoat                                           //
// http://github.com/lefthandedgoat/                         //
///////////////////////////////////////////////////////////////

//#r @"C:\projects\bachfs\bachfs\packages\Bespoke-OSC-Library.1.0.0\lib\Bespoke.Common.dll"
//#r @"C:\projects\bachfs\bachfs\packages\Bespoke-OSC-Library.1.0.0\lib\Bespoke.Common.Osc.dll"

open System
open sc












///////////////////////////////////////////////////////////////
// Sine waves                                                //
///////////////////////////////////////////////////////////////

let tone freq = SinOsc freq |> play

let doubleTone freq1 freq2 =
    tone freq1
    tone freq2

let beep freq duration = Envelope(1.0, 0.0, duration, (SinOsc freq)) |> play

tone 300.0
doubleTone 300.0 300.0
beep 300.0 1.0
stop()






///////////////////////////////////////////////////////////////
// Harmonics                                                 //
///////////////////////////////////////////////////////////////

let bell freq duration harmonics =
    let defaultHarmonics = [1.0; 0.6; 0.4; 0.25; 0.2; 0.15]
    let harmonicSeries = [1.0; 2.0; 3.0; 4.0; 5.0; 6.0] //[1.0; 2.0; 3.0; 4.2; 5.4; 6.8]
    let proportions = 
        harmonics @ (Seq.skip (List.length harmonics) defaultHarmonics |> List.ofSeq)
    let component' harmonic proportion =
        PercEnvelope(0.01, (proportion * duration), 1.0, -4.0, proportion, (SinOsc (harmonic * freq)))
    let whole = 
        Mix (List.map2 component' harmonicSeries proportions)
    DetectSilence whole |> play


beep 300.0 1.0
bell 300.0 10.0 []


/////////////////////////////////////////////////////
// Psycho-acoustics                                //
/////////////////////////////////////////////////////

bell 600.0 10.0 []
bell 500.0 10.0 [0.0]
bell 400.0 10.0 [0.0; 0.0]


///////////////////////////////////////////////////////////////
// Equal temperament                                         //
///////////////////////////////////////////////////////////////

let midi2hertz (midi : int) = 8.1757989156 * (System.Math.Pow(2.0, (Convert.ToDouble(midi) / 12.0)))

midi2hertz 69

let ding midi = bell (midi2hertz midi) 3.0 []

ding 69