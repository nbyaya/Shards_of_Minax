using System;
using System.Collections.Generic;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class RallyingCry : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Rallying Cry", "Fortis Corda",
            //SpellCircle.Fourth,
            21005,
            9400
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public RallyingCry(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private RallyingCry m_Owner;

            public InternalTarget(RallyingCry owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D p)
                {
                    m_Owner.Target(p);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);
                Map map = Caster.Map;

                // Visual and sound effects
                Effects.SendLocationParticles(EffectItem.Create(loc, map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5020);
                Effects.PlaySound(loc, map, 0x64F);

                // Apply effects to nearby allies
                List<Mobile> allies = new List<Mobile>();
                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m != Caster && m is PlayerMobile && m.Alive && Caster.CanBeBeneficial(m))
                    {
                        allies.Add(m);
                    }
                }

                foreach (Mobile ally in allies)
                {
                    Caster.DoBeneficial(ally);

                    int healthRestore = Utility.RandomMinMax(5, 10);
                    int staminaRestore = Utility.RandomMinMax(5, 10);
                    int fearResistanceBoost = Utility.RandomMinMax(5, 10);

                    ally.Hits += healthRestore;
                    ally.Stam += staminaRestore;
                    ally.SendMessage("You feel invigorated by the rallying cry!");
                    ally.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                    ally.PlaySound(0x1F7);

                    // Applying a buff to enhance resistance to fear and panic effects
                    ResistanceMod fearResistMod = new ResistanceMod(ResistanceType.Poison, fearResistanceBoost);
                    ally.AddResistanceMod(fearResistMod);
                    Timer.DelayCall(TimeSpan.FromSeconds(30), () => ally.RemoveResistanceMod(fearResistMod));
                }
            }

            FinishSequence();
        }
    }
}
