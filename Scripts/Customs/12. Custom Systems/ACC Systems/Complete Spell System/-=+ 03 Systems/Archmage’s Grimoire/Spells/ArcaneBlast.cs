using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MageryMagic
{
    public class ArcaneBlast : MagerySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Arcane Blast", "In Vas Flam",
            21004, // Spell icon ID
            9300 // Casting animation
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Set to Fourth Circle for balance
        }

        public override double CastDelay { get { return 0.2; } } // Casting delay in seconds
        public override double RequiredSkill { get { return 65.0; } } // Required Magery skill
        public override int RequiredMana { get { return 20; } } // Mana cost

        public ArcaneBlast(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ArcaneBlast m_Spell;

            public InternalTarget(ArcaneBlast spell) : base(12, false, TargetFlags.Harmful)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target && from.CanBeHarmful(target))
                {
                    from.DoHarmful(target);
                    m_Spell.CastEffect(target);
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen or is invalid
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Spell.FinishSequence();
            }
        }

        public void CastEffect(Mobile target)
        {
            if (CheckSequence())
            {
                // Visual and Sound Effects
                Caster.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head); // Visual effect on caster
                target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Waist); // Visual effect on target
                target.PlaySound(0x307); // Explosion sound

                // Calculate Damage
                int damage = (int)(Caster.Skills[SkillName.Magery].Value / 5.0) + Utility.RandomMinMax(15, 25); // Damage based on caster skill and randomness

                // Apply Damage
                SpellHelper.Damage(this, target, damage, 0, 100, 0, 0, 0); // Pure Energy damage

                // Secondary Effect - Knockback
                if (Utility.RandomDouble() < 0.2) // 20% chance
                {
                    Point3D knockbackLoc = GetKnockbackLocation(target);
                    target.MoveToWorld(knockbackLoc, target.Map);
                    target.SendMessage("You are knocked back by the force of the Arcane Blast!");
                }
            }

            FinishSequence();
        }

        private Point3D GetKnockbackLocation(Mobile target)
        {
            // Define knockback offset based on direction
            int offsetX = 0;
            int offsetY = 0;
            
            switch (target.Direction)
            {
                case Direction.North: offsetY = -1; break;
                case Direction.South: offsetY = 1; break;
                case Direction.East: offsetX = 1; break;
                case Direction.West: offsetX = -1; break;
            }

            // Apply the offset to the target's current location
            Point3D knockbackLoc = new Point3D(target.X + offsetX, target.Y + offsetY, target.Z);
            return knockbackLoc;
        }
    }
}
