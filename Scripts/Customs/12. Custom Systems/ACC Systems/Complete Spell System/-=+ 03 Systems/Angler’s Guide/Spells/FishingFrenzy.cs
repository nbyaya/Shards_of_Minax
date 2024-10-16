using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class FishingFrenzy : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Fishing Frenzy", "Summon Fish!",
                                                        21004, // Sound ID for casting
                                                        9300  // Animation ID for casting
                                                       );

        public override SpellCircle Circle => SpellCircle.Second;
        public override double CastDelay => 0.2;
        public override double RequiredSkill => 25.0;
        public override int RequiredMana => 15;

        private static readonly Type[] FishTypes = new Type[]
        {
            typeof(Dolphin),
            typeof(Dolphin),
            typeof(Dolphin),
            typeof(Dolphin),
            typeof(Dolphin)
        };

        public FishingFrenzy(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
                return;
            }

            if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                Point3D loc = new Point3D(p);
                Map map = Caster.Map;

                if (map == null)
                    return;

                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5052); // Fish effect
                Effects.PlaySound(loc, map, 21004); // Sound of water splashing

                for (int i = 0; i < Utility.RandomMinMax(3, 6); i++) // Random number of fish between 3 and 6
                {
                    Type fishType = FishTypes[Utility.Random(FishTypes.Length)];
                    BaseCreature fish = (BaseCreature)Activator.CreateInstance(fishType);
                    fish.MoveToWorld(loc, map);
                    fish.PlaySound(0x026); // Fish splash sound
                }

            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private FishingFrenzy m_Owner;

            public InternalTarget(FishingFrenzy owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point)
                {
                    m_Owner.Target(point);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
