using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a toxic lich's corpse")]
    public class ToxicLich : BaseCreature
    {
        private DateTime m_NextToxicBurst;
        private DateTime m_NextVenomousStrike;
        private DateTime m_NextAcidicSplash;

        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public ToxicLich()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a toxic lich";
            Body = 24;
            Hue = 2132; // Unique hue for toxic appearance
			BaseSoundID = 0x3E9;

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

        public ToxicLich(Serial serial)
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
                    m_NextToxicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextVenomousStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextAcidicSplash = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 50));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextToxicBurst)
                {
                    ToxicBurst();
                }

                if (DateTime.UtcNow >= m_NextVenomousStrike)
                {
                    VenomousStrike();
                }

                if (DateTime.UtcNow >= m_NextAcidicSplash)
                {
                    AcidicSplash();
                }
            }
        }

        private void ToxicBurst()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Releases a toxic burst! *");
            FixedEffect(0x36D4, 10, 16);

            foreach (Mobile m in GetMobilesInRange(3))
            {
                if (m != this && m.Alive && !m.IsDeadBondedPet)
                {
                    m.SendMessage("You are caught in a cloud of poisonous gas!");
                    m.ApplyPoison(this, Poison.Lethal);
                    m.Damage(20, this);
                }
            }

            m_NextToxicBurst = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for ToxicBurst
        }

        private void VenomousStrike()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Strikes with venomous fury! *");
            FixedEffect(0x376A, 10, 16);

            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                target.SendMessage("You have been struck with a powerful poison!");
                target.ApplyPoison(this, Poison.Greater);
                target.Damage(15, this);
            }

            m_NextVenomousStrike = DateTime.UtcNow + TimeSpan.FromSeconds(40); // Cooldown for VenomousStrike
        }

        private void AcidicSplash()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Throws a vial of acid! *");
            FixedEffect(0x373A, 10, 16);

            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                target.SendMessage("The acidic splash burns you and corrodes your armor!");
                target.Damage(25, this);
                target.VirtualArmor -= 10; // Reduce target's armor effectiveness
            }

            m_NextAcidicSplash = DateTime.UtcNow + TimeSpan.FromSeconds(50); // Cooldown for AcidicSplash
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
