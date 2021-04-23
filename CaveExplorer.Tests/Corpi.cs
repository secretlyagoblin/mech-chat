using System;
using System.Collections.Generic;
using System.Text;

namespace CaveExplorer.Tests
{
    static class Corpi
    {
        public static string CoprusA = @"INCLUDE SwampDeck
LIST factions = Frogtide, Deepburrow, Crowbrigade, Doormouse
~factions = LIST_ALL(factions)

VAR missionCount = 3
~missionCount = RANDOM(2,4)
VAR alwaysFalse = false


-> Entry

=== Entry ===

When the time is right, your handler reaches out with news...

{~First|Firstly|Most urgently|As you expected|Unexpectedly}, {GenerateMission()} #IN_m1

{~Second|Secondly|Also|As well as}, {GenerateMission()} #IN_m2

{missionCount>2:{missionCount == 3: Finally}{missionCount == 4:{~Third|Thirdly|Also of note}}, {GenerateMission()}} #IN_m3

{missionCount==4:Finally, {GenerateMission()}} #IN_m4

*OUT_m1 #OUT_m1
*OUT_m2 #OUT_m2
*{missionCount>2}OUT_m3 #OUT_m3
*{missionCount>3}OUT_m4 #OUT_m4
*Return -> Entry

 
-Your mission selected, the mission unfurls, and new details begin to emerge...

 ->CoreLoop

=== function GenerateMission() ===
LIST types = mechs, infantry, commandos, scientists, specialists
~types = LIST_ALL(types)

VAR faction = factions.Frogtide
~faction = LIST_RANDOM(factions)
VAR otherFaction = factions.Frogtide
~otherFaction = LIST_ALL(factions)
~otherFaction -= faction
~otherFaction = LIST_RANDOM(otherFaction)
VAR type = types.mechs
~ type = LIST_RANDOM(types)

~temp tags = faction
~tags +=type


<>{shuffle:
    - rumours # rumors
    - word from an informant #word
}<> of <>{shuffle:
    -Frogtide # frog
    -Deepburrow #deep
    -Crowbrigade #crow
    -Doormouse #mouse
} <>{type} <>{shuffle:
    -in
    -around
    -sending scouts into #scouts
    -beneath
    -marching on
    -defending attacks from {otherFaction} {InAround()}
    -attacking {otherFaction} {~facilities|depos|patrols} {InAround()}
} <> {shuffle:
    -the mines #mines
    -the root system #roots
    -no mans land #no mans land
    -the canopy #canopy
    -the town outskirts # town
    -the old trainyards # trainyards
    -our staging area #staging
}

=== function InAround() ===
<>{shuffle:
-in
-around
-beneath
-within
}

== CoreLoop
LIST loopOptions = Obstacle, Twist, Complication, Boon
~loopOptions = LIST_ALL(loopOptions)

+ {loopOptions ? loopOptions.Obstacle}...an obstacle?
-> Obstacles
+ {loopOptions ? loopOptions.Twist}...a shocking twist!
-> Twists
+ {loopOptions ? loopOptions.Complication}...a complication?
->Complications
+ {loopOptions ? loopOptions.Boon}...a boon!
->Boons
+ ...or finally, we reach our conclusion..
->Conclusions

- ->CoreLoop

== Twists
NAVIGATE_Twists_CoreLoop
-> ThrowNavigate

== Obstacles
NAVIGATE_Obstacles_CoreLoop
-> ThrowNavigate

== Complications
NAVIGATE_Complications_CoreLoop
-> ThrowNavigate

== Boons
NAVIGATE_Boons_CoreLoop
-> ThrowNavigate

== Conclusions
NAVIGATE_Conclusions_CoreLoop
-> ThrowNavigate

== ThrowNavigate

This text should never display, NAVIGATE text should be managed at the CSharp layer. ->END

";
    }
}
