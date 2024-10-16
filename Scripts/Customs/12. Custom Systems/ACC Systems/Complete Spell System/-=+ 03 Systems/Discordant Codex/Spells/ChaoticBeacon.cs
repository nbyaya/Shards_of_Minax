using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;

namespace Server.ACC.CSS.Systems.DiscordanceMagic
{
    public class ChaoticBeacon : DiscordanceSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Chaotic Beacon", "Infusio Chaotica",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ChaoticBeacon(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private readonly ChaoticBeacon m_Spell;

            public InternalTarget(ChaoticBeacon spell) : base(12, false, TargetFlags.Harmful)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is Mobile)
                {
                    Mobile targ = (Mobile)target;

                    if (!from.CanBeHarmful(targ, false))
                    {
                        from.SendLocalizedMessage(1049535); // Target cannot be harmed.
                        return;
                    }

                    if (CheckResistancesAlreadyReduced(targ))
                    {
                        from.SendLocalizedMessage(1049537); // Target's resistances are already reduced.
                        return;
                    }

                    if (m_Spell.CheckSequence())
                    {
                        from.DoHarmful(targ);
                        from.Mana -= m_Spell.RequiredMana;

                        // Create visual and sound effects
                        Effects.SendTargetParticles(targ, 0x374A, 10, 15, 5021, EffectLayer.Waist);
                        Effects.PlaySound(targ.Location, targ.Map, 0x1F1);

                        // Reduce target resistances massively
                        int reductionAmount = -30; // Adjust the amount of reduction as needed
                        ApplyResistanceReduction(targ, reductionAmount);

                        from.SendMessage("You unleash chaotic energy, severely weakening your target's defenses!");
                        targ.SendMessage("A wave of chaotic energy washes over you, weakening your resistances!");

                        // Set a timer to restore resistances after some time
                        Timer.DelayCall(TimeSpan.FromSeconds(30), () => RestoreResistances(targ));
                    }
                }
                else
                {
                    from.SendLocalizedMessage(1049535); // Target cannot be harmed.
                }

                m_Spell.FinishSequence();
            }
        }

        private static void ApplyResistanceReduction(Mobile target, int amount)
        {
            // Apply resistance mods to reduce all resistances by 'amount'
            target.AddResistanceMod(new ResistanceMod(ResistanceType.Physical, amount));
            target.AddResistanceMod(new ResistanceMod(ResistanceType.Fire, amount));
            target.AddResistanceMod(new ResistanceMod(ResistanceType.Cold, amount));
            target.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, amount));
            target.AddResistanceMod(new ResistanceMod(ResistanceType.Energy, amount));
        }

        private static void RestoreResistances(Mobile target)
        {
            // Restore resistances back to their original state
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, -30));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Fire, -30));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Cold, -30));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Poison, -30));
            target.RemoveResistanceMod(new ResistanceMod(ResistanceType.Energy, -30));
        }

        private static bool CheckResistancesAlreadyReduced(Mobile target)
        {
            if (target == null || target.ResistanceMods == null)
            {
                return false; // Consider resistances not reduced if target or its resistance mods are null
            }

            // Check if the target already has reduced resistances
            foreach (ResistanceMod mod in target.ResistanceMods)
            {
                if (mod.Type == ResistanceType.Physical && mod.Offset < 0)
                {
                    return true;
                }
            }

            return false;
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.0);
        }
    }
}
