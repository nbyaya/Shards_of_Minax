using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an eldritch toad corpse")]
    public class EldritchToad : GiantToad
    {
        private DateTime m_NextRealityWarp;
        private DateTime m_NextEldritchBlast;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public EldritchToad()
            : base()
        {
            Name = "an eldritch toad";
            Hue = 2447; // Unique hue for the Eldritch Toad
            m_AbilitiesInitialized = false; // Set flag to false
            BaseSoundID = 0x26B;
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
        }

        public EldritchToad(Serial serial)
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
                    m_NextRealityWarp = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEldritchBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextRealityWarp)
                {
                    RealityWarp();
                }

                if (DateTime.UtcNow >= m_NextEldritchBlast)
                {
                    EldritchBlast();
                }
            }
        }

        private void RealityWarp()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The eldritch toad warps reality around it! *");
            PlaySound(0x20E); // Sound effect for warp

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("The eldritch toad's power confuses and disorients you!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            m_NextRealityWarp = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for RealityWarp
        }

        private void EldritchBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The eldritch toad unleashes a blast of arcane energy! *");
            PlaySound(0x20E); // Sound effect for blast

            Mobile target = Combatant as Mobile;

            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(20, 40);
                target.Damage(damage, this);
                
                if (Utility.RandomDouble() < 0.25) // 25% chance to inflict madness or hallucinations
                {
                    target.SendMessage("The eldritch blast causes you to hallucinate!");
                    // Apply a temporary debuff or visual effect here
                    // Example: Increase confusion effect or apply a status effect
                }
            }

            m_NextEldritchBlast = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for EldritchBlast
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
            m_AbilitiesInitialized = false; // Reset the initialization flag
        }
    }
}
