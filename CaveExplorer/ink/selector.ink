INCLUDE SwampDeck
LIST factions = Frogtide, Deepburrow, Crowbrigade, Doormouse
~factions = LIST_ALL(factions)

VAR missionCount = 3
~missionCount = RANDOM(2,4)
VAR alwaysFalse = false

{shuffle:
-{alwaysFalse:item}
-doyg
-nurthle
}



-> Entry

=== Entry ===

When the time is right, your handler reaches out with news...

 * [{~First|Firstly|Most urgently|As you expected|Unexpectedly}, {GenerateMission()}]
 * [{~Second|Secondly|Also|As well as}, {GenerateMission()}]
 * {missionCount>2}[{missionCount == 3: Finally}{missionCount == 4:{~Third|Thirdly|Also of note}}, {GenerateMission()}]
 *{missionCount==4}[Finally, {GenerateMission()}]
 + You've {~got diarea|got the snorts|been hung over for days}, lets reschedule... 
 ->Entry
 
-Your mission selected, the mission unfurls, and new details begin to emerge...

 ->CoreLoop

=== function GenerateMission() ===
LIST types = mechs, infantry, commandos, scientists, specialists

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


{shuffle:
    - rumours
    - word from an informant 
} of {faction} {type} {shuffle:
    -in
    -around
    -sending scouts into
    -beneath
    -marching on
    -defending attacks from {otherFaction} {InAround()}
    -attacking {otherFaction} {~facilities|depos|patrols} {InAround()}
} {shuffle:
    -the mines
    -the root system
    -no mans land
    -the canopy
    -the town outskirts
    -the old trainyards
    -our staging area
}

=== function InAround() ===
{shuffle:
-in
-around
-beneath
-within
}

== CoreLoop
VAR IsTwists = true

+ ...an obstacle?
+ {IsTwists}...a shocking twist! -> Twists ->
+ ...a complication?
+ ...a boon!
+ ...or finally, we reach our conclusion.. -> END

- ->CoreLoop

== Twists
{ shuffle stopping:
-COol factions
-Big nuts
-Hot wheat
 -
 ~IsTwists = false
 BIG FINAL TWIST
}->->