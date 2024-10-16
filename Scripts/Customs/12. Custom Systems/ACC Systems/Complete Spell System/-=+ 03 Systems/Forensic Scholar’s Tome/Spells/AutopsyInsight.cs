using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;


namespace Server.ACC.CSS.Systems.ForensicsMagic
{
    public class AutopsyInsight : ForensicsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Autopsy Insight", "Inspect Mortem",
            21004, // Icon ID
            9300   // Cast Sound
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 25; } }

        public AutopsyInsight(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private AutopsyInsight m_Owner;

            public InternalTarget(AutopsyInsight owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Corpse)
                {
                    Corpse corpse = (Corpse)o;
                    if (corpse.IsDecoContainer)
                    {
                        from.SendMessage("You cannot perform an autopsy on this.");
                        return;
                    }

                    if (m_Owner.CheckSequence())
                    {
                        // Play visual and sound effects
                        from.PlaySound(0x214);
                        from.FixedParticles(0x374A, 10, 15, 5036, EffectLayer.Head);

                        // List of possible "fortunes"
                        string[] fortunes = new string[]
                        {
                            "They met a swift end.",
                            "They suffered a lingering illness.",
                            "They were felled by a mighty blow.",
                            "Poison claimed their life.",
                            "A tragic accident led to their demise.",
                            "They died of natural causes.",
                            "A fierce battle took them.",
                            "A beast claimed their life.",
                            "They drowned in water.",
                            "Their fate was sealed by betrayal."
                        };

                        // Randomly select a fortune
                        string fortune = fortunes[Utility.Random(fortunes.Length)];
                        from.SendMessage($"Upon examining the corpse, you determine: {fortune}");

                        // Additional effects for flair
                        corpse.PublicOverheadMessage(MessageType.Emote, 0x3B2, false, "*A mysterious aura surrounds the corpse as you examine it*");
                        Effects.SendLocationParticles(EffectItem.Create(corpse.Location, corpse.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                        Effects.PlaySound(corpse.Location, corpse.Map, 0x1FB);
                    }
                }
                else
                {
                    from.SendMessage("You must target a corpse.");
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
