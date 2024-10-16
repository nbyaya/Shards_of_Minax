using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a gorgon viper corpse")]
    public class GorgonViper : BaseCreature
    {
        private DateTime m_NextToxicCloud;
        private DateTime m_NextFatalStrike;
        private DateTime m_NextVipersVigil;
        private DateTime m_NextSummonAllies;

        private bool m_AbilitiesInitialized;

        [Constructable]
        public GorgonViper()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a gorgon viper";
            Body = 0x15; // Giant Serpent body
            Hue = 1776; // Unique hue
            BaseSoundID = 219;

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

            m_AbilitiesInitialized = false; // Initialize the flag
        }

        public GorgonViper(Serial serial) : base(serial) { }

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
                    // Set random intervals for abilities
                    Random rand = new Random();
                    m_NextToxicCloud = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextFatalStrike = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 40));
                    m_NextVipersVigil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextSummonAllies = DateTime.UtcNow + TimeSpan.FromMinutes(rand.Next(1, 3));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextToxicCloud)
                {
                    ToxicCloud();
                }

                if (DateTime.UtcNow >= m_NextFatalStrike)
                {
                    FatalStrike();
                }

                if (DateTime.UtcNow >= m_NextVipersVigil)
                {
                    VipersVigil();
                }

                if (DateTime.UtcNow >= m_NextSummonAllies && Hits < (HitsMax * 0.5))
                {
                    SummonAllies();
                }
            }
        }

        private void ToxicCloud()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "A cloud of toxic gas surrounds you!");
            FixedEffect(0x36D4, 10, 16);
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive && m != Combatant)
                {
                    m.SendMessage("The Gorgon Viper's toxic cloud blinds and poisons you!");
                    m.ApplyPoison(this, Poison.Greater);
                    m.Freeze(TimeSpan.FromSeconds(4));
                }
            }
            m_NextToxicCloud = DateTime.UtcNow + TimeSpan.FromSeconds(30);
        }

        private void FatalStrike()
        {
            if (Combatant != null && Combatant.Hits < 75)
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Gorgon Viper delivers a fatal strike!");
                AOS.Damage(Combatant, this, Combatant.Hits, 0, 100, 0, 0, 0);
                ((Mobile)Combatant).Kill(); // Instantly kill the target
            }
            m_NextFatalStrike = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }

        private void VipersVigil()
        {
            if (Hits < (HitsMax * 0.3))
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Gorgon Viper enters a frenzied state!");
                // Set the speed and damage increase here
                SetDamage((int)(DamageMin * 1.5), (int)(DamageMax * 1.5));
                // Apply a red hue or a visual effect to indicate the frenzied state
                Hue = 0x7A0; // Change hue or use another effect
            }
            m_NextVipersVigil = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        private void SummonAllies()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "The Gorgon Viper summons venomous serpents!");
            for (int i = 0; i < 2; i++)
            {
                GiantSerpent ally = new GiantSerpent(); // Ensure this class exists or replace with an appropriate class
                ally.MoveToWorld(Location, Map);
            }
            m_NextSummonAllies = DateTime.UtcNow + TimeSpan.FromMinutes(1);
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
            m_AbilitiesInitialized = false; // Reset initialization flag
        }
    }
}
