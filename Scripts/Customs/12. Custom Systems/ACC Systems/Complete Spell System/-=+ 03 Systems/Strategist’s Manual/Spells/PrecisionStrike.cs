using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.TacticsMagic
{
    public class PrecisionStrike : TacticsSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Precision Strike", "Precicio!",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        // Removed the override keyword for Name
        public string CustomName => "Precision Strike";
        public override int RequiredMana => 20;
        public override double CastDelay => 0.2; // 2 seconds delay
        public override double RequiredSkill => 60.0; // Required skill level

        public PrecisionStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info) { }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        private class InternalTarget : Target
        {
            private PrecisionStrike m_Owner;

            public InternalTarget(PrecisionStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && from != target)
                {
                    if (from.CanBeHarmful(target))
                    {
                        from.DoHarmful(target);
                        m_Owner.Effect(target);
                    }
                }
                m_Owner.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

		public void Effect(Mobile target)
		{
			double skillValue = Caster.Skills[SkillName.Tactics].Value;
			double damageBonus = skillValue / 200.0; // Max 50% damage bonus at 100 skill level
			double bypassChance = skillValue / 100.0; // Max 50% bypass chance at 100 skill level

			bool bypassArmor = Utility.RandomDouble() < bypassChance;

			// Visual and sound effects
			Caster.FixedParticles(0x3779, 1, 15, 5005, EffectLayer.Waist); // Flashy particle effect
			Caster.PlaySound(0x20E); // Sound effect for precision

			int baseDamage = 10; // Default damage if no weapon is equipped
			BaseWeapon weapon = Caster.Weapon as BaseWeapon;

			if (weapon != null)
			{
				baseDamage = weapon.MaxDamage; // Changed from DamageMax to MaxDamage
			}

			int damage = (int)(baseDamage * (1 + damageBonus));

			if (bypassArmor)
			{
				target.SendMessage("Your armor is bypassed by a precise strike!");
				AOS.Damage(target, Caster, damage, 100, 0, 0, 0, 0);
			}
			else
			{
				AOS.Damage(target, Caster, damage, 0, 100, 0, 0, 0);
			}
		}


    }
}
