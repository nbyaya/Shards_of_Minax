using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a yin steed corpse")]
    public class YinSteed : BaseMount
    {
        private DateTime m_NextShadowStrike;
        private DateTime m_NextDarknessCloak;
        private DateTime m_NextSoulDrain;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public YinSteed()
            : base("a yin steed", 0xE2, 0x3EA0, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Body = 0xE2; // Horse body
            Hue = 2081; // Dark hue for Yin (Darkness)
            BaseSoundID = 0xA8;

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

        public YinSteed(Serial serial)
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
                    m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextDarknessCloak = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 25));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextShadowStrike)
                {
                    ShadowStrike();
                }

                if (DateTime.UtcNow >= m_NextDarknessCloak)
                {
                    DarknessCloak();
                }

                if (DateTime.UtcNow >= m_NextSoulDrain)
                {
                    SoulDrain();
                }
            }
        }

        private void ShadowStrike()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = (int)(DamageMax * 2); // Double damage
                target.Damage(damage, this);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Yin Steed strikes from the shadows!*");
                m_NextShadowStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ShadowStrike
            }
        }

        private void DarknessCloak()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*Darkness engulfs the area!*");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m is PlayerMobile)
                {
                    m.SendMessage("Your accuracy is decreased by the encroaching darkness!");
                    m.SendMessage("You have a chance to miss your next attack!");
                    // Logic for decreasing accuracy
                }
            }
            m_NextDarknessCloak = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Cooldown for DarknessCloak
        }

        private void SoulDrain()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(10, 20);
                target.Damage(damage, this);
                Hits = Math.Min(Hits + damage, HitsMax);
                target.PlaySound(0x1F1);
                target.FixedEffect(0x376A, 10, 16);
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "*The Yin Steed drains your soul!*");
                m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for SoulDrain
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
}
