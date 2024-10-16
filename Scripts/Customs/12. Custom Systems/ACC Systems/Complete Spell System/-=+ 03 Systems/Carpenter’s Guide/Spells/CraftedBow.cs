using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class CraftedBow : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Crafted Bow", "Arma Facta",
            21004, // Icon ID
            9300   // Cast Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }  // No specific skill required
        public override int RequiredMana { get { return 30; } }

        public CraftedBow(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play visual and sound effects
                Caster.PlaySound(0x213); // Play a casting sound
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5022);

                // Create the bow and arrows
                Bow bow = new Bow();
                Arrow arrows = new Arrow(20); // Create 20 arrows

                // Add to caster's backpack
                if (Caster.Backpack != null)
                {
                    Caster.Backpack.DropItem(bow);
                    Caster.Backpack.DropItem(arrows);
                    Caster.SendMessage("A crafted bow and 20 arrows have been placed in your backpack.");
                }
                else
                {
                    Caster.SendMessage("You need a backpack to receive the crafted bow and arrows.");
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.0);
        }
    }
}
