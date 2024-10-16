using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class ExplorersFortune : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Explorer’s Fortune", "Fortuna Exlporatus",
            21004, // Icon ID for spellbook
            9300   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        // Change return type to double and return the number of seconds
        public override double CastDelay { get { return 3.0; } } // 3 seconds delay
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 50; } }

        private static readonly TimeSpan Duration = TimeSpan.FromMinutes(30);
        private static readonly TimeSpan Cooldown = TimeSpan.FromHours(1);

        public ExplorersFortune(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
            else
            {
                Caster.SendMessage("The spell fizzles.");
            }

            FinishSequence();
        }

        private void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            SpellHelper.Turn(Caster, p);

            Point3D loc = new Point3D(p);
            Map map = Caster.Map;

            if (map == null)
                return;

            Effects.PlaySound(loc, map, 0x2CE); // Sound effect
            Caster.FixedParticles(0x375A, 10, 30, 5013, EffectLayer.Waist); // Visual effect

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(2.0), (int)(Duration.TotalSeconds / 2.0), delegate
            {
                SpawnTreasure(loc, map, Caster);
            });

            Caster.SendMessage("You invoke the fortune of explorers, scattering treasures around!");
            Caster.BeginAction(typeof(ExplorersFortune));

            Timer.DelayCall(Duration, delegate
            {
                Caster.EndAction(typeof(ExplorersFortune));
            });
        }

        private void SpawnTreasure(Point3D loc, Map map, Mobile caster)
        {
            if (caster == null || caster.Deleted || !caster.Alive)
                return;

            int x = loc.X + Utility.RandomMinMax(-40, 40);
            int y = loc.Y + Utility.RandomMinMax(-40, 40);
            int z = map.GetAverageZ(x, y);

            Point3D spawnLocation = new Point3D(x, y, z);
            
            if (!map.CanSpawnMobile(spawnLocation))
                return;

            Item treasure = null;
            if (Utility.RandomDouble() < 0.7) // 70% chance for gold
            {
                treasure = new Gold(Utility.RandomMinMax(50, 200)); // Random gold pile
            }
            else // 30% chance for gems
            {
                treasure = new Diamond(); // Assume Diamond is a valid item in the system
                treasure.Amount = Utility.RandomMinMax(1, 3); // Random number of gems
            }

            treasure.MoveToWorld(spawnLocation, map);

            Effects.SendLocationParticles(EffectItem.Create(spawnLocation, map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5024); // Sparkle effect
        }

        private class InternalTarget : Target
        {
            private ExplorersFortune m_Owner;

            public InternalTarget(ExplorersFortune owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                {
                    m_Owner.Target(p);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
