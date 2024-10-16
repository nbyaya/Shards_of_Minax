using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.BeggingMagic
{
    public class CompellingRequest : BeggingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Compelling Request", "Give Generously!",
                                                        //SpellCircle.First,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }
        public override int RequiredMana { get { return 12; } }

        public CompellingRequest(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private CompellingRequest m_Owner;

            public InternalTarget(CompellingRequest owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile && ((Mobile)o).Player == false && ((Mobile)o).Alive)
                {
                    Mobile target = (Mobile)o;

                    if (m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(m_Owner.Caster, target);

                        if (Utility.RandomDouble() < 0.75) // 75% chance of successful beg
                        {
                            // Visual effects and sound
                            Effects.SendTargetParticles(target, 0x376A, 9, 32, 5008, EffectLayer.Waist);
                            target.PlaySound(0x1FB);

                            // Boost the amount of gold or items received
                            int baseAmount = Utility.RandomMinMax(5, 20);
                            int bonusAmount = (int)(baseAmount * 0.5 + m_Owner.Caster.Skills[SkillName.Begging].Value * 0.1);

                            Gold gold = new Gold(baseAmount + bonusAmount);
                            target.Backpack.DropItem(gold);

                            m_Owner.Caster.SendMessage($"You successfully begged and received {gold.Amount} gold!");
                        }
                        else
                        {
                            m_Owner.Caster.SendMessage("Your begging attempt failed.");
                        }
                    }

                    m_Owner.FinishSequence();
                }
                else
                {
                    from.SendMessage("You must target a living NPC.");
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
