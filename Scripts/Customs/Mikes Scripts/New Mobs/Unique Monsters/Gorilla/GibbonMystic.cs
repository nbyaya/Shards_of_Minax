using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("a gibbon mystic corpse")]
    public class GibbonMystic : BaseCreature
    {
        private DateTime m_NextEchoingCall;
        private DateTime m_NextTemporalShift;
        private DateTime m_NextMysticRoar;
        private bool m_AbilitiesInitialized; // Flag to track if abilities have been initialized

        [Constructable]
        public GibbonMystic()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a gibbon mystic";
            Body = 0x1D; // Gorilla body
            Hue = 1963; // Unique hue
			this.BaseSoundID = 0x9E;

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

        public GibbonMystic(Serial serial)
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
                    m_NextEchoingCall = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextTemporalShift = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_NextMysticRoar = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 90));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextEchoingCall)
                {
                    EchoingCall();
                }

                if (DateTime.UtcNow >= m_NextTemporalShift)
                {
                    TemporalShift();
                }

                if (DateTime.UtcNow >= m_NextMysticRoar)
                {
                    MysticRoar();
                }
            }
        }

        private void EchoingCall()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gibbon Mystic's call reverberates through your mind, causing confusion and summoning illusions!*");

            // Confuse and freeze
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("You are disoriented by the Gibbon Mystic's call!");
                    m.Freeze(TimeSpan.FromSeconds(3));
                }
            }

            // Create illusions
            for (int i = 0; i < 2; i++)
            {
                BaseCreature illusion = new IllusionaryGibbon(this);
                illusion.MoveToWorld(GetSpawnPosition(2), Map);
                illusion.Combatant = Combatant;
            }

            m_NextEchoingCall = DateTime.UtcNow + TimeSpan.FromSeconds(45); // Cooldown for EchoingCall
        }

        private void TemporalShift()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gibbon Mystic slows time around you and creates a temporal disturbance! *");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("Your attacks are slowed by the Gibbon Mystic's temporal shift!");
                    m.Freeze(TimeSpan.FromSeconds(5));
                }
            }

            // Create a zone of disturbance
            Timer.DelayCall(TimeSpan.FromSeconds(1), () => ApplyTemporalZone());

            m_NextTemporalShift = DateTime.UtcNow + TimeSpan.FromMinutes(1); // Cooldown for TemporalShift
        }

        private void ApplyTemporalZone()
        {
            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    m.SendMessage("The temporal disturbance around the Gibbon Mystic slows your attack speed!");
                    m.Freeze(TimeSpan.FromSeconds(5));
                }
            }
        }

        private void MysticRoar()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Gibbon Mystic lets out a powerful roar, shaking the ground!*");

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Alive)
                {
                    if (Utility.RandomDouble() < 0.3) // 30% chance to disarm
                    {
                        m.SendMessage("You are disarmed by the Gibbon Mystic's roar!");
                        m.SendMessage("Your weapon is knocked from your hands!");
                        m.SendMessage("Use / disarm to pick up your weapon.");
                        m.SendMessage("Check your inventory.");
                        m.SendMessage("You have been disarmed!");

                        // Disarm effect
                        Item weapon = m.FindItemOnLayer(Layer.OneHanded) ?? m.FindItemOnLayer(Layer.TwoHanded);
                        if (weapon != null)
                        {
                            weapon.MoveToWorld(GetSpawnPosition(1), Map);
                        }
                    }
                }
            }

            m_NextMysticRoar = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Cooldown for MysticRoar
        }

        private Point3D GetSpawnPosition(int range)
        {
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-range, range);
                int y = Y + Utility.RandomMinMax(-range, range);
                int z = Map.GetAverageZ(x, y);

                Point3D p = new Point3D(x, y, z);

                if (Map.CanSpawnMobile(p))
                    return p;
            }

            return Point3D.Zero;
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

            // Reset initialization flag to reinitialize on next combat
            m_AbilitiesInitialized = false;
        }
    }

    public class IllusionaryGibbon : BaseCreature
    {
        private Mobile m_Master;

        public IllusionaryGibbon(Mobile master)
            : base(AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4)
        {
            m_Master = master;

            Body = master.Body;
            Hue = master.Hue;
            Name = master.Name;

            SetStr(1);
            SetDex(1);
            SetInt(1);

            SetHits(1);
            SetDamage(0);

            SetResistance(ResistanceType.Physical, 100);
            SetResistance(ResistanceType.Fire, 100);
            SetResistance(ResistanceType.Cold, 100);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 100);

            VirtualArmor = 100;
        }

        public IllusionaryGibbon(Serial serial)
            : base(serial)
        {
        }

        public override bool DeleteCorpseOnDeath { get { return true; } }

        public override void OnThink()
        {
            if (m_Master == null || m_Master.Deleted)
            {
                Delete();
                return;
            }

            if (Combatant == null)
                Combatant = m_Master.Combatant;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_Master);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_Master = reader.ReadMobile();
        }
    }
}
