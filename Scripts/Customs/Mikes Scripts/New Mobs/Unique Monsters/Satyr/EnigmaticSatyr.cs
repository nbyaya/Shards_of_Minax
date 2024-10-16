using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an enigmatic satyr's corpse")]
    public class EnigmaticSatyr : BaseCreature
    {
        private DateTime m_NextVeiledSerenade;
        private DateTime m_NextMysticVeil;
        private DateTime m_NextEchoingMirage;
        private bool m_AbilitiesInitialized;

        [Constructable]
        public EnigmaticSatyr()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "an enigmatic satyr";
            Body = 271; // Satyr body
            Hue = 2332; // Unique hue for the Enigmatic Satyr
            BaseSoundID = 0x586;

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

        public EnigmaticSatyr(Serial serial)
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
                    m_NextVeiledSerenade = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 20));
                    m_NextMysticVeil = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 30));
                    m_NextEchoingMirage = DateTime.UtcNow + TimeSpan.FromSeconds(rand.Next(1, 60));
                    m_AbilitiesInitialized = true; // Set the flag to prevent re-initializing
                }

                if (DateTime.UtcNow >= m_NextVeiledSerenade)
                {
                    VeiledSerenade();
                }

                if (DateTime.UtcNow >= m_NextMysticVeil)
                {
                    MysticVeil();
                }

                if (DateTime.UtcNow >= m_NextEchoingMirage)
                {
                    EchoingMirage();
                }
            }
        }

        private void VeiledSerenade()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Enigmatic Satyr plays a haunting melody *");
            FixedEffect(0x3728, 10, 16);

            foreach (Mobile m in GetMobilesInRange(5))
            {
                if (m != this && m.Player)
                {
                    m.SendMessage("A fog of illusions surrounds you, making it harder to target the Satyr!");
                    // Apply an effect here to reduce hit chance, like a debuff.
                }
            }

            m_NextVeiledSerenade = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Reset with fixed cooldown
        }

        private void MysticVeil()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Enigmatic Satyr conjures a mystical barrier of sound *");
            FixedEffect(0x373A, 10, 16);

            // Apply a damage reduction effect here for a short duration.
            // Example: Reduce incoming damage by 50% for 10 seconds.

            m_NextMysticVeil = DateTime.UtcNow + TimeSpan.FromSeconds(60); // Reset with fixed cooldown
        }

        private void EchoingMirage()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* The Enigmatic Satyr creates echoing mirages of itself *");
            FixedEffect(0x3728, 10, 16);

            // Create mirage clones here
            Point3D loc = GetSpawnPosition(3);
            if (loc != Point3D.Zero)
            {
                EchoingMirageClone clone = new EchoingMirageClone(this);
                clone.MoveToWorld(loc, Map);

                Timer.DelayCall(TimeSpan.FromSeconds(30), new TimerCallback(delegate() 
                {
                    if (!clone.Deleted)
                        clone.Delete(); 
                }));
            }

            m_NextEchoingMirage = DateTime.UtcNow + TimeSpan.FromMinutes(2); // Reset with fixed cooldown
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
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextVeiledSerenade = DateTime.UtcNow;
            m_NextMysticVeil = DateTime.UtcNow;
            m_NextEchoingMirage = DateTime.UtcNow;
            m_AbilitiesInitialized = false; // Reset the flag on deserialize
        }
    }

    public class EchoingMirageClone : BaseCreature
    {
        private Mobile m_Master;

        public EchoingMirageClone(Mobile master)
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

        public EchoingMirageClone(Serial serial)
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
