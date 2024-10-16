using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;

namespace Server.ACC.CSS.Systems.AlchemyMagic
{
    public class ExplosivePotion : AlchemySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Explosive Potion", "Bombatum Explodicus",
            //SpellCircle.Fourth,
            21005, // GumpID
            9301, // EffectID
            false, // Reagents
            Reagent.BlackPearl,
            Reagent.Nightshade,
            Reagent.SulfurousAsh
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public ExplosivePotion(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                if (this.Scroll != null)
                    Scroll.Consume();
                
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);
                
                Point3D loc = new Point3D(p);
                Map map = Caster.Map;

                Effects.SendLocationEffect(loc, map, 0x36BD, 20, 10, 0, 0); // Explosion visual effect
                Effects.PlaySound(loc, map, 0x307); // Explosion sound

                // Deal damage in a small radius
                ArrayList targets = new ArrayList();
                foreach (Mobile m in map.GetMobilesInRange(loc, 2)) // 2 tile radius
                {
                    if (Caster.CanBeHarmful(m, false))
                        targets.Add(m);
                }

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];
                    Caster.DoHarmful(m);
                    AOS.Damage(m, Caster, Utility.RandomMinMax(10, 30), 0, 100, 0, 0, 0); // Random damage between 10-30
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private ExplosivePotion m_Owner;

            public InternalTarget(ExplosivePotion owner) : base(12, true, TargetFlags.None)
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
