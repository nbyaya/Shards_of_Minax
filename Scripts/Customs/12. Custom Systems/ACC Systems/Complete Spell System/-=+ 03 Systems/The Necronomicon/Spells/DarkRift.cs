using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;
using System.Collections;
using System.Collections.Generic;
using Server;

namespace Server.ACC.CSS.Systems.NecromancyMagic
{
    public class DarkRift : NecromancySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Dark Rift", "Klaatu Varada",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Sixth; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 70.0; } }
        public override int RequiredMana { get { return 20; } }

        public DarkRift(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        private static Type[] m_ShadowCreatures = new Type[]
        {
            typeof(ShadowFiend),
            typeof(ShadowWolf),
            typeof(ShadowWraith)
        };

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
                SpellHelper.Turn(Caster, p);
                SpellHelper.GetSurfaceTop(ref p);

                Point3D loc = new Point3D(p);

                Effects.PlaySound(loc, Caster.Map, 0x20F); // Play a dark sound
                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, TimeSpan.FromSeconds(1.0)), 0x3728, 10, 10, 2023); // Shadowy visuals

                List<BaseCreature> summonedCreatures = new List<BaseCreature>();

                for (int i = 0; i < 3; i++) // Summon up to 3 creatures
                {
                    Type creatureType = m_ShadowCreatures[Utility.Random(m_ShadowCreatures.Length)];
                    BaseCreature creature = (BaseCreature)Activator.CreateInstance(creatureType);

                    if (creature != null)
                    {
                        creature.MoveToWorld(loc, Caster.Map);
                        creature.Controlled = true;
                        creature.ControlMaster = Caster;
                        creature.Summoned = true;
                        creature.SummonMaster = Caster;
                        summonedCreatures.Add(creature);

                        // Set up a timer to delete the creature after 30 seconds
                        Timer.DelayCall(TimeSpan.FromSeconds(30.0), () =>
                        {
                            if (creature != null && !creature.Deleted)
                                creature.Delete(); // Delete after 30 seconds
                        });
                    }
                }

                FinishSequence();
            }
        }

        private class InternalTarget : Target
        {
            private DarkRift m_Owner;

            public InternalTarget(DarkRift owner) : base(12, true, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D p)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }

    public class ShadowFiend : BaseCreature
    {
        public ShadowFiend() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow fiend";
            Body = 0x9E;
            Hue = 0x4001;
            SetStr(50);
            SetDex(50);
            SetInt(10);

            SetHits(30);
            SetDamage(5, 10);

            SetSkill(SkillName.MagicResist, 20.0);
            SetSkill(SkillName.Tactics, 40.0);
            SetSkill(SkillName.Wrestling, 40.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 16;
        }

        public ShadowFiend(Serial serial) : base(serial)
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

    public class ShadowWolf : BaseCreature
    {
        public ShadowWolf() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow wolf";
            Body = 0xE1;
            Hue = 0x4001;
            SetStr(40);
            SetDex(60);
            SetInt(10);

            SetHits(25);
            SetDamage(4, 8);

            SetSkill(SkillName.MagicResist, 20.0);
            SetSkill(SkillName.Tactics, 50.0);
            SetSkill(SkillName.Wrestling, 40.0);

            Fame = 1200;
            Karma = -1200;

            VirtualArmor = 14;
        }

        public ShadowWolf(Serial serial) : base(serial)
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

    public class ShadowWraith : BaseCreature
    {
        public ShadowWraith() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow wraith";
            Body = 0xA4;
            Hue = 0x4001;
            SetStr(30);
            SetDex(40);
            SetInt(80);

            SetHits(20);
            SetDamage(3, 7);

            SetSkill(SkillName.MagicResist, 30.0);
            SetSkill(SkillName.Tactics, 30.0);
            SetSkill(SkillName.Wrestling, 30.0);
            SetSkill(SkillName.Magery, 50.0);

            Fame = 1800;
            Karma = -1800;

            VirtualArmor = 12;
        }

        public ShadowWraith(Serial serial) : base(serial)
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
