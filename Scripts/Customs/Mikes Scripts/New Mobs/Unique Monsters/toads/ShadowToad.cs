using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a shadow toad corpse")]
    public class ShadowToad : BaseCreature
    {
        private DateTime m_NextShadowmeld;
        private DateTime m_NextShadowStrike;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ShadowToad()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a shadow toad";
            Body = 80; // Giant Toad body
            BaseSoundID = 0x26B;
            Hue = 2444; // Dark purple hue for shadowy effect

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

        public ShadowToad(Serial serial)
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
                    m_NextShadowmeld = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowmeld)
                {
                    ShadowMeld();
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                {
                    ShadowStrike();
                }
            }
        }

        private void ShadowMeld()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Toad melts into the shadows! *");
            Timer.DelayCall(TimeSpan.FromSeconds(5), new TimerCallback(Reappear));

            m_NextShadowmeld = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ShadowMeld
        }

        private void Reappear()
        {
            if (this != null && !this.Deleted)
            {
                this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Toad reappears from the shadows! *");
            }
        }

        private void ShadowStrike()
        {
            if (Combatant != null)
            {
                int extraDamage = 10; // Additional damage for surprise attack
                Combatant.Damage(extraDamage, this);
                this.PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Shadow Toad strikes from the shadows! *");
                PlaySound(0x1F5);
                FixedEffect(0x376A, 10, 16);

                m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Cooldown for ShadowStrike
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

            m_AbilitiesInitialized = false; // Reset the initialization flag on deserialize
        }
    }
}
