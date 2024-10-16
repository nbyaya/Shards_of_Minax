using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.RemoveTrapMagic
{
    public class TrapAnalysis : RemoveTrapSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Trap Analysis", "Analiza Cupla",
            21001,
            9200
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 20.0; } }
        public override int RequiredMana { get { return 15; } }

        public TrapAnalysis(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private TrapAnalysis m_Owner;

            public InternalTarget(TrapAnalysis owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item trap) // Assuming traps are items
                {
                    if (!from.CanSee(targeted))
                    {
                        from.SendLocalizedMessage(500237); // Target cannot be seen.
                    }
                    else if (m_Owner.CheckSequence())
                    {
                        // Generic message about the trap
                        string trapName = trap.Name ?? "Unknown Trap"; // Default to "Unknown Trap" if Name is null

                        from.SendMessage($"You have analyzed the trap: {trapName}");
                        from.SendMessage("You gain insight into disarming this trap more effectively!");

                        // Play a sound and create visual effects
                        from.PlaySound(0x1FB); // Sound for detecting traps
                        trap.PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*analyzes the trap*");
                        Effects.SendLocationParticles(EffectItem.Create(trap.Location, trap.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5025);
                        
                        // Apply temporary bonus to disarm trap
                        from.AddStatMod(new StatMod(StatType.Int, "TrapAnalysisBonus", 5, TimeSpan.FromMinutes(1)));

                        // Success animation and end sequence
                        from.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Head);
                    }
                }
                else
                {
                    from.SendMessage("That is not a trap!");
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
