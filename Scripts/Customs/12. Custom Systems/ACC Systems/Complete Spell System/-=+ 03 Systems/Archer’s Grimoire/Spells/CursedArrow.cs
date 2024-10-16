using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.ArcheryMagic
{
    public class CursedArrow : ArcherySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Cursed Arrow", "Aris Cursus",
            21005, // Icon ID
            9301  // Action ID
        );

        public override SpellCircle Circle { get { return SpellCircle.Third; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public CursedArrow(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void Target(Mobile target)
        {
            if (!Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckHSequence(target))
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, target);

                // Play a flashy visual effect on the target
                target.FixedParticles(0x374A, 10, 30, 5054, EffectLayer.Head);
                target.PlaySound(0x1FB); // Sound effect

                // Inflict a curse that lowers the target's resistances
                target.SendMessage("You feel a dark magic weakening your resistances!");
                ApplyCurseEffect(target);

                FinishSequence();
            }
        }

        private void ApplyCurseEffect(Mobile target)
        {
            // Apply a debuff to lower the target's resistances for a duration
            // Here we lower Physical and Magical Resistances by 10 for 30 seconds
            ResistanceMod[] mods = new ResistanceMod[]
            {
                new ResistanceMod(ResistanceType.Physical, -10),
                new ResistanceMod(ResistanceType.Fire, -10),
                new ResistanceMod(ResistanceType.Cold, -10),
                new ResistanceMod(ResistanceType.Poison, -10),
                new ResistanceMod(ResistanceType.Energy, -10)
            };

            for (int i = 0; i < mods.Length; i++)
                target.AddResistanceMod(mods[i]);

            Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
            {
                for (int i = 0; i < mods.Length; i++)
                    target.RemoveResistanceMod(mods[i]);

                target.SendMessage("The dark magic dissipates, and your resistances return to normal.");
            });
        }

        private class InternalTarget : Target
        {
            private CursedArrow m_Owner;

            public InternalTarget(CursedArrow owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                    m_Owner.Target(target);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
