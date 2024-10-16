using System;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a mandrill shaman corpse")]
    public class MandrillShaman : BaseCreature
    {
        private static readonly int MysticAuraHue = 1153; // Unique hue for Mystic Aura effect
        private DateTime m_NextMysticAura;
        private DateTime m_NextSpiritSummon;
        private DateTime m_NextCursedHowl;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public MandrillShaman()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Mandrill Shaman";
            Body = 0x1D; // Gorilla body
            Hue = 1961; // Unique hue for the Mandrill Shaman
			this.BaseSoundID = 0x9E;

            SetStr(1000, 1200);
            SetDex(177, 255);
            SetInt(151, 250);
			
            SetHits(700, 1200);
			
            SetDamage(29, 35);
			
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 65, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 65, 80);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 25.1, 50.0);
            SetSkill(SkillName.EvalInt, 90.1, 100.0);
            SetSkill(SkillName.Magery, 95.5, 100.0);
            SetSkill(SkillName.Meditation, 25.1, 50.0);
            SetSkill(SkillName.MagicResist, 100.5, 150.0);
            SetSkill(SkillName.Tactics, 90.1, 100.0);
            SetSkill(SkillName.Wrestling, 90.1, 100.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;
			
            Tamable = true;
            ControlSlots = 3;
            MinTameSkill = 93.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public MandrillShaman(Serial serial)
            : base(serial)
        {
        }

        public override bool ReacquireOnMovement => !Controlled;
        public override bool AutoDispel => !Controlled;
        public override int TreasureMapLevel => 5;
		public override bool CanAngerOnTame => true;
		public override void GenerateLoot()
		{
			this.AddLoot(LootPack.FilthyRich, 2);
			this.AddLoot(LootPack.Rich);
			this.AddLoot(LootPack.Gems, 8);
		}		

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (!m_AbilitiesInitialized)
                {
                    Random rand = new Random();
                    m_NextMysticAura = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSpiritSummon = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextCursedHowl = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextMysticAura)
                {
                    MysticAura();
                    m_NextMysticAura = DateTime.UtcNow + TimeSpan.FromSeconds(30);
                }

                if (DateTime.UtcNow >= m_NextSpiritSummon)
                {
                    SpiritSummon();
                    m_NextSpiritSummon = DateTime.UtcNow + TimeSpan.FromSeconds(45);
                }

                if (DateTime.UtcNow >= m_NextCursedHowl)
                {
                    CursedHowl();
                    m_NextCursedHowl = DateTime.UtcNow + TimeSpan.FromMinutes(1);
                }
            }
        }

        private void MysticAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mandrill Shaman's mystic aura shields its allies from magical harm and invigorates them! *");
            FixedEffect(0x376A, 10, 16); // Aura visual effect

            // Apply a damage reduction effect to nearby allies (if applicable)
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature && m != this)
                {
                    m.SendMessage("The mystic aura of the Mandrill Shaman shields you from magical harm!");
                }
            }
        }

        private void SpiritSummon()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mandrill Shaman calls upon powerful spirit animals to assist in battle! *");

            int count = Utility.RandomMinMax(1, 3); // Summon between 1 and 3 spirits
            for (int i = 0; i < count; i++)
            {
                BaseCreature spirit;
                switch (Utility.Random(3))
                {
                    case 0:
                        spirit = new SpiritBear(); // Replace with actual spirit types
                        break;
                    case 1:
                        spirit = new SpiritWolf();
                        break;
                    default:
                        spirit = new SpiritEagle();
                        break;
                }

                spirit.Team = this.Team;
                spirit.MoveToWorld(this.Location, this.Map);
                spirit.PlaySound(0x4F1); // Custom summon sound
            }
        }

        private void CursedHowl()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Mandrill Shaman unleashes a terrifying cursed howl! *");
            PlaySound(0x204); // Custom howl sound

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player && m.Alive)
                {
                    m.SendMessage("You are struck with fear from the Mandrill Shaman's howl!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }
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

            m_AbilitiesInitialized = false; // Reset flag on deserialize
        }
    }

    // Spirit animal classes
    public class SpiritBear : BaseCreature
    {
        [Constructable]
        public SpiritBear()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a spirit bear";
            Body = 0xD4; // Bear body
            Hue = 1155; // Unique hue for spirit bear

            SetStr(75);
            SetDex(50);
            SetInt(30);

            SetHits(75);
            SetMana(0);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 25, 35);

            Fame = 600;
            Karma = 0;

            Tamable = false;
        }

        public SpiritBear(Serial serial)
            : base(serial)
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

    public class SpiritWolf : BaseCreature
    {
        [Constructable]
        public SpiritWolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a spirit wolf";
            Body = 0xE0; // Wolf body
            Hue = 1156; // Unique hue for spirit wolf

            SetStr(60);
            SetDex(70);
            SetInt(30);

            SetHits(60);
            SetMana(0);

            SetDamage(8, 12);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 30);

            Fame = 500;
            Karma = 0;

            Tamable = false;
        }

        public SpiritWolf(Serial serial)
            : base(serial)
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

    public class SpiritEagle : BaseCreature
    {
        [Constructable]
        public SpiritEagle()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a spirit eagle";
            Body = 0xF2; // Eagle body
            Hue = 1157; // Unique hue for spirit eagle

            SetStr(50);
            SetDex(80);
            SetInt(30);

            SetHits(50);
            SetMana(0);

            SetDamage(6, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 25);

            Fame = 400;
            Karma = 0;

            Tamable = false;
        }

        public SpiritEagle(Serial serial)
            : base(serial)
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
