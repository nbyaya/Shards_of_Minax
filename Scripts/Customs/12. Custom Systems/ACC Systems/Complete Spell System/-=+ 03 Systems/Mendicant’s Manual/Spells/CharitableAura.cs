using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using System.Collections.Generic;
using Server.Items;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class CharitableAura : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Charitable Aura", "Augmentum Aurum",
                                                        //SpellCircle.Fourth,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public CharitableAura(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Play casting sound and effect
                Caster.PlaySound(0x1FA); // Magic sound
                Caster.FixedParticles(0x375A, 10, 30, 5037, EffectLayer.Waist);

                // Find all NPCs within 5 tiles
                List<Mobile> npcs = new List<Mobile>();
                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m is BaseCreature && !m.Player && m.Alive)
                    {
                        npcs.Add(m);
                    }
                }

                // Add gold to each NPC and play a visual effect
                foreach (Mobile m in npcs)
                {
                    int goldAmount = Utility.RandomMinMax(50, 500);
                    m.SendMessage("A mysterious force adds {0} gold to your possession.", goldAmount);
                    Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                    m.PlaySound(0x2E6); // Coin sound

                    // Casting the backpack to the Container type
                    Container container = m.Backpack;

                    // Check if the container is a Backpack and handle accordingly
                    if (container == null)
                    {
                        container = new Backpack();
                        m.AddItem(container); // Add the backpack to the mobile
                    }

                    // If the container is a Backpack, add the gold to it
                    if (container is Backpack backpack)
                    {
                        backpack.DropItem(new Gold(goldAmount)); // Add the gold to the backpack
                    }
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
