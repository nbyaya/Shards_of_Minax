using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Spells;
using Server.Targeting;
using Server;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class TreeForm : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tree Form", "Entaria Arborum",
            21004,
            9300,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Seventh; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 40; } }

        public TreeForm(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x376A, 1, 62, 9934, EffectLayer.Waist); // Visual effect for transformation
                Caster.PlaySound(0x1F7); // Sound effect for transformation

                Caster.Say("*takes the form of a mighty tree*");

                // Spawn the tree item and apply effects
                new TreeFormEffect(Caster).Start();
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }

        private class TreeFormEffect : Timer
        {
            private Mobile m_Caster;
            private DateTime m_End;
            private Item m_TreeGraphic;
            private int m_OriginalArmorRating;
            private int m_OriginalHits;
            private int m_OriginalPhysicalResistance;

            public TreeFormEffect(Mobile caster) : base(TimeSpan.Zero, TimeSpan.FromSeconds(1.0))
            {
                m_Caster = caster;
                m_End = DateTime.Now + TimeSpan.FromSeconds(30.0); // Duration of tree form

                m_Caster.Hidden = true; // Caster becomes hidden to simulate tree form

                // Spawn a tree graphic at the caster's location
                m_TreeGraphic = new Item(0x0C9E);
                m_TreeGraphic.Name = "Mighty Tree";
                m_TreeGraphic.Hue = 0x8A5; // Optional: hue modification for the tree
                m_TreeGraphic.MoveToWorld(m_Caster.Location, m_Caster.Map);

                m_OriginalHits = m_Caster.Hits;

                // Increase armor and health
                ApplyTemporaryEffects(m_Caster);

                m_Caster.SendMessage("You have transformed into a tree, gaining enhanced armor and health regeneration.");

                Start();
            }

            private void ApplyTemporaryEffects(Mobile caster)
            {
                // Temporarily boost physical resistance


                // Temporarily boost health
                caster.Hits += 20; // Adding an integer value directly
            }

            protected override void OnTick()
            {
                if (DateTime.Now >= m_End || m_Caster == null || m_Caster.Deleted || !m_Caster.Alive)
                {
                    StopEffect();
                    return;
                }

                m_Caster.Hits += 5; // Regenerate health over time

                // Periodic visual effects while in tree form
                Effects.SendLocationParticles(EffectItem.Create(m_Caster.Location, m_Caster.Map, EffectItem.DefaultDuration), 0x373A, 10, 15, 5018);
            }

            public void StopEffect()
            {
                if (m_Caster != null && !m_Caster.Deleted && m_Caster.Alive)
                {
                    m_Caster.Hidden = false; // Make caster visible again

                    // Revert armor and health to original values

                    m_Caster.Hits = m_OriginalHits;
                    m_Caster.SendMessage("You have returned to your normal form.");
                }

                if (m_TreeGraphic != null && !m_TreeGraphic.Deleted)
                {
                    m_TreeGraphic.Delete(); // Remove the tree graphic
                }

                Stop();
            }
        }
    }
}
