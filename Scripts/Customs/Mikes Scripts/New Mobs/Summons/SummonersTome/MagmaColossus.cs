using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a magma colossus corpse")]
    public class MagmaColossus : BaseCreature
    {
        private Mobile m_Summoner;
        private double m_SpiritSpeakSkill;

        [Constructable]
        public MagmaColossus()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a magma colossus";
            Body = 720; // Same body as lava elemental for consistency
            Hue = 1359; // Slightly different hue for distinction



            m_SpiritSpeakSkill = 0;

            int spiritBonus = (int)(m_SpiritSpeakSkill / 2); // Scaling factor

            SetStr(500 + spiritBonus, 550 + spiritBonus);
            SetDex(180, 200);
            SetInt(400, 460);

            SetHits(400 + spiritBonus * 3, 500 + spiritBonus * 3);
            SetDamage(18 + spiritBonus / 10, 25 + spiritBonus / 8);

            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 90);

            SetResistance(ResistanceType.Physical, 60 + spiritBonus / 5, 75 + spiritBonus / 5);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Magery, 100.0, 110.0);
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 16000;
            Karma = -16000;

            VirtualArmor = 60;

            // Special ability timer
            Timer.DelayCall(TimeSpan.FromSeconds(5.0), TimeSpan.FromSeconds(8.0), PerformMoltenSlam);
        }

        public MagmaColossus(Serial serial)
            : base(serial)
        {
        }

        private void PerformMoltenSlam()
        {
            if (Deleted || !Alive)
                return;

            Effects.PlaySound(Location, Map, 0x208); // Ground-shaking slam sound
            Effects.SendLocationEffect(Location, Map, 0x3709, 30, 10, 0x489, 0); // Magma effect

            IPooledEnumerable eable = GetMobilesInRange(3);
            foreach (Mobile m in eable)
            {
                if (m != this && m_Summoner != null && m != m_Summoner && CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 100, 0, 0, 0); // Pure fire damage
                    m.SendMessage("You are scorched by molten flames!");
                }
            }
            eable.Free();

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), DropMagmaPool);
        }

        private void DropMagmaPool()
        {
            // Leaves behind a temporary damaging lava pool
            Item lavaPool = new InternalLavaPool();
            lavaPool.MoveToWorld(Location, Map);
        }

        public override void OnThink()
        {
            base.OnThink();

            // You can also add persistent aura damage here if desired
        }

        public override bool DeleteCorpseOnDeath => true;

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }

        private class InternalLavaPool : Item
        {
            public InternalLavaPool() : base(0x398C)
            {
                Name = "molten lava";
                Hue = 1359;
                Movable = false;
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), Delete);

                // Damage nearby players
                Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(2.0), () =>
                {
                    if (Deleted || Map == Map.Internal)
                        return;

                    foreach (Mobile m in GetMobilesInRange(1))
                    {
                        if (m.Alive && m.AccessLevel == AccessLevel.Player)
                        {
                            m.Damage(10, null);
                            m.SendMessage("The lava burns your feet!");
                        }
                    }
                });
            }

            public InternalLavaPool(Serial serial) : base(serial) { }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write(0);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}
