using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("Acererak's corpse")]
    public class Acererak : BaseCreature
    {
        private DateTime m_NextSoulDrain;
        private DateTime m_PhylacteryEndTime;
        private bool m_PhylacteryActive;
        private static readonly int PhylacteryItemID = 0x1F0D; // You may need to choose an appropriate item ID for the phylactery
        
        [Constructable]
        public Acererak()
            : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Acererak, the Devourer";
            Body = 78; // AncientLich body
            Hue = 2100; // Unique hue
            BaseSoundID = 412;

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

            PackNecroReg(200, 300);
        }

        public Acererak(Serial serial)
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
                // Soul Drain Ability
                if (DateTime.UtcNow >= m_NextSoulDrain)
                {
                    SoulDrain();
                }

                // Phylactery Protection
                if (m_PhylacteryActive && DateTime.UtcNow >= m_PhylacteryEndTime)
                {
                    DeactivatePhylactery();
                }

                // Aura and Chains Effect
                if (m_PhylacteryActive)
                {
                    CreatePhylacteryAura();
                }
            }
        }

        private void SoulDrain()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Acererak, the Devourer, drains the souls around him! *");

            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m != this && m.Alive && m is PlayerMobile player)
                {
                    int manaDrain = Utility.RandomMinMax(20, 50);
                    player.Mana -= manaDrain;
                    if (player.Mana < 0) player.Mana = 0;

                    int damage = manaDrain / 2; // Damage is half of the drained mana
                    AOS.Damage(player, this, damage, 0, 100, 0, 0, 0);

                    player.SendMessage("You feel your life energy being drained!");
                }
            }

            m_NextSoulDrain = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown for SoulDrain
        }

        private void ActivatePhylactery()
        {
            m_PhylacteryActive = true;
            m_PhylacteryEndTime = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Phylactery active time
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Acererak activates his phylactery, rendering him invulnerable! *");
        }

        private void DeactivatePhylactery()
        {
            m_PhylacteryActive = false;
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Acererak's phylactery fades, allowing him to take damage again. *");
        }

		private void CreatePhylacteryAura()
		{
			// Create a pulsing red and purple aura around Acererak
			Effects.SendLocationEffect(Location, Map, 0x376A, 10, 30); // Aura effect

			// Create spectral chains
			Effects.SendLocationEffect(Location, Map, 0x1F7C, 30, 10); // Spectral chains effect
		}


        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            if (m_PhylacteryActive && amount > 0)
            {
                amount = 0; // No damage while phylactery is active
                from.SendMessage("Your attack is deflected by the phylactery's protection!");
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_PhylacteryActive);
            writer.Write(m_PhylacteryEndTime);
            writer.Write(m_NextSoulDrain);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_PhylacteryActive = reader.ReadBool();
            m_PhylacteryEndTime = reader.ReadDateTime();
            m_NextSoulDrain = reader.ReadDateTime();
        }
    }
}
