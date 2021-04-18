-> Begin


EXTERNAL Get(x,y)

=== function Get(x,y) ===
~return "Small"


EXTERNAL Add(a,b)

=== function Add(x,y) ===
~return 30

=== Begin ===

INIT-VARIABLES

A cave lurks on the horizon...

 * Leave forever.
 * Go into the cave. -> TheCave ->

- They lived happily ever after.
    -> END



=== TheCave ===

VAR caveSize = ""
~caveSize = Get("Cave","Size")

VAR caveCount = 1
~caveCount = Add(7,11)



{caveSize == "Huge":->Huge->}
{caveSize == "Small":->Small->}
However, the cave count is {caveCount}.

What shit, what nuts, what horrible guts.

->->

=Huge
A cave {~as big as|like|as cavernous as|that dwarfed me like} this was truly something to be seen. ->->


=Small
Tinsey fuck, I hate it ->->