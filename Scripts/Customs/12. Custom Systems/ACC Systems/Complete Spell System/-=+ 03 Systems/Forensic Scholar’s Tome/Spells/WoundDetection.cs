using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Gumps;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class WoundDetection : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Wound Detection", "Detcus Hox",
            21004, // Sound effect
            9300  // Icon ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 25; } }

        public WoundDetection(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private WoundDetection m_Owner;

            public InternalTarget(WoundDetection owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (!from.CanSee(target))
                    {
                        from.SendLocalizedMessage(500237); // Target can not be seen.
                    }
                    else if (m_Owner.CheckSequence())
                    {
                        from.PlaySound(2104); // Play sound effect for casting
                        Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x376A, 1, 29, 0x13B5, 2, 9962, 0); // Show particle effect

                        from.SendMessage("You reveal the target's wounds and stats.");
                        target.SendMessage("You feel a strange presence inspecting your wounds.");

                        // Show Gump with health and stats
                        from.SendGump(new WoundDetectionGump(target));

                        m_Owner.FinishSequence();
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class WoundDetectionGump : Gump
        {
            private Mobile m_Target;

            public WoundDetectionGump(Mobile target) : base(50, 50)
            {
                m_Target = target;

                AddPage(0);

                AddBackground(0, 0, 300, 200, 5054);
                AddLabel(50, 20, 1152, "Wound Detection");

                AddLabel(50, 60, 0, $"Health: {m_Target.Hits}/{m_Target.HitsMax}");
                AddLabel(50, 80, 0, $"Mana: {m_Target.Mana}/{m_Target.ManaMax}");
                AddLabel(50, 100, 0, $"Stamina: {m_Target.Stam}/{m_Target.StamMax}");

                AddLabel(50, 140, 0, $"Strength: {m_Target.Str}");
                AddLabel(50, 160, 0, $"Dexterity: {m_Target.Dex}");
                AddLabel(50, 180, 0, $"Intelligence: {m_Target.Int}");
            }
        }
    }
}
