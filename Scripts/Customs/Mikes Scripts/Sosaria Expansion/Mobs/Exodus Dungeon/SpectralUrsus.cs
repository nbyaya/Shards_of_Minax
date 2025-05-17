using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a spectral ursus corpse")]
    public class SpectralUrsus : BaseCreature
    {
        private DateTime m_NextFrostRoar;
        private DateTime m_NextSpiritPulse;
        private DateTime m_NextPhantomStep;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public SpectralUrsus()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Spectral Ursus";
            Body = 213; // Polar bear body
            Hue = 1150; // Pale spectral blue hue
            BaseSoundID = 0xA3;

            SetStr(850, 1000);
            SetDex(120, 160);
            SetInt(200, 300);

            SetHits(900, 1300);
            SetMana(300, 500);

            SetDamage(22, 30);
            SetDamageType(ResistanceType.Cold, 50);
            SetDamageType(ResistanceType.Physical, 50);

            SetResistance(ResistanceType.Physical, 50, 70);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 80, 100);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 100.0, 125.0);
            SetSkill(SkillName.Tactics, 90.0, 110.0);
            SetSkill(SkillName.Wrestling, 85.0, 100.0);
            SetSkill(SkillName.SpiritSpeak, 90.0, 110.0);

            Fame = 23000;
            Karma = -15000;
            VirtualArmor = 60;

            Tamable = false;
        }

        public SpectralUrsus(Serial serial) : base(serial) { }

        public override bool AutoDispel => true;
        public override bool BleedImmune => true;
        public override int Meat => 0;
        public override int Hides => 0;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    var rand = new Random();
                    m_NextFrostRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(8, 16));
                    m_NextSpiritPulse = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(15, 30));
                    m_NextPhantomStep = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(10, 25));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextFrostRoar)
                    FrostRoar();

                if (DateTime.UtcNow >= m_NextSpiritPulse)
                    SpiritPulse();

                if (DateTime.UtcNow >= m_NextPhantomStep)
                    PhantomStep();
            }
        }

        private void FrostRoar()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "* Spectral Ursus lets out a chilling roar! *");
            PlaySound(0x229); // Bear roar

            if (Combatant is Mobile target)
            {
                target.Freeze(TimeSpan.FromSeconds(2));
                target.SendMessage(0x22, "You are frozen in place by a spectral roar!");
            }

            m_NextFrostRoar = DateTime.UtcNow + TimeSpan.FromSeconds(15 + Utility.RandomMinMax(5, 10));
        }

        private void SpiritPulse()
        {
            PublicOverheadMessage(MessageType.Emote, 0x480, true, "* Spectral Ursus releases a pulse of ghostly energy! *");
            PlaySound(0x15E); // Ethereal whoosh

            foreach (Mobile m in GetMobilesInRange(4))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(20, 40);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100); // Pure energy damage

                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage(0x22, "A spectral wave drains your spirit!");
                        mobile.Stam -= Utility.RandomMinMax(5, 15);
                        mobile.Mana -= Utility.RandomMinMax(10, 20);
                    }
                }
            }

            m_NextSpiritPulse = DateTime.UtcNow + TimeSpan.FromSeconds(30 + Utility.RandomMinMax(10, 20));
        }

        private void PhantomStep()
        {
            if (Combatant is Mobile target)
            {
                Point3D to = new Point3D(
                    target.X + Utility.RandomMinMax(-1, 1),
                    target.Y + Utility.RandomMinMax(-1, 1),
                    target.Z
                );

                PublicOverheadMessage(MessageType.Emote, 0x480, true, "* Spectral Ursus vanishes and reappears nearby! *");
                Effects.SendLocationEffect(Location, Map, 0x3728, 13, 1); // Vanish effect
                MoveToWorld(to, Map);
                Effects.SendLocationEffect(Location, Map, 0x3728, 13, 1); // Reappear effect
                PlaySound(0x20E); // Blink sound

                if (target.Alive)
                {
                    target.SendMessage(0x22, "You feel something cold behind you...");
                }
            }

            m_NextPhantomStep = DateTime.UtcNow + TimeSpan.FromSeconds(20 + Utility.RandomMinMax(5, 10));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Gems, 4);
            if (Utility.RandomDouble() < 0.005)
                PackItem(new SpectralClaw()); // Rare drop
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
            m_AbilitiesInitialized = false;
        }
    }

    public class SpectralClaw : Item
    {
        [Constructable]
        public SpectralClaw() : base(0x26B6)
        {
            Hue = 1150;
            Name = "Claw of the Spectral Ursus";
            Weight = 1.0;
            LootType = LootType.Blessed;
        }

        public SpectralClaw(Serial serial) : base(serial) { }

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
