using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a glacial ogre corpse")]
    public class GlacialOgre : BaseCreature
    {
        private DateTime m_NextFrostStomp;
        private DateTime m_NextIceSpikes;
        private DateTime m_NextFrozenRegen;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public GlacialOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Glacial Ogre";
            Body = 1; // Same body as base ogre
            BaseSoundID = 427;
            Hue = 1150; // Icy blue custom hue


            SetStr(300, 350);
            SetDex(80, 100);
            SetInt(80, 100);

            SetHits(700, 950);
            SetMana(100);

            SetDamage(16, 22);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 5, 10);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 75.0, 95.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 100.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 60;

            PackItem(new Club());

            m_AbilitiesInitialized = false;
        }

        public GlacialOgre(Serial serial)
            : base(serial)
        {
        }

        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 3;
        public override int Meat => 2;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Gems, 2);

            if (Utility.RandomDouble() < 0.005) // 1 in 200 chance
            {
                PackItem(new FrostyCrown());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!m_AbilitiesInitialized)
            {
                m_NextFrostStomp = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(5, 15));
                m_NextIceSpikes = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 25));
                m_NextFrozenRegen = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(30, 60));
                m_AbilitiesInitialized = true;
            }

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextFrostStomp)
                    FrostStomp();

                if (DateTime.UtcNow >= m_NextIceSpikes)
                    IceSpikes();

                if (DateTime.UtcNow >= m_NextFrozenRegen)
                    FrozenRegeneration();
            }
        }

        private void FrostStomp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Glacial Ogre slams the ground with freezing fury!*");
            PlaySound(0x65A); // Earth-shaking thud

            Effects.SendLocationEffect(Location, Map, 0x10CF, 20); // Shockwave effect

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    AOS.Damage(m, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                    m.Freeze(TimeSpan.FromSeconds(2));

                    if (m is Mobile target)
                    {
                        target.SendMessage("You are frozen in place by the icy stomp!");
                    }
                }
            }

            m_NextFrostStomp = DateTime.UtcNow + TimeSpan.FromSeconds(20 + Utility.RandomDouble() * 10);
        }

        private void IceSpikes()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*Icy spikes erupt from the ground!*");
            PlaySound(0x64C); // Crackling frost sound

            for (int i = 0; i < 5; i++)
            {
                Timer.DelayCall(TimeSpan.FromMilliseconds(i * 300), () =>
                {
                    Point3D spikeLoc = new Point3D(X + Utility.RandomMinMax(-2, 2), Y + Utility.RandomMinMax(-2, 2), Z);
                    Effects.SendLocationEffect(spikeLoc, Map, 0x3709, 30);
                    Effects.PlaySound(spikeLoc, Map, 0x64F);

                    foreach (Mobile m in GetMobilesInRange(3))
                    {
                        if (m != this && m.Alive && !m.IsDeadBondedPet && m.InRange(spikeLoc, 1))
                        {
                            AOS.Damage(m, this, Utility.RandomMinMax(10, 18), 0, 100, 0, 0, 0);
                            m.SendMessage("An icy spike pierces you!");
                        }
                    }
                });
            }

            m_NextIceSpikes = DateTime.UtcNow + TimeSpan.FromSeconds(25 + Utility.RandomDouble() * 15);
        }

        private void FrozenRegeneration()
        {
            PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Glacial Ogre absorbs cold energy to heal!*");
            PlaySound(0x659); // Magical healing-ish sound

            int healAmount = Utility.RandomMinMax(60, 100);
            Hits = Math.Min(HitsMax, Hits + healAmount);

            Effects.SendTargetParticles(this, 0x376A, 10, 15, 2023, EffectLayer.Waist);
            m_NextFrozenRegen = DateTime.UtcNow + TimeSpan.FromSeconds(60 + Utility.RandomDouble() * 30);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (!willKill && Utility.RandomDouble() < 0.1)
            {
                PublicOverheadMessage(MessageType.Regular, 0x480, true, "*The Glacial Ogre retaliates with a burst of frost!*");
                PlaySound(0x64D);

                if (from != null && from.Alive && from.Map == Map && from.InRange(this.Location, 2))
                {
                    AOS.Damage(from, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);
                    if (from is Mobile target)
                        target.SendMessage("You are chilled to the bone by the icy retaliation!");
                }
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Utility.RandomDouble() < 0.15)
            {
                Mobile frostRevenant = new FrostRevenant();
                frostRevenant.MoveToWorld(Location, Map);
                Say("*From the shattered ogre corpse, a frost revenant rises!*");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_AbilitiesInitialized = false;
        }
    }

    public class FrostyCrown : BaseHat
    {
        [Constructable]
        public FrostyCrown() : base(0x171C)
        {
            Name = "Crown of the Glacial Ogre";
            Hue = 1150;
            LootType = LootType.Blessed;
        }

        public FrostyCrown(Serial serial) : base(serial)
        {
        }

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

    public class FrostRevenant : BaseCreature
    {
        [Constructable]
        public FrostRevenant() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a frost revenant";
            Body = 400;
            BaseSoundID = 0x482;
            Hue = 0x480;

            SetStr(90, 120);
            SetDex(60, 80);
            SetInt(40, 60);
            SetHits(150, 200);

            SetDamage(12, 18);
            SetDamageType(ResistanceType.Cold, 100);

            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Fire, 0, 5);
            SetResistance(ResistanceType.Physical, 30, 40);

            Fame = 6000;
            Karma = -6000;
        }

        public FrostRevenant(Serial serial) : base(serial)
        {
        }

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
