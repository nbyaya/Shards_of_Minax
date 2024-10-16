using System;
using Server.Items;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a urukoth corpse")]
    public class UruKoth : ElderGazer
    {
        private DateTime m_NextHungryMaw;
        private DateTime m_NextDevourEssence;
        private DateTime m_NextVoraciousCharge;
        private double m_EssenceBoost;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public UruKoth()
            : base()
        {
            Name = "Uru'Koth the Insatiable";
            Body = 22; // ElderGazer body
            Hue = 1764; // Custom hue for Uru'Koth
			BaseSoundID = 377;

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

            m_EssenceBoost = 1.0; // Initial essence boost multiplier
            m_AbilitiesInitialized = false; // Initialize flag
        }

        public UruKoth(Serial serial)
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
                    m_NextHungryMaw = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDevourEssence = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextVoraciousCharge = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextHungryMaw)
                {
                    HungryMaw();
                }

                if (DateTime.UtcNow >= m_NextDevourEssence)
                {
                    DevourEssence();
                }

                if (DateTime.UtcNow >= m_NextVoraciousCharge)
                {
                    VoraciousCharge();
                }
            }
        }

        private void HungryMaw()
        {
            if (Combatant != null && Combatant.Alive)
            {
                int damage = Utility.RandomMinMax(20, 30);
                Combatant.Damage(damage, this);
                Hits = Math.Min(Hits + (int)(damage * 0.5), HitsMax); // Heal for 50% of damage dealt
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Uru'Koth devours its prey with a hungry maw! *");
                m_NextHungryMaw = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for HungryMaw
            }
        }

        private void DevourEssence()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            m_EssenceBoost += 0.1; // Increase boost by 10%
            SetStr((int)(Str * (1 + m_EssenceBoost)));
            SetDex((int)(Dex * (1 + m_EssenceBoost)));
            SetInt((int)(Int * (1 + m_EssenceBoost)));
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Uru'Koth absorbs the essence of its foe! *");
            m_NextDevourEssence = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for DevourEssence
        }

        private void VoraciousCharge()
        {
            if (Combatant == null || !Combatant.Alive)
                return;

            int damage = Utility.RandomMinMax(30, 40);
            Combatant.Damage(damage, this);
            Combatant.Location = new Point3D(Combatant.X + Utility.RandomMinMax(-2, 2), Combatant.Y + Utility.RandomMinMax(-2, 2), Combatant.Z);
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Uru'Koth charges at its enemy with unrestrained hunger! *");
            m_NextVoraciousCharge = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for VoraciousCharge
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
}
