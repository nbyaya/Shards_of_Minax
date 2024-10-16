using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class EchoesOfRebirth : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Echoes of Rebirth", "Ars Revo",
                                                        //SpellCircle.Fifth,
                                                        21004,
                                                        9300,
                                                        false,
                                                        Reagent.BlackPearl,
                                                        Reagent.Bloodmoss,
                                                        Reagent.MandrakeRoot
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 3.0; } }
        public override double RequiredSkill { get { return 80.0; } }
        public override int RequiredMana { get { return 55; } }

        public EchoesOfRebirth(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private EchoesOfRebirth m_Owner;

            public InternalTarget(EchoesOfRebirth owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile && ((Mobile)o).Deleted == false && ((Mobile)o).IsDeadBondedPet)
                {
                    Mobile target = (Mobile)o;
                    if (m_Owner.CheckSequence())
                    {
                        SpellHelper.Turn(m_Owner.Caster, target);
                        
                        double healing = m_Owner.Caster.Skills[SkillName.Musicianship].Value * 0.5;
                        double karmaBonus = m_Owner.Caster.Karma > 0 ? (m_Owner.Caster.Karma / 1000.0) : 0.0;
                        double totalHealing = healing + karmaBonus;

                        // Visual and sound effects for revival
                        target.PlaySound(0x214);
                        target.FixedParticles(0x375A, 10, 15, 5032, EffectLayer.Waist);

                        // Revive the target with a portion of health restored
                        target.Resurrect();

                        int hitsRestored = (int)(totalHealing);
                        target.Hits = Math.Min(target.HitsMax, hitsRestored);

                        m_Owner.Caster.SendMessage(0x5, "You have revived " + target.Name + " with the Echoes of Rebirth!");

                        // Additional Flashy Effect around the revived ally
                        target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                        target.FixedEffect(0x373A, 10, 15);
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500947); // Target cannot be resurrected.
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
