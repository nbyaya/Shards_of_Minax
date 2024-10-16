using System;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.ParryMagic
{
    public class ReinforcedShield : ParrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Reinforced Shield", "Come Shield!",
            21005,
            9301,
            false,
            Reagent.BlackPearl,
            Reagent.SpidersSilk
        );

        public override SpellCircle Circle => SpellCircle.Sixth;

        public override double CastDelay => 1.5;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 25;

        public ReinforcedShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Visual effects
                Caster.FixedParticles(0x375A, 10, 15, 5010, EffectLayer.Waist);
                Caster.PlaySound(0x1F2);

                // Summoning shield
                BaseShield shield = GetRandomShield();
                if (shield != null)
                {
                    Caster.AddToBackpack(shield);
                    Caster.SendMessage("A reinforced shield appears in your backpack!");
                }
                else
                {
                    Caster.SendMessage("The spell fizzles.");
                }
            }

            FinishSequence();
        }

        private BaseShield GetRandomShield()
        {
            Type[] shieldTypes = new Type[]
            {
                typeof(BronzeShield),
                typeof(Buckler),
                typeof(MetalShield),
                typeof(HeaterShield),
                typeof(WoodenKiteShield),
                typeof(WoodenShield)
            };

            try
            {
                int randomIndex = Utility.Random(shieldTypes.Length);
                BaseShield shield = (BaseShield)Activator.CreateInstance(shieldTypes[randomIndex]);

                // Apply a random durability bonus to the shield
                shield.HitPoints = Utility.RandomMinMax(50, 100);
                shield.MaxHitPoints = shield.HitPoints;
                return shield;
            }
            catch
            {
                return null;
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
