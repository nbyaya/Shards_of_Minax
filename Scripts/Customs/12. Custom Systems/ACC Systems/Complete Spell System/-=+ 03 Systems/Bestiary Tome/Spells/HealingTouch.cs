using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.AnimalLoreMagic
{
    public class HealingTouch : AnimalLoreSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Healing Touch", "An Ex Pura",
                                                        21005,
                                                        9400,
                                                        false,
                                                        Reagent.Ginseng,
                                                        Reagent.Garlic,
                                                        Reagent.SpidersSilk
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 18; } }

        public HealingTouch(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist); // Green healing particles
                Caster.PlaySound(0x1F2); // Healing sound

                HealTarget(Caster);

                foreach (Mobile m in GetMobilesInRange(5))
                {
                    if (m is BaseCreature)
                    {
                        BaseCreature creature = (BaseCreature)m;
                        if (creature.ControlMaster == Caster || creature == Caster)
                        {
                            HealTarget(creature);
                        }
                    }
                }
            }

            FinishSequence();
        }

        private void HealTarget(Mobile target)
        {
            int healAmount = (int)(target.HitsMax * 0.2); // Heal 20% of maximum hits
            target.Hits += healAmount;

            if (target.Poison != null)
            {
                target.CurePoison(Caster); // Cure any poison
                target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Curing particles
                target.PlaySound(0x1E0); // Curing sound
            }

            target.FixedEffect(0x376A, 10, 16); // Healing visual effect
            target.SendMessage("You feel a warm energy flow through you, healing your wounds and cleansing any poison.");
        }

        private List<Mobile> GetMobilesInRange(int range)
        {
            List<Mobile> list = new List<Mobile>();

            foreach (Mobile m in Caster.GetMobilesInRange(range))
            {
                if (m != null && m.Alive && Caster.CanBeBeneficial(m))
                {
                    list.Add(m);
                }
            }

            return list;
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(3.0);
        }
    }
}
