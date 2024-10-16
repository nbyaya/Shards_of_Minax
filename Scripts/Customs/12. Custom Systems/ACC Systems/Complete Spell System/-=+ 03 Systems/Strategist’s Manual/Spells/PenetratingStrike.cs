using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Items;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class PenetratingStrike : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Penetrating Strike", "Concentrate and Strike",
                                                        //SpellCircle.Fifth,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.5; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 25; } }

        public PenetratingStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private PenetratingStrike m_Owner;

            public InternalTarget(PenetratingStrike owner) : base(1, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile target)
                {
                    if (from.CanBeHarmful(target) && m_Owner.CheckSequence())
                    {
                        from.DoHarmful(target);

                        double chanceToPenetrate = 0.2 + (from.Skills[SkillName.Tactics].Value / 500); // 20% base chance, increases with Tactics skill
                        bool isPenetratingHit = Utility.RandomDouble() < chanceToPenetrate;

                        if (isPenetratingHit)
                        {
                            // Play penetrating hit effect
                            Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x3779, 10, 30, 5052);
                            target.FixedParticles(0x37B9, 1, 15, 9912, 1153, 0, EffectLayer.Waist);

                            // Play sound effect
                            target.PlaySound(0x22F);

                            // Calculate damage with armor penetration
                            int damage = (int)(from.Skills[SkillName.Tactics].Value / 2); // Damage based on Tactics skill
                            AOS.Damage(target, from, damage, 100, 0, 0, 0, 0);

                            from.SendMessage("Your strike penetrates the target's defenses!");
                            target.SendMessage("You feel a sharp pain as the strike bypasses your defenses!");
                        }
                        else
                        {
                            // Regular hit without penetration
                            Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, EffectItem.DefaultDuration), 0x36BD, 20, 10, 5044);
                            target.FixedParticles(0x37B9, 1, 15, 9910, 1108, 0, EffectLayer.Waist);

                            // Play sound effect
                            target.PlaySound(0x1F5);

                            from.SendMessage("Your strike hits the target, but does not penetrate their defenses.");
                            target.SendMessage("The attack hits you, but your armor holds.");
                        }
                    }
                }

                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(0.5);
        }
    }
}
