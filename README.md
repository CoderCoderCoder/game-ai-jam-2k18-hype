# DeepPCG - An Endless Underwater Arcade Adventure
Hi everyone! I'm Samantha and this is the demo I built for the Game AI Summer School 2018 Jam. The subdomain of AI I wanted to fiddle around with was procedural generation, and here I've used a very simple algorithm to create an endless series of levels for players to explore.  
  
<img src="https://i.imgur.com/cZLeeo3.png" alt="DeepPCG Screenshot 1 (You know this demo is cool because you can shoot arrows FROM your knees.)" width="400"/> <img src="https://i.imgur.com/Hql19CV.png" alt="DeepPCG Screenshot 2 (Does anyone really READ the alt text anymore? Kids these days.)" width="400"/>

(Disclaimer: Thanks to all the great contributers to sites like [itch.io](https://itch.io/), [OpenGameArt.org](https://opengameart.org/), and [freesound.org](https://freesound.org/) - all assets are pulled from various free sources on the net with minor modifications done in GIMP and Audacity.)

## The Tech
Level generation in DeepPCG is split into two main phases, both of which are based heavily on techniques we discussed over the past week. Here's how it works:
### Phase 1 - Cave Generation
The series of caves is built using cellular automata in the same fashion described by [Johnson, Yannakakis, and Togelius (2010)](http://julian.togelius.com/Johnson2010Cellular.pdf). Here, I simply generate a grid of 128x128 cells at a time, starting with a random population of rock cells and a few iterations to carve out/smooth the cave geometry. To simplify the issue of dealing with connections/runtime loading, a single map is loaded at a time and padded to create barriers at the edges of the map. Furthermore, a small blank water 'room' is carved out near the player's starting location to ensure navigability and prevent enemies that "live" at spawn.

After the geometry is generated, basic information about a rock cell's neighbourhood is used to select an appropriate sprite. Blank water tiles and floor tiles are added to lists of coordinates suitable for population with flora (floor tiles) and fauna/mines/treasures (water tiles). 

With the exception of edge barriers, the environment is destructible, letting players tunnel their way into smaller spaces to explore and collect treasure. Players can "warp" to a new map in-game once they're satisfied with the score/treasure/bonus they've accumulated in the current level. 
### Phase 2 - Entity Population
Entities are spawned using an L-system with a stochastic ruleset that can be customized from the Unity editor. Different entities (flora, hazards, treasures) and empty cells are encoded as individual symbols. Two separate systems are propagated - one for populating the cave "floor" with plants and one to inject enemies, mines, and treasure into the water cells. Each system is encoded as a simple string. After completing a specified number of iterations, the complete string is re-parsed while simultaneously iterating along the list of floor/water coordinates created by the cave generator. The steps taken along the coordinate list are determined randomly within a designer-specified interval to introduce further variation. When a symbol representing an entity is found, a Unity prefab is selected at random from a list of suitable entities (e.g., a "treasure" symbol may result in a ring, diamond, ruby, emerald, orb, or gold bar). 

Like cave cells, the set of entities is regenerated for each new level.
## How To Play
You play as Coral Flimbit, a deep-sea diver that *might* be a lizard-man wearing people clothes. While this is a whimsical reminder indeed, it serves to remind us all of the current [global reptilian conspiracy](http://reddit.com/r/sneks), ssssomething which should never be taken lightly.

Your core motivation is an undying devotion to shiny objects, and so you have resigned yourself to a life of treasure-hunting in the perilous depths of the Stack Overflowcean. Armed with nothing but your trusty and completely legally purchased harpoon gun, you must brave the ragefish-infested waters to collect as many baubles as possible. How long can you survive before being condemned to a watery grave? (Or becoming complacent and cursing at a mine peeking out from beneath a patch of seaweed?)

- Swim around using the WASD keys.  
- Aim your harpoon gun using the arrow keys. If you don't aim, it will shoot in the direction the player is facing.  
- Shoot a harpoon using the spacebar. You have unlimited ammo, because this is the magical world of video games.  
- Collect the shiny things. Avoid (or shoot) the fishy things. Avoid the mines. You can't destroy a sea mine with a flimsy bamboo harpoon, what do you think this is, Chuck E. Cheese?  
- If you collect at least half of the treasures in a level, you will be rewarded with an extra life, which can only delay your inevitable doom (or boredom, if you're anywhere decent at arcade games).  
- Warp to a new level by pressing enter.  
  
**Have fun!**  
  
\- Samantha Stahlke, Charlatan of Doom
