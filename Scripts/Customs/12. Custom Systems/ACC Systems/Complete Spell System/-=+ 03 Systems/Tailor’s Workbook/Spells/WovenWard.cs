using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items; // Import for EffectItem

namespace Server.ACC.CSS.Systems.TailoringMagic
{
    public class WovenWard : TailoringSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Woven Ward", "Proxus!",
            21005, 9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 30; } }

        public WovenWard(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private WovenWard m_Owner;

            public InternalTarget(WovenWard owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile m = (Mobile)target;

                    if (m_Owner.CheckBSequence(m))
                    {
                        SpellHelper.Turn(m_Owner.Caster, m);

                        // Play sound effect
                        m.PlaySound(0x1F2);

                        // Visual effect around the target
                        Effects.SendLocationParticles(m, 0x376A, 9, 32, 5020);

                        // Apply buff (Replace with an appropriate BuffIcon if necessary)
                        BuffInfo.AddBuff(m, new BuffInfo(BuffIcon.Bless, 1075812, 1075813, TimeSpan.FromSeconds(30), m));

                        // Increase defense and resistance
                        m.VirtualArmorMod += 10; // Increase armor by 10
                        m.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, 5));
                        m.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, 5));
                        m.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, 5));
                        m.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, 5));
                        m.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, 5));

                        // Timer to remove buffs after 30 seconds
                        Timer.DelayCall(TimeSpan.FromSeconds(30), () => RemoveBuff(m));
                    }
                }
                else
                {
                    from.SendMessage("You cannot target that.");
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }

            private void RemoveBuff(Mobile m)
            {
                if (m == null || m.Deleted)
                    return;

                m.VirtualArmorMod -= 10;
                m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, 5));
                m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Fire, 5));
                m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Cold, 5));
                m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Poison, 5));
                m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Energy, 5));

                BuffInfo.RemoveBuff(m, BuffIcon.Bless); // Use correct buff icon here
            }
        }
    }
}
