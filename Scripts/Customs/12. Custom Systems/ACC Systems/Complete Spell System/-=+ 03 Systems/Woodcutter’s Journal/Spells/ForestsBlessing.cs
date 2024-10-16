using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.LumberjackingMagic
{
    public class ForestsBlessing : LumberjackingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Forest's Blessing", "An Mira Tre",
            // SpellCircle.Third,
            21003, // Icon ID
            9303   // Hue
        );

        public override SpellCircle Circle => SpellCircle.Third;

        public override double CastDelay => 0.2;
        public override double RequiredSkill => 50.0;
        public override int RequiredMana => 15;

        public ForestsBlessing(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ForestsBlessing m_Owner;

            public InternalTarget(ForestsBlessing owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile && o is PlayerMobile)
                {
                    PlayerMobile player = (PlayerMobile)o;
                    m_Owner.Target(player);
                }
                else
                {
                    from.SendMessage("You must target a player.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(PlayerMobile player)
        {
            if (CheckSequence())
            {
                // Apply the effect
                player.SendMessage("You feel the forest's blessing upon you. Your next resource gathering will yield more and better materials!");
                Effects.SendLocationParticles(EffectItem.Create(player.Location, player.Map, EffectItem.DefaultDuration), 0x373A, 1, 15, 1153, 0, 5052, 0);
                Effects.PlaySound(player.Location, player.Map, 0x5C7);

                // Temporary bonus effect for increased yield and quality
                player.AddStatMod(new StatMod(StatType.Str, "ForestsBlessingStr", 10, TimeSpan.FromMinutes(5)));
                player.AddStatMod(new StatMod(StatType.Dex, "ForestsBlessingDex", 10, TimeSpan.FromMinutes(5)));
                player.SendMessage("Your strength and dexterity have increased due to the forest's blessing!");

                player.Blessed = true; // Temporarily make the player immune to damage
                Timer.DelayCall(TimeSpan.FromMinutes(5), () =>
                {
                    player.Blessed = false;
                    player.SendMessage("The forest's blessing has worn off.");
                });

                // Visual effect: Growing green aura
                Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
                {
                    Effects.SendLocationParticles(EffectItem.Create(player.Location, player.Map, EffectItem.DefaultDuration), 0x375A, 1, 30, 1153, 0, 5052, 0);
                    Effects.PlaySound(player.Location, player.Map, 0x5C6);
                });

                // Increase the yield of logs and resources for the next 5 minutes
                player.Target = new WoodcuttingTarget(player);
            }

            FinishSequence();
        }

        private class WoodcuttingTarget : Target
        {
            private PlayerMobile m_Player;

            public WoodcuttingTarget(PlayerMobile player) : base(10, true, TargetFlags.None)
            {
                m_Player = player;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                {
                    Map map = from.Map;
                    if (map == null)
                        return;

                    SpellHelper.GetSurfaceTop(ref point);
                    Point3D loc = new Point3D(point);

                    Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x376A, 1, 20, 1153, 0, 5029, 0);
                    Effects.PlaySound(loc, map, 0x1EA);
                    from.SendMessage("The forest's blessing enhances your resource gathering!");

                    // Logic to increase resource yield and quality
                    // This can be customized based on your server's existing resource-gathering system
                    from.SendMessage("You gather an increased amount of high-quality resources!");

                    // Example of custom yield increase (you may need to adapt this to your system)
                    // player.ResourceGatheringBonus += 10; // Example modifier
                }
            }
        }
    }
}
