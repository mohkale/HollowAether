<p>
  <a href="https://github.com/mohkale/HollowAether">
    <img alt="header" src="./.github/header.jpg"/>
  </a>
</p>

This is a repository for what was my final year project from [TRC][trc].
A ~~tiny~~ 2D game engine (and level editor) written with C#, [MonoGame][monogame]
and WinForms.

[trc]: https://www.trc.ac.uk/
[monogame]: https://www.monogame.net/

I'm uploading it here for archival purposes, when I get a chance I might clean this
repository up a bit, but consider it archived :grin:.

**NOTE**: I've been having some trouble getting this to compile inside a window VM, when
I get a chance I'll install a new windows partition so that I can share a release
binary. In the meantime, in lieu of a demo video/gif, I'll just upload my final
[report][report].

[report]: ./report.pdf

## The Game
The game itself is a simple platformer, I repurposed a lot of art from
[Cave Story](https://store.steampowered.com/app/200900/Cave_Story/).

<div>
  <img alt="cover" src="./.github/image37.png" />
  <img alt="cover" src="./.github/image40.png" />
</div>

The main character has a sword hovering around them which they can throw forward.
You can call the sword back manually (by releasing the throw button) or wait until it
reaches the apex of it's throw (at which point it'll return automatically).

There are chests scattered around the level which release coins, the simple aim is to
get to a door at the end of the level while collecting as many coins as you can and
avoiding any monsters.

There are 4 monster types in the game:
- The badeye hovers in a circle and releases projectiles towards the player.
- The bat seeks out the player and harms him by coming in contact.
- The jumper jumps up to collide with the player when they try going over him.
- The crusher drops over the player (this enemy was taken from the Mario franchise).

## Level Editor
<img alt="Cover" src="./.github/image31.png" align="left" />

The level editor is a WinForms application that lets you select sprites from a tile
map and then drop them into the map (or *zone*). Each dropped sprite has an Entity
type describing how other sprites interact with them. You can select an edit the
properties of one or more Entities or even modify their entity type. You can also
create animations from tilemaps that you can later attach to sprites.

<table><tbody><tr><th></th></tr></tbody></table>

The rest of this section just consists of some screenshots of the level editor in
action (I seem to have taken quite a lot of these).

<p align="center">
  <img alt="Edit" src="./.github/image29.png"/>
  <img alt="Animation" src="./.github/image32.png"/>
  <img alt="Animation" src="./.github/image33.png"/>
  <img alt="Animation" src="./.github/image27.png"/>
</p>

## Credits
I downloaded assets from multiple sources whilst developing, here's a list of the
ones I ended up using:
- [sci-fi-platformer-tileset](https://opengameart.org/content/sci-fi-platformer-tileset)
- [meditator-statues](https://opengameart.org/content/meditator-statues-32x32)
