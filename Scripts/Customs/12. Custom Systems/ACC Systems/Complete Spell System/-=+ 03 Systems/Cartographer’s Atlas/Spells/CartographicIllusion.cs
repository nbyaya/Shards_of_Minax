using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.CartographyMagic
{
    public class CartographicIllusion : CartographySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Cartographic Illusion", "Summon Elemental",
                                                        21004,
                                                        9300,
                                                        false
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 60; } }

        public CartographicIllusion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        private static Type[] m_Elementals = new Type[]
        {
            typeof(FireElemental),
            typeof(WaterElemental),
            typeof(EarthElemental),
			typeof(RandomElemental),
            typeof(AirElemental)
        };

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    Type elementalType = m_Elementals[Utility.Random(m_Elementals.Length)];
                    BaseCreature elemental = (BaseCreature)Activator.CreateInstance(elementalType);

                    SpellHelper.Summon(elemental, Caster, 0x216, TimeSpan.FromMinutes(10), false, false);

                    // Add some flashy effects and sounds for a more dramatic appearance
                    Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x3709, 30, 10, 0x47E, 0);
                    Effects.PlaySound(Caster.Location, Caster.Map, 0x20E);

                    Caster.SendMessage("You summon a powerful elemental to fight by your side!");
                }
                catch (Exception ex)
                {
                    Caster.SendMessage("An error occurred while summoning the elemental.");
                    Console.WriteLine("Error in CartographicIllusion: " + ex.Message);
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }

        public TimeSpan GetCooldown()
        {
            return TimeSpan.FromHours(1.0);
        }
    }
}
