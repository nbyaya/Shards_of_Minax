using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.ChivalryMagic
{
    public class MartyrsSacrifice : ChivalrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Martyr's Sacrifice", "In Vas Muni",
            //SpellCircle.Seventh,
            21015,
            9300
        );

        public override SpellCircle Circle => SpellCircle.Seventh;

        public override double CastDelay => 1.5; // Reduced cast delay for a quicker, dramatic effect
        public override double RequiredSkill => 80.0;
        public override int RequiredMana => 30;

        public MartyrsSacrifice(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist); // Flashy particles around caster
                Caster.PlaySound(0x1F7); // Play sacrifice sound
                
                double healthSacrifice = Caster.Hits * 0.2; // Sacrifice 20% of the caster's health
                Caster.Hits -= (int)healthSacrifice;

                ArrayList targets = new ArrayList();

                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m != Caster && m.Player && m.Alive && Caster.CanBeBeneficial(m, false, true))
                    {
                        targets.Add(m);
                    }
                }

                if (targets.Count > 0)
                {
                    foreach (Mobile ally in targets)
                    {
                        Caster.DoBeneficial(ally);

                        if (Utility.RandomBool()) // 50% chance to heal or cleanse
                        {
                            ally.Hits += (int)healthSacrifice;
                            ally.FixedEffect(0x376A, 10, 16); // Healing effect
                            ally.SendMessage("You feel rejuvenated by the sacrifice of your ally!");
                        }
                        else
                        {
                            Effects.SendTargetParticles(ally, 0x373A, 10, 15, 5018, EffectLayer.Waist); // Cleansing effect
                            ally.SendMessage("All negative effects are cleansed from your body!");

                        }
                    }
                }

                FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0); // Slightly longer delay for dramatic effect
        }
    }
}
