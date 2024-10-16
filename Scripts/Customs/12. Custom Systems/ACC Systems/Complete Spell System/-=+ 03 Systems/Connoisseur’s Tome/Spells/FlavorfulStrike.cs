using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.TasteIDMagic
{
    public class FlavorfulStrike : TasteIDSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Flavorful Strike", "Elementa Flavour",
            //SpellCircle.Fifth,
            21009,
            9305
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 18; } }

        public FlavorfulStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private FlavorfulStrike m_Owner;

            public InternalTarget(FlavorfulStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && m_Owner.CheckHSequence(target))
                {
                    SpellHelper.Turn(m_Owner.Caster, target);

                    // Randomly select an elemental damage type
                    int randomElement = Utility.Random(5);
                    int damageAmount = Utility.RandomMinMax(30, 60); // Large amount of damage

                    switch (randomElement)
                    {
                        case 0: // Fire
                            target.Damage(damageAmount, m_Owner.Caster);
                            target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.Head);
                            target.PlaySound(0x208);
                            break;
                        case 1: // Cold
                            target.Damage(damageAmount, m_Owner.Caster);
                            target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                            target.PlaySound(0x1E3);
                            break;
                        case 2: // Poison
                            target.Damage(damageAmount, m_Owner.Caster);
                            target.FixedParticles(0x374A, 10, 15, 5021, EffectLayer.Waist);
                            target.PlaySound(0x205);
                            break;
                        case 3: // Energy
                            target.Damage(damageAmount, m_Owner.Caster);
                            target.FixedParticles(0x3818, 1, 15, 9502, 1153, 7, EffectLayer.Head);
                            target.PlaySound(0x29);
                            break;
                        case 4: // Earth
                            target.Damage(damageAmount, m_Owner.Caster);
                            target.FixedParticles(0x36BD, 1, 30, 9964, 0x9C2, 0, EffectLayer.Head);
                            target.PlaySound(0x307);
                            break;
                    }

                    from.SendMessage("You strike with a flavorful blow!");
                    target.SendMessage("You are hit by a burst of elemental energy!");

                    m_Owner.FinishSequence();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(1.5);
        }
    }
}
