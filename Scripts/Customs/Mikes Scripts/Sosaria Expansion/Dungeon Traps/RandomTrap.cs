using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Network;
using Server.Spells.Ninjitsu;

namespace Server.Items
{
    public class RandomTrap : Item
    {
        [Constructable]
        public RandomTrap() : base(0x1B72)
        {
            Visible = false;
            Movable = false;
            Name = "an invisible trap";
        }

        public RandomTrap(Serial serial) : base(serial)
        {
        }

        public override bool OnMoveOver(Mobile m)
        {
            if (m is PlayerMobile pm && !pm.Alive)
                return true;

            if (m is PlayerMobile player)
            {
                // Manual clamping for older frameworks
                double trapSkill = player.Skills.RemoveTrap.Value;
                trapSkill = trapSkill < 0 ? 0 : trapSkill > 200 ? 200 : trapSkill;
                double disableChance = (trapSkill / 200.0) * 100;

                if (Utility.RandomDouble() * 100 < disableChance)
                {
                    player.SendMessage("You successfully detect and disable a hidden trap!");
                    Delete();
                    return true;
                }

                ActivateTrap(player);
            }

            return base.OnMoveOver(m);
        }

        private void ActivateTrap(PlayerMobile player)
        {
            player.SendMessage("You trigger a hidden trap!");
            Delete();

            switch (Utility.Random(11))
            {
                case 0: StunRune(player); break;
                case 1: TeleportTrap(player); break;
                case 2: WebTrap(player); break;
                case 3: FireTrap(player); break;
                case 4: SawTrap(player); break;
                case 5: SpikeTrap(player); break;
                case 6: MushroomTrap(player); break;
                case 7: GasTrap(player); break;
                case 8: MagicTrap(player); break;
                case 9: AxeTrap(player); break;
                case 10: GuillotineTrap(player); break;
            }
        }

        private void PlayAnimation(int id, int duration, int speed, int particleEffect)
        {
            Effects.SendLocationEffect(Location, Map, id, duration, speed, particleEffect);
            Effects.PlaySound(Location, Map, 0x1FA);
        }

        #region Trap Effects
        private void StunRune(PlayerMobile player)
        {
            PlayAnimation(0x376A, 1000, 10, 0); // 10 second duration
            player.Freeze(TimeSpan.FromSeconds(10));
            player.SendMessage("You're frozen in place by a stun rune!");
        }

        private void TeleportTrap(PlayerMobile player)
        {
            PlayAnimation(0x1CC4, 500, 10, 0); // 5 second duration
            Point3D loc = player.Location;
            loc.X += Utility.RandomMinMax(-5, 5);
            loc.Y += Utility.RandomMinMax(-5, 5);
            player.MoveToWorld(loc, player.Map);
            player.SendMessage("You're teleported by a trap!");
        }

        private void WebTrap(PlayerMobile player)
        {
            PlayAnimation(0x10DA, 400, 10, 0); // 4 second duration
            player.Freeze(TimeSpan.FromSeconds(4));
            player.ApplyPoison(player, Poison.Deadly);
            player.SendMessage("You're caught in a poisonous web!");
        }

        private void FireTrap(PlayerMobile player)
        {
            PlayAnimation(0x10F7, 500, 15, 0); // 5 second duration
            AOS.Damage(player, 50, 0, 100, 0, 0, 0);
            player.SendMessage("You're burned by a fire trap!");
        }

        private void SawTrap(PlayerMobile player)
        {
            PlayAnimation(0x11AD, 500, 15, 0); // 5 second duration
            AOS.Damage(player, 60, 0, 100, 0, 0, 0);
            player.SendMessage("You're slashed by saw blades!");
        }

        private void SpikeTrap(PlayerMobile player)
        {
            PlayAnimation(0x119A, 500, 15, 0); // 5 second duration
            AOS.Damage(player, 40, 0, 100, 0, 0, 0);
            player.SendMessage("You're impaled by spikes!");
        }

        private void MushroomTrap(PlayerMobile player)
        {
            PlayAnimation(0x1126, 500, 10, 0); // 5 second duration
            player.Stam = 0;
            player.SendMessage("Toxic mushrooms drain your stamina!");
        }

        private void GasTrap(PlayerMobile player)
        {
            PlayAnimation(0x1126, 500, 10, 0); // 5 second duration
            player.ApplyPoison(player, Poison.Deadly);
            player.SendMessage("You're poisoned by toxic gas!");
        }

        private void MagicTrap(PlayerMobile player)
        {
            PlayAnimation(0x1126, 500, 10, 0); // 5 second duration
            player.Mana = 0;
            player.SendMessage("Your mana is drained by arcane forces!");
        }

        private void AxeTrap(PlayerMobile player)
        {
            PlayAnimation(0x1193, 500, 10, 0); // 5 second duration
            player.Hits = 1;
            player.SendMessage("An axe trap nearly kills you!");
        }

        private void GuillotineTrap(PlayerMobile player)
        {
            PlayAnimation(0x1245, 500, 10, 0); // 5 second duration
            player.Kill();
            player.SendMessage("A guillotine trap kills you instantly!");
        }
        #endregion

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}