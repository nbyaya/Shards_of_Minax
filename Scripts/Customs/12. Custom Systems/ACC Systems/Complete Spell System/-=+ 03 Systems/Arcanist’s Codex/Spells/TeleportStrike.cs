using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server.Gumps;
using System.Collections;
using Server.Engines.Craft;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class TeleportStrike : EvalIntSpell
    {
        private static readonly SpellInfo m_Info = new SpellInfo(
            "Teleport Strike", "Teleporta y Ataca!",
            // SpellCircle.Sixth,
            212,
            9040
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public TeleportStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private void PerformTeleportStrike(BaseCreature target)
        {
            if (target == null || !Caster.CanSee(target))
            {
                Caster.SendLocalizedMessage(501942); // Target is not in sight.
                FinishSequence();
                return;
            }

            // Teleport behind the target
            Point3D targetLocation = target.Location;
            Map targetMap = target.Map;

            int offsetX = (target.X - Caster.X) > 0 ? -3 : 3;
            int offsetY = (target.Y - Caster.Y) > 0 ? -3 : 3;
            
            Point3D newLocation = new Point3D(target.X + offsetX, target.Y + offsetY, Caster.Z);
            
            if (!targetMap.CanSpawnMobile(newLocation) || !Caster.Map.CanSpawnMobile(newLocation))
            {
                Caster.SendLocalizedMessage(501942); // Target location is invalid.
                FinishSequence();
                return;
            }

            Caster.MoveToWorld(newLocation, targetMap);

            // Play sound effect
            Effects.PlaySound(Caster.Location, Caster.Map, 0x1F4);

            // Visual Effect - Dust/Explosion
            Effects.SendLocationEffect(Caster.Location, Caster.Map, 0x376A, 20, 10);

            // Inflict damage
            double baseDamage = 10.0; // Base damage
            double skillFactor = Caster.Skills[SkillName.Swords].Value / 100.0; // Skill factor for damage
            int totalDamage = (int)(baseDamage + baseDamage * skillFactor);
            target.Damage(totalDamage, Caster);

            Caster.SendMessage($"You teleported behind {target.Name} and struck with {totalDamage} damage!");

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private TeleportStrike m_Owner;

            public InternalTarget(TeleportStrike owner) : base(12, true, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object target)
            {
                if (target is BaseCreature creature)
                {
                    m_Owner.PerformTeleportStrike(creature);
                }
                else
                {
                    from.SendLocalizedMessage(501942); // Target is invalid.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
