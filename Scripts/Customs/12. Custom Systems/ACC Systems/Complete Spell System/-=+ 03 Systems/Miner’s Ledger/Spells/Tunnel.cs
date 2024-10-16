using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.MiningMagic
{
    public class Tunnel : MiningSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Tunnel", "Telpo Port",
            21006,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override int RequiredMana => 35; // Mana cost for the spell
        public override double CastDelay => 1.5; // Cast delay in seconds
        public override double RequiredSkill => 50.0; // Skill level required to cast the spell

        public Tunnel(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);

                Point3D toLocation = new Point3D(p);

                // Visual and sound effects for teleportation
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 30, 5052);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1FE); // Sound effect before teleport

                // Teleport the caster to the selected location
                Caster.MoveToWorld(toLocation, Caster.Map);

                // Additional visual effects at the destination
                Effects.SendLocationParticles(EffectItem.Create(toLocation, Caster.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5052);
                Effects.PlaySound(toLocation, Caster.Map, 0x1FE); // Sound effect after teleport

                Caster.SendMessage("You have tunneled to the selected location!");
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private Tunnel m_Owner;

            public InternalTarget(Tunnel owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
