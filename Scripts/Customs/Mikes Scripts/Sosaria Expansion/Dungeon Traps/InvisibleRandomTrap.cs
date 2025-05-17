using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells; // Needed for Poison and SpellHelper

namespace Server.Items
{
    public class InvisibleRandomTrap : Item
    {
        // --- Configuration ---
        private const double MaxSkillValue = 200.0; // Skill value at which trap never triggers
        private const int TeleportRange = 8; // How far the teleport trap can send the player
        // ---------------------

        [Constructable]
        public InvisibleRandomTrap() : base(0x1BC3) // Placeholder ItemID, it's invisible
        {
            Visible = false; // Make it invisible
            Movable = false; // Prevent players from picking it up
            Name = "Invisible Random Trap"; // Name for GM identification
        }

        public InvisibleRandomTrap(Serial serial) : base(serial)
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m is PlayerMobile && m.Alive && !m.IsDeadBondedPet) // Check if it's a living player
            {
                PlayerMobile player = (PlayerMobile)m;

                // Get RemoveTrap skill
                double removeTrapSkill = player.Skills[SkillName.RemoveTrap].Value;

                // Calculate chance for the trap to ACTIVATE (higher skill = lower chance)
                // At 0 skill = 1.0 (100%), at MaxSkillValue skill = 0.0 (0%)
                double chanceToActivate = Math.Max(0.0, 1.0 - (removeTrapSkill / MaxSkillValue));

                // Roll the dice
                if (Utility.RandomDouble() < chanceToActivate)
                {
                    // --- Trap Activates! ---
                    TriggerTrapEffect(player);
                }
                else
                {
                    // --- Trap Detected and Disabled ---
                    player.SendMessage("You skillfully detect and disable an invisible trap!");
                    // Optional: Give skill points for detecting
                    player.CheckSkill(SkillName.RemoveTrap, 0.0, 100.0); // Adjust min/max gain if desired
                    this.Delete(); // Trap is removed
                }
                // Return false to allow movement over the spot AFTER the check.
                // If you returned true upon detection/activation, the player might get stuck momentarily.
                return false;
            }

            // Allow non-players (monsters, pets maybe?) to pass without triggering or blocking
            return base.OnMoveOver(m);
        }

        private void TriggerTrapEffect(PlayerMobile player)
        {
            if (player == null || player.Map == null || player.Map == Map.Internal)
                return; // Basic safety check

            // Choose a random trap effect (0 to 10, inclusive = 11 types)
            int trapType = Utility.Random(11);

            // Get player location for effects
            Point3D loc = player.Location;
            Map map = player.Map;

            // Send message to player *before* potential death or disconnect
            player.SendMessage("You trigger a hidden trap!");

            try // Use try-catch for safety during effect application
            {
                 switch (trapType)
                 {
                    case 0: // Stun Rune (10 seconds)
                        player.SendMessage("A magical rune flares, freezing you in place!");
                        Effects.SendLocationEffect(loc, map, 0x0E62, 16, 1, 0, 0); // Animation
                        player.PlaySound(0x28C); // Example sound
                        player.Frozen = true;
                        Timer.DelayCall(TimeSpan.FromSeconds(10), UnfreezePlayer_Callback, player); // Pass player directly
                        break;

                    case 1: // Teleport Trap
                        player.SendMessage("The ground shimmers and you are suddenly elsewhere!");
                        Effects.SendLocationEffect(loc, map, 0x0F6C, 16, 1, 0, 0); // Animation
                        player.PlaySound(0x1FE); // Teleport sound
                        Point3D newLoc = FindValidTeleportLocation(player, TeleportRange);
                        if (newLoc != Point3D.Zero)
                        {
                            // Teleport the player
                            player.MoveToWorld(newLoc, player.Map);
                            // Play effect at the *new* location too if desired
                            // Effects.SendLocationEffect(newLoc, player.Map, 0x0F6C, 16, 1, 0, 0);
                        }
                        else {
                             player.SendMessage("...but the teleportation fizzles."); // Fallback
                        }
                        break;

                    case 2: // Web Trap (4 seconds + Deadly Poison)
                        player.SendMessage("Sticky webs erupt, ensnaring you and delivering a venomous bite!");
                        Effects.SendLocationEffect(loc, map, 0x10DA, 16, 1, 0, 0); // Animation
                        player.PlaySound(0x164); // Example sound
                        player.Frozen = true;
                        player.ApplyPoison(player, Poison.Deadly); // Apply deadly poison
                        Timer.DelayCall(TimeSpan.FromSeconds(4), UnfreezePlayer_Callback, player); // Pass player directly
                        break;

                    case 3: // Fire Trap (50 damage)
                        player.SendMessage("Flames erupt from the floor!");
                        Effects.SendLocationEffect(loc, map, 0x10F7, 16, 1, 0, 0); // Animation (Could also use 0x3709)
                        player.PlaySound(0x208); // Fire sound
                        // *** FIX: Pass null as attacker for environmental damage ***
                        player.Damage(50, null);
                        break;

                    case 4: // Saw Trap (60 damage)
                         player.SendMessage("Whirring saw blades slice at you from below!");
                        Effects.SendLocationEffect(loc, map, 0x11AD, 16, 1, 0, 0); // Animation
                        player.PlaySound(0x21C); // Example sound (Blade hit)
                        // *** FIX: Pass null as attacker for environmental damage ***
                        player.Damage(60, null);
                        break;

                    case 5: // Spike Trap (40 damage)
                        player.SendMessage("Sharp spikes shoot from the floor!");
                        Effects.SendLocationEffect(loc, map, 0x119A, 16, 1, 0, 0); // Animation
                        player.PlaySound(0x236); // Spike sound
                        // *** FIX: Pass null as attacker for environmental damage ***
                        player.Damage(40, null);
                        break;

                    case 6: // Mushroom Trap (Drain Stamina)
                        player.SendMessage("A strange puffball explodes, draining your energy!");
                        Effects.SendLocationEffect(loc, map, 0x1126, 16, 1, 0, 0); // Animation (Generic puff)
                        player.PlaySound(0x48F); // Example sound (Puff)
                        player.Stam = 0; // Drain all stamina
                        break;

                    case 7: // Gas Trap (Deadly Poison)
                        player.SendMessage("Noxious gas billows around you!");
                        Effects.SendLocationEffect(loc, map, 0x1126, 16, 1, 0, 0); // Animation (Generic puff)
                        player.PlaySound(0x230); // Gas sound
                        player.ApplyPoison(player, Poison.Deadly); // Apply deadly poison
                        break;

                    case 8: // Magic Trap (Drain Mana)
                        player.SendMessage("A surge of energy drains your magical reserves!");
                        Effects.SendLocationEffect(loc, map, 0x1126, 16, 1, 0, 0); // Animation (Generic puff)
                        player.PlaySound(0x211); // Example sound (Fizzle)
                        player.Mana = 0; // Drain all mana
                        break;

                    case 9: // Axe Trap (Set HP to 1)
                        player.SendMessage("A hidden axe swings out, nearly killing you!");
                        Effects.SendLocationEffect(loc, map, 0x1193, 16, 1, 0, 0); // Animation
                        player.PlaySound(0x21D); // Example sound (Swing/Hit)
                        if (player.Hits > 1) // Don't set HP if already 1 or less
                        {
                             player.Hits = 1;
                        }
                        break;

                    case 10: // Guillotine Trap (Instant Death)
                        player.SendMessage("A massive blade descends swiftly... darkness.");
                        Effects.SendLocationEffect(loc, map, 0x1245, 16, 1, 0, 0); // Animation
                        player.PlaySound(0x21D); // Example sound (Heavy blade)
                        // player.Damage(player.HitsMax + 50, null); // Alternative: Massive damage
                        player.Kill(); // Instant death
                        break;
                 }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error applying trap effect for {player.Name}: {e}");
                // Log error appropriately for your server setup
            }
            finally
            {
                // Trap is used up, delete it ONLY if it hasn't already been deleted
                // (e.g., if an error occurred before deletion but after activation started)
                 if (!this.Deleted)
                     this.Delete();
            }
        }

        // Timer callback to unfreeze the player
        // Note: Made the argument type 'object' to match Timer.DelayCall signature
        private static void UnfreezePlayer_Callback(object state)
        {
            if (state is PlayerMobile player && player != null && !player.Deleted) // Check if player is valid
            {
                player.Frozen = false;
                player.SendMessage("You can move again.");
            }
        }

        // Helper to find a valid nearby location for teleportation
        private Point3D FindValidTeleportLocation(PlayerMobile pm, int range)
        {
            if (pm == null || pm.Map == null || pm.Map == Map.Internal)
                return Point3D.Zero;

            Map map = pm.Map;

            for (int i = 0; i < 20; ++i) // Try 20 times to find a spot
            {
                int x = pm.X + Utility.RandomMinMax(-range, range);
                int y = pm.Y + Utility.RandomMinMax(-range, range);
                int z = 0; // Z will be determined below

                // --- Manual Z Calculation (Compatible Method) ---
                int landZ = map.Tiles.GetLandTile(x, y).Z; // Get the base ground Z
                int topZ = landZ; // Start with land Z as the highest point initially

                // Check static items at the location for potential surfaces
                StaticTile[] staticTiles = map.Tiles.GetStaticTiles(x, y, true); // Get all static items

                if (staticTiles != null && staticTiles.Length > 0)
                {
                    foreach (StaticTile tile in staticTiles)
                    {
                        ItemData itemData = TileData.ItemTable[tile.ID & TileData.MaxItemValue]; // Masking ID is safer

                        // Check if the item is a surface or impassable below the player's potential feet
                        // We want the highest Z coordinate that is considered a surface the player can stand on.
                        if (itemData.Surface || itemData.Impassable) // Impassable needed for things like steps, large rocks etc.
                        {
                            int currentTileTop = tile.Z + itemData.CalcHeight; // CalcHeight preferred over Height

                            // If this tile's top is higher than the current highest surface we know, update topZ
                            // We only care about surfaces below or at a reasonable step height above the current topZ
                            // This prevents picking the top of a tall wall if land is lower.
                            // A simple highest check often works well enough here:
                            if (currentTileTop > topZ)
                            {
                                // Check further: Is this new height significantly higher than the land?
                                // If so, it might be a wall top. A simple check might be needed.
                                // For now, we assume the highest surface/impassable is the target.
                                topZ = currentTileTop;
                            }
                             // More precise logic could check if the tile's Z is closer to the current topZ
                             // or if it's significantly higher (indicating a wall).
                             // Example refinement (optional):
                             // if (currentTileTop > topZ && currentTileTop <= topZ + 8) // Only consider surfaces within 8 units of height difference?
                             //     topZ = currentTileTop;
                             // else if (tile.Z > topZ && itemData.Surface) // Handle base Z of surface items slightly above ground
                             //      topZ = tile.Z; // This might be wrong, top is better.
                        }
                    }
                }
                z = topZ; // The calculated Z coordinate for the surface
                // --- End Manual Z Calculation ---

                Point3D potentialLoc = new Point3D(x, y, z);

                // Check if location is valid for spawning (basic check)
                // AND Use CanFit to check if a player (height 16) can stand there without obstruction above.
                // AND check if the player is allowed to teleport from their origin.
                if (map.CanSpawnMobile(potentialLoc)
                    && map.CanFit(x, y, z, 16, false, false, true) // Check height clearance
                    && SpellHelper.CheckTravel(pm, TravelCheckType.TeleportFrom))
                {
                    return potentialLoc;
                }
            }

            // Fallback: If no suitable spot found after tries, return Zero.
            Console.WriteLine($"DEBUG: InvisibleRandomTrap could not find valid teleport location for {pm.Name} near ({pm.X},{pm.Y},{pm.Z})");
            return Point3D.Zero;
        }


        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

             // Ensure it's invisible on load, just in case
             Timer.DelayCall(TimeSpan.Zero, () => { if (this != null) this.Visible = false; });
        }
    }
}