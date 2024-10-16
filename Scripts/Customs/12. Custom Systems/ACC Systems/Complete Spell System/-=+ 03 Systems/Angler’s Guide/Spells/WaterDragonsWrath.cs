using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using Server;
using Server.Regions;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class WaterDragonsWrath : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Water Dragon's Wrath", "Torridis Aquari!",
            21004, // Spell Effect ID
            9300   // Sound ID
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 15; } }

        public WaterDragonsWrath(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                try
                {
                    // Target the location to summon the water dragon
                    Caster.Target = new InternalTarget(this);
                }
                catch
                {
                }
            }
        }

        private class InternalTarget : Target
        {
            private WaterDragonsWrath m_Owner;

            public InternalTarget(WaterDragonsWrath owner) : base(12, true, TargetFlags.None)
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

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen
                return;
            }

            if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                // Consume the scroll if necessary
                if (Scroll != null)
                    Scroll.Consume();

                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                // Play sound and create visual effects
                Effects.PlaySound(new Point3D(p), Caster.Map, 0x20E); // Splash sound effect
                Effects.SendLocationEffect(new Point3D(p), Caster.Map, 0x3709, 30, 10, 0, 0); // Water splash effect

                // Summon the water dragon
                WaterDragon dragon = new WaterDragon();
                dragon.MoveToWorld(new Point3D(p), Caster.Map);
                dragon.Combatant = Caster; // Target the caster's enemies

                // Apply a basic water effect
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => 
                {
                    foreach (Mobile mobile in Caster.GetMobilesInRange(10))
                    {
                        if (mobile is BaseCreature && mobile != dragon)
                        {
                            if (mobile.Alive)
                            {
                                mobile.SendMessage("A powerful water dragon attacks you!");
                                mobile.FixedEffect(0x37C4, 1, 12); // Water effect
                                mobile.Damage(20, dragon); // Adjust damage as needed
                            }
                        }
                    }
                });
            }
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }

    public class WaterDragon : BaseCreature
    {
        public WaterDragon() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 0xE8; // Water Dragon Body ID
            Name = "Water Dragon";
            SetStr(300, 400);
            SetDex(100, 150);
            SetInt(150, 200);

            SetHits(200, 250);
            SetDamage(20, 30);

            SetSkill(SkillName.Wrestling, 80.0);
            SetSkill(SkillName.Tactics, 70.0);
            SetSkill(SkillName.Anatomy, 60.0);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            Effects.PlaySound(Location, Map, 0x20F); // Water splash sound effect
        }

        public WaterDragon(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt(); // version
        }
    }
}
