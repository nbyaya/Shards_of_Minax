using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a cerebral ettin corpse")]
    public class CerebralEttin : BaseCreature
    {
        private DateTime m_NextBrainBlast;
        private DateTime m_NextTelepathicCommunication;
        private DateTime m_NextMentalBarrier;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public CerebralEttin()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a cerebral ettin";
            Body = 18; // Ettin body
            Hue = 1566; // Unique hue
            BaseSoundID = 367;

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

        public CerebralEttin(Serial serial)
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
                    m_NextBrainBlast = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 15));
                    m_NextTelepathicCommunication = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextMentalBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextBrainBlast)
                {
                    BrainBlast();
                }

                if (DateTime.UtcNow >= m_NextTelepathicCommunication)
                {
                    TelepathicCommunication();
                }

                if (DateTime.UtcNow >= m_NextMentalBarrier)
                {
                    MentalBarrier();
                }
            }
        }

        private void BrainBlast()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cerebral Ettin releases a psychic wave! *");
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are hit by a psychic wave and feel confused!");
                    m.Damage(10, this);
                    m.SendMessage("Your accuracy is reduced!");

                    if (m is PlayerMobile player)
                    {
                        player.Skills[SkillName.Tactics].Base -= 5; // Reduce Tactics skill by 5 points
                    }
                }
            }
            m_NextBrainBlast = DateTime.UtcNow + TimeSpan.FromSeconds(20); // Set cooldown
        }

        private void TelepathicCommunication()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cerebral Ettin communicates telepathically with nearby creatures! *");
            foreach (Mobile m in GetMobilesInRange(10))
            {
                if (m is BaseCreature creature && creature != this && creature.Combatant != null)
                {
                    Mobile combatant = (Mobile)creature.Combatant;
                    if (combatant != null)
                    {
                        combatant.SendMessage("The Cerebral Ettin's telepathic command boosts your resolve!");
                        creature.Damage(5, this); // Add extra damage as a temporary effect
                    }
                }
            }
            m_NextTelepathicCommunication = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Set cooldown
        }

        private void MentalBarrier()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Cerebral Ettin raises a mental barrier! *");
            SetResistance(ResistanceType.Energy, 70); // Increase energy resistance

            Timer.DelayCall(TimeSpan.FromSeconds(10), () => SetResistance(ResistanceType.Energy, 30)); // Reset to normal resistance
            m_NextMentalBarrier = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Set cooldown
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
