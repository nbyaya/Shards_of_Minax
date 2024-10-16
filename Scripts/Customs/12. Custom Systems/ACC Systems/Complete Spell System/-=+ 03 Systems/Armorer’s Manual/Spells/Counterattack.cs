using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Misc;

namespace Server.ACC.CSS.Systems.ArmsLoreMagic
{
    public class Counterattack : ArmsLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Counterattack", "Vas Kal An Flam",
            21004, // Icon ID
            9300 // Sound ID
        );
		
        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public Counterattack(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (Caster is PlayerMobile)
            {
                PlayerMobile player = (PlayerMobile)Caster;
                player.SendMessage("You prepare to counterattack your foes.");

                // Subscribe to event handlers for blocking and parrying


                // Add a temporary buff or effect here
                // Create a visual effect around the player
                Effects.SendLocationEffect(player.Location, player.Map, 0x376A, 30, 10, 1153, 0); // Flashy effect
                Effects.PlaySound(player.Location, player.Map, 0x2E3); // Sound effect when preparing

                // Set a timer to remove event handlers after some time or when mana runs out

            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
