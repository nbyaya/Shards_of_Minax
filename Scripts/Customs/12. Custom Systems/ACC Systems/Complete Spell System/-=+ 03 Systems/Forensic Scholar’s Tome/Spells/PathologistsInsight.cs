using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Gumps;
using Server.Spells;

namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class PathologistsInsight : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Pathologist's Insight", "Reveal Resisting Flesh",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 25; } }

        public PathologistsInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PathologistsInsight m_Owner;

            public InternalTarget(PathologistsInsight owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && target is BaseCreature)
                {
                    m_Owner.DisplayResistances(from, (BaseCreature)target);
                }
                else
                {
                    from.SendMessage("You must target a creature to see its resistances.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void DisplayResistances(Mobile from, BaseCreature target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                Effects.SendLocationEffect(target.Location, target.Map, 0x376A, 10, 1, 1153, 0); // Flashy visual effect
                Effects.PlaySound(target.Location, target.Map, 0x5C3); // Sound effect

                from.SendGump(new ResistanceGump(target)); // Open gump to display resistances
            }

            FinishSequence();
        }
    }

    public class ResistanceGump : Gump
    {
        private BaseCreature m_Target;

        public ResistanceGump(BaseCreature target) : base(50, 50)
        {
            m_Target = target;

            AddPage(0);

            AddBackground(0, 0, 260, 170, 5054);
            AddLabel(100, 20, 0, "Creature Resistances");

            AddLabel(50, 50, 0, $"Physical: {m_Target.PhysicalResistance}%");
            AddLabel(50, 70, 0, $"Fire: {m_Target.FireResistance}%");
            AddLabel(50, 90, 0, $"Cold: {m_Target.ColdResistance}%");
            AddLabel(50, 110, 0, $"Poison: {m_Target.PoisonResistance}%");
            AddLabel(50, 130, 0, $"Energy: {m_Target.EnergyResistance}%");
        }
    }
}
