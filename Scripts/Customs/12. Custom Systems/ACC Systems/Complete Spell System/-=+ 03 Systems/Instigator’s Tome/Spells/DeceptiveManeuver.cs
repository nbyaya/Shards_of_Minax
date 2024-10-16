using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;
using Server.Items;
using Server.Misc;
using System.Collections.Generic;

namespace Server.ACC.CSS.Systems.ProvocationMagic
{
    public class DeceptiveManeuver : ProvocationSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Deceptive Maneuver", "Illusio Umbra",
            21005,
            9400,
            false,
            Reagent.BlackPearl,
            Reagent.Bloodmoss
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public DeceptiveManeuver(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
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
            private DeceptiveManeuver m_Owner;

            public InternalTarget(DeceptiveManeuver owner) : base(10, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D)
                {
                    IPoint3D p = (IPoint3D)targeted;
                    SpellHelper.GetSurfaceTop(ref p);

                    Point3D loc = new Point3D(p);

                    if (from.InRange(loc, 10) && from.Map.CanSpawnMobile(loc.X, loc.Y, loc.Z))
                    {
                        List<Mobile> phantoms = new List<Mobile>();

                        for (int i = 0; i < 3; i++)
                        {
                            Mobile phantom = new PhantomIllusion();
                            phantom.MoveToWorld(new Point3D(loc.X + Utility.RandomMinMax(-2, 2), loc.Y + Utility.RandomMinMax(-2, 2), loc.Z), from.Map);
                            phantoms.Add(phantom);
                        }

                        from.FixedParticles(0x376A, 10, 15, 5037, EffectLayer.Waist);
                        from.PlaySound(0x1E9);

                        Timer.DelayCall(TimeSpan.FromSeconds(10), () => CleanUpPhantoms(phantoms));
                    }
                    else
                    {
                        from.SendMessage("You cannot place an illusion there.");
                    }
                }
            }

            private void CleanUpPhantoms(List<Mobile> phantoms)
            {
                foreach (Mobile phantom in phantoms)
                {
                    if (phantom != null && !phantom.Deleted)
                        phantom.Delete();
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class PhantomIllusion : BaseCreature
        {
            public PhantomIllusion() : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
            {
                Body = Utility.RandomList(0x190, 0x191); // Random male/female illusion
                Name = "a phantom illusion";
                Hue = 1150; // Ghostly appearance
                Blessed = true;
                CantWalk = true;

                SetStr(20);
                SetDex(20);
                SetInt(20);

                SetHits(50);
                SetDamage(5, 10);

                SetSkill(SkillName.MagicResist, 40.0);
                SetSkill(SkillName.Tactics, 50.0);
                SetSkill(SkillName.Wrestling, 50.0);

                ControlSlots = 0;
            }

            public override bool OnBeforeDeath()
            {
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3728, 10, 15, 5028);
                PlaySound(0x1FB);
                return base.OnBeforeDeath();
            }

            public override void OnThink()
            {
                base.OnThink();
                if (Combatant == null || !Combatant.Alive || Deleted)
                    Delete(); // Disappear when there's no valid target or if the illusion is "killed".
            }

            public PhantomIllusion(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}
