using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.StealingMagic
{
    public class InformationGatherer : StealingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Information Gatherer", "In Legi Verum",
            21004, // Icon ID
            9300   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; } // Adjust as needed
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 10; } }

        public InformationGatherer(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private InformationGatherer m_Owner;

            public InternalTarget(InformationGatherer owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (target == m_Owner.Caster)
                    {
                        from.SendMessage("You cannot gather information about yourself.");
                        return;
                    }

                    if (!from.CanSee(target))
                    {
                        from.SendLocalizedMessage(500237); // Target can not be seen.
                    }
                    else if (m_Owner.CheckSequence())
                    {
                        m_Owner.Caster.SendMessage("You start observing your target...");
                        
                        // Apply effect: Gain insight on target's resistances
                        string weaknesses = GetTargetWeaknesses(target);
                        m_Owner.Caster.SendMessage($"You observe and note the following weaknesses: {weaknesses}");

                        // Visual and sound effects
                        Effects.PlaySound(from.Location, from.Map, 0x653);
                        from.FixedParticles(0x374A, 10, 15, 5037, EffectLayer.Head);

                        // Apply buff to the caster to exploit the weaknesses
                        from.SendMessage("Your knowledge improves your ability to exploit weaknesses.");
                        from.AddStatMod(new StatMod(StatType.Dex, "InfoGathererDexBonus", 10, TimeSpan.FromSeconds(30)));

                        // Apply debuff to the target if they are an NPC
                        if (target is BaseCreature && !(target is PlayerMobile))
                        {
                            target.SendMessage("You feel exposed as your weaknesses are revealed.");
                            target.AddStatMod(new StatMod(StatType.Dex, "InfoGathererDexDebuff", -10, TimeSpan.FromSeconds(30)));
                        }
                    }
                }
                else
                {
                    from.SendMessage("That is not a valid target.");
                }

                m_Owner.FinishSequence();
            }

            private string GetTargetWeaknesses(Mobile target)
            {
                // Example logic to determine target's weaknesses based on resistances
                string weaknesses = "none";
                int lowestResistance = Math.Min(target.PhysicalResistance, Math.Min(target.FireResistance, Math.Min(target.ColdResistance, Math.Min(target.PoisonResistance, target.EnergyResistance))));

                if (lowestResistance == target.PhysicalResistance)
                    weaknesses = "Physical";
                else if (lowestResistance == target.FireResistance)
                    weaknesses = "Fire";
                else if (lowestResistance == target.ColdResistance)
                    weaknesses = "Cold";
                else if (lowestResistance == target.PoisonResistance)
                    weaknesses = "Poison";
                else if (lowestResistance == target.EnergyResistance)
                    weaknesses = "Energy";

                return weaknesses;
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
