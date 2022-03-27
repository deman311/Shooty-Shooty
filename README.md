# Shooty Shooty 🔫
Unity Game Development Course - Final Project -> 2 days, a perfect score (100/100 grade).

## Introduction ⭐
This project was the final project of a course I took at the end of my second year. I wanted to experiment with 'swift' development and see how fast
can I finish this project if I set down from start to finish in a sort of 'coding marathon'.

### Setting 🗒️
So the setting of the project was pretty simplistic, we had to create two teams of 2 players that will compete with each other. Theres the enemy team 🟥
and your team 🟦, obviously anyone other than yourself is an NPC. At the start of each round, the are guns and granades spawned on the level and
each 'contestant' has to pick both of them up before he can start shooting. The first team to eliminate the other team wins 🏆.

## Development
### Map 🗻
I've first started with creating the map, I decided I'm going to create an outside setting arena and use a trick where I encircle it with mountains so
that you wont be able to see the 'empty' world outside of it. I used a neat Unity tool called 'Terrain Creator' and shaped some mountains that encircle
a large area in the middle, then I've added colors to make it more realistic and painted rocks, cliffs and grass. Then I started thinking of stuff I can
add to this map, because it is an FPS game I though of things I can add that you can use for cover and that will fit, I chose containers and barrels. After
I created them and filled the map with some of them I thought the map started to look like a construction or garbage disposal zone and I liked the idea so
I added some garbage hills and it looked great! 🤩

[MAP VIEW] -> TBA MEDIA

### Models 🤼‍♂️
Because I wanted everything to be pretty simplistic so that it won't take too much time, I decided I'm going to make the models very basic with a body
obviously, head for headshots and hands to hold the weapons. Most complete 3d models online require you to pay for them and I didn't want to do that.
I created some basic animations and found a cool gun model for free and set down to create all the animations I'm going to need. After completing the gun
model and setting up all of the initial positions, various testings, I came up with the idea to add some visual effects like ricochets.

[MODEL VIEW] [ANIMATION GIF]

### Effects 🟤❇️🩸
In this part I started with writing all the calculations and Debug (i.e., Pathfinding lines, Fire lines, Hit points, etc...). I created 2 different
effects: one for when you shoot the ground that looks like dust and dirt flying off and another when you hit an object that looks like sparkles. 
After that I also added a blood spilling effect for when you hit the player. All of this really added to the immersion and I liked it!

[DIRT SPARKLE BLOOD]

### NPC AI 🧠
So for the last part I kind of skipped explaining the section where I had to calculate bullet direction, hitboxes, recoil etc... but it also took a couple
of hours before this logic was complete and the models could detect opposing team members, detect items and move around the map. After the basic stuff for
testing were complete I started writing scripts where the NPCs will walk around the map randomly and search for the initial items they need to pick up in
order to start playing. (This was not a part of the requirements but I thought it really cool that even the NPC's have to search for the items and it also
was a challange to make it realistic and good enough for them to compete with a human player and not be too easy / hard - it took some balancing).

Specifically, I had to write a 2 different scripts for the NPCs, one for the 'team leader' and one for the 'soldier' this is because I decided that the
game would be more fun if the teams moved together and not just randomly to make it feel more immersive - this way one member will always follow the other.
The player will be followed by the other blue member.

[FIGHTING GIF]

### Misc
Lastly, I've decided that I want to add a main menu to the game with music, so that at the end of each round the player could reset the game and also so
that the application would feel much more 'game-like', this also made me create a 'DeathCam' for when you die so that you can keep watching the game. 
The menu included controls and an exit button. I've also tweaked the volume and sounds of the in game effects, added a 'death sound' and played with
the lighting. Finally I've added some colored smoke effects and UI that marks the winning team at the end of a game.

[MAIN MENU] [WINNING SMOKE]

## Final Notes
This project was super fun and helped me sharpen my skills in Unity and C#, I ended up recieving a perfect score for it. There was something really cool
about doint a 'coding-marathon' and this is something I definitely want to test our more and even write about in the feature - maybe even streaming
the process or writing a more in dept documentry of the development as this is just a retrospective description of the process.

### Updates
After completing the project I gave a couple of people a copy so that they can test it out and I can have their opinion. I've added a few minor updates
such as gravity, a light machine gun for fun and some NPC script tweaks to optimize the difficulty.

[LMG SHOOTING]
