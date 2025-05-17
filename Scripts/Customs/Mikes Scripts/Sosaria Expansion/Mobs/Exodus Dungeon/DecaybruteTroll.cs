using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a decaybrute corpse")]
    public class DecaybruteTroll : BaseCreature
    {
        private DateTime m_NextDecayPulse;
        private DateTime m_NextSporeBurst;
        private DateTime m_NextMutantHowl;
        private bool m_Initialized;

        [Constructable]
        public DecaybruteTroll()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Decaybrute Troll";
            Body = Utility.RandomList(53, 54);
            Hue = 2967; // Sickly greenish/rotting hue
            BaseSoundID = 461;

            SetStr(950, 1150);
            SetDex(65, 95);
            SetInt(45, 80);

            SetHits(800, 1100);
            SetStam(120, 180);
            SetMana(100, 150);

            SetDamage(18, 30);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 25, 35);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 85.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 100.0);
            SetSkill(SkillName.Poisoning, 80.0, 95.0);

            Fame = 22000;
            Karma = -22000;

            VirtualArmor = 70;

            m_Initialized = false;
        }

        public override bool AutoDispel => true;
        public override bool CanRummageCorpses => true;
        public override int TreasureMapLevel => 4;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Gems, 4);

            if (Utility.RandomDouble() < 0.015)
            {
                PackItem(new FungalHeart()); // Rare drop item idea
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_Initialized)
                {
                    m_NextDecayPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(10, 20));
                    m_NextSporeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(20, 40));
                    m_NextMutantHowl = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(30, 50));
                    m_Initialized = true;
                }

                if (DateTime.UtcNow >= m_NextDecayPulse)
                    DecayPulse();

                if (DateTime.UtcNow >= m_NextSporeBurst)
                    SporeBurst();

                if (DateTime.UtcNow >= m_NextMutantHowl)
                    MutantHowl();
            }
        }

        private void DecayPulse()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* A wave of rot pulses from the Decaybrute! *");
            PlaySound(0x22F);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 0, 0, 0, 100); // Pure poison
                    if (m is Mobile mobile)
                    {
                        mobile.ApplyPoison(this, Poison.Regular);
                        mobile.SendMessage("You feel your skin crawling as decay seeps into your body!");
                    }
                }
            }

            m_NextDecayPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(15, 25));
        }

        private void SporeBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Decaybrute erupts in a cloud of toxic spores! *");
            PlaySound(0x230);

            Effects.SendLocationEffect(Location, Map, 0x36BD, 20, 10);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet && Utility.RandomDouble() < 0.6)
                {
                    if (m is Mobile mobile)
                    {
                        mobile.ApplyPoison(this, Poison.Greater);
                        mobile.SendMessage("You are engulfed in toxic spores!");
                    }
                }
            }

            m_NextSporeBurst = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(30, 50));
        }

        private void MutantHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Decaybrute howls, disrupting magical concentration! *");
            PlaySound(0x482);

            foreach (Mobile m in GetMobilesInRange(8))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    if (m is Mobile mobile)
                    {
                        mobile.SendMessage("Your focus wavers as the air twists!");
                        mobile.Paralyze(TimeSpan.FromSeconds(2));
                        mobile.Mana -= Utility.RandomMinMax(10, 20);
                    }
                }
            }

            m_NextMutantHowl = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.Random(40, 60));
        }

        public DecaybruteTroll(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Initialized = false;
        }
    }

    public class FungalHeart : Item
    {
        [Constructable]
        public FungalHeart() : base(0xF91)
        {
            Hue = 1271;
            Name = "a pulsing fungal heart";
            Weight = 1.0;
        }

        public FungalHeart(Serial serial) : base(serial) { }

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
