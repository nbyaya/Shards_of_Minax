using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Items;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.FletchingMagic
{
    public class BowyersBlessing : FletchingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Bowyer's Blessing", "Arcus Fortis",
            21005, 9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 22; } }

        public BowyersBlessing(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private BowyersBlessing m_Owner;

            public InternalTarget(BowyersBlessing owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is BaseWeapon weapon && weapon is BaseRanged)
                {
                    m_Owner.Target(weapon);
                }
                else
                {
                    from.SendMessage("You must target a ranged weapon.");
                    m_Owner.FinishSequence();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(BaseWeapon weapon)
        {
            if (!Caster.CanSee(weapon))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                Caster.SendMessage("You enhance the durability and quality of your weapon!");

                // Visual and sound effects
                Effects.SendLocationParticles(
                    EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration),
                    0x375A, 1, 15, 1153, 4, 9502, 0
                );
                Caster.PlaySound(0x1F2);

                // Enhancing weapon stats temporarily
                weapon.WeaponAttributes.HitPhysicalArea += 10; // Adds 10% physical hit chance
                weapon.Attributes.WeaponDamage += 25; // Increases weapon damage by 25%

                Timer.DelayCall(TimeSpan.FromSeconds(30.0), () => RevertEffect(weapon));
            }

            FinishSequence();
        }

        private void RevertEffect(BaseWeapon weapon)
        {
            if (weapon == null || weapon.Deleted)
                return;

            weapon.WeaponAttributes.HitPhysicalArea -= 10;
            weapon.Attributes.WeaponDamage -= 25;

            Caster.SendMessage("The effect of Bowyer's Blessing fades away.");
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5.0);
        }
    }
}
