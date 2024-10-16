using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a babirusa beast corpse")]
    public class BabirusaBeast : BaseCreature
    {
        private DateTime m_NextFrenziedBite;
        private DateTime m_NextBoneArmor;
        private bool m_BoneArmorActive;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public BabirusaBeast()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a babirusa beast";
            Body = 0xCB; // Pig body
            Hue = 2227; // Gray hue
			BaseSoundID = 0xC4;

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

        public BabirusaBeast(Serial serial)
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
                    // Initialize random intervals for abilities
                    Random rand = new Random();
                    m_NextFrenziedBite = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextBoneArmor = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextFrenziedBite)
                {
                    FrenziedBite();
                }

                if (DateTime.UtcNow >= m_NextBoneArmor)
                {
                    BoneArmor();
                }
            }

            if (m_BoneArmorActive && DateTime.UtcNow >= m_NextBoneArmor)
            {
                DeactivateBoneArmor();
            }
        }

        private void FrenziedBite()
        {
            if (Combatant != null)
            {
                Mobile target = Combatant as Mobile;
                int damage = Utility.RandomMinMax(10, 15);
                target.Damage(damage, this);
                target.SendMessage("You are bitten and start bleeding!");
                target.SendMessage("You are suffering from a frenzied bite!");
                target.SendMessage("You take damage over time!");

                target.PlaySound(0x23D);
                target.ApplyPoison(this, Poison.Lethal);
                target.FixedEffect(0x376A, 10, 16);

                m_NextFrenziedBite = DateTime.UtcNow + TimeSpan.FromSeconds(15); // Reset cooldown
            }
        }

        private void BoneArmor()
        {
            if (!m_BoneArmorActive)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Bone Armor activated! *");
                PlaySound(0x1B2);
                FixedEffect(0x376A, 10, 16);

                VirtualArmor += 30;
                m_BoneArmorActive = true;
                m_NextBoneArmor = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset cooldown
            }
        }

        private void DeactivateBoneArmor()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Bone Armor deactivated! *");
            PlaySound(0x1B3);
            FixedEffect(0x376A, 10, 16);

            VirtualArmor -= 30;
            m_BoneArmorActive = false;
            m_NextBoneArmor = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Set next activation interval
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

            m_AbilitiesInitialized = false; // Reset initialization flag on deserialize
        }
    }
}
