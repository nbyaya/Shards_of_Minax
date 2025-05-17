using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a glacial ooze corpse")]
    public class GlacialOoze : BaseCreature
    {
        private DateTime m_NextFrostNova;
        private DateTime m_NextShardStorm;
        private DateTime m_NextPhaseShift;

        [Constructable]
        public GlacialOoze()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a glacial ooze";
            Body = 94; // Same as Frost Ooze
            Hue = 1152; // Unique icy blue hue
            BaseSoundID = 456;

            SetStr(400, 500);
            SetDex(70, 100);
            SetInt(100, 150);

            SetHits(600, 800);
            SetDamage(15, 22);

            SetDamageType(ResistanceType.Cold, 80);
            SetDamageType(ResistanceType.Physical, 20);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Cold, 80, 95);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 90.0, 120.0);
            SetSkill(SkillName.Tactics, 85.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 50;
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextFrostNova)
                    FrostNova();

                if (DateTime.UtcNow >= m_NextShardStorm)
                    ShardStorm();

                if (DateTime.UtcNow >= m_NextPhaseShift)
                    PhaseShift();
            }
        }

        private void FrostNova()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*The glacial ooze erupts in a blast of icy energy!*");
            PlaySound(0x64D);
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 1152, 2, 9910, 0);

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 0, 100, 0, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(2));
                    if (m is Mobile mob)
                        mob.SendMessage("A wave of freezing air engulfs you!");
                }
            }

            m_NextFrostNova = DateTime.UtcNow + TimeSpan.FromSeconds(45);
        }

        private void ShardStorm()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*Shards of jagged ice erupt in all directions!*");
            PlaySound(0x10B);

            for (int i = 0; i < 3; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 500), () =>
                {
                    foreach (Mobile m in GetMobilesInRange(6))
                    {
                        if (m != this && m.Alive && !m.IsDeadBondedPet)
                        {
                            int damage = Utility.RandomMinMax(10, 18);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                            m.PlaySound(0x208);
                            if (m is Mobile mobile)
                                mobile.SendMessage("Razor ice shards cut into your flesh!");
                        }
                    }
                });
            }

            m_NextShardStorm = DateTime.UtcNow + TimeSpan.FromSeconds(60);
        }

        private void PhaseShift()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            PublicOverheadMessage(MessageType.Emote, 0x480, true, "*The ooze shimmers and momentarily splits through time!*");
            PlaySound(0x659);

            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x37CC, 10, 15, 1152, 3, 9912, 0);

            if (Combatant is Mobile target)
            {
                Point3D newLocation = new Point3D(target.X + Utility.RandomMinMax(-2, 2), target.Y + Utility.RandomMinMax(-2, 2), target.Z);
                MoveToWorld(newLocation, Map);
                target.SendMessage("The Glacial Ooze suddenly reappears behind you!");
            }

            m_NextPhaseShift = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(2, 4));

            if (Utility.RandomDouble() < 0.005) // 1 in 200 chance
                PackItem(new IcecoreCrystal());
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
            c.DropItem(new GlacialSlime()); // Always drops one unique reagent
        }

        public GlacialOoze(Serial serial) : base(serial) { }

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
    }

    public class GlacialSlime : Item
    {
        [Constructable]
        public GlacialSlime() : base(0x122A)
        {
            Name = "glacial slime";
            Hue = 1152;
            Weight = 1.0;
        }

        public GlacialSlime(Serial serial) : base(serial) { }

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
    }

    public class IcecoreCrystal : Item
    {
        [Constructable]
        public IcecoreCrystal() : base(0x1F19)
        {
            Name = "icecore crystal";
            Hue = 1152;
            Weight = 2.0;
        }

        public IcecoreCrystal(Serial serial) : base(serial) { }

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
    }
}
