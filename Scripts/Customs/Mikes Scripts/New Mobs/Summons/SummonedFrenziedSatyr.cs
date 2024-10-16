using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a frenzied satyr's corpse")]
    public class SummonedFrenziedSatyr : Satyr
    {
        private DateTime m_NextRagingRhythms;
        private DateTime m_NextDiscordantWail;
        private DateTime m_NextBerserkBeat;
        private DateTime m_BerserkEnd;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public SummonedFrenziedSatyr()
            : base()
        {
            Name = "a frenzied satyr";
            Body = 271;
            Hue = 2328; // Unique hue for Frenzied Satyr
			this.BaseSoundID = 0x586;

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
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_AbilitiesInitialized = false; // Initialize flag
        }

        public SummonedFrenziedSatyr(Serial serial)
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
                    m_NextRagingRhythms = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextDiscordantWail = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 45));
                    m_NextBerserkBeat = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true;
                }

                if (DateTime.UtcNow >= m_NextRagingRhythms)
                {
                    RagingRhythms();
                }

                if (DateTime.UtcNow >= m_NextDiscordantWail)
                {
                    DiscordantWail();
                }

                if (DateTime.UtcNow >= m_NextBerserkBeat && DateTime.UtcNow >= m_BerserkEnd)
                {
                    BerserkBeat();
                }

                if (DateTime.UtcNow >= m_BerserkEnd && m_BerserkEnd != DateTime.MinValue)
                {
                    DeactivateBerserkBeat();
                }
            }
        }

        private void RagingRhythms()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Plays a raging rhythm! *");
            PlaySound(0x1F5);
            FixedEffect(0x376A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature creature && creature != this)
                {
                    // Increase creature's damage temporarily
                    creature.SetDamage(creature.DamageMin + 5, creature.DamageMax + 5);
                }
            }

            m_NextRagingRhythms = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for RagingRhythms
        }

        private void DiscordantWail()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Wails discordantly! *");
            PlaySound(0x1F6);
            FixedEffect(0x375A, 10, 16);

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The Frenzied Satyr's discordant wail confuses you!");
                    m.Freeze(TimeSpan.FromSeconds(2));
                }
            }

            m_NextDiscordantWail = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for DiscordantWail
        }

        private void BerserkBeat()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Goes into a berserk beat! *");
            PlaySound(0x1F7);
            FixedEffect(0x37C4, 10, 36);

            // Increase Strength, Dexterity, and Intelligence temporarily
            SetStr(Str + 40);
            SetDex(Dex + 30);
            SetInt(Int + 20);

            m_BerserkEnd = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            m_NextBerserkBeat = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for BerserkBeat
        }

        private void DeactivateBerserkBeat()
        {
            // Revert Strength, Dexterity, and Intelligence to original values
            SetStr(Str - 40);
            SetDex(Dex - 30);
            SetInt(Int - 20);

            m_BerserkEnd = DateTime.MinValue;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextRagingRhythms);
            writer.Write(m_NextDiscordantWail);
            writer.Write(m_NextBerserkBeat);
            writer.Write(m_BerserkEnd);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextRagingRhythms = reader.ReadDateTime();
            m_NextDiscordantWail = reader.ReadDateTime();
            m_NextBerserkBeat = reader.ReadDateTime();
            m_BerserkEnd = reader.ReadDateTime();

            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }
}
